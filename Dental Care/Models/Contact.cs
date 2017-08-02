using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Care.Models
{
    public class Contact
    {
        // Propriétés
        [Key] // Primary key
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Nom")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sujet")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
    }
}