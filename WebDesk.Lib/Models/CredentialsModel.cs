﻿namespace WebLib.Models
{
    public class CredentialsModel
    {
        [RequiredEx]
        [Title("UserId")]
        public string UserId { get; set; }

        [RequiredEx]
        [DataType(DataType.Password)]
        [Title("Password")]
        public string Password { get; set; }
    }
}
