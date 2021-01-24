using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
     
    public partial class Form1 : Form
    {

        public Form1()
        {
            ////////////////////////////STARTUP/////////////////////////////////
            InitializeComponent();
            this.MaximumSize = this.MinimumSize = this.Size; // For static size

            Variables.LISTPORTS = System.IO.Ports.SerialPort.GetPortNames(); // COM port management
            int i =0;
            for (i=0; i<Variables.LISTPORTS.Length; i++)
            {
                comboBoxCOM.Items.Insert(i, Variables.LISTPORTS[i]);
            }
            comboBoxCOM.SelectedIndex = 0;

            if (Variables.COMPORTOPENED == false)
            {
                PortOK.Visible = false;
                PortNOK.Visible = true;
            }

            comboBoxEEPROM.Items.Insert(0, "24xx256");
            comboBoxEEPROM.Items.Insert(1, "24xx512");
            //comboBoxEEPROM.Items.Insert(2, "24xx1025");
            comboBoxEEPROM.SelectedIndex = 0;
            eeprom_select(comboBoxEEPROM.SelectedIndex);

            Variables.invert_dtr = true;
            Variables.invert_rts = true;
            checkBox2.Checked = Variables.invert_rts;
            checkBox3.Checked = Variables.invert_dtr;

            dateDisplay.Text = "";
        }
        
        private void bin_data_to_hex(byte[] data, int data_size)
        {
            int address = 0;
            int line_size = 16;
            char ch_start = ':';
            byte record_type = 0;
            Variables.hex_data = "";
            int bytes_converted = 0;
            while (bytes_converted<data_size)
            {
                byte checksum = 0;
                checksum += (byte)(line_size);
                checksum += (byte)((address>>8)&0xff);
                checksum += (byte)(address & 0xff);
                checksum += record_type;
                Variables.hex_data += ch_start;
                Variables.hex_data += line_size.ToString("X2");
                Variables.hex_data += address.ToString("X4");
                Variables.hex_data += record_type.ToString("X2");
                for (int i = 0; i < line_size; i++)
                {
                    byte ch = 0xff; // ensure padding with 0xff.
                    if (bytes_converted < data_size)
                    {
                        ch = data[bytes_converted];
                    }
                    checksum += ch;
                    Variables.hex_data += ch.ToString("X2");
                    bytes_converted += 1;
                }
                checksum ^= 0xff;
                checksum += 1;
                Variables.hex_data += checksum.ToString("X2");
                Variables.hex_data += "\r\n";
                address += line_size;
            }
            // Terminate with End Of File record.
            Variables.hex_data += ":00000001FF\r\n";
        }

        private bool load_sigmastudio_hex_file()
        {
            // Read the file and convert it to binary.
            // According to the datasheet of the ADAU1701, it will need no more than 9248 bytes.
            // A line looks like this:
            // 0x01 , 0x00 , 0x05 , 0x00 , 0x08 , 0x1C , 0x00 , 0x58 ,<CR><LF>
            StreamReader streamReader = new StreamReader(Variables.FullPathAndName);
            Variables.bin_data_index = 0;
            string line;
            do
            {
                line = streamReader.ReadLine();
                if (line != null)
                {
                    line = line.Replace(" ", "");
                    line = line.Replace("0x", "");
                    line = line.Replace(",", "");
                    for (int i = 0; i < line.Length; i += 2)
                    {
                        Variables.bin_data[Variables.bin_data_index] = hex_to_bin2(line[i], line[i + 1]);
                        Variables.bin_data_index += 1;
                        if (Variables.bin_data_index >= Variables.max_file_size)
                        {
                            MessageBox.Show("File too large, aborting");
                            Variables.bin_data_index = 0;
                            return false;
                        }
                    }
                }
            }
            while (line != null);
            streamReader.Close();
            return true;
        }

        private bool load_intel_hex_file()
        {
            // Read the file and convert it to binary.
            // A line looks something like this:
            // :1000000001000500081C0058030303030303030356
            StreamReader streamReader = new StreamReader(Variables.FullPathAndName);
            Variables.bin_data_index = 0;
            string line;
            do
            {
                line = streamReader.ReadLine();
                if (line != null)
                {
                    int i = 1; // Skip ':'
                    byte checksum = hex_to_bin2(line[i], line[i + 1]); // data size
                    int data_digits = 2 * checksum;
                    i += 2;
                    checksum += hex_to_bin2(line[i], line[i + 1]); // address msb
                    i += 2;
                    checksum += hex_to_bin2(line[i], line[i + 1]); // address lsb
                    i += 2;
                    byte record_type = hex_to_bin2(line[i], line[i + 1]);
                    if (record_type > 1)
                    {
                        MessageBox.Show("Extended Hex format is not supported, aborting");
                        Variables.bin_data_index = 0;
                        return false;
                    }
                    checksum += record_type;
                    i += 2;

                    for (; i < data_digits+9; i += 2)
                    {
                        byte ch = hex_to_bin2(line[i], line[i + 1]);
                        Variables.bin_data[Variables.bin_data_index] = ch;
                        checksum += ch;
                        Variables.bin_data_index += 1;
                        if (Variables.bin_data_index >= Variables.max_file_size)
                        {
                            MessageBox.Show("File too large, aborting");
                            Variables.bin_data_index = 0;
                            return false;
                        }
                    }

                    checksum ^= 0xff;
                    checksum += 1;
                    if (checksum != hex_to_bin2(line[i], line[i + 1]))
                    {
                        MessageBox.Show("Invalid checksum, aborting");
                        Variables.bin_data_index = 0;
                        return false;
                    }
                }
            }
            while (line != null);
            streamReader.Close();
            return true;
        }

        private void BROWSE_Click(object sender, EventArgs e)
        {
            //BROWSE FOR FILE BUTTON
            
            OpenFileDialog browse = new OpenFileDialog();
            browse.Title = "Open File";
            browse.Filter = "SigmaStudio Output (.Hex)|E2Prom.Hex"; // Only allow "E2Prom.Hex"
            browse.FilterIndex = 1;
            browse.RestoreDirectory = true;
            if (browse.ShowDialog() == DialogResult.OK)
            {
                string FullPathAndFilename = browse.FileName;
                Variables.FullPathAndName = FullPathAndFilename;
                label3.Text = Variables.FullPathAndName;
                string FullPathAndFilenameTemp = FullPathAndFilename.Replace(".Hex", "_converted.hex");
                load_sigmastudio_hex_file();
                // Create Intel HEX file.
                bin_data_to_hex(Variables.bin_data,Variables.bin_data_index);
    
                Variables.FullPathAndNameDest = FullPathAndFilenameTemp;
                label4.Text = Variables.FullPathAndNameDest;
                label2.Text = Variables.hex_data;

                Variables.STATE = "SigmaStudio Hex file loaded";
                stateDisplay.Text = Variables.STATE;
                Variables.HexImported = false;

                CreateFileWatcher(Variables.FullPathAndName);
                dateDisplay.Text = File.GetLastWriteTime(Variables.FullPathAndName).ToString();
                dateDisplay.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                Variables.STATE = "Error opening file";
                stateDisplay.Text = Variables.STATE;
            }
        }

        public byte hex_to_bin(char ch)
        {
            // Assumes uppercase characters.
            if (ch > '9') return (byte)(ch - 'A' + 10);
            else return (byte)(ch - '0');
        }

        public byte hex_to_bin(byte ch)
        {
            // Assumes uppercase characters.
            if (ch > '9') return (byte)(ch - 'A' + 10);
            else return (byte)(ch - '0');
        }

        public byte hex_to_bin2(char ch1, char ch2)
        {
            byte value = hex_to_bin(ch1);
            value <<= 4;
            value += hex_to_bin(ch2);
            return value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // SAVE FILE
            //if (Variables.HexImported == true)
            //{
                // Write file
                File.WriteAllText(Variables.FullPathAndNameDest, Variables.hex_data);

                // Read back written file for verification.
                StreamReader streamReader = new StreamReader(Variables.FullPathAndNameDest);
                string hex_data_readback = streamReader.ReadToEnd();
                streamReader.Close();

                if (hex_data_readback == Variables.hex_data)
                {
                    MessageBox.Show("File written to " + Variables.FullPathAndNameDest);
                    stateDisplay.Text = "Intel Hex file written";
                }
                else 
                { 
                    MessageBox.Show("File write error"); 
                }
            //}
            //else
            //{
            //    MessageBox.Show("No file to export");
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button 3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button 4");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Variables.NUMPORT = (string)comboBoxCOM.SelectedItem;
        }

        public static SerialPort sp;

        public bool initComPort()
        {
            sp = new SerialPort(Variables.NUMPORT);
            bool porterror = false; // false = no error, true = error 

            try
            {
                sp.Open();
            }
            catch (Exception ex)
            {
                stateDisplay.Text = "Cannot open port, exception " + ex.Message.ToString();
                porterror = true;
            }
            //finally { 

            if (porterror == false)
            {
                Variables.COMPORTOPENED = true;
                PortOK.Visible = true;
                PortNOK.Visible = false;
                PortOK.Refresh();
                stateDisplay.Refresh();
            }
            else
            {
                Variables.COMPORTOPENED = false;
                PortOK.Visible = false;
                PortNOK.Visible = true;
                PortNOK.Refresh();
            }

            // Initialise I2C SCL and SDA lines.
            i2c_scl(true); // SCL
            i2c_sda(true); // SDA

            return Variables.COMPORTOPENED;
        }
        
        public void closeComPort()
        {
            if (Variables.COMPORTOPENED==true)
            {
                sp.Close();
                Variables.COMPORTOPENED = false;
                PortOK.Visible = false;
                PortNOK.Visible = true;
                PortNOK.Refresh();
            }
        }

        public static void i2c_scl(bool value)
        {
            if (Variables.invert_rts==false) sp.RtsEnable = value;
            else sp.RtsEnable = !value;
        }

        public static void i2c_sda(bool value)
        {
            if (Variables.invert_dtr == false) sp.DtrEnable = value;
            else sp.DtrEnable = !value;
        }

        public static bool i2c_sda_read()
        {
            if (Variables.invert_dtr == false) return !sp.CtsHolding;
            else return sp.CtsHolding;
        }
        
        public static void i2c_start()
        {
            i2c_sda(true);
            i2c_scl(true);
            i2c_sda(false);
        }
       
        public static void i2c_stop()
        {
            i2c_scl(false);
            i2c_sda(false);
            i2c_scl(true);
            i2c_sda(true);
        }

        public static void i2c_send_bit(bool value)
        {
            i2c_scl(false); // SDA may only change when SCL is low.
            i2c_sda(value); // Set or clear SDA.
            i2c_scl(true); // SCL high.
            i2c_scl(false);
        }

        // Read a bit.
        public static bool rx_bit()
        {
            bool temp;
            i2c_scl(false);
            i2c_sda(true);
            i2c_scl(true);
            temp = i2c_sda_read();
            i2c_scl(false);
            return temp;
        }

        // Transmit a byte bit by bit.
        public static bool i2c_send_byte(byte value)
        {
            // Send bits.
            for (byte mask=0x80; mask!=0; mask>>=1)
            {
                i2c_send_bit((value & mask)!=0);
            }
            // Read acknowledge bit.
            bool ack;
            i2c_sda(true);
            i2c_scl(true);
            ack = i2c_sda_read();
            i2c_scl(false);
            return ack;
        }

        // Read a byte.
        public static byte i2c_recv_byte_no_ack()
        {
            int i;
            byte value = 0;
            for (i=0; i<8; i++)
            {
                value <<= 1;
                if (rx_bit() == false) value |= 0x01;
            }
            return value;
        }

        // Read a byte and send an acknowledge.
        public static byte i2c_recv_byte(bool ack)
        {
            byte value = i2c_recv_byte_no_ack();
            i2c_send_bit(!ack);
            return value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////

        private int eeprom_write_page(int offset, byte page_size, byte address_msb, byte address_lsb)
        {
            i2c_start();
            // Write device control code "1010" + chip select bits A0-A2 ("000") + write mode ("0")
            if (i2c_send_byte(0xa0) == false)
            {
                i2c_stop();
                MessageBox.Show("EEPROM did not acknowlegde, aborting.");
                return -1;
            }
            i2c_send_byte(address_msb); // Write memory address MSB
            i2c_send_byte(address_lsb); // Write memory address LSB

            int bytes_written = 0;
            for (int i = offset; i < offset+page_size; i++)
            {
                stateDisplay.Text = "Programming " + (100 * Variables.progress_counter / Variables.bin_data_index).ToString() + " %";
                stateDisplay.Refresh();
                i2c_send_byte(Variables.bin_data[i]);
                bytes_written += 1;
                Variables.progress_counter += 1;
            }
            i2c_stop();

            System.Threading.Thread.Sleep(10); // Datasheet specifies 5ms max for a page write.
            return bytes_written;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (initComPort()==false)
            {
                MessageBox.Show("Could not open COM port");
            }
            else if (Variables.bin_data_index==0)
            {
                MessageBox.Show("No file loaded, can't program the EEPROM");
            }
            else
            {
                int data_size = Variables.bin_data_index;
                int offset = 0;
                byte address_msb = 0;
                byte address_lsb = 0;

                stateDisplay.Text = "Programming                      ";
                stateDisplay.Refresh();
                System.Threading.Thread.Sleep(250);

                Variables.progress_counter = 0;
                int i;
                for (i = 0; i < data_size; i += Variables.page_size)
                {
                    // Update progress indicator.
                    //stateDisplay.Text = ("Programming page " + (i/Variables.page_size).ToString());
                    //stateDisplay.Refresh();

                    int bytes_written = eeprom_write_page(offset, Variables.page_size, address_msb, address_lsb);
                    if (bytes_written < 0)
                    {
                        stateDisplay.Text = "Programming failed";
                        closeComPort();
                        return;
                    }

                    // Update counters etc.
                    offset += Variables.page_size;
                    address_lsb += Variables.page_size; // page_size must be a multiple of 2.
                    if (address_lsb==0) address_msb += 1;
                }
                dateDisplay.ForeColor = System.Drawing.Color.Black;
                stateDisplay.Text = "Programming OK";
            }
            closeComPort();
        }

        private string eeprom_random_read(byte address_msb, byte address_lsb)
        {
            string result = "";
            int decValue;

            i2c_start();
            if (i2c_send_byte(0xa0) == false)
            {
                i2c_stop();
                MessageBox.Show("EEPROM did not acknowlegde, aborting.");
                return result;
            }

            i2c_send_byte(address_msb); // Memory address MSB
            i2c_send_byte(address_lsb); // Memory address LSB
            i2c_start(); // Restart.
            i2c_send_byte(0xa1); // Switch to read mode.

            // Read one byte.
            decValue = i2c_recv_byte(false); // false : pas d'ack
            i2c_stop();

            result = decValue.ToString("X");
            return result;
        }

        private int eeprom_read_all(byte[] data, int data_size)
        {
            int i;

            i2c_start();
            if (i2c_send_byte(0xa0) == false)
            {
                i2c_stop();
                MessageBox.Show("EEPROM did not acknowlegde, aborting.");
                return -1;
            }

            stateDisplay.Text = "Reading                     ";
            stateDisplay.Refresh();
            System.Threading.Thread.Sleep(250);

            i2c_send_byte(0); // Memory address MSB
            i2c_send_byte(0); // Memory address LSB
            i2c_start(); // Restart.
            i2c_send_byte(0xa1); // Switch to read mode.

            for (i = 0; i < data_size - 1; i++)
            {
                stateDisplay.Text = "Reading " + (100*i/data_size).ToString() + " %";
                stateDisplay.Refresh();
                data[i] = i2c_recv_byte(true);
            }
            // Last byte read must not be acknowledged.
            data[i++] = i2c_recv_byte_no_ack();
            i2c_stop();

            return i;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (Variables.read_buffer_index <= 0 || Variables.bin_data_index<=0)
            {
                MessageBox.Show("No data to verify");
            }
            else
            {
                for (int i = 0; i < Variables.bin_data_index; i++)
                {
                    if (Variables.read_buffer[i] != Variables.bin_data[i])
                    {
                        MessageBox.Show("Verify failed");
                        stateDisplay.Text = "Verify failed";
                        return;
                    }
                    stateDisplay.Text = "Verify OK";
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //Browse for .hex 

            OpenFileDialog browse = new OpenFileDialog();
            browse.Title = "Open File";
            browse.Filter = "Intel Hex files (.hex)|*.hex";
            browse.FilterIndex = 1;
            browse.RestoreDirectory = true;
            if (browse.ShowDialog() == DialogResult.OK)
            {
                string FullPathAndFilename = browse.FileName;
                Variables.FullPathAndName = FullPathAndFilename;
                label3.Text = Variables.FullPathAndName;
                string FullPathAndFilenameTemp = FullPathAndFilename.Replace(".Hex", "_converted.hex");

                if (load_intel_hex_file() == true)
                {
                    Variables.STATE = "Intel Hex file loaded";
                    Variables.HexImported = true;
                    // Create Intel HEX file.
                    bin_data_to_hex(Variables.bin_data, Variables.bin_data_index);
                    Variables.FullPathAndNameDest = FullPathAndFilenameTemp;
                    label4.Text = Variables.FullPathAndNameDest;
                    label2.Text = Variables.hex_data;
                    dateDisplay.Text = File.GetLastWriteTime(Variables.FullPathAndName).ToString();
                    dateDisplay.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    Variables.STATE = "Error opening Intel Hex file";
                    Variables.HexImported = false;
                }
                stateDisplay.Text = Variables.STATE;
            }
            else
            {
                Variables.STATE = "Error opening Intel Hex file";
                stateDisplay.Text = Variables.STATE;
                Variables.HexImported = false;
            }
        }

        public void CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path.Replace("E2Prom.Hex","");
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.Hex";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
           // watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        public static void OnChanged(object source, FileSystemEventArgs e) // normalement doit etre static mais bon ...
        {

            if (Variables.checkChanges == true)
            {
                Variables.hasChanged = true;
                // Specify what is done when a file is changed, created, or deleted.
                //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                //DialogResult dialogchanged = MessageBox.Show("File at " + Variables.FullPathAndName + " has changed, Do you want to reload and reprogram it ?", "File Changed", MessageBoxButtons.YesNo);

                //if (dialogchanged == DialogResult.Yes)
                //{

                //    //convertDatToHexAndSTRProg();
                //    Variables.hasChanged = true;



                //}
                //else if (dialogchanged == DialogResult.No)
                //{
                //    Variables.hasChanged = false; //do something else
                //}
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Variables.checkChanges = checkBox1.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer to refresh checkbox state and for file change detection.
            Variables.checkChanges = checkBox1.Checked;

            if (Variables.hasChanged == true && checkBox1.Checked == true)
            {
                //convertDatToHexAndSTRProg();
                bin_data_to_hex(Variables.bin_data, Variables.bin_data_index);

                //while (Variables.COMPORTOPENED == true)
                //{ // tant que le port com est utilisé, on ne fait rien
                //    //trap
                //    stateDisplay.Text = stateDisplay.Text + "Waiting com port to close...";
                //}

                //button6_Click(sender,e);// etape de programmation
                //sortie de trap
                //DateTime tmp = File.GetLastAccessTime(Variables.FullPathAndName);
                dateDisplay.Text = File.GetLastWriteTime(Variables.FullPathAndName).ToString() ;
                dateDisplay.ForeColor = System.Drawing.Color.Red;
                stateDisplay.Text = stateDisplay.Text + ", file has changed!";
                label2.Refresh();
                Variables.hasChanged = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Variables.invert_rts = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Variables.invert_dtr = checkBox3.Checked;
        }

        private void eeprom_select(int index)
        {
            switch (index)
            {
                case 0: 
                    Variables.page_size = 64; 
                    break;

                case 1: 
                    Variables.page_size = 128; 
                    break;

                case 2: 
                    //Variables.page_size = 128; 
                    //break;

                default: 
                    comboBoxEEPROM.SelectedIndex = 0;
                    Variables.page_size = 64; 
                    break;
            }
        }

        private void comboBoxEEPROM_SelectedIndexChanged(object sender, EventArgs e)
        {
            eeprom_select(comboBoxEEPROM.SelectedIndex);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (initComPort() == false)
            {
                MessageBox.Show("Could not open COM port");
            }
            else
            {
                int data_size = Variables.bin_data_index;
                if (data_size == 0) data_size = Variables.max_file_size;
                Variables.read_buffer_index = eeprom_read_all(Variables.read_buffer,data_size);
                if (Variables.read_buffer_index < 0)
                {
                    MessageBox.Show("Read failed");
                    stateDisplay.Text = "Read failed";
                }
                else
                {
                    stateDisplay.Text = "Read OK";
                    bin_data_to_hex(Variables.read_buffer, Variables.read_buffer_index);
                    Variables.FullPathAndNameDest = Variables.FullPathAndNameDest.Replace(".hex", "_readback.hex");
                    label4.Text = Variables.FullPathAndNameDest;
                    label2.Text = Variables.hex_data;
                }
                closeComPort();
            }
        }

    }
}

/// //////////////////////////////////////////////////////////////////////////////////////
