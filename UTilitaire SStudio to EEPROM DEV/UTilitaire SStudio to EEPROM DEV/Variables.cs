using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Variables
    {
        public static string FullPathAndName = "";
        public static string FullPathAndNameDest = "";

        //public static string to_export = "";
        //public static string ContentFilteredForDirectProgramming = "";
        public static string Content= "";
        public static string STATE = "";//ConversionOK
        //public static string BITLENGHT = "";
        public static string NUMPORT = "COM0";
        public static bool COMPORTOPENED = false;
        public static bool HexImported = false;
        public static string[] LISTPORTS = new string[10];

        public static bool hasChanged = false;
        public static bool checkChanges = false;
        public static bool autoReprogram = false;
        public static bool invert_rts = true;
        public static bool invert_dtr = true;
        public static byte page_size = 64;

        public const int max_file_size = 10000;
        public static byte[] bin_data = new byte[max_file_size];
        public static int bin_data_index = 0;
        public static byte[] read_buffer = new byte[max_file_size];
        public static int read_buffer_index = 0;
        public static int progress_counter = 0;
        public static string hex_data;
    }
}



