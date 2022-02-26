using System.ComponentModel.DataAnnotations;

namespace AnalyticsServer.Authentication
{
    public class UserDto 
    {
        [Key]
        public string UserName { get; set;} = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
