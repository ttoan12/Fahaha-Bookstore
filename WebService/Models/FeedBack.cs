using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCuaHangSach.Models
{
    public class FeedBack
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }

        public string email { get; set; }

        public string content { get; set; }

        public double mark { get; set; }
    }
}