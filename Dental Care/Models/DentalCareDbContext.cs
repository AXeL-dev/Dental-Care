using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Dental_Care.Models
{
    public class DentalCareDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Rdv> Rdvs { get; set; }

        public DbSet<PatientForm> PatientForms { get; set; }

        public DbSet<Dents> Dents { get; set; }

        public DbSet<Consultation> Consultations { get; set; }

        public DbSet<Ordonance> Ordonances { get; set; }

        public DbSet<Facture> Factures { get; set; }

        public DbSet<Contact> Contacts { get; set; }
    }
}