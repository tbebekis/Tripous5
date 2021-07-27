/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


namespace Tripous
{
    /// <summary>
    /// Helper class for defining a Date format
    /// </summary>
    public class DateFormat
    {
        /* private */
        DatePattern pattern;
        DateSeparator separator;
        bool twoDigitYear;

        /* private */
        private void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DateFormat()
            : this(DatePattern.YMD, DateSeparator.Hyphen, false)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public DateFormat(DatePattern Pattern, DateSeparator Separator, bool TwoDigitYear)
        {
            this.Pattern = Pattern;
            this.Separator = Separator;
            this.TwoDigitYear = TwoDigitYear;
        }

        /* static */
        /// <summary>
        /// Returns an array of all possible formats
        /// that can be constructed using this class
        /// </summary>
        static public DateFormat[] GetAll()
        {
            List<DateFormat> List = new List<DateFormat>();
            DatePattern[] Patterns = { DatePattern.YMD, DatePattern.DMY, DatePattern.MDY };
            DateSeparator[] Separators = { DateSeparator.Hyphen, DateSeparator.Slash, DateSeparator.Dot };
            bool[] TwoDigitYears = { false, true };

            foreach (var TwoDigitYear in TwoDigitYears)
            {
                foreach (var Pattern in Patterns)
                {
                    foreach (var Separator in Separators)
                    {
                        DateFormat DF = new DateFormat(Pattern, Separator, TwoDigitYear);
                        List.Add(DF);
                    }
                }
            }

            return List.ToArray();
        }
        /// <summary>
        /// Constructs and returns a date format based on the specified arguments
        /// </summary>
        static public string GetFormat(DatePattern Pattern, DateSeparator Separator, bool TwoDigitYear)
        {
            string Year4 = "yyyy";
            string Year2 = "yy";
            string Year;
            string Month = "MM";
            string Day = "dd";
            string Sep;

            Year = TwoDigitYear ? Year2 : Year4;

            if (Separator == DateSeparator.Slash)
                Sep = "/";
            else if (Separator == DateSeparator.Dot)
                Sep = ".";
            else
                Sep = "-";

            // 0 = Sep
            // 1 = Year
            // 2 = Month
            // 3 = Day

            string Format;
            if (Pattern == DatePattern.DMY)
                Format = "{3}{0}{2}{0}{1}";                 // dd MM yyyy
            else if (Pattern == DatePattern.MDY)
                Format = "{2}{0}{3}{0}{1}";                 // MM dd yyyy
            else
                Format = "{1}{0}{2}{0}{3}";                 // yyyy MM dd

            string Result = string.Format(Format, Sep, Year, Month, Day);

            return Result;
        }
        /// <summary>
        /// Returns the date separator character of the Info culture
        /// CAUTION: Info should be a Specific culture, not a Neutral one
        /// </summary>
        static public char GetDateSeparator(CultureInfo CultureInfo)
        {
            return CultureInfo.DateTimeFormat.DateSeparator.Trim()[0];
        }
        /// <summary>
        /// Returns the date separator character of the current UI culture
        /// </summary>
        static public char GetDateSeparator()
        {
            return GetDateSeparator(Sys.GetCurrentCulture());
        }
        /// <summary>
        /// Returns the time separator character of the Info culture
        /// CAUTION: Info should be a Specific culture, not a Neutral one
        /// </summary>
        /// <param name="Info">A Specific Culture object</param>
        static public char GetTimeSeparator(CultureInfo Info)
        {
            return Info.DateTimeFormat.TimeSeparator.Trim()[0];
        }
        /// <summary>
        /// Returns the time separator character of the current UI culture
        /// </summary>
        static public char GetTimeSeparator()
        {
            return GetTimeSeparator(Sys.GetCurrentCulture());
        }
        /// <summary>
        /// Returns a <see cref="DatePattern"/> value by analyzing the DateFormat.
        /// </summary>
        static public DatePattern GetDatePattern(string DateFormat)
        {
            DatePattern Res = DatePattern.DMY;
            DateFormat = DateFormat.Trim();
            char C = char.ToUpper(DateFormat[0]);

            if (C == 'Y')
                Res = DatePattern.YMD;
            else if (C == 'M')
                Res = DatePattern.MDY;
            else
                Res = DatePattern.DMY;

            return Res;
        }
        /// <summary>
        /// Returns a <see cref="DatePattern"/> value by analyzing the ShortDatePattern of the current culture.
        /// </summary>
        static public DatePattern GetDatePattern()
        {
            return GetDatePattern(Sys.GetCurrentCulture().DateTimeFormat.ShortDatePattern);
        }
        /// <summary>
        /// Returns a unified date format string usefull in formatting dates by using the DateTime.ToString() method
        /// i.e DT.ToString(Mask.GetDateMaskFormat(true, Info))
        /// CAUTION: Info should be a Specific culture, not a Neutral one
        /// </summary>
        /// <param name="TwoDigitYear">true for two digit year formatting</param>
        /// <param name="Info">A Specific Culture object</param>
        static public string GetDateMaskFormat(bool TwoDigitYear, CultureInfo Info)
        {
            char DateSeparator = Info.DateTimeFormat.DateSeparator.Trim()[0];

            if (TwoDigitYear)
                switch (GetDatePattern(Info.DateTimeFormat.ShortDatePattern))
                {
                    case DatePattern.MDY: return string.Format("MM{0}dd{0}yy", DateSeparator);
                    case DatePattern.DMY: return string.Format("dd{0}MM{0}yy", DateSeparator);
                    case DatePattern.YMD: return string.Format("yy{0}MM{0}dd", DateSeparator);
                }
            else
                switch (GetDatePattern(Info.DateTimeFormat.ShortDatePattern))
                {
                    case DatePattern.MDY: return string.Format("MM{0}dd{0}yyyy", DateSeparator);
                    case DatePattern.DMY: return string.Format("dd{0}MM{0}yyyy", DateSeparator);
                    case DatePattern.YMD: return string.Format("yyyy{0}MM{0}dd", DateSeparator);
                }

            return string.Format("YYYY{0}MM{0}DD", DateSeparator);
        }
        /// <summary>
        /// Returns a unified date format string usefull in formatting dates by using the DateTime.ToString() method
        /// i.e DT.ToString(Mask.GetDateMaskFormat(true, Info))
        /// </summary>
        static public string GetDateMaskFormat(bool TwoDigitYear)
        {
            return GetDateMaskFormat(TwoDigitYear, Sys.GetCurrentCulture());
        }
        /// <summary>
        /// Normalizes a date string according to the date mask format the GetDateMaskFormat() returns.
        /// </summary>
        static public string NormalizeDateTime(string Text, CultureInfo CultureInfo)
        {
            string Result = Text;

            char Separator = GetDateSeparator(CultureInfo);
            string ValidChars = "0123456789 " + Separator.ToString();

            foreach (Char C in Result)
            {
                if (ValidChars.IndexOf(C) == -1)
                    return string.Empty;
            }


            string sDD = "";
            string sMM = "";
            string sYY = "";

            string[] List = Result.Split(Separator);
            if (List != null)
                for (int i = 0; i < List.Length; i++)
                    List[i] = List[i].Trim();

            DatePattern DatePattern = GetDatePattern(CultureInfo.DateTimeFormat.ShortDatePattern);

            switch (DatePattern)
            {
                case DatePattern.MDY:
                    if (List.Length < 2)
                        return Result;
                    sDD = List[1];
                    sMM = List[0];
                    break;

                case DatePattern.DMY:
                    if (List.Length < 1)
                        return Result;
                    sDD = List[0];
                    if (List.Length >= 2)
                        sMM = List[1];
                    if (List.Length >= 3)
                        sYY = List[2];
                    break;

                case DatePattern.YMD:
                    if (List.Length < 3)
                        return Result;
                    sDD = List[2];
                    sMM = List[1];
                    sYY = List[0];
                    break;
            }

            if (string.IsNullOrEmpty(sDD + sMM + sYY))
                return Result;

            int Value = 0;

            /* DAY */
            if (Sys.TryStrToInt(sDD, out Value))
                sDD = sDD.PadLeft(2, '0');
            else
                sDD = DateTime.Today.Day.ToString().PadLeft(2, '0');

            /* MONTH */
            if (Sys.TryStrToInt(sMM, out Value))
                sMM = sMM.PadLeft(2, '0');
            else
                sMM = DateTime.Today.Month.ToString().PadLeft(2, '0');

            /* YEAR */
            if (Sys.TryStrToInt(sYY, out Value))
                sYY = Value.ToString();
            else
                sYY = "";


            if (sYY.Length < 4)
            {
                string S = DateTime.Today.Year.ToString();
                S = S.Remove(S.Length - sYY.Length, sYY.Length);
                sYY = S + sYY;
            }

            /* DATE */
            switch (DatePattern)
            {
                case DatePattern.MDY: Result = sMM + Separator + sDD + Separator + sYY; break;
                case DatePattern.DMY: Result = sDD + Separator + sMM + Separator + sYY; break;
                case DatePattern.YMD: Result = sYY + Separator + sMM + Separator + sDD; break;
            }


            return Result;
        }
        /// <summary>
        /// Normalizes a date string according to the date mask format the GetDateMaskFormat() returns.
        /// </summary>
        static public string NormalizeDateTime(string Text)
        {
            return NormalizeDateTime(Text, Sys.GetCurrentCulture());
        }



