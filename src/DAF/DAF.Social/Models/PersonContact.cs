using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Social.Models
{
    [Table("sns_PersonContact")]
    public class PersonContact
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string PersonId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string ContactType { get; set; }
        public string Detail { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
        [ForeignKey("ContactType")]
        public virtual Contact Contact { get; set; }
    }
}
