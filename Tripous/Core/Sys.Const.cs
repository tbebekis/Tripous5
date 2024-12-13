namespace Tripous
{
    static partial class Sys
    {
        #region ISO DateTime formats array
        /// <summary>
        /// An array of ISO8601 datetime formats 
        /// </summary>
        static public readonly string[] ISODateTimeFormats = new string[] {
          "yyyy-MM-dd HH:mm:ss.fffffff",
          "yyyy-MM-dd HH:mm:ss",
          "yyyy-MM-dd HH:mm",
          "yyyyMMddHHmmss",
          "yyyyMMddHHmm",
          "yyyyMMddTHHmmssfffffff",
          "yyyy-MM-dd",
          "yy-MM-dd",
          "yyyyMMdd",
          "HH:mm:ss",
          "HH:mm",
          "THHmmss",
          "THHmm",
          "yyyy-MM-dd HH:mm:ss.fff",
          "yyyy-MM-ddTHH:mm",
          "yyyy-MM-ddTHH:mm:ss",
          "yyyy-MM-ddTHH:mm:ss.fff",
          "yyyy-MM-ddTHH:mm:ss.ffffff",
          "HH:mm:ss.fff"
          };
        #endregion


        #region Ascii chart
        /// <summary>
        /// Ascii chart
        /// </summary>
        static public readonly string[] AsciiChart = new string[] {
                                                    "NUL",   // 0     	00
                                                    "SOH",   // 1     	01     start of header
                                                    "STX",   // 2     	02     start of text
                                                    "ETX",   // 3     	03     end of text
                                                    "EOT",   // 4     	04     end of transmission
                                                    "ENQ",   // 5     	05     enquiry
                                                    "ACK",   // 6     	06     acknowledge
                                                    "BEL",   // 7     	07     bell
                                                    "BS ",   // 8     	08     backspace
                                                    "TAB",   // 9     	09     horizontal tab
                                                    "LF ",   // 10    	0A     line feed
                                                    "VT ",   // 11    	0B     vertical tab
                                                    "FF ",   // 12    	0C     form feed
                                                    "CR ",   // 13    	0D     carriage return
                                                    "SO ",   // 14    	0E     shift out
                                                    "SI ",   // 15    	0F     shift in
                                                    "DLE",   // 16    	10     data link escape
                                                    "DC1",   // 17    	11     device control, usually XON
                                                    "DC2",   // 18    	12     device control
                                                    "DC3",   // 19    	13     device control, usually XOFF
                                                    "DC4",   // 20    	14     device control
                                                    "NAK",   // 21    	15     negative acknowledge
                                                    "SYN",   // 22    	16     synchronous idle
                                                    "ETB",   // 23    	17     end of transmission block
                                                    "CAN",   // 24    	18     cancel
                                                    "EM ",   // 25    	19     end of medium
                                                    "SUB",   // 26    	1A     substitute
                                                    "ESC",   // 27    	1B     escape
                                                    "FS ",   // 28    	1C     file seperator
                                                    "GS ",   // 29    	1D     group seperator
                                                    "RS ",   // 30    	1E     record seperator
                                                    "US ",   // 31    	1F     unit seperator
                                                    "SPC",   // 32    	20     space
                                                    "!  ",   // 33    	21
                                                    "\" ",   // 34    	22
                                                    "#  ",   // 35    	23
                                                    "$  ",   // 36    	24
                                                    "%  ",   // 37    	25
                                                    "&  ",   // 38    	26
                                                    "'  ",   // 39    	27
                                                    "(  ",   // 40    	28
                                                    ")  ",   // 41    	29
                                                    "*  ",   // 42    	2A
                                                    "+  ",   // 43    	2B
                                                    ",  ",   // 44    	2C
                                                    "-  ",   // 45    	2D
                                                    ".  ",   // 46    	2E
                                                    "/  ",   // 47    	2F
                                                    "0  ",   // 48    	30
                                                    "1  ",   // 49    	31
                                                    "2  ",   // 50    	32
                                                    "3  ",   // 51    	33 
                                                    "4  ",   // 52    	34
                                                    "5  ",   // 53    	35 
                                                    "6  ",   // 54    	36
                                                    "7  ",   // 55    	37
                                                    "8  ",   // 56    	38
                                                    "9  ",   // 57    	39 
                                                    ":  ",   // 58    	3A
                                                    ";  ",   // 59    	3B
                                                    "<  ",   // 60    	3C
                                                    "=  ",   // 61    	3D 
                                                    ">  ",   // 62    	3E
                                                    "?  ",   // 63    	3F
                                                    "@  ",   // 64    	40
                                                    "A  ",   // 65    	41 
                                                    "B  ",   // 66    	42
                                                    "C  ",   // 67    	43
                                                    "D  ",   // 68    	44
                                                    "E  ",   // 69    	45 
                                                    "F  ",   // 70    	46
                                                    "G  ",   // 71    	47
                                                    "H  ",   // 72    	48
                                                    "I  ",   // 73    	49 
                                                    "J  ",   // 74    	4A
                                                    "K  ",   // 75    	4B
                                                    "L  ",   // 76    	4C
                                                    "M  ",   // 77    	4D 
                                                    "N  ",   // 78    	4E
                                                    "O  ",   // 79    	4F
                                                    "P  ",   // 80    	50
                                                    "Q  ",   // 81    	51 
                                                    "R  ",   // 82    	52
                                                    "S  ",   // 83    	53
                                                    "T  ",   // 84    	54
                                                    "U  ",   // 85    	55 
                                                    "V  ",   // 86    	56
                                                    "W  ",   // 87    	57
                                                    "X  ",   // 88    	58
                                                    "Y  ",   // 89    	59 
                                                    "Z  ",   // 90    	5A
                                                    "[  ",   // 91    	5B
                                                    "\\ ",   // 92    	5C
                                                    "]  ",   // 93    	5D 
                                                    "^  ",   // 94    	5E
                                                    "_  ",   // 95    	5F
                                                    "`  ",   // 96    	60
                                                    "a  ",   // 97    	61 
                                                    "b  ",   // 98    	62
                                                    "c  ",   // 99    	63
                                                    "d  ",   // 100   	64
                                                    "e  ",   // 101   	65 
                                                    "f  ",   // 102   	66
                                                    "g  ",   // 103   	67
                                                    "h  ",   // 104   	68
                                                    "i  ",   // 105   	69 
                                                    "j  ",   // 106   	6A
                                                    "k  ",   // 107   	6B
                                                    "l  ",   // 108   	6C
                                                    "m  ",   // 109   	6D 
                                                    "n  ",   // 110   	6E
                                                    "o  ",   // 111   	6F
                                                    "p  ",   // 112   	70
                                                    "q  ",   // 113   	71 
                                                    "r  ",   // 114   	72
                                                    "s  ",   // 115   	73
                                                    "t  ",   // 116   	74
                                                    "u  ",   // 117   	75 
                                                    "v  ",   // 118   	76
                                                    "w  ",   // 119   	77
                                                    "x  ",   // 120   	78
                                                    "y  ",   // 121   	79 
                                                    "z  ",   // 122   	7A
                                                    "{  ",   // 123   	7B
                                                    "|  ",   // 124   	7C
                                                    "}  ",   // 125   	7D 
                                                    "~  ",   // 126   	7E
                                                    "DEL"    // 127   	7F
                                                    };

        #endregion


        /* constants */
        /// <summary>
        /// Constant
        /// </summary>
        public const string None = "[none]";
        /// <summary>
        /// Constant
        /// </summary>
        public const string NULL = "___null___"; 
        /// <summary>
        /// Constant
        /// </summary>
        public const string MASTER_KEY_FIELD_NAME = "MASTER_KEY_FIELD_NAME";
        /// <summary>
        /// Constant
        /// </summary>
        public const string FromField = "_FROM";
        /// <summary>
        /// Constant
        /// </summary>
        public const string ToField = "_TO";


        /// <summary>
        /// Constant
        /// </summary>
        public const string NamePathSep = ".";
        /// <summary>
        /// Constant
        /// </summary>
        public const string FieldAliasSep = "__"; 

        /* characters */
        /// <summary>
        /// Constant
        /// </summary>
        public const char CharNull = '\0';
        /// <summary>
        /// Constant
        /// </summary>
        public const char CharEnter = '\r';
        /// <summary>
        /// Constant
        /// </summary>
        public const char CharNewLine = '\n';
        /// <summary>
        /// Constant
        /// </summary>
        public const char CharTab = '\t';
        /// <summary>
        /// Constant
        /// </summary>
        public const char CharEscape = (char)27;
        /// <summary>
        /// Constant
        /// </summary>
        public const char CharBackspace = '\b';

        /* schema */
        /// <summary>
        /// Constant
        /// </summary>
        public const string SYSTEM = "System";
        /// <summary>
        /// Constant
        /// </summary>
        public const string APPLICATION = "Application";

        /* miscs */
        /// <summary>
        /// Constant
        /// </summary>
        public const string AppIco = "AppIco";
        /// <summary>
        /// Constant
        /// </summary>
        public const string StandardCompanyGuid = "74772779-BF08-4B22-8F87-196FB87EC7C2";
        /// <summary>
        /// Constant
        /// </summary>
        public const string InvalidId = "27C15428-7892-4F7D-B28F-9BA059C94BA4";
        /// <summary>
        /// Constant
        /// </summary>
        public const string EnId = "D4997C35-6E89-499A-87BF-D5750D0D3F06";
        /// <summary>
        /// Constant
        /// </summary>
        public const string GrId = "92A158E7-25CA-4367-BA57-FB79C40D775C";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MainSelect = "MainSelect";

        /* application messages */
        /// <summary>
        /// Constant
        /// </summary>
        public const string EventName = "EventName";

        /// <summary>
        /// The name of the Default sql connection setting.
        /// </summary>
        public const string DEFAULT = "Default";
 
    }
}
