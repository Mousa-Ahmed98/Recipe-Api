using System;

namespace Core.Entities
{
    public class Follow
    {
        public int Id { get; set; }
        public string FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }
        public string FolloweeId { get; set; }
        public ApplicationUser Followee { get; set; }
        public DateTime FollowDate { get; set; } = DateTime.Now;
    }
}