using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CodeFirsExample.Model
{
    public class ExampleContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDetails> UserDetailses { get; set; }
        public virtual DbSet<Temperature> Temperatures { get; set; }
    }
}
