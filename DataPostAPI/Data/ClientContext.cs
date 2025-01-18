using DataPostAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataPostAPI.Data
{
    public class ClientContext : DbContext
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
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=ASDGHALB\SQLEXPRESS;Database=DataPostDB;TrustServerCertificate=True;Integrated Security=True;");
            }
        }
    }
}
