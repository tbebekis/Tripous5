using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using WebDesk.AspNet;

namespace WebDesk.Models
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
