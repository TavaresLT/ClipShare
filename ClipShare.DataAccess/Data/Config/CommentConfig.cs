using ClipShare.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClipShare.DataAccess.Data.Config
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => new { x.AppUserId, x.VideoId });
            
            builder.HasOne(x => x.AppUser)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(x => x.Video).WithMany(x => x.Comments)
                .HasForeignKey(x => x.VideoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
