using Microsoft.AspNetCore.Identity;

namespace DevConnect.Models;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
}