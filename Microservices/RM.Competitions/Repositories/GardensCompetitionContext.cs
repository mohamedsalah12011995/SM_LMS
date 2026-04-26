using System;
using Microsoft.EntityFrameworkCore;
using RM.Models.Competitions;

#nullable disable

namespace RM.Competitions.Repositories
{
    public partial class GardensCompetitionContext : DbContext
    {

        public GardensCompetitionContext(DbContextOptions<GardensCompetitionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }
        public virtual DbSet<Competitor> Competitors { get; set; }
        public virtual DbSet<CompetitorsType> CompetitorsTypes { get; set; }
        public virtual DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Competitor)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.CompetitorId)
                    .HasConstraintName("FK_Attachments_Competitors");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Attachments_AttachmentType");
            });

            modelBuilder.Entity<AttachmentType>(entity =>
            {
                entity.ToTable("AttachmentType");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Competitor>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CandidatedDate).HasColumnType("datetime");

                entity.Property(e => e.ComplateAttachDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Competitors)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Competitors_CompetitorsType");
            });

            modelBuilder.Entity<CompetitorsType>(entity =>
            {
                entity.ToTable("CompetitorsType");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Competitor)
                    .WithMany(p => p.TeamMembers)
                    .HasForeignKey(d => d.CompetitorId)
                    .HasConstraintName("FK_TeamMembers_Competitors");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
