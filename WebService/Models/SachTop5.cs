using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCuaHangSach.Models
{
    public class SachTop5
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public string name_author { get; set; }

        public double price { get; set; }

        public double price_discount { get; set; }

        public double discount { get; set; }

        public string img { get; set; }
    }
}