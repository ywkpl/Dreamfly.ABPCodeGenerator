using System;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace Dreamfly.JavaEstateCodeGenerator
{
    public partial class ADJUSTDBContext : DbContext
    {
        public ADJUSTDBContext()
        {
        }

        public ADJUSTDBContext(DbContextOptions<ADJUSTDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HousingFieldSettings1> HousingFieldSettings1 { get; set; }
        public virtual DbSet<HousingTableSettings> HousingTableSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(SettingHelper.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<HousingFieldSettings1>(entity =>
            {
                entity.ToTable("Housing_FieldSettings1");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addtodb)
                    .HasMaxLength(1)
                    .HasColumnName("addtodb")
                    .IsFixedLength(true);

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Del)
                    .HasMaxLength(1)
                    .HasColumnName("del")
                    .IsFixedLength(true);

                entity.Property(e => e.Fielddesc)
                    .HasMaxLength(50)
                    .HasColumnName("fielddesc");

                entity.Property(e => e.Fieldname)
                    .HasMaxLength(50)
                    .HasColumnName("fieldname");

                entity.Property(e => e.Memo)
                    .HasMaxLength(500)
                    .HasColumnName("memo");

                entity.Property(e => e.NoUser)
                    .HasMaxLength(20)
                    .HasColumnName("no_user");

                entity.Property(e => e.Property)
                    .HasMaxLength(50)
                    .HasColumnName("property");

                entity.Property(e => e.Selector)
                    .HasMaxLength(500)
                    .HasColumnName("selector");

                entity.Property(e => e.Seq).HasColumnName("seq");

                entity.Property(e => e.Tablename)
                    .HasMaxLength(50)
                    .HasColumnName("tablename");

                entity.Property(e => e.TablesettingsId).HasColumnName("Tablesettings_id");
            });

            modelBuilder.Entity<HousingTableSettings>(entity =>
            {
                entity.ToTable("Housing_TableSettings");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Addtodb)
                    .HasMaxLength(1)
                    .HasColumnName("addtodb")
                    .IsFixedLength(true);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Del)
                    .HasMaxLength(1)
                    .HasColumnName("del")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Memo)
                    .HasMaxLength(300)
                    .HasColumnName("memo");

                entity.Property(e => e.NoUser)
                    .HasMaxLength(20)
                    .HasColumnName("no_user");

                entity.Property(e => e.Project)
                    .HasMaxLength(20)
                    .HasColumnName("project");

                entity.Property(e => e.Protype)
                    .HasMaxLength(50)
                    .HasColumnName("protype");

                entity.Property(e => e.Tablename)
                    .HasMaxLength(50)
                    .HasColumnName("tablename");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
