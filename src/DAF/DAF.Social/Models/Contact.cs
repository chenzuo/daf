using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Social.Models
{
    [Table("sns_Contact")]
    public class Contact
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string ContactType { get; set; }
        [StringLength(50)]
        public string ValidationRegex { get; set; }
    }
}
