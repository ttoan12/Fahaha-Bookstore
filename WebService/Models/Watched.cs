using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class Watched
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }

        [ForeignKey("Account")]
        public System.Nullable<int> id_account { get; set; }

        [ForeignKey("Book")]
        public int id_book { get; set; }


        public Account Account { get; set; }

        public Book Book { get; set; }
    }
}