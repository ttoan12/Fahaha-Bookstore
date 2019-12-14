using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebService.Models
{
    public class Role
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public string Description { get; set; }

        public Role()
        { }
    }
}