using Microsoft.EntityFrameworkCore;
using Animou.Business.Models;
using Animou.Business.Interfaces;

namespace Animou.Data.Context
{
    public class AnimouContext : DbContext, IUnitOfWork
    {
        public AnimouContext(DbContextOptions<AnimouContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Dislike> Dislikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(50)");


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimouContext).Assembly);
        }

        public async Task<bool> Commit() => await base.SaveChangesAsync() > 0;
    }
}
