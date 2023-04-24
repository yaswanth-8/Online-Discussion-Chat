using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityDiscussion.Models
{
    public class Message
    {
        [Key]
        public int MesId { get; set; }

        public int MemId { get; set; }

        [ForeignKey("MemId")]
        public virtual Member Member { get; set; }

        public string Chat { get; set; }
    }
}
