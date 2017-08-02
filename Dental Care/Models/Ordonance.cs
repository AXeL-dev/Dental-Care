using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Care.Models
{
    [Table("Ordonance")] // Table name
    public class Ordonance
    {
        // Propriétés
        [Key] // Primary key
        public int Id { get; set; }

        [Display(Name = "Référence Ordonance")]
        public string Ref { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Date d'ordonance")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Contenu")]
        public string Contenu { get; set; }

        [ForeignKey("Consultation")]
        [Display(Name = "Consultation")]
        public int ConsultationId { get; set; }

        // This will keep track of the Consultation
        public virtual Consultation Consultation { get; set; }
    }
}