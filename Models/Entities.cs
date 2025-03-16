using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        
        [StringLength(1000)]
        public string Description { get; set; }
        
        [StringLength(500)]
        public string AvatarUrl { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }
        
        // Skills as comma-separated string (could be expanded to a separate entity if needed)
        [StringLength(500)]
        public string Skills { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        
        // Subscriptions (as follower and as being followed)
        [InverseProperty("Follower")]
        public virtual ICollection<Subscription> Following { get; set; }
        
        [InverseProperty("Following")]
        public virtual ICollection<Subscription> Followers { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set
            {
                var split = value.Split(' ');
                FirstName = split[0];
                LastName = split[1];
            }
        }
    }
    
    [Index(nameof(ProfileId), nameof(CreatedAt))]
    public class Post
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ProfileId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("ProfileId")]
        public virtual Profile Author { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }
        
        public virtual ICollection<Like> Likes { get; set; }
    }
    
    [Index(nameof(PostId), nameof(CreatedAt))]
    [Index(nameof(ProfileId), nameof(CreatedAt))]
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PostId { get; set; }
        
        [Required]
        public int ProfileId { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        
        [ForeignKey("ProfileId")]
        public virtual Profile Author { get; set; }
    }
    
    [Index(nameof(FollowerId), nameof(FollowingId), IsUnique = true)]
    [Index(nameof(FollowerId))]
    [Index(nameof(FollowingId))]
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int FollowerId { get; set; }
        
        [Required]
        public int FollowingId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("FollowerId")]
        public virtual Profile Follower { get; set; }
        
        [ForeignKey("FollowingId")]
        public virtual Profile Following { get; set; }
    }
    
    [Index(nameof(PostId), nameof(ProfileId), IsUnique = true)]
    [Index(nameof(ProfileId))]
    public class Like
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PostId { get; set; }
        
        [Required]
        public int ProfileId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        
        [ForeignKey("ProfileId")]
        public virtual Profile Author { get; set; }
    }
    
    // DbContext class to configure the entities
    public class DevConnectDbContext : DbContext
    {
        public DevConnectDbContext(DbContextOptions<DevConnectDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Like> Likes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure the one-to-many relationship between Profile and Posts
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Posts)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure the one-to-many relationship between Profile and Comments
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Author)
                .HasForeignKey(c => c.ProfileId)
                .OnDelete(DeleteBehavior.NoAction); // Prevents circular cascade delete
            
            // Configure the one-to-many relationship between Post and Comments
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure the one-to-many relationship between Profile and Likes
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Likes)
                .WithOne(l => l.Author)
                .HasForeignKey(l => l.ProfileId)
                .OnDelete(DeleteBehavior.NoAction); // Prevents circular cascade delete
            
            // Configure the one-to-many relationship between Post and Likes
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithOne(l => l.Post)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure the unique constraint for Subscription
            modelBuilder.Entity<Subscription>()
                .HasIndex(s => new { s.FollowerId, s.FollowingId })
                .IsUnique();
            
            // Configure the unique constraint for Like
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.PostId, l.ProfileId })
                .IsUnique();
        }
    }
}