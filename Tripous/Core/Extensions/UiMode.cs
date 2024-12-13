namespace Tripous
{
    /// <summary>
    /// Indicates the platform where a Ui element may displayed, such as 
    /// in desktop or web applications, or any kind of application.
    /// </summary>
    [Flags]
    public enum UiMode
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Desktop
        /// </summary>
        Desktop = 1,
        /// <summary>
        /// Web
        /// </summary>
        Web = 2,
    }


    /// <summary>
    /// Helper
    /// </summary>
    static public class UiModes
    {
        /// <summary>
        /// Constant
        /// </summary>
        static public readonly UiMode All = (UiMode)Bf.All(typeof(UiMode));
    }
}
