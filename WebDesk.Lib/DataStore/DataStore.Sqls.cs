namespace WebLib
{
    static public partial class DataStore
    {
        public const string SSysDataOwnerName = "App";

        static readonly string SSelectLanguages = $@"select * from {SysTables.Lang}";
    }
}
