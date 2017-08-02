using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Care.Models
{
    [Table("PatientForm")] // Table name
    public class PatientForm
    {
        // Propriétés
        [Key] // Primary key
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        [Display(Name = "Nom du patient")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(1)]
        [Display(Name = "Sexe")]
        public string Sexe { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(20)]
        [Display(Name = "Téléphone")]
        public string Tel { get; set; }

        [Display(Name = "Date de création")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreationDate { get; set; }

        [ForeignKey("Dents")]
        [Display(Name = "Dents")]
        public int DentsId { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Associé à")]
        public int UserId { get; set; }

        // This will keep track of the user
        public virtual User User { get; set; }

        // This will keep track of the dents
        public virtual Dents Dents { get; set; }

        // This is to maintain the many consultations associated with a PatientForm entity
        public virtual ICollection<Consultation> Consultations { get; set; }
    }
}