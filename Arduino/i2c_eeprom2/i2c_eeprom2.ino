/*
130232 Universal Through-hole Audio DSP board
(c) Elektor, 12/2013
Author: CPV

This is a sketch to quickly program the I2C EEPROM on the Elektor
130232-1 Audio DSP board. 

0. Install the Arduino libraries EEPROM24, I2CMaster & SoftI2C.

1. Load the sketch to the Arduino board.

2. Connect connector K9 to an Arduino Uno or Mega, like this:

   K9   | Arduino
   -----+--------
   SDA  |   10
   SCL  |   11
   WP   |   12
   RST  |   13
   GND  |   GND
   
3. Do not place jumper JP1 of the DSP board. 
   
4. Send the E2Prom.hex file generated in SigmaStudio to the 
   Arduino board. The baudrate is 19200 baud. The DSP board
   must be powered.
   
License: identical as EEPROM24, I2CMaster & SoftI2C.
*/

#include <EEPROM24.h>
#include <I2CMaster.h>
#include <SoftI2C.h>

SoftI2C i2c(10,11); // SDA=10, SCL=11
EEPROM24 eeprom(i2c,EEPROM_24LC256);

const int write_protect_pin = 12;
const int reset_pin = 13;
uint8_t in_buffer[128]; // Largest possible page size.
size_t in_buffer_index = 0;
uint32_t dst_address = 0;
int rx_state = 0;
uint32_t timeout = 0;
const uint32_t baudrate = 19200;
uint32_t bytes_received = 0;
boolean busy = false;

void print_hex_char(uint8_t ch)
{
  Serial.print((ch>>4)&0x0f,HEX);
  Serial.print(ch&0x0f,HEX);
}

void print_buffer(uint8_t *p_data, size_t data_size)
{
  for (uint8_t i=0; i<data_size; i++)
  {
    print_hex_char(p_data[i]);
  }
  Serial.println();
}

uint8_t hex_to_bin(uint8_t ch)
{
  if (ch>='0' && ch<='9') return ch - '0'; // 0-9
  else if (ch>='A' && ch<='F') return ch - 'A' + 10; // 10-15
  else if (ch>='a' && ch<='f') return ch - 'a' + 10; // 10-15
  return 0;
}

void reset(void)
{
  digitalWrite(write_protect_pin,1);
  digitalWrite(reset_pin,0);
  delay(5);
  digitalWrite(reset_pin,1);
  busy = false;
  rx_state = 0;
  in_buffer_index = 0;
  dst_address = 0;
  bytes_received = 0;
  timeout = millis();
  Serial.println("waiting for E2Prom.Hex file...");
}

void setup(void)
{
  pinMode(write_protect_pin,INPUT_PULLUP);
  pinMode(reset_pin,INPUT_PULLUP);
  digitalWrite(reset_pin,1);
  digitalWrite(write_protect_pin,1);
  pinMode(write_protect_pin,OUTPUT);
  pinMode(reset_pin,OUTPUT);
  
  Serial.begin(baudrate);
  Serial.println("Elektor Project 130232 Audio DSP Board");
  Serial.println("EEPROM Programmer");
  Serial.print("eeprom ");
  if (eeprom.available()==false) Serial.print("not ");
  Serial.println("found");
  
  reset();
}

void loop(void)
{
  if (busy==true && millis()-timeout>500)
  {
    // Serial port idle too long, consider file transfer done.
    // Write any remaining data to eeprom.
    eeprom.write(dst_address,in_buffer,in_buffer_index);
    Serial.print("Received ");
    Serial.print(bytes_received);
    Serial.println(" characters");
    Serial.print("Programmed ");
    Serial.print(dst_address+in_buffer_index);
    Serial.println(" bytes");
    Serial.println();
    
    // Read programmed data back to serial port.
    Serial.println("EEPROM contents:");
    uint16_t i = 0;
    while (i<dst_address+in_buffer_index)
    {
      eeprom.read(i,in_buffer,8);
      print_buffer(in_buffer,8);
      i += 8;
      if (i%eeprom.pageSize()==0) Serial.println();
    }
    
    // Get ready for a new file.
    reset();
  }
  
  while (Serial.available()!=0)
  {
    busy = true;
    digitalWrite(write_protect_pin,0);
    timeout = millis();
    uint8_t ch = Serial.read();
    bytes_received += 1;
    switch (rx_state)
    {
      case 0:
        if (ch=='x') rx_state = 1;
        else if (ch==0x0d || ch==0x0a) Serial.write(ch);
        break;
        
      case 1:
        in_buffer[in_buffer_index] = hex_to_bin(ch) << 4;
        rx_state = 2;
        break;
        
      case 2:
        in_buffer[in_buffer_index] += hex_to_bin(ch);
        print_hex_char(in_buffer[in_buffer_index]);
        in_buffer_index += 1;
        if (in_buffer_index==eeprom.pageSize())
        {
          eeprom.write(dst_address,in_buffer,in_buffer_index);
          dst_address += in_buffer_index;
          in_buffer_index = 0;
          Serial.println();
        }
        rx_state = 0;
        break;
    }
  }
}
