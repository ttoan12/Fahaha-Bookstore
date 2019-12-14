using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class BookStoreInfo
    {
        [Key]
        public int id { get; set; }
    }
}