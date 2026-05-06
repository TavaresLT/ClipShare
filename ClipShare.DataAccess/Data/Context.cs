using ClipShare.Core.Entities;
using ClipShare.DataAccess.Data.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClipShare.DataAccess.Data
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Category> Category { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Video> Video { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            // Maneira rapida de chamar todas as configurações de entidades, mas pode ser menos performática em projetos maiores
            //$ modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Maneira mais explícita de chamar as configurações, garantindo melhor performance e controle sobre quais configurações são aplicadas
            modelBuilder.ApplyConfiguration(new CommentConfig());
            modelBuilder.ApplyConfiguration(new LikeDislikeConfig());
            modelBuilder.ApplyConfiguration(new SubscribeConfig());
        }


    }
}
