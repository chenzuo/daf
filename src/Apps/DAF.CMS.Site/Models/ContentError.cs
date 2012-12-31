using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DAF.CMS.Site.Models
{
    [Table("cms_ContentError")]
    public class ContentError
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string ErrorId { get; set; }
        [StringLength(50)]
        public string ContentId { get; set; }
        [StringLength(500)]
        public string Url { get; set; }
        [StringLength(500)]
        public string ErrorInfo { get; set; }
        [StringLength(50)]
        public string Reporter { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public DateTime? ReportTime { get; set; }
        [StringLength(500)]
        public string Response { get; set; }
        [StringLength(50)]
        public string Responser { get; set; }
        public DateTime? ResponseTime { get; set; }
    }
}