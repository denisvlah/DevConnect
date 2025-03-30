using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Models;

public class AspIdentityContext : IdentityDbContext<User>
{
    public AspIdentityContext(DbContextOptions<AspIdentityContext> options) : base(options)
    {
    }
}