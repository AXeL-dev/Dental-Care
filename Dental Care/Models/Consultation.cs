using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Care.Models
{
    [Table("Consultation")] // Table name
    public class Consultation
    {
        // Propriétés
        [Key] // Primary key
        public int Id { get; set; }

        [Display(Name = "Référence Consultation")]
        public string Ref { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Date de consultation")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Contenu")]
        public string Contenu { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Antecedant")]
        public string Antecedant { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Traitement")]
        public string Traitement { get; set; }

        [ForeignKey("PatientForm")]
        [Display(Name = "Patient")]
        public int PatientFormId { get; set; }

        // This will keep track of the PatientForm
        public virtual PatientForm PatientForm { get; set; }

        // This is to maintain the many Ordonances associated with a PatientForm entity
        public virtual ICollection<Ordonance> Ordonances { get; set; }

        // This is to maintain the many Factures associated with a PatientForm entity
        public virtual ICollection<Facture> Factures { get; set; }
    }
}