using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
    public class BookType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }

        public BookType()
        { }
    }
}