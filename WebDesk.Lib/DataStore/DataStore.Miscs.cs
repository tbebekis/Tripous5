namespace WebLib
{

    /// <summary>
    /// Represents the database
    /// </summary>
    static public partial class DataStore
    {
 
        /*
        /// <summary>
        /// Computes the hash value of a specifed byte array
        /// </summary>
        static string ComputeHash(byte[] Buffer, string AlgorithmName = "SHA1")
        {
            if (string.IsNullOrWhiteSpace(AlgorithmName))
                AlgorithmName = "SHA1";

            HashAlgorithm Algorithm = HashAlgorithm.Create(AlgorithmName);
            if (Algorithm == null)
                throw new ArgumentException("Wrong Hash Algorith Name");

            Buffer = Algorithm.ComputeHash(Buffer);
            return BitConverter.ToString(Buffer).Replace("-", "");
        }
        */
        /// <summary>
        /// Validates the password of a user/requestor
        /// </summary>
        static bool ValidatePassword(string PlainTextPassword, string Base64SaltKey, string Base64HashedPassword)
        {
            if (string.IsNullOrWhiteSpace(PlainTextPassword) || string.IsNullOrWhiteSpace(Base64SaltKey))
                return false;

            var Settings = GetSettings();
            string SuperUserPassword = Settings.General.SuperUserPassword;
            if (!string.IsNullOrWhiteSpace(SuperUserPassword) && (PlainTextPassword == SuperUserPassword))
                return true;

            return Hasher.Validate(PlainTextPassword, Base64HashedPassword, Base64SaltKey); 
        }
    
        static Command[] GetMainMenuDemo()
        {
            List<Command> Result = new List<Command>();

            Command BarItem = new Command() { TitleKey = "File Kai Ase Re File" };
            Result.Add(BarItem);
            BarItem.AddUi("New");
            BarItem.AddUi("Open");
            BarItem.AddUi("Exit");

            BarItem = new Command() { TitleKey = "Edit kai Edit" };
            Result.Add(BarItem);
            BarItem.AddUi("Cut");
            BarItem.AddUi("Copy");
            BarItem.AddUi("Paste");
            BarItem.AddUi("Delete");

            BarItem = new Command() { TitleKey = "View Ki Apanw Tourla" };
            Result.Add(BarItem);
            BarItem.AddUi("Document");
            BarItem.AddUi("Image");
            BarItem.AddUi("Table");
            BarItem.AddUi("Window");

            BarItem = new Command() { TitleKey = "Project Apisteyto" };
            Result.Add(BarItem);
            BarItem.AddUi("Run");
            BarItem.AddUi("Debug");

            BarItem = new Command() { TitleKey = "Extensions" };
            Result.Add(BarItem);
            BarItem.AddUi("One");
            BarItem.AddUi("Two");
            BarItem.AddUi("Three");

 

            return Result.ToArray();
        }
    }
}
