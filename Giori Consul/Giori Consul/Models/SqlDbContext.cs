namespace Giori_Consul.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SqlDbContext : DbContext
    {
        public SqlDbContext()
            : base("name=SqlDbContext")
        {
        }
        public virtual DbSet<Medico> Medicos { get; set; }
        public virtual DbSet<Secretaria> Secretarias { get; set; }
        public virtual DbSet<Usuario> Users { get; set; }
        public virtual DbSet<Paciente> Pacientes { get; set; }
        public virtual DbSet<Medicamento> Medicamentos { get; set; }
        public virtual DbSet<ReceitaMedica> ReceitaMedicas { get; set; }
        public virtual DbSet<Consulta> Consultas { get; set; }
        public virtual DbSet<Exames> Exames { get; set; }
        public virtual DbSet<ItensReceita> ItensReceita { get; set; }
        public virtual DbSet<ExamesRealizados> ExamesRealizados { get; set; }
        public virtual DbSet<HistoricoExame> HistoricoExames { get; set; }
        public virtual DbSet<HistoricoExamesRealizado> HistoricoExamesRealizados { get; set; }
        public virtual DbSet<HistoricoMedicamento> HistoricoMedicamentos { get; set; }
        public virtual DbSet<TipoExame> TipoExames { get; set; }
        public DbSet<HistoricosClinicos> HistoricosClinicos { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exames>()
              .Property(e => e.ExamesSolicitados)
              .IsUnicode(false);
            modelBuilder.Entity<Exames>()
               .HasMany(e => e.ExamesRealizados)
               .WithRequired(e => e.Exames)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Exames>()
                .HasMany(e => e.HistoricoExames)
                .WithRequired(e => e.Exames)
                .HasForeignKey(e => e.IDExames)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExamesRealizados>()
                .HasMany(e => e.HistoricoExamesRealizados)
                .WithRequired(e => e.ExamesRealizados)
                .HasForeignKey(e => e.IDExamesRealizados)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoricosClinicos>()
                .HasMany(e => e.HistoricoExames)
                .WithRequired(e => e.HistoricosClinicos)
                .HasForeignKey(e => e.IDHistoricoClinico)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoricosClinicos>()
                .HasMany(e => e.HistoricoExamesRealizados)
                .WithRequired(e => e.HistoricosClinicos)
                .HasForeignKey(e => e.IDHistoricoClinico)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoricosClinicos>()
                .HasMany(e => e.HistoricoMedicamentos)
                .WithRequired(e => e.HistoricosClinicos)
                .HasForeignKey(e => e.IDHistoricoClinico)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Medicos)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.IDUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Secretarias)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.IDUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Medicamento>()
                   .HasMany(e => e.ItensReceita)
                   .WithRequired(e => e.Medicamentos)
                   .HasForeignKey(e => e.IDMedicamento)
                   .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReceitaMedica>()
                .HasMany(e => e.ItensReceita)
                .WithRequired(e => e.ReceitaMedica)
                .HasForeignKey(e => e.IDReceita)
                .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<Giori_Consul.Models.EnvioEmail> EnvioEmails { get; set; }
    }
}
