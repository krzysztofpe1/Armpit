using System.ComponentModel.DataAnnotations;

namespace WebServer.DataModels.Account;

public class AuthCredentials
{
    [Required]
    [Display(Name = "User name")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
