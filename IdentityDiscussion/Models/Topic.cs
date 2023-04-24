using System.ComponentModel.DataAnnotations;

namespace IdentityDiscussion.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }

        public string TopicName { get; set; }
    }
}
