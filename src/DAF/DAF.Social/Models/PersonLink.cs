using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Social.Models
{
    [Table("sns_PersonLink")]
    public class PersonLink
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string PersonId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string LinkPersonId { get; set; }
        public PersonLinkType LinkType { get; set; }
        [StringLength(50)]
        public string LinkRemark { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Me { get; set; }

        [ForeignKey("LinkPersonId")]
        public virtual Person LinkPerson { get; set; }
    }
}
