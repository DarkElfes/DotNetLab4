using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Server.Models;
using Server.Models.Chats;
using Server.Models.Messages;

namespace Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<BaseChat> BaseChats { get; set; }
    public DbSet<PersonalChat> PersonalChats { get; set; }
    public DbSet<GroupChat> GroupChats { get; set; }

    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseChat>().UseTphMappingStrategy();

        modelBuilder.Entity<GroupChat>()
            .HasOne(b => b.Owner)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

       /* modelBuilder.Entity<BaseChat>()
            .HasMany(x => x.Users)
            .WithMany(y => y.Chats)
            .UsingEntity(j => j.ToTable("BaseChatApplicationUser"));*/

        /*  modelBuilder.Entity<GroupChat>()
              .HasMany(x => x.Users)
              .WithMany(y => y.GroupChats)
              .UsingEntity(j => j.ToTable("GroupChatApplicationUser"));

          modelBuilder.Entity<PersonalChat>()
              .HasMany(x => x.Users)
              .WithMany(y => y.PersonalChats)
              .UsingEntity(j => j.ToTable("PersonalChatApplicationUser"));*/


        base.OnModelCreating(modelBuilder);
    }

}
