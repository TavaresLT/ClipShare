using ClipShare.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClipShare.DataAccess.Data.Config
{
    public class LikeDislikeConfig : IEntityTypeConfiguration<LikeDislike>
    {
        public void Configure(EntityTypeBuilder<LikeDislike> builder) 
        {
            builder.HasKey(ld => new { ld.AppUserId, ld.VideoId });
            
            builder.HasOne(ld => ld.AppUser)
                .WithMany(ap => ap.LikesDislikes)
                .HasForeignKey(ld => ld.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(ld => ld.Video)
                .WithMany(v => v.LikesDeslikes)
                .HasForeignKey(ld => ld.VideoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
