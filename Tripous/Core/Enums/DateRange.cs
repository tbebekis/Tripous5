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
        LastWeek,
        /// <summary>
        /// From 14 days before To today
        /// </summary>
        LastTwoWeeks,
        /// <summary>
        /// From 30 before To today
        /// </summary>
        LastMonth,
        /// <summary>
        /// From 60 before To today
        /// </summary>
        LastTwoMonths,
        /// <summary>
        /// From 90 before To today
        /// </summary>
        LastThreeMonths,
        /// <summary>
        /// From 180 before To today
        /// </summary>
        LastSemester,
        /// <summary>
        /// From 365 before To today
        /// </summary>
        LastYear,
        /// <summary>
        /// From 730 before To today
        /// </summary>
        LastTwoYears,

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
