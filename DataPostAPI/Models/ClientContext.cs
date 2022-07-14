using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Models
{
    public class ClientContext :DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options) : base(options)
        {

        }
  

        public DbSet<Client> Client { get; set; }
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<Camera> cameras { get; set; }
        public DbSet<PostedDataModel> postedDatas { get; set; }
        public DbSet<AdminModel> Admins { get; set; }


        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-P944USQ\SQLEXPRESS01;Initial Catalog=NewDB;Integrated Security=True;");
        }
    }
}
