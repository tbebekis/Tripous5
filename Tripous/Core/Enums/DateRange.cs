/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;

namespace Tripous
{

    /// <summary>
    /// Indicates how to construct a range between two dates.
    /// </summary>
    public enum DateRange
    {
        /// <summary>
        /// Custom range
        /// </summary>
        Custom,

        /// <summary>
        /// From today To today
        /// </summary>
        Today,
        /// <summary>
        /// From yesterday To today
        /// </summary>
        Yesterday,
        /// <summary>
        /// From today To Tomorrow
        /// </summary>
        Tomorrow,

        /* From Today To ... */

        /// <summary>
        /// From 7 days before To today
        /// </summary>
        PreviousWeek,
        /// <summary>
        /// From 14 days before To today
        /// </summary>
        PreviousTwoWeeks,
        /// <summary>
        /// From 30 before To today
        /// </summary>
        PreviousMonth,
        /// <summary>
        /// From 60 before To today
        /// </summary>
        PreviousTwoMonths,
        /// <summary>
        /// From 90 before To today
        /// </summary>
        PreviousThreeMonths,
        /// <summary>
        /// From 180 before To today
        /// </summary>
        PreviousSemester,
        /// <summary>
        /// From 365 before To today
        /// </summary>
        PreviousYear,
        /// <summary>
        /// From 730 before To today
        /// </summary>
        PreviousTwoYears,

        /* From ... To Today */

        /// <summary>
        /// NextWeek
        /// </summary>
        NextWeek,
        /// <summary>
        /// NextTwoWeeks
        /// </summary>
        NextTwoWeeks,
        /// <summary>
        /// NextMonth
        /// </summary>
        NextMonth,
        /// <summary>
        /// NextTwoMonths
        /// </summary>
        NextTwoMonths,
        /// <summary>
        /// NextThreeMonths
        /// </summary>
        NextThreeMonths,
        /// <summary>
        /// NextSemester
        /// </summary>
        NextSemester,
        /// <summary>
        /// NextYear 
        /// </summary>
        NextYear,
        /// <summary>
        /// NextTwoYears
        /// </summary>
        NextTwoYears,
    }


}
