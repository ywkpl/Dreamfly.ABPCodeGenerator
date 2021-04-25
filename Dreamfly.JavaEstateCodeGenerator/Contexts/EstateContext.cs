using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Dreamfly.JavaEstateCodeGenerator.MariadbModels;

#nullable disable

namespace Dreamfly.JavaEstateCodeGenerator.Contexts
{
    public partial class EstateContext : DbContext
    {
        public EstateContext()
        {
        }

        public EstateContext(DbContextOptions<EstateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SysCode> SysCodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string mySqlConnection = "server=210.241.233.140;port=3306;database=estate;user=user;password=1qaz2wsx";
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysCode>(entity =>
            {
                entity.ToTable("SysCode");

                entity.HasIndex(e => e.Pid, "FK_sysCode_pid");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasColumnType("varchar(30)")
                    .HasColumnName("code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("companyId");

                entity.Property(e => e.CreationTime)
                    .HasMaxLength(6)
                    .HasColumnName("creationTime");

                entity.Property(e => e.CreatorUserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("creatorUserId");

                entity.Property(e => e.DeleterUserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("deleterUserId");

                entity.Property(e => e.DeletionTime)
                    .HasMaxLength(6)
                    .HasColumnName("deletionTime");

                entity.Property(e => e.IsDefault)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isDefault");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isDeleted")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.LastModificationTime)
                    .HasMaxLength(6)
                    .HasColumnName("lastModificationTime");

                entity.Property(e => e.LastModifierUserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("lastModifierUserId");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(50)")
                    .HasColumnName("name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Ord)
                    .HasColumnType("int(11)")
                    .HasColumnName("ord");

                entity.Property(e => e.Pid)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("pid");

                entity.Property(e => e.Rank)
                    .HasColumnType("int(11)")
                    .HasColumnName("rank");

                entity.Property(e => e.TenantId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("tenantId");

                entity.HasOne(d => d.PidNavigation)
                    .WithMany(p => p.InversePidNavigation)
                    .HasForeignKey(d => d.Pid)
                    .HasConstraintName("FK_sysCode_pid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
