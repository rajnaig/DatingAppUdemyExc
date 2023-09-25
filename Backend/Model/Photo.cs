using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model
{
    [Table("Photos")]
    public class Photo
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public string? AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public Photo()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}