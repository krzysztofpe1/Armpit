using System.ComponentModel.DataAnnotations;
using WebServer.DataModels.Account;

namespace WebServer.DatabaseManagement.Auth;

public class AccountInformation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public AccountType Type { get; set; } = AccountType.OutsideUser;

}