        /* overrides */
        /// <summary>
        /// Returns a string representation of this instance, 
        /// actully the date format plus a formatted sample inside parentheses
        /// </summary>
        public override string ToString()
        {

            return string.Format("{0,-11}  ({1})", this.Format, this.Sample);

        }

        /* properties */
        /// <summary>
        /// The pattern (or Endian) of the date format.
        /// <para>Could be MDY (Middle-endian), DMY (Little-endian) or YMD (Big-endian) </para>
        /// </summary>
        public DatePattern Pattern
        {
            get { return pattern; }
            set
            {
                if (value != pattern)
                {
                    pattern = value;
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// The date separator
        /// </summary>
        public DateSeparator Separator
        {
            get { return separator; }
            set
            {
                if (value != separator)
                {
                    separator = value;
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// True when a two digit year is wanted
        /// </summary>
        public bool TwoDigitYear
        {
            get { return twoDigitYear; }
            set
            {
                if (value != twoDigitYear)
                {
                    twoDigitYear = value;
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// Gets the date format
        /// </summary>
        public string Format { get { return GetFormat(Pattern, Separator, TwoDigitYear); } }
        /// <summary>
        /// Returns a formatted sample
        /// </summary>
        public string Sample { get { return new DateTime(2012, 12, 21).ToString(Format); } }


        /* events */
        /// <summary>
        /// Occurs when any of the properties changes value
        /// </summary>
        public event EventHandler Changed;
    }
}
