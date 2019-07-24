using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestyMcTestFaceAuth.Models
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
