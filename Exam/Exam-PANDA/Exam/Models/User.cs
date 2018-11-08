using System.Collections.Generic;
using Exam.Models;
using Exam.Models.Enums;
using PANDA.Models;

namespace PANDA.Models
{
    public class User
    {
        public User()
        {
            OrderedPackages = new List<Package>();
            ReceiptsRecieved = new List<Receipt>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public virtual ICollection<Package> OrderedPackages { get; set; }

        public virtual ICollection<Receipt> ReceiptsRecieved { get; set; }
    }
}
