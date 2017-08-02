using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Care.Models
{
    [Table("Dents")] // Table name
    public class Dents
    {
        // Propriétés
        [Key] // Primary key
        public int Id { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }
    }
}