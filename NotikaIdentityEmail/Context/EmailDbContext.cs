using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Entities;

namespace NotikaIdentityEmail.Context;

public class EmailDbContext : IdentityDbContext<AppUser>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=ATSTRNB133;Initial Catalog=NotikaEmailDb;User Id=sa;Password=CanATS2025.;Trust Server Certificate=true;");
    }
}