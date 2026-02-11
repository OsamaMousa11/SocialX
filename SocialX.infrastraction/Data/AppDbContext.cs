using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialX.Core.Domain.Entites;
using SocialX.Core.Domain.IdentityEntites;
using SocialX.Core.storeCore.Domain.IdentityEntites;
using System.Reflection;

namespace SocialX.Infrastructure.Data
{
    public class AppDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

 
        public DbSet<Profile> Profiles => Set<Profile>();
        public DbSet<Tweet> Tweets => Set<Tweet>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Attachment> Attachments => Set<Attachment>();
        public DbSet<Notification> Notifications => Set<Notification>();

        public DbSet<Follow> Follows => Set<Follow>();

    

        public DbSet <Bookmark>Bookmarks => Set<Bookmark>();

        public DbSet<Hashtag> Hashtags => Set<Hashtag>();

        public DbSet<TweetHashtag> TweetHashtags => Set<TweetHashtag>();

        public DbSet<Mention> Mentions => Set<Mention>();

        public DbSet<UserConnection> UserConnections => Set<UserConnection>();





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

          
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

         
      
        }
    }
}
