using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Care.Models
{
    [Table("Rdv")] // Table name
    public class Rdv
    {
        // Propriétés
        [Key] // Primary key
        public int Id { get; set; }

        [Display(Name = "Numéro du Rdv")]
        public string Ref { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        [Display(Name = "Nom du bénéficiaire")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(1)]
        [Display(Name = "Sexe")]
        public string Sexe { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(20)]
        [Display(Name = "Téléphone")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Date du rendez-vous")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Etat")]
        public int State { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Créé par")]
        public int UserId { get; set; }

        // This will keep track of the user
        public virtual User User { get; set; }
    }
}