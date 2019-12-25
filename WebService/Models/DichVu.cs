using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCuaHangSach.Models
{
    public class DichVu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }

        public string nameDV { get; set; }

        public double priceDv { get; set; }
    }
}