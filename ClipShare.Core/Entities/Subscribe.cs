namespace ClipShare.Core.Entities
{
    public class Subscribe
    {
        public int AppUserId { get; set; }
        public int ChannelId { get; set; }

        //Navigations
        public AppUser AppUser { get; set; }
        public Channel Channel { get; set; }
    }
}
