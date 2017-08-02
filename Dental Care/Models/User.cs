using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental_Care.Models
{
    [Table("User")] // Table name
    public class User
    {
        // Propriétés
        [Key] // Primary key
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Mot de passe")]
        public string Pwd { get; set; }

        [Required(ErrorMessage = "*")]
        [Index(IsUnique = true)]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MaxLength(20)]
        [Display(Name = "Téléphone")]
        public string Tel { get; set; }

        [Display(Name = "Adresse")]
        public string Address { get; set; }

        [Display(Name = "Administrateur")]
        public bool isAdmin { get; set; }

        [Display(Name = "Dentiste")]
        public bool isDentist { get; set; }

        // This is to maintain the many rdvs associated with a user entity
        public virtual ICollection<Rdv> Rdvs { get; set; }

        // This is to maintain the many patientForms associated with a user entity
        public virtual ICollection<PatientForm> PatientForms { get; set; }
    }
}