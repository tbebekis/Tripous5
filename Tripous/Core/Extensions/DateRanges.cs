/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Resources;

namespace Tripous
{



    /// <summary>
    /// DateRange extensions
    /// </summary>
    static public class DateRanges
    {
        /// <summary>
        /// Constant
        /// </summary>
        static public readonly string PrefixFrom = "FROM_DATE_RANGE_";
        /// <summary>
        /// Constant
        /// </summary>
        static public readonly string PrefixTo = "TO_DATE_RANGE_";
        /// <summary>
        /// Constant
        /// </summary>
        static public readonly DateRange[] WhereRanges = {
                                                        DateRange.Custom,
                                                        DateRange.Today,
                                                        DateRange.Yesterday,
                                                        DateRange.PreviousWeek,
                                                        DateRange.PreviousTwoWeeks,
                                                        DateRange.PreviousMonth,
                                                        DateRange.PreviousTwoMonths,
                                                        DateRange.PreviousThreeMonths,
                                                        DateRange.PreviousSemester,
                                                        DateRange.PreviousYear,
                                                        DateRange.PreviousTwoYears,
                                                    };

        /// <summary>
        /// Converts a <see cref="DateRange"/> to two DateTime values.
        /// </summary>
        static public bool ToDates(this DateRange Range, DateTime Today, ref DateTime FromDate, ref DateTime ToDate)
        {
            bool Result = true;

            FromDate = Today;
            ToDate = Today;

            switch (Range)
            {
                case DateRange.Today: break;
                case DateRange.Yesterday: { FromDate = FromDate.AddDays(-1); ToDate = ToDate.AddDays(-1); } break;
                case DateRange.Tomorrow: { FromDate = FromDate.AddDays(1); ToDate = ToDate.AddDays(1); } break;

                case DateRange.PreviousWeek: FromDate = FromDate.AddDays(-7); break;
                case DateRange.PreviousTwoWeeks: FromDate = FromDate.AddDays(-14); break;
                case DateRange.PreviousMonth: FromDate = FromDate.AddDays(-30); break;
                case DateRange.PreviousTwoMonths: FromDate = FromDate.AddDays(-60); break;
                case DateRange.PreviousThreeMonths: FromDate = FromDate.AddDays(-90); break;
                case DateRange.PreviousSemester: FromDate = FromDate.AddDays(-180); break;
                case DateRange.PreviousYear: FromDate = FromDate.AddDays(-365); break;
                case DateRange.PreviousTwoYears: FromDate = FromDate.AddDays(-730); break;

                case DateRange.NextWeek: ToDate = ToDate.AddDays(7); break;
                case DateRange.NextTwoWeeks: ToDate = ToDate.AddDays(14); break;
                case DateRange.NextMonth: ToDate = ToDate.AddDays(30); break;
                case DateRange.NextTwoMonths: ToDate = ToDate.AddDays(60); break;
                case DateRange.NextThreeMonths: ToDate = ToDate.AddDays(90); break;
                case DateRange.NextSemester: ToDate = ToDate.AddDays(180); break;
                case DateRange.NextYear: ToDate = ToDate.AddDays(365); break;
                case DateRange.NextTwoYears: ToDate = ToDate.AddDays(730); break;

                default: Result = false; break;
            }

            return Result;
        }
        /// <summary>
        /// True if Range denotes a past time (Today included)
        /// </summary>
        static public bool IsPast(this DateRange Range)
        {
            switch (Range)
            {
                case DateRange.Today:
                case DateRange.Yesterday:

                case DateRange.PreviousWeek:
                case DateRange.PreviousTwoWeeks:
                case DateRange.PreviousMonth:
                case DateRange.PreviousTwoMonths:
                case DateRange.PreviousThreeMonths:
                case DateRange.PreviousSemester:
                case DateRange.PreviousYear:
                case DateRange.PreviousTwoYears:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Creates an array of ListerItem items based on WhereRanges.
        /// <para>NOTE: For use with ComboBoxes, etc.</para>
        /// </summary>
        static public ListerItem[] GetWhereRangeItems()
        {
            List<ListerItem> List = new List<ListerItem>();
             
            ListerItem LI;
            string TypeName = typeof(DateRange).Name;
            string EnumName;
            string S;
            foreach (DateRange Range in DateRanges.WhereRanges)
            {
                EnumName = Range.ToString();
                S = Res.GS(TypeName + "_" + EnumName, EnumName.SplitCamelCase());  
                LI = new ListerItem(Range, S);
                List.Add(LI);
            }


            return List.ToArray();
        }
    }
}
