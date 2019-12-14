using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public virtual List<Book> Books { get; set; }
    }
}