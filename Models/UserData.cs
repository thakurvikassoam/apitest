using System.ComponentModel.DataAnnotations;

namespace CFAWebApi.Models
{
    public class UserData
    {
        [Key]
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Role
        {
            get;
            set;
        }
        public string EmailAddress
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
    }
}
