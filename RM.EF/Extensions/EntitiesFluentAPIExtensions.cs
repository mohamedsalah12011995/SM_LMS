using Microsoft.EntityFrameworkCore;


namespace RM.Models.Extensions
{
    public static class EntitiesFluentAPIExtensions
    {
        public static void AdminMenuConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminMenu>(entity =>
            {
                entity.ToTable("AdminMenu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AdminMenus)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_AdminMenu_Entities");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_AdminMenu_AdminMenu");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.AdminMenus)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_AdminMenu_References");

                entity.HasOne(d => d.ReferencesMajor)
                    .WithMany(p => p.AdminMenus)
                    .HasForeignKey(d => d.ReferencesMajorId)
                    .HasConstraintName("FK_AdminMenu_ReferencesMajor");
            });
        }

        public static void AdvertisementConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advertisement>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.AdvertisementActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_Advertisements_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AdvertisementCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Advertisements_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AdvertisementDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Advertisements_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Advertisements)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Advertisements_Entities");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Advertisements)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Advertisements_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.AdvertisementUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Advertisements_UpdatedBy");

                entity.HasOne(d => d.Region)
                 .WithMany(p => p.AdvertisementRegions)
                 .HasForeignKey(d => d.RegionId)
                 .HasConstraintName("FK_Advertisements_Region");

            });
        }

        public static void AwardConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AmanahAward>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.AmanahAwardActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_AmanahAwards_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AmanahAwardCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_AmanahAwards_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AmanahAwardDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_AmanahAwards_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AmanahAwards)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_AmanahAwards_Entities");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.AmanahAwards)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_AmanahAwards_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.AmanahAwardUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_AmanahAwards_UsersUpdatedBy");
            });
        }

        public static void ArticleConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                        .WithMany(p => p.ArticleActivatedByNavigations)
                        .HasForeignKey(d => d.ActivatedBy)
                        .HasConstraintName("FK_Articles_UsersPublishedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                        .WithMany(p => p.ArticleCreatedByNavigations)
                        .HasForeignKey(d => d.CreatedBy)
                        .HasConstraintName("FK_Articles_UsersCreatedBy");

                entity.HasOne(d => d.Entity)
                        .WithMany(p => p.Articles)
                        .HasForeignKey(d => d.EntityId)
                        .HasConstraintName("FK_Articles_Entities");

                entity.HasOne(d => d.Reference)
                        .WithMany(p => p.Articles)
                        .HasForeignKey(d => d.ReferenceId)
                        .HasConstraintName("FK_Articles_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                        .WithMany(p => p.ArticleUpdatedByNavigations)
                        .HasForeignKey(d => d.UpdatedBy)
                        .HasConstraintName("FK_Articles_Users");
            });
        }

        public static void ArticlesPublishConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticlesPublish>(entity =>
            {
                entity.ToTable("ArticlesPublish");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PublishedDate).HasColumnType("datetime");

                entity.HasOne(d => d.MajorReference)
                    .WithMany(p => p.ArticlesPublishes)
                    .HasForeignKey(d => d.MajorReferenceId)
                    .HasConstraintName("FK_ArticlesPublish_ReferencesMajor");

                entity.HasOne(d => d.PublishedByNavigation)
                    .WithMany(p => p.ArticlesPublishes)
                    .HasForeignKey(d => d.PublishedBy)
                    .HasConstraintName("FK_ArticlesPublish_Users");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.ArticlesPublishes)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_ArticlesPublish_References");
            });
        }

        public static void AttachmentConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Extention).HasMaxLength(5);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Attachments_UsersCreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Attachments_Entities");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_Attachments_MobileApplications");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_Attachments_Multimedias");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Attachments_References");
            });
        }
        public static void BeneficiaryConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beneficiary>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                            .WithMany(p => p.BeneficiaryCreatedByNavigations)
                            .HasForeignKey(d => d.CreatedBy)
                            .HasConstraintName("FK_Beneficiaries_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                            .WithMany(p => p.BeneficiaryDeletedByNavigations)
                            .HasForeignKey(d => d.DeletedBy)
                            .HasConstraintName("FK_Beneficiaries_UsersDeletedBy");
            });
        }
        public static void CommentConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");


                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReplyDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ApprovedByNavigation)
                    .WithMany(p => p.CommentApprovedByNavigations)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("FK_Comments_UsersApprovedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CommentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Comments_UsersCreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Comments_Entities");



                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Comments_References");

                entity.HasOne(d => d.RepliedByNavigation)
                    .WithMany(p => p.CommentRepliedByNavigations)
                    .HasForeignKey(d => d.RepliedBy)
                    .HasConstraintName("FK_Comments_UsersRepliedBy");
            });
        }
        public static void ContactUsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactU>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ComplainId).HasColumnName("ComplainID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.ContactUCreatedByNavigations)
            .HasForeignKey(d => d.CreatedBy)
            .HasConstraintName("FK_ContactUs_CreatedBy");

                entity.HasOne(d => d.Entity)
            .WithMany(p => p.ContactUs)
            .HasForeignKey(d => d.EntityId)
            .HasConstraintName("FK_ContactUs_EntityId");

                entity.HasOne(d => d.ModifiedByNavigation)
            .WithMany(p => p.ContactUModifiedByNavigations)
            .HasForeignKey(d => d.ModifiedBy)
            .HasConstraintName("FK_ContactUs_ModifiedBy");

                entity.HasOne(d => d.Reference)
            .WithMany(p => p.ContactUs)
            .HasForeignKey(d => d.ReferenceId)
            .HasConstraintName("FK_ContactUs_ReferenceId");


            });
        }
        public static void DocumentConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.DocumentActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_Documents_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DocumentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Documents_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.DocumentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Documents_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Documents_Entities");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Documents_Documents");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Documents_References");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Documents_DocumentsType");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.DocumentUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Documents_UsersUpdatedBy");
            });
        }
        public static void DocumentsTypeConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentsType>(entity =>
            {
                entity.ToTable("DocumentsType");

                entity.Property(e => e.Id).HasColumnName("ID");
            });
        }
        public static void EntitiesTypeConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntitiesType>(entity =>
            {
                entity.ToTable("EntitiesType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NameAr).HasMaxLength(255);

                entity.Property(e => e.NameEn).HasMaxLength(255);
            });
        }
        public static void EntityConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BackendIdentity).HasColumnName("backendIdentity");

                entity.Property(e => e.NameAr).HasMaxLength(255);

                entity.Property(e => e.NameEn).HasMaxLength(255);

                entity.HasOne(d => d.Reference)
                            .WithMany(p => p.Entities)
                            .HasForeignKey(d => d.ReferenceId)
                            .HasConstraintName("FK_Entities_References");

                entity.HasOne(d => d.ReferencesMajor)
                            .WithMany(p => p.Entities)
                            .HasForeignKey(d => d.ReferencesMajorId)
                            .HasConstraintName("FK_Entities_ReferencesMajor");

                entity.HasOne(d => d.Type)
                            .WithMany(p => p.Entities)
                            .HasForeignKey(d => d.TypeId)
                            .HasConstraintName("FK_Entities_EntitiesType");

                entity.HasOne(d => d.Parent)
                           .WithMany(p => p.InverseParent)
                           .HasForeignKey(d => d.ParentId)
                           .HasConstraintName("FK_Entities_EntitiesParentId");

            });

        }

        public static void EserviceConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Eservice>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.TitleEn)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.EserviceActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_Eservices_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EserviceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Eservices_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EserviceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Eservices_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Eservices)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Eservices_Entities");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Eservices_EservicesParentId");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Eservices)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Eservices_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EserviceUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Eservices_UsersUpdatedBy");
            });
        }

        public static void ExternalSiteConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExternalSite>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.NameAr).HasMaxLength(500);

                entity.Property(e => e.NameEn).HasMaxLength(500);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.ExternalSiteActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_ExternalSites_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ExternalSiteCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ExternalSites_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ExternalSiteDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_ExternalSites_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.ExternalSites)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ExternalSites_Entities");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_ExternalSites_ExternalSites");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.ExternalSites)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_ExternalSites_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ExternalSiteUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_ExternalSites_UsersUpdatedBy");
            });
        }

        public static void GovServiceConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GovService>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.GovServiceActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_GovServices_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.GovServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.GovServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_GovServices_UsersDeletedBy");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_ParentService");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.GovServices)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_RefernceId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.GovServiceUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_ModifiedBy");
            });
        }
        public static void InitiativesProjectConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InitiativesProject>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InitiativeDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.InitiativesProjectActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_InitiativesProjects_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InitiativesProjectCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_InitiativesProjects_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InitiativesProjectDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_InitiativesProjects_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InitiativesProjects)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_InitiativesProjects_Entities");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.InitiativesProjects)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_InitiativesProjects_References");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.InitiativesProjects)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_InitiativesProjects_InitiativesProjectsType");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InitiativesProjectUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_InitiativesProjects_UsersUpdatedBy");
            });
        }
        public static void InitiativesProjectsBeneficiaryConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InitiativesProjectsBeneficiary>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Beneficiary)
                    .WithMany(p => p.InitiativesProjectsBeneficiaries)
                    .HasForeignKey(d => d.BeneficiaryId)
                    .HasConstraintName("FK_InitiativesProjectsBeneficiaries_Beneficiaries");

                entity.HasOne(d => d.InitiativesProject)
                    .WithMany(p => p.InitiativesProjectsBeneficiaries)
                    .HasForeignKey(d => d.InitiativesProjectId)
                    .HasConstraintName("FK_InitiativesProjectsBeneficiaries_InitiativesProjects");
            });
        }

        public static void InitiativesProjectsTypeConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InitiativesProjectsType>(entity =>
            {
                entity.ToTable("InitiativesProjectsType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            });
        }
        public static void InteractionStatisticConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InteractionStatistic>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsHelpfulCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.NotHelpfulCount).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InteractionStatistics)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_InteractionStatistics_Entities");

                entity.HasOne(d => d.InteractionStatisticsTypeNavigation)
                    .WithMany(p => p.InteractionStatistics)
                    .HasForeignKey(d => d.InteractionStatisticsType)
                    .HasConstraintName("FK_InteractionStatistics_InteractionStatisticsType");


                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.InteractionStatistics)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_InteractionStatistics_References");

                entity.HasOne(d => d.ReferenceMajor)
                    .WithMany(p => p.InteractionStatistics)
                    .HasForeignKey(d => d.ReferenceMajorId)
                    .HasConstraintName("FK_InteractionStatistics_ReferencesMajor");
            });

        }

        public static void InteractionStatisticsTypeConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InteractionStatisticsType>(entity =>
            {
                entity.ToTable("InteractionStatisticsType");

                entity.Property(e => e.Id).HasColumnName("ID");
            });
        }

        public static void InvestmentConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Investment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.LastAdmissionDate).HasColumnType("datetime");

                entity.Property(e => e.LastInquiryDate).HasColumnType("datetime");

                entity.Property(e => e.OpenBidDate).HasColumnType("datetime");

                entity.Property(e => e.OpportunityDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.InvestmentActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_Investments_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvestmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Investments_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvestmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Investments_UsersDeletedBy");

                entity.HasOne(d => d.OpportunityTypeNavigation)
                    .WithMany(p => p.Investments)
                    .HasForeignKey(d => d.OpportunityType)
                    .HasConstraintName("FK_Investments_Type");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvestmentUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Investments_UsersUpdatedBy");
            });
        }

        public static void InvestmentTypeConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvestmentType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });
        }
        public static void JobRoleConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobRole>(entity =>
            {
                entity.ToTable("JobRole");

                entity.Property(e => e.Id).HasColumnName("ID");
            });
        }

        public static void LoginWayConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginWay>(entity =>
            {
                entity.ToTable("LoginWay");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NameAr).HasMaxLength(255);

                entity.Property(e => e.NameEn).HasMaxLength(255);
            });
        }


        public static void MajorLookupConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MajorLookup>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_MajorLookups_MajorLookups");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.MajorLookups)
                    .HasForeignKey(d => d.TypeId)
                    // .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MajorLookups_MajorLookupsType");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.MajorLookups)
                    .HasForeignKey(d => d.ReferenceId)
                    //  .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MajorLookups_References");
            });
        }

        public static void MajorLookupsTypeConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MajorLookupsType>(entity =>
            {
                entity.ToTable("MajorLookupsType");

                entity.Property(e => e.Id).HasColumnName("ID");
            });
        }

        public static void MenuConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsHidden).HasDefaultValueSql("((0))");

                entity.Property(e => e.MenuOrder).HasDefaultValueSql("((100))");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK_Menu_Articles");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Menu_Users");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Menu_Entities");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Menu_MenuParentID");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Menu_References");

                entity.HasOne(d => d.ReferenceMajor)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.ReferenceMajorId)
                    .HasConstraintName("FK_Menu_ReferencesMajor");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Menu_MenuType");
            });
        }

        public static void MenuTypeConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuType>(entity =>
            {
                entity.ToTable("MenuType");

                entity.Property(e => e.Id).HasColumnName("ID");
            });
        }
        public static void MobileApplicationConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MobileApplication>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.MobileApplicationActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_MobileApplications_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.MobileApplicationCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_MobileApplications_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.MobileApplicationDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_MobileApplications_UsersDeletedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.MobileApplicationUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_MobileApplications_UsersUpdatedBy");
            });
        }


        public static void MultimediaConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Multimedia>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.MultimediaActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_Multimedias_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.MultimediaCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Multimedias_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.MultimediaDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Multimedias_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Multimedia)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Multimedias_Entities");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Multimedia)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Multimedias_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.MultimediaUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Multimedias_UsersUpdatedBy");
            });
        }


        public static void NewsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>(entity =>
                {
                    entity.Property(e => e.Id).HasColumnName("ID");

                    entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                    entity.Property(e => e.BriefeContentAr).HasMaxLength(300);

                    entity.Property(e => e.BriefeContentEn).HasMaxLength(300);

                    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                    entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                    entity.Property(e => e.NewsDate).HasColumnType("datetime");

                    entity.Property(e => e.NewsDateH).HasMaxLength(50);

                    entity.Property(e => e.NewsSourceAr).HasMaxLength(50);

                    entity.Property(e => e.OriginalPic).HasMaxLength(300);

                    entity.Property(e => e.ThumpPic).HasMaxLength(50);

                    entity.Property(e => e.TitleAr).HasMaxLength(300);

                    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                    entity.HasOne(d => d.ActivatedByNavigation)
                            .WithMany(p => p.NewsActivatedByNavigations)
                            .HasForeignKey(d => d.ActivatedBy)
                            .HasConstraintName("FK_News_UsersActivatedBy");

                    entity.HasOne(d => d.CreatedByNavigation)
                            .WithMany(p => p.NewsCreatedByNavigations)
                            .HasForeignKey(d => d.CreatedBy)
                            .HasConstraintName("FK_News_UsersCreatedBy");

                    entity.HasOne(d => d.DeletedByNavigation)
                            .WithMany(p => p.NewsDeletedByNavigations)
                            .HasForeignKey(d => d.DeletedBy)
                            .HasConstraintName("FK_News_UsersDeletedBy");

                    entity.HasOne(d => d.Entity)
                            .WithMany(p => p.News)
                            .HasForeignKey(d => d.EntityId)
                            .HasConstraintName("FK_News_Entities");

                    entity.HasOne(d => d.Reference)
                            .WithMany(p => p.News)
                            .HasForeignKey(d => d.ReferenceId)
                            .HasConstraintName("FK_News_References");

                    entity.HasOne(d => d.UpdatedByNavigation)
                            .WithMany(p => p.NewsUpdatedByNavigations)
                            .HasForeignKey(d => d.UpdatedBy)
                            .HasConstraintName("FK_News_UsersUpdatedBy");
                });
        }

        public static void OfficialConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Official>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.PeriodFrom).HasColumnType("date");

                entity.Property(e => e.PeriodTo).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                                .WithMany(p => p.OfficialActivatedByNavigations)
                                .HasForeignKey(d => d.ActivatedBy)
                                .HasConstraintName("FK_Officials_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                                .WithMany(p => p.OfficialCreatedByNavigations)
                                .HasForeignKey(d => d.CreatedBy)
                                .HasConstraintName("FK_Officials_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                                .WithMany(p => p.OfficialDeletedByNavigations)
                                .HasForeignKey(d => d.DeletedBy)
                                .HasConstraintName("FK_Officials_DeletedBy");

                entity.HasOne(d => d.Entity)
                                .WithMany(p => p.Officials)
                                .HasForeignKey(d => d.EntityId)
                                .HasConstraintName("FK_Officials_EntityID");

                entity.HasOne(d => d.Reference)
                                .WithMany(p => p.Officials)
                                .HasForeignKey(d => d.ReferenceId)
                                .HasConstraintName("FK_Officials_Reference");

                entity.HasOne(d => d.UpdatedByNavigation)
                                .WithMany(p => p.OfficialUpdatedByNavigations)
                                .HasForeignKey(d => d.UpdatedBy)
                                .HasConstraintName("FK_Officials_UpdatedBy");
            });
        }

        public static void PartnerConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.ContractDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.RmdepartmentNameAr).HasColumnName("RMDepartmentNameAr");

                entity.Property(e => e.RmdepartmentNameEn).HasColumnName("RMDepartmentNameEn");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.PartnerActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_Partners_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.PartnerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Partners_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.PartnerDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Partners_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Partners_Entities");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Partners_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.PartnerUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Partners_UsersUpdatedBy");
            });
        }
        public static void PermissionLevelConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionLevel>(entity =>
            {
                entity.ToTable("PermissionLevel");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NameAr).HasMaxLength(255);

                entity.Property(e => e.NameEn).HasMaxLength(255);
            });
        }
        public static void QuestionsAnswerConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionsAnswer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QuestionsAnswerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QuestionsAnswers_UsersCreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.QuestionsAnswers)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_QuestionsAnswers_Entities");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.QuestionsAnswers)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_QuestionsAnswers_MobileApplications");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.QuestionsAnswers)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_QuestionsAnswers_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.QuestionsAnswerUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_QuestionsAnswers_UsersUpdatedBy");
            });
        }

        public static void RateConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Rates_Users");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Rates_Entities");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Rates_References");
            });
        }

        public static void ReferenceConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reference>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NameAr).HasMaxLength(255);

                entity.Property(e => e.NameEn).HasMaxLength(255);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ReferenceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_References_UsersCreatedBy");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_References_ReferencesParentId");

                entity.HasOne(d => d.ReferencesMajor)
                    .WithMany(p => p.References)
                    .HasForeignKey(d => d.ReferencesMajorId)
                    .HasConstraintName("FK_References_ReferencesMajor");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ReferenceUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_References_UsersUpdatedBy");
            });
        }

        public static void ReferenceContentConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferenceContent>(entity =>
                {
                    entity.ToTable("ReferenceContent");

                    entity.Property(e => e.Id).HasColumnName("ID");

                    entity.Property(e => e.Email).HasMaxLength(30);

                    entity.Property(e => e.Latitude).HasMaxLength(30);

                    entity.Property(e => e.Longtitude).HasMaxLength(30);

                    entity.Property(e => e.Phone).HasMaxLength(30);

                    entity.Property(e => e.ReferenceId).HasColumnName("ReferenceID");

                    entity.HasOne(d => d.Entity)
                            .WithMany(p => p.ReferenceContents)
                            .HasForeignKey(d => d.EntityId)
                            .HasConstraintName("FK_ReferenceContent_Entities");

                    entity.HasOne(d => d.Reference)
                            .WithMany(p => p.ReferenceContents)
                            .HasForeignKey(d => d.ReferenceId)
                            .HasConstraintName("FK_ReferenceContent_References");
                });
        }

        public static void ReferencesJobRoleConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferencesJobRole>(entity =>
            {
                entity.ToTable("ReferencesJobRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.JobRole)
                                .WithMany(p => p.ReferencesJobRoles)
                                .HasForeignKey(d => d.JobRoleId)
                                .HasConstraintName("FK_ReferencesJobRole_JobRole");

                entity.HasOne(d => d.Reference)
                                .WithMany(p => p.ReferencesJobRoles)
                                .HasForeignKey(d => d.ReferenceId)
                                .HasConstraintName("FK_ReferencesJobRole_References");
            });
        }

        public static void ReferencesMajorConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferencesMajor>(entity =>
{
    entity.ToTable("ReferencesMajor");

    entity.Property(e => e.Id).HasColumnName("ID");

    entity.Property(e => e.NameAr).HasMaxLength(255);

    entity.Property(e => e.NameEn).HasMaxLength(255);

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.ReferencesMajorCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_ReferencesMajor_UsersCreatedBy");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.ReferencesMajors)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_ReferencesMajor_Entities");

    entity.HasOne(d => d.LoginWay)
        .WithMany(p => p.ReferencesMajors)
        .HasForeignKey(d => d.LoginWayId)
        .HasConstraintName("FK_ReferencesMajor_LoginWay");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.ReferencesMajorUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_ReferencesMajor_UpdatedBy");
});
        }

        public static void RoleConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("ID");

    entity.Property(e => e.Name).HasMaxLength(255);

    entity.Property(e => e.NameEn).HasMaxLength(255);

    entity.Property(e => e.ReferenceId).HasColumnName("ReferenceID");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.Roles)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_Roles_References");
});
        }
        public static void RolesPermissionLevelConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<RolesPermissionLevel>(entity =>
{
    entity.ToTable("RolesPermissionLevel");

    entity.Property(e => e.Id).HasColumnName("ID");

    entity.HasOne(d => d.PermissionLevel)
        .WithMany(p => p.RolesPermissionLevels)
        .HasForeignKey(d => d.PermissionLevelId)
        .HasConstraintName("FK_RolesPermissionLevel_PermissionLevel");

    entity.HasOne(d => d.Role)
        .WithMany(p => p.RolesPermissionLevels)
        .HasForeignKey(d => d.RoleId)
        .HasConstraintName("FK_RolesPermissionLevel_Roles");
});
        }
        public static void SessionConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Session>(entity =>
{
    entity.ToTable("Session");

    entity.Property(e => e.Id).HasColumnName("ID");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.LastOperation).HasColumnType("datetime");

    entity.Property(e => e.SystemId).HasColumnName("SystemID");

    entity.HasOne(d => d.User)
        .WithMany(p => p.Sessions)
        .HasForeignKey(d => d.UserId)
        .HasConstraintName("FK_Session_Users");
});
        }

        public static void TagConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });
        }
        public static void TermsAndRegulationConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TermsAndRegulation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                                .WithMany(p => p.TermsAndRegulationActivatedByNavigations)
                                .HasForeignKey(d => d.ActivatedBy)
                                .HasConstraintName("FK_TermsAndRegulations_UsersActivatedBy");

                entity.HasOne(d => d.CreatedByNavigation)
                                .WithMany(p => p.TermsAndRegulationCreatedByNavigations)
                                .HasForeignKey(d => d.CreatedBy)
                                .HasConstraintName("FK_TermsAndRegulations_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                                .WithMany(p => p.TermsAndRegulationDeletedByNavigations)
                                .HasForeignKey(d => d.DeletedBy)
                                .HasConstraintName("FK_TermsAndRegulations_UsersDeletedBy");

                entity.HasOne(d => d.Entity)
                                .WithMany(p => p.TermsAndRegulations)
                                .HasForeignKey(d => d.EntityId)
                                .HasConstraintName("FK_TermsAndRegulations_Entities");

                entity.HasOne(d => d.Parent)
                                .WithMany(p => p.InverseParent)
                                .HasForeignKey(d => d.ParentId)
                                .HasConstraintName("FK_TermsAndRegulations_TermsAndRegulations");

                entity.HasOne(d => d.Reference)
                                .WithMany(p => p.TermsAndRegulations)
                                .HasForeignKey(d => d.ReferenceId)
                                .HasConstraintName("FK_TermsAndRegulations_References");

                entity.HasOne(d => d.UpdatedByNavigation)
                                .WithMany(p => p.TermsAndRegulationUpdatedByNavigations)
                                .HasForeignKey(d => d.UpdatedBy)
                                .HasConstraintName("FK_TermsAndRegulations_UsersUpdatedBy");
            });
        }

        public static void UserConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("ID");

    entity.Property(e => e.BirthDate).HasColumnType("datetime");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

    entity.Property(e => e.IdCardNumber).HasMaxLength(10);

    entity.Property(e => e.ReferenceId).HasColumnName("ReferenceID");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.Users)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_Users_References");

    entity.HasOne(d => d.Role)
        .WithMany(p => p.Users)
        .HasForeignKey(d => d.RoleId)
        .HasConstraintName("FK_Users_Roles");
});
        }

        public static void UsersEntityConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersEntity>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("ID");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.UsersEntities)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_UsersEntities_Entities");

    entity.HasOne(d => d.User)
        .WithMany(p => p.UsersEntities)
        .HasForeignKey(d => d.UserId)
        .HasConstraintName("FK_UsersEntities_Users");
});
        }
        public static void UsersPermissionLevelConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersPermissionLevel>(entity =>
{
    entity.ToTable("UsersPermissionLevel");

    entity.Property(e => e.Id).HasColumnName("ID");

    entity.HasOne(d => d.PermissionLevel)
        .WithMany(p => p.UsersPermissionLevels)
        .HasForeignKey(d => d.PermissionLevelId)
        .HasConstraintName("FK_UsersPermissionLevel_PermissionLevel");

    entity.HasOne(d => d.User)
        .WithMany(p => p.UsersPermissionLevels)
        .HasForeignKey(d => d.UserId)
        .HasConstraintName("FK_UsersPermissionLevel_Users");
});
        }

        public static void VolunteerConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Volunteer>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("ID");

    entity.Property(e => e.AgeId).HasColumnName("AgeID");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

    entity.Property(e => e.EntityId).HasDefaultValueSql("((34))");

    entity.Property(e => e.GenderId).HasColumnName("GenderID");

    entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

    entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

    entity.Property(e => e.QualificationId).HasColumnName("QualificationID");

    entity.Property(e => e.VolunteerFieldId).HasColumnName("VolunteerFieldId");

    entity.Property(e => e.UserId).HasColumnName("UserID");
    entity.Property(e => e.Birthday).HasColumnType("datetime");


    entity.HasOne(d => d.Age)
        .WithMany(p => p.VolunteerAges)
        .HasForeignKey(d => d.AgeId)
        .HasConstraintName("FK_Volunteers_AgeRange");

    entity.HasOne(d => d.District)
        .WithMany(p => p.VolunteerDistricts)
        .HasForeignKey(d => d.DistrictId)
        .HasConstraintName("FK_Volunteers_District");

    entity.HasOne(d => d.Gender)
        .WithMany(p => p.VolunteerGenders)
        .HasForeignKey(d => d.GenderId)
        .HasConstraintName("FK_Volunteers_Gender");

    entity.HasOne(d => d.Qualification)
        .WithMany(p => p.VolunteerQualifications)
        .HasForeignKey(d => d.QualificationId)
        .HasConstraintName("FK_Volunteers_Qualification");

    entity.HasOne(d => d.VolunteerField)
        .WithMany(p => p.VolunteerFields)
        .HasForeignKey(d => d.VolunteerFieldId)
        .HasConstraintName("FK_Volunteers_VolunteerField");
});
        }

        public static void JobAdvertisementConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobAdvertisement>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("ID");

    entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.DeletedDate).HasColumnType("datetime");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");



    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.JobAdvertisementCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_JobAdvertisement_UsersCreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
         .WithMany(p => p.JobAdvertisementUpdatedByNavigations)
         .HasForeignKey(d => d.UpdatedBy)
         .HasConstraintName("FK_JobAdvertisement_UsersUpdatedBy");

    entity.HasOne(d => d.ActivatedByNavigation)
       .WithMany(p => p.JobAdvertisementActivatedByNavigations)
       .HasForeignKey(d => d.ActivatedBy)
       .HasConstraintName("FK_JobAdvertisement_UsersActivatedBy");

    entity.HasOne(d => d.DeletedByNavigation)
     .WithMany(p => p.JobAdvertisementDeletedByNavigations)
     .HasForeignKey(d => d.DeletedBy)
     .HasConstraintName("FK_JobAdvertisement_UsersDeletedBy");

});
        }

        public static void PermitsRequestConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermitsRequest>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");



                entity.HasOne(d => d.CreatedByNavigation)
                            .WithMany(p => p.PermitsRequestCreatedByNavigations)
                            .HasForeignKey(d => d.CreatedBy)
                            .HasConstraintName("FK_PermitRequest_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                             .WithMany(p => p.PermitsRequestUpdatedByNavigations)
                             .HasForeignKey(d => d.UpdatedBy)
                             .HasConstraintName("FK_PermitRequest_UsersUpdatedBy");

                entity.HasOne(d => d.ActivatedByNavigation)
                           .WithMany(p => p.PermitsRequestActivatedByNavigations)
                           .HasForeignKey(d => d.ActivatedBy)
                           .HasConstraintName("FK_PermitRequest_UsersActivatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                         .WithMany(p => p.PermitsRequestDeletedByNavigations)
                         .HasForeignKey(d => d.DeletedBy)
                         .HasConstraintName("FK_PermitRequest_UsersDeletedBy");

                entity.HasOne(d => d.CurrentStepNavigation)
                        .WithMany(p => p.CurrentStepNavigations)
                        .HasForeignKey(d => d.CurrentStep)
                        .HasConstraintName("CurrentStepNavigation");

                entity.HasOne(d => d.NextStepNavigation)
                       .WithMany(p => p.NextStepNavigations)
                       .HasForeignKey(d => d.NextStep)
                       .HasConstraintName("NextStepNavigation");



                //

            });
        }

        public static void PermitActionConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermitAction>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");


                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");



                entity.HasOne(d => d.CreatedByNavigation)
                                .WithMany(p => p.PrintRequestCreatedByNavigations)
                                .HasForeignKey(d => d.CreatedBy)
                                .HasConstraintName("FK_PtintRequest_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                                 .WithMany(p => p.PrintRequestUpdatedByNavigations)
                                 .HasForeignKey(d => d.UpdatedBy)
                                 .HasConstraintName("FK_PtintRequest_UsersUpdatedBy");

                entity.HasOne(d => d.StepNavigation)
                                .WithMany(p => p.StepNavigations)
                                .HasForeignKey(d => d.StepId)
                                .HasConstraintName("StepNavigation");
                //

            });
        }
        public static void PermitWorkSiteConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermitWorkSite>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");


    entity.HasOne(d => d.WorksiteNavigation)
        .WithMany(p => p.PermitWorkSites)
        .HasForeignKey(d => d.WorksiteId)
        .HasConstraintName("FK_PermitsWorkSite_MajorLookUps");
});
        }
        public static void OpenDataConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpenData>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

    entity.Property(e => e.EntityId).HasDefaultValueSql("((58))");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.OpenDataCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_OpenData_CreatedBy");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.OpenDatas)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_OpenData_EntityId");

    entity.HasOne(d => d.ModifiedByNavigation)
        .WithMany(p => p.OpenDataModifiedByNavigations)
        .HasForeignKey(d => d.ModifiedBy)
        .HasConstraintName("FK_OpenData_ModifiedBy");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.OpenDatas)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_OpenData_ReferenceId");

    entity.HasOne(d => d.District)
        .WithMany(p => p.OpenDataDistricts)
        .HasForeignKey(d => d.DistrictId)
        .HasConstraintName("FK_OpenData_District");

    entity.HasOne(d => d.Type)
        .WithMany(p => p.OpenDataTypes)
        .HasForeignKey(d => d.TypeId)
        .HasConstraintName("FK_OpenData_Type");
});
        }

        public static void OpenDataTempConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpenDataTemp>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

    entity.Property(e => e.EntityId).HasDefaultValueSql("((58))");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.OpenDataTempCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_OpenDataTemp_CreatedBy");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.OpenDataTemps)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_OpenDataTemp_EntityId");

    entity.HasOne(d => d.ModifiedByNavigation)
        .WithMany(p => p.OpenDataTempModifiedByNavigations)
        .HasForeignKey(d => d.ModifiedBy)
        .HasConstraintName("FK_OpenDataTemp_ModifiedBy");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.OpenDataTemps)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_OpenDataTemp_ReferenceId");

    entity.HasOne(d => d.District)
        .WithMany(p => p.OpenDataTempDistricts)
        .HasForeignKey(d => d.DistrictId)
        .HasConstraintName("FK_OpenDataTemp_District");

    entity.HasOne(d => d.Type)
        .WithMany(p => p.OpenDataTempTypes)
        .HasForeignKey(d => d.TypeId)
        .HasConstraintName("FK_OpenDataTemp_Type");
});
        }


        public static void OpenDataRequestConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpenDataRequest>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.OpenDataRequestCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_OpenDataRequest_CreatedBy");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.OpenDataRequests)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_OpenDataRequest_EntityId");

    entity.HasOne(d => d.ModifiedByNavigation)
        .WithMany(p => p.OpenDataRequestModifiedByNavigations)
        .HasForeignKey(d => d.ModifiedBy)
        .HasConstraintName("FK_OpenDataRequest_ModifiedBy");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.OpenDataRequests)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_OpenDataRequest_ReferenceId");
});
        }

        public static void OpenDataStatisticsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpenDataStatistics>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("ID");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.OpenDataStatistics)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_OpenDataStatistics_ReferenceId");

    entity.HasOne(d => d.District)
        .WithMany(p => p.OpenDataStatisticsDistricts)
        .HasForeignKey(d => d.DistrictId)
        .HasConstraintName("FK_OpenDataStatistics_District");

    entity.HasOne(d => d.Type)
        .WithMany(p => p.OpenDataStatisticsTypes)
        .HasForeignKey(d => d.TypeId)
        .HasConstraintName("FK_OpenDataStatistics_Type");
});
        }

        public static void FAQConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FAQ>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.FAQCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_FAQ_CreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.FAQUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_FAQ_UpdatedBy");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.FAQs)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_FAQ_ReferenceId");
});
        }
        public static void StatusConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>(entity =>
    {

        entity.HasOne(d => d.MajorStatus)
            .WithMany(p => p.Status)
            .HasForeignKey(d => d.MajorStatusId)
            .HasConstraintName("FK_ContactStatus");


    });
        }
        public static void ActionsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actions>(entity =>
    {

        entity.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.ActionsCreatedByNavigations)
            .HasForeignKey(d => d.CreatedBy)
            .HasConstraintName("FK_UserActions");

        entity.HasOne(d => d.Complain)
           .WithMany(p => p.Actions)
           .HasForeignKey(d => d.ContactId)
           .HasConstraintName("FK_ActionComplain");

        entity.HasOne(d => d.FromUser)
          .WithMany(p => p.ActionsFromUserNavigations)
          .HasForeignKey(d => d.FromUserId)
          .HasConstraintName("FK_ActionFromUser");

        entity.HasOne(d => d.Status)
        .WithMany(p => p.ActionsStatus)
        .HasForeignKey(d => d.StatusId)
        .HasConstraintName("FK_ActionStatus");

        //entity.HasOne(d => d.ToReference)
        //.WithMany(p => p.Actions)
        //.HasForeignKey(d => d.ToReferenceId)
        //.HasConstraintName("FK_ActionToReference");

    });

        }

        public static void SurveyConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Survey>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

    entity.Property(e => e.DeletedDate).HasColumnType("datetime");

    entity.Property(e => e.FromDate).HasColumnType("datetime");

    entity.Property(e => e.ToDate).HasColumnType("datetime");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.SurveyCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_Surveys_UsersCreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.SurveyUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_Surveys_UsersUpdatedBy");

    entity.HasOne(d => d.DeletedByNavigation)
        .WithMany(p => p.SurveyDeletedByNavigations)
        .HasForeignKey(d => d.DeletedBy)
        .HasConstraintName("FK_Surveys_UsersDeletedBy");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.Surveys)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_Surveys_References");
});
        }
        public static void SurveyQuestionConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SurveyQuestion>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.DeletedDate).HasColumnType("datetime");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");


    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.SurveyQuestionCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_Questions_UsersCreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.SurveyQuestionUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_Questions_UsersUpdatedBy");

    entity.HasOne(d => d.DeletedByNavigation)
        .WithMany(p => p.SurveyQuestionDeletedByNavigations)
        .HasForeignKey(d => d.DeletedBy)
        .HasConstraintName("FK_Questions_UsersDeletedBy");

    entity.HasOne(d => d.Survey)
        .WithMany(p => p.SurveyQuestions)
        .HasForeignKey(d => d.SurveyId)
        .HasConstraintName("FK_Questions_Surveys");
});
        }
        public static void SurveyDataSourceConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SurveyDataSource>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

    entity.Property(e => e.DeletedDate).HasColumnType("datetime");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.SurveyDataSourceCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_DataSource_UsersCreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.SurveyDataSourceUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_DataSource_UsersUpdatedBy");

    entity.HasOne(d => d.DeletedByNavigation)
        .WithMany(p => p.SurveyDataSourceDeletedByNavigations)
        .HasForeignKey(d => d.DeletedBy)
        .HasConstraintName("FK_DataSource_UsersDeletedBy");

    entity.HasOne(d => d.SurveyQuestion)
        .WithMany(p => p.SurveyDataSources)
        .HasForeignKey(d => d.QuestionId)
        .HasConstraintName("FK_DataSource_Questions");
});
        }

        public static void SurveyAnswerActionConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SurveyAnswerAction>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                            .WithMany(p => p.SurveyAnswerActionCreatedByNavigations)
                            .HasForeignKey(d => d.CreatedBy)
                            .HasConstraintName("FK_SurveyAnswerAction_UsersCreatedBy");

                entity.HasOne(d => d.Survey)
                            .WithMany(p => p.SurveyAnswerActions)
                            .HasForeignKey(d => d.SurveyId)
                            .HasConstraintName("FK_SurveyAnswerAction_Surveys");
            });
        }

        public static void SurveyQuestionAnswerConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SurveyQuestionAnswer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.HasOne(d => d.SurveyAnswerAction)
                                .WithMany(p => p.SurveyQuestionAnswers)
                                .HasForeignKey(d => d.SurveyAnswerActionId)
                                .HasConstraintName("FK_QuestionAnswers_SurveyAnswerAction");

                entity.HasOne(d => d.Question)
                                .WithMany(p => p.SurveyQuestionAnswers)
                                .HasForeignKey(d => d.QuestionId)
                                .HasConstraintName("FK_QuestionAnswers_Questions");

                entity.HasOne(d => d.DataSource)
                                .WithMany(p => p.SurveyQuestionAnswers)
                                .HasForeignKey(d => d.DataSourceId)
                                .HasConstraintName("FK_QuestionAnswers_DataSource");
            });
        }

        public static void QuestionsRecommendationsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionsRecommendations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.HasOne(d => d.Question)
                                .WithOne(p => p.QuestionsRecommendations)
                                .HasConstraintName("FK_QuestionsRecommendations_Questions");

                entity.HasOne(d => d.LessAverage)
                                .WithMany(p => p.QuestionRecommendationLessAverages)
                                .HasConstraintName("FK_QuestionRecommendationLessAverages");


                entity.HasOne(d => d.Average)
                                .WithMany(p => p.QuestionRecommendationAverages)
                                .HasConstraintName("FK_QuestionRecommendationAverages");

                entity.HasOne(d => d.AboveAverage)
                                .WithMany(p => p.QuestionRecommendationAboveAverages)
                                .HasConstraintName("FK_QuestionRecommendationAboveAverages");
            });
        }

        public static void RecommendationsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recommendations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");


                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RecommendationsCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Recommendations_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.RecommendationsUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Recommendations_UsersUpdatedBy");


                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Recommendations)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Recommendations_References");

            });
        }

        public static void CronSettingsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CronSettings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.HasOne(d => d.Entity)
                .WithMany(p => p.CronSettings)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK_Entity_CronSettings");

                entity.HasOne(d => d.SubEntity)
                .WithMany(p => p.CronSubEntitySettings)
                .HasForeignKey(d => d.SubEntityId)
                .HasConstraintName("FK_Entity_CronSubEntitySettings");

                entity.HasOne(d => d.Survey)
                .WithMany(p => p.CronSettings)
                .HasConstraintName("FK_Survey_CronSettings");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");


                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CronSettingsCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CronSettings_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CronSettingsUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CronSettings_UsersUpdatedBy");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.CronSettings)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_CronSettings_References");
            });
        }


        public static void FeedbackConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasOne(d => d.ContactUs)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.ContactUsId)
                    .HasConstraintName("FK_ContactFeedback");
            });
        }

        public static void ScientificLettersConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScientificLetters>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");
    entity.Property(e => e.CreatedDate).HasColumnType("datetime");
    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
    entity.Property(e => e.ActivatedDate).HasColumnType("datetime");
    entity.Property(e => e.DeletedDate).HasColumnType("datetime");
    entity.Property(e => e.PublishedOn).HasColumnType("datetime");


    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.ScientificLettersCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_ScientificLetters_UsersCreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.ScientificLettersUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_ScientificLetters_UsersUpdatedBy");

    entity.HasOne(d => d.DeletedByNavigation)
        .WithMany(p => p.ScientificLettersDeletedByNavigations)
        .HasForeignKey(d => d.DeletedBy)
        .HasConstraintName("FK_ScientificLetters_UsersDeletedBy");

    entity.HasOne(d => d.ActivatedByNavigation)
        .WithMany(p => p.ScientificLettersActivatedByNavigations)
        .HasForeignKey(d => d.ActivatedBy)
        .HasConstraintName("FK_ScientificLetters_UsersActivatedBy");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.ScientificLetters)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_ScientificLetters_Entities");

    entity.HasOne(d => d.Degree)
        .WithMany(p => p.ScientificLettersDegrees)
        .HasForeignKey(d => d.DegreeId)
        .HasConstraintName("FK_ScientificLetters_Degree");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.ScientificLetters)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_ScientificLetters_References");

});

        }

        public static void OrderConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
{

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.OrderCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_OrderUser");

    entity.HasOne(d => d.DeletedByNavigation)
        .WithMany(p => p.OrderDeletedByNavigations)
        .HasForeignKey(d => d.DeletedBy)
        .HasConstraintName("FK_OrderDeleteUser");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.Order)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_OrderEntity");


    entity.HasOne(d => d.Reference)
        .WithMany(p => p.Order)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_OrderReference");

});
        }

        public static void OrderActionsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderActions>(entity =>
{

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.OrderActionsCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_UserActions");

    entity.HasOne(d => d.Order)
       .WithMany(p => p.Actions)
       .HasForeignKey(d => d.OrderId)
       .HasConstraintName("FK_ActionsOrder");

    entity.HasOne(d => d.Type)
        .WithMany(p => p.OrderActionTypes)
        .HasForeignKey(d => d.TypeId)
        .HasConstraintName("FK_ActionType");


});
        }
        public static void ActionFilesConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionFiles>(entity =>
{

    entity.HasOne(d => d.Action)
        .WithMany(p => p.ActionFiles)
        .HasForeignKey(d => d.ActionId)
        .HasConstraintName("FK_ActionFiles");



});
        }

        public static void LogInformationConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogInformation>(entity =>
            {

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.LogInformations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_OpenData_Entity_Log");

                entity.HasOne(d => d.Reference)
                          .WithMany(p => p.LogInformations)
                          .HasForeignKey(d => d.ReferenceId)
                          .HasConstraintName("FK_OpenData_Reference_Log");

            });
        }

        public static void ExamConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");



                entity.HasOne(d => d.CreatedByNavigation)
                                .WithMany(p => p.ExamCreatedByNavigations)
                                .HasForeignKey(d => d.CreatedBy)
                                .HasConstraintName("FK_Exams_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                                .WithMany(p => p.ExamUpdatedByNavigations)
                                .HasForeignKey(d => d.UpdatedBy)
                                .HasConstraintName("FK_Exams_UsersUpdatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                                .WithMany(p => p.ExamDeletedByNavigations)
                                .HasForeignKey(d => d.DeletedBy)
                                .HasConstraintName("FK_Exams_UsersDeletedBy");

                entity.HasOne(d => d.Reference)
                                .WithMany(p => p.Exams)
                                .HasForeignKey(d => d.ReferenceId)
                                .HasConstraintName("FK_Exams_References");
            });
        }

        public static void ExamQuestionConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamQuestion>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.DeletedDate).HasColumnType("datetime");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");


    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.ExamQuestionCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_ExamsQuestion_UsersCreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.ExamQuestionUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_ExamsQuestion_UsersUpdatedBy");

    entity.HasOne(d => d.DeletedByNavigation)
        .WithMany(p => p.ExamQuestionDeletedByNavigations)
        .HasForeignKey(d => d.DeletedBy)
        .HasConstraintName("FK_ExamsQuestion_UsersDeletedBy");

    entity.HasOne(d => d.Exam)
        .WithMany(p => p.ExamQuestions)
        .HasForeignKey(d => d.ExamId)
        .HasConstraintName("FK_Questions_Exams");
});
        }

        public static void ExamDataSourceConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamDataSource>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

    entity.Property(e => e.DeletedDate).HasColumnType("datetime");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.ExamDataSourceCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_DataSource_UsersCreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.ExamDataSourceUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_DataSource_UsersUpdatedBy");

    entity.HasOne(d => d.DeletedByNavigation)
        .WithMany(p => p.ExamDataSourceDeletedByNavigations)
        .HasForeignKey(d => d.DeletedBy)
        .HasConstraintName("FK_DataSource_UsersDeletedBy");

    entity.HasOne(d => d.ExamQuestion)
        .WithMany(p => p.ExamDataSources)
        .HasForeignKey(d => d.QuestionId)
        .HasConstraintName("FK_DataSource_Questions");
});
        }

        public static void ExamAnswerActionConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamAnswerAction>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.ExamAnswerActionCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_ExamAnswerAction_UsersCreatedBy");

    entity.HasOne(d => d.Exam)
        .WithMany(p => p.ExamAnswerActions)
        .HasForeignKey(d => d.ExamId)
        .HasConstraintName("FK_ExamAnswerAction_Exams");
});
        }

        public static void ExamQuestionAnswerConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamQuestionAnswer>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.HasOne(d => d.ExamAnswerAction)
        .WithMany(p => p.ExamQuestionAnswers)
        .HasForeignKey(d => d.ExamAnswerActionId)
        .HasConstraintName("FK_QuestionAnswers_ExamAnswerAction");

    entity.HasOne(d => d.Question)
        .WithMany(p => p.ExamQuestionAnswers)
        .HasForeignKey(d => d.QuestionId)
        .HasConstraintName("FK_QuestionAnswers_Questions");

    entity.HasOne(d => d.DataSource)
        .WithMany(p => p.ExamQuestionAnswers)
        .HasForeignKey(d => d.DataSourceId)
        .HasConstraintName("FK_QuestionAnswers_DataSource");
});
        }

        public static void JobApplicationExamsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobApplicationExams>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("Id");

    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

    entity.Property(e => e.DeletedDate).HasColumnType("datetime");

    entity.Property(e => e.StartAt).HasColumnType("datetime");

    entity.Property(e => e.EndAt).HasColumnType("datetime");
    entity.Property(e => e.FromDate).HasColumnType("datetime");

    entity.Property(e => e.ToDate).HasColumnType("datetime");

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.JobApplicationExamCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_JobApplicationExams_CreatedBy");

    entity.HasOne(d => d.UpdatedByNavigation)
        .WithMany(p => p.JobApplicationExamUpdatedByNavigations)
        .HasForeignKey(d => d.UpdatedBy)
        .HasConstraintName("FK_JobApplicationExams_UpdatedBy");

    entity.HasOne(d => d.DeletedByNavigation)
        .WithMany(p => p.JobApplicationExamDeletedByNavigations)
        .HasForeignKey(d => d.DeletedBy)
        .HasConstraintName("FK_JobApplicationExams_DeletedBy");
});
        }
        public static void ProjectConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
{

    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.ProjectsCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_CreatedBy_Project");

    entity.HasOne(d => d.UpdatedByNavigation)
         .WithMany(p => p.ProjectsUpdatedByNavigations)
         .HasForeignKey(d => d.UpdatedBy)
         .HasConstraintName("FK_UpdatedBy_Project");

    entity.HasOne(d => d.DeletedByNavigation)
     .WithMany(p => p.ProjectsDeletedByNavigations)
     .HasForeignKey(d => d.DeletedBy)
     .HasConstraintName("FK_DeletedBy_Project");

    //

});
        }
        public static void PublishEntitiesConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PublishEntities>(entity =>
{
    entity.HasOne(d => d.CreatedByNavigation)
        .WithMany(p => p.PublishEntityCreatedByNavigations)
        .HasForeignKey(d => d.CreatedBy)
        .HasConstraintName("FK_CreatedBy_PublishEntities");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.PublishEntities)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_Entities");

    entity.HasOne(d => d.Reference)
        .WithMany(p => p.PublishEntities)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_Reference");
});
        }

        public static void FormConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FormCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CreatedBy_Form");

                entity.HasOne(d => d.UpdatedByNavigation)
                   .WithMany(p => p.FormUpdatedByNavigations)
                   .HasForeignKey(d => d.UpdatedBy)
                   .HasConstraintName("FK_UpdatedBy_Form");


                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Form)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Forms_Reference");
                entity.HasOne(d => d.FormType)
                   .WithMany(p => p.Forms)
                   .HasForeignKey(d => d.FormTypeId)
                   .HasConstraintName("Fk_Form_FormType");

                entity.HasOne(d => d.Theme)
                .WithMany(p => p.Forms)
                .HasForeignKey(d => d.ThemeId)
                .HasConstraintName("Fk_Form_Theme");

            });
        }

        public static void FormInputConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormInput>(entity =>
            {
                entity.HasOne(d => d.InputsType)
                    .WithMany(p => p.FormInputs)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("Fk_FormInputs_InputType");

                entity.HasOne(d => d.Form)
                   .WithMany(p => p.FormInputs)
                   .HasForeignKey(d => d.FormId)
                   .HasConstraintName("Fk_FormInputs_Form");

            });
        }

        public static void FormValueConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormValue>(entity =>
            {
                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.FormValue)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("Fk_FormValue_Entity");

                entity.HasOne(d => d.Form)
                   .WithMany(p => p.FormValue)
                   .HasForeignKey(d => d.FormId)
                   .HasConstraintName("Fk_FormValue_Form");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FormValueCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_FormValue_CreatedByNavigation");

                entity.HasOne(d => d.UpdatedByNavigation)
                   .WithMany(p => p.FormValueUpdatedByNavigations)
                   .HasForeignKey(d => d.UpdatedBy)
                   .HasConstraintName("FK_FormValue_UpdatedByNavigation");

            });
        }

        public static void FormInputDataSourceConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormInputDataSource>(entity =>
            {
                entity.HasOne(d => d.FormInput)
                    .WithMany(p => p.FormInputDataSource)
                    .HasForeignKey(d => d.FormInputId)
                    .HasConstraintName("Fk_DataSource_FormInput");
            });

        }

        public static void FormValueDetailsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormValueDetails>(entity =>
            {
                entity.HasOne(d => d.FormValue)
                    .WithMany(p => p.FormValueDetails)
                    .HasForeignKey(d => d.FormValueId)
                    .HasConstraintName("Fk_FormValue_FormValuDetail");

                entity.HasOne(d => d.FormInput)
                   .WithMany(p => p.FormValueDetails)
                   .HasForeignKey(d => d.InputKey)
                   .HasConstraintName("Fk_FormInput_FormInputs");

            });
        }

        public static void EngineConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Engine>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EngineCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Engine_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                   .WithMany(p => p.EngineUpdatedByNavigations)
                   .HasForeignKey(d => d.UpdatedBy)
                   .HasConstraintName("FK_Engine_UsersUpdatedBy");

                entity.HasOne(d => d.Reference)
                  .WithMany(p => p.EnginReferences)
                  .HasForeignKey(d => d.ReferenceId)
                  .HasConstraintName("FK_Engine_References");
            });
        }

        public static void WorkFlowActionsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkFlowActions>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.WorkFlowActionsCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Actions_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                   .WithMany(p => p.WorkFlowActionsUpdatedByNavigations)
                   .HasForeignKey(d => d.UpdatedBy)
                   .HasConstraintName("FK_Actions_UsersUpdatedBy");

                entity.HasOne(d => d.Reference)
                  .WithMany(p => p.WorkFlowActionsReferences)
                  .HasForeignKey(d => d.ReferenceId)
                  .HasConstraintName("FK_Actions_References");

            });
        }

        public static void EngineActionJobRoleConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EngineActionJobRole>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EnginesActionsJobRoleCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_EnginesActionsJobRole_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                      .WithMany(p => p.EnginesActionsJobRoleUpdatedByNavigations)
                      .HasForeignKey(d => d.UpdatedBy)
                      .HasConstraintName("FK_EnginesActionsJobRole_UsersUpdatedBy");

                entity.HasOne(d => d.ActionNavigation)
                      .WithMany(p => p.Actions)
                      .HasForeignKey(d => d.ActionId)
                      .HasConstraintName("Fk_EnginesActionsJobRole_ActionId");

                entity.HasOne(d => d.EngineNavigation)
                        .WithMany(p => p.EnginesActionsJobRoles)
                        .HasForeignKey(d => d.EngineId)
                        .HasConstraintName("Fk_EnginesActionsJobRole_EngineId");

                entity.HasOne(d => d.NextStepNavigation)
                     .WithMany(p => p.NextSteps)
                     .HasForeignKey(d => d.NextStep)
                     .HasConstraintName("Fk_EnginesActionsJobRole_NextStep");

                entity.HasOne(d => d.ReturnStepNavigation)
                 .WithMany(p => p.ReturnSteps)
                 .HasForeignKey(d => d.ReturnStep)
                 .HasConstraintName("Fk_EnginesActionsJobRole_ReturnStep");

                entity.HasOne(d => d.RejectStepNavigation)
               .WithMany(p => p.RejectSteps)
               .HasForeignKey(d => d.RejectStep)
               .HasConstraintName("Fk_EnginesActionsJobRole_RejectStep");

                entity.HasOne(d => d.CloseStepNavigation)
                .WithMany(p => p.CloseSteps)
                .HasForeignKey(d => d.CloseStep)
                .HasConstraintName("Fk_EnginesActionsJobRole_CloseStep");

                entity.HasOne(d => d.JobRole)
                .WithMany(p => p.EnginesActionsJobRole)
                .HasForeignKey(d => d.JobRoleId)
                .HasConstraintName("FK_EnginesActionsJobRole_JobRoleId");


            });
        }

        public static void FormInputsActionsConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FormInputsActions>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FormInputsActionsCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_FormInputsActions_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                     .WithMany(p => p.FormInputsActionsUpdatedByNavigations)
                     .HasForeignKey(d => d.UpdatedBy)
                     .HasConstraintName("FK_FormInputsActions_UsersUpdatedBy");

                entity.HasOne(d => d.Action)
                   .WithMany(p => p.FormInputsActions)
                   .HasForeignKey(d => d.ActionId)
                   .HasConstraintName("Fk_FormInputsActions_ActionId");

                entity.HasOne(d => d.EngineActionJobRoleNavigation)
                   .WithMany(p => p.FormInputsEnginActionJobRoles)
                   .HasForeignKey(d => d.EngineActionJobRoleId)
                   .HasConstraintName("EngineActionJobRoleNavigation");



                entity.HasOne(d => d.FormInput)
                  .WithMany(p => p.FormInputsActionsNavigation)
                  .HasForeignKey(d => d.FormInputId)
                  .HasConstraintName("Fk_FormInputsActions_FormInputId");


            });
        }

        public static void FormValuesActionsConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormValuesActions>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FormValuesActionsCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_FormValuesActions_UsersCreatedBy");


                entity.HasOne(d => d.Action)
                   .WithMany(p => p.FormValuesActions)
                   .HasForeignKey(d => d.ActionId)
                   .HasConstraintName("Fk_FormValuesActions_ActionId");


                entity.HasOne(d => d.FormValue)
                  .WithMany(p => p.FormValuesActions)
                  .HasForeignKey(d => d.FormValueId)
                  .HasConstraintName("Fk_FormValuesActions_FormValueId");

                entity.HasOne(d => d.EngineActionJobRole)
                .WithMany(p => p.FormValuesActions)
                .HasForeignKey(d => d.EngineActionJobRoleId)
                .HasConstraintName("Fk_FormValuesActions_EngineActionJobRoleId");

                entity.HasOne(d => d.FromUser)
                .WithMany(p => p.FromUserNavigations)
                .HasForeignKey(d => d.FromUserId)
                .HasConstraintName("FK_FormValuesActions_FromUserId");

                entity.HasOne(d => d.ToReference)
                .WithMany(p => p.FormValuesActionsToReferences)
                .HasForeignKey(d => d.ToReferenceId)
                .HasConstraintName("FK_FormValuesActions_References");


            });
        }
        public static void UsersEntityReferenceConfiguration(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UsersEntityReference>(entity =>
{
    entity.Property(e => e.Id).HasColumnName("ID");


    entity.HasOne(d => d.User)
        .WithMany(p => p.UsersEntitiesReferences)
        .HasForeignKey(d => d.UserId)
        .HasConstraintName("FK_UsersEntitiesReferences_Users");

    entity.HasOne(d => d.Entity)
        .WithMany(p => p.UsersEntitiesReferences)
        .HasForeignKey(d => d.EntityId)
        .HasConstraintName("FK_UsersEntitiesReferences_Entities");


    entity.HasOne(d => d.Reference)
        .WithMany(p => p.UsersEntitiesReferences)
        .HasForeignKey(d => d.ReferenceId)
        .HasConstraintName("FK_UsersEntitiesReferences_References");
});
        }


        public static void EidFiterRequestConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EidFiterRequest>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Longtitude).HasMaxLength(50);
            });
        }

        public static void InnovationConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Idea>(entity =>
            {
                entity.ToTable("Ideas", "Innovation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.Country).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ActivatedByNavigation)
                    .WithMany(p => p.IdeaActivatedByNavigations)
                    .HasForeignKey(d => d.ActivatedBy)
                    .HasConstraintName("FK_Ideas_UsersActivatedBy");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.IdeaCategoryNavigations)
                    .HasForeignKey(d => d.Category)
                    .HasConstraintName("FK_Ideas_MajorLookupsCategory");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.IdeaCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Ideas_UsersCreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.IdeaDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Ideas_UsersDeletedBy");

                entity.HasOne(d => d.PriorityNavigation)
                    .WithMany(p => p.IdeaPriorityNavigations)
                    .HasForeignKey(d => d.Priority)
                    .HasConstraintName("FK_Ideas_MajorLookupsPriority");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.IdeaStatusNavigations)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_Ideas_MajorLookupsStatus");

                entity.HasOne(d => d.ToReferenceNavigation)
                    .WithMany(p => p.Ideas)
                    .HasForeignKey(d => d.ToReference)
                    .HasConstraintName("FK_Ideas_ToReferences");

                entity.HasOne(d => d.ReferenceNavigation)
                    .WithMany(p => p.IdeasTo)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Ideas_References");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.IdeaTypeNavigations)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_Ideas_MajorLookupsType");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.IdeaUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Ideas_UsersUpdatedBy");
            });

            modelBuilder.Entity<IdeaAction>(entity =>
            {
                entity.ToTable("IdeaActions", "Innovation");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.IdeaActions)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_IdeaActions_UsersCreatedBy");

                entity.HasOne(d => d.Idea)
                    .WithMany(p => p.IdeaActions)
                    .HasForeignKey(d => d.IdeaId)
                    .HasConstraintName("FK_IdeaActions_Ideas");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.IdeaActions)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_IdeaActions_MajorLookupsType");
            });


            modelBuilder.Entity<IdeaComment>(entity =>
            {
                entity.ToTable("IdeaComments", "Innovation");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CommentCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_IdeaComments_UsersCreatedBy");

                entity.HasOne(d => d.ApprovedByNavigation)
                    .WithMany(p => p.CommentApprovedByNavigation)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("FK_IdeaComments_UsersApprovedBy");

                entity.HasOne(d => d.RepliedByNavigation)
                    .WithMany(p => p.CommentRepliedByNavigation)
                    .HasForeignKey(d => d.RepliedBy)
                    .HasConstraintName("FK_IdeaComments_UsersRepliedBy");

                entity.HasOne(d => d.Idea)
                    .WithMany(p => p.IdeaComments)
                    .HasForeignKey(d => d.IdeaId)
                    .HasConstraintName("FK_IdeaComments_Idea");

            });


            modelBuilder.Entity<IdeasCompetentAuthority>(entity =>
            {
                entity.ToTable("IdeasCompetentAuthorities", "Innovation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ReferenceId).HasColumnName("ReferenceID");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.IdeasCompetentAuthorities)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_IdeasCompetentAuthorities_References");
            });

        }



        public static void FeedbacksConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedbacks>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");


                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FeedbacksCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Feedbacks_UsersCreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.FeedbacksUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Feedbacks_UsersUpdatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.FeedbacksDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Feedbacks_UsersDeletedBy");

                entity.HasOne(d => d.Reference)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.ReferenceId)
                    .HasConstraintName("FK_Feedbacks_References");
            });
        }

        public static void FeedbacksDataSourceConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbacksDataSource>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");


                entity.HasOne(d => d.Feedbacks)
                    .WithMany(p => p.FeedbacksDataSources)
                    .HasForeignKey(d => d.FeedbacksId)
                    .HasConstraintName("FK_FeedbacksDataSource_Feedbacks");
            });
        }

        public static void FeedbacksAnswerActionConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbacksAnswerAction>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Feedbacks)
                            .WithMany(p => p.FeedbacksAnswerActions)
                            .HasForeignKey(d => d.FeedbacksId)
                            .HasConstraintName("FK_FeedbacksAnswerAction_Feedbacks");

                entity.HasOne(d => d.Entity)
                            .WithMany(p => p.FeedbacksAnswerActions)
                            .HasForeignKey(d => d.EntityId)
                            .HasConstraintName("FK_FeedbacksAnswerAction_Entity");
            });
        }

        public static void FeedbacksAnswerConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbacksAnswer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.HasOne(d => d.FeedbacksAnswerAction)
                                .WithMany(p => p.FeedbacksAnswers)
                                .HasForeignKey(d => d.FeedbacksAnswerActionId)
                                .HasConstraintName("FK_FeedbacksAnswers_FeedbacksAnswerAction");

                entity.HasOne(d => d.FeedbacksDataSource)
                                .WithMany(p => p.FeedbacksAnswers)
                                .HasForeignKey(d => d.FeedbacksDataSourceId)
                                .HasConstraintName("FK_FeedbacksAnswers_FeedbacksDataSource");
            });
        }


    }
}


