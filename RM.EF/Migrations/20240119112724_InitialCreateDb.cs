#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Contact");

            migrationBuilder.EnsureSchema(
                name: "Orders");

            migrationBuilder.EnsureSchema(
                name: "Exams");

            migrationBuilder.EnsureSchema(
                name: "Survey");

            migrationBuilder.EnsureSchema(
                name: "WorkFlow");

            migrationBuilder.EnsureSchema(
                name: "Permits");

            migrationBuilder.EnsureSchema(
                name: "DynamicForm");

            migrationBuilder.EnsureSchema(
                name: "Job");

            migrationBuilder.EnsureSchema(
                name: "OpenData");

            migrationBuilder.EnsureSchema(
                name: "ScientificLetters");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "DocumentsType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EntitiesLatestUpdate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntitiesLatestUpdate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntitiesType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntitiesType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FlowStepper",
                schema: "Permits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowStepper", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormsDataSource",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormsDataSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormsDataSource_FormsDataSource_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "DynamicForm",
                        principalTable: "FormsDataSource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormType",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeEn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InitiativesProjectsType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativesProjectsType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InputsType",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputsType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InteractionStatisticsType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionStatisticsType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JobRole",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRole", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LoginWay",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginWay", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MajorLookupsType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorLookupsType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MajorStatus",
                schema: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PermissionLevel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionLevel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "QuestionType",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasDataSource = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionType",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasDataSource = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MajorStatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactStatus",
                        column: x => x.MajorStatusId,
                        principalSchema: "Contact",
                        principalTable: "MajorStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionFiles",
                schema: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Actions",
                schema: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    ToReferenceId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionStatus",
                        column: x => x.StatusId,
                        principalSchema: "Contact",
                        principalTable: "Status",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Actions",
                schema: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminMenu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ReferencesMajorId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    MenuOrder = table.Column<int>(type: "int", nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMenu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdminMenu_AdminMenu",
                        column: x => x.ParentId,
                        principalTable: "AdminMenu",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsPopup = table.Column<bool>(type: "bit", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsHomeSliderAd = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    AdvertisementOrder = table.Column<int>(type: "int", nullable: true),
                    SerialNum = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AmanahAwards",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    AwardOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmanahAwards", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    FrontIdentity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowBySearch = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArticlesPublish",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    MajorReferenceId = table.Column<int>(type: "int", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PublishedBy = table.Column<int>(type: "int", nullable: true),
                    Removed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlesPublish", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extention = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiaries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiaries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReplyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplyDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RepliedBy = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    ItemUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAgreeTerms = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                schema: "Contact",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplainID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastStatusDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstStatusId = table.Column<int>(type: "int", nullable: true),
                    LastStatusId = table.Column<int>(type: "int", nullable: true),
                    FirstActionId = table.Column<int>(type: "int", nullable: true),
                    LastActionId = table.Column<int>(type: "int", nullable: true),
                    LastActionUser = table.Column<int>(type: "int", nullable: true),
                    LastActionReference = table.Column<int>(type: "int", nullable: true),
                    TransferFromManager = table.Column<bool>(type: "bit", nullable: true),
                    ProcessingTimesCount = table.Column<int>(type: "int", nullable: true),
                    IdeaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ContactUs_Actions_FirstActionId",
                        column: x => x.FirstActionId,
                        principalSchema: "Contact",
                        principalTable: "Actions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactUs_Actions_LastActionId",
                        column: x => x.LastActionId,
                        principalSchema: "Contact",
                        principalTable: "Actions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactUs_Status_FirstStatusId",
                        column: x => x.FirstStatusId,
                        principalSchema: "Contact",
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactUs_Status_LastStatusId",
                        column: x => x.LastStatusId,
                        principalSchema: "Contact",
                        principalTable: "Status",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                schema: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactUsId = table.Column<int>(type: "int", nullable: true),
                    IsPositive = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactFeedback",
                        column: x => x.ContactUsId,
                        principalSchema: "Contact",
                        principalTable: "ContactUs",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DataSource",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataSource",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    IsFinalRoot = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Documents_Documents",
                        column: x => x.ParentId,
                        principalTable: "Documents",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Documents_DocumentsType",
                        column: x => x.TypeId,
                        principalTable: "DocumentsType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Engine",
                schema: "WorkFlow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EngineActionJobRole",
                schema: "WorkFlow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EngineId = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: true),
                    NextStep = table.Column<int>(type: "int", nullable: true),
                    ReturnStep = table.Column<int>(type: "int", nullable: true),
                    RejectStep = table.Column<int>(type: "int", nullable: true),
                    CloseStep = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobRoleId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StepNo = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    HasNote = table.Column<bool>(type: "bit", nullable: true),
                    NoteIsRequired = table.Column<bool>(type: "bit", nullable: true),
                    IsTransferToReference = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineActionJobRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnginesActionsJobRole_JobRoleId",
                        column: x => x.JobRoleId,
                        principalTable: "JobRole",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "Fk_EnginesActionsJobRole_EngineId",
                        column: x => x.EngineId,
                        principalSchema: "WorkFlow",
                        principalTable: "Engine",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EngineForms",
                schema: "WorkFlow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: true),
                    WorkFlowEngineId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngineForms_Engine_WorkFlowEngineId",
                        column: x => x.WorkFlowEngineId,
                        principalSchema: "WorkFlow",
                        principalTable: "Engine",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    FrontIdentity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    backendIdentity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CmsIdentity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Searchable = table.Column<bool>(type: "bit", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ReferencesMajorId = table.Column<int>(type: "int", nullable: true),
                    ShowMenuIdentifier = table.Column<bool>(type: "bit", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Entities_EntitiesParentId",
                        column: x => x.ParentId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Entities_EntitiesType",
                        column: x => x.TypeId,
                        principalTable: "EntitiesType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Eservices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryChannelAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryChannelEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Maturity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeesAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeesEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeesInk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentChannelAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentChannelEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionTimeEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequirementsAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequirementsEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentsAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentsEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProceduresAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProceduresEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserGuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRoot = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eservices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Eservices_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Eservices_EservicesParentId",
                        column: x => x.ParentId,
                        principalTable: "Eservices",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ExamAnswerAction",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAnswerAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    TotalMark = table.Column<int>(type: "int", nullable: true),
                    SuccessMark = table.Column<int>(type: "int", nullable: true),
                    DistributionGradeMethod = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ExternalSites",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BriefeContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalSites", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExternalSites_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ExternalSites_ExternalSites",
                        column: x => x.ParentId,
                        principalTable: "ExternalSites",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FAQ",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    QuestionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQ", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlowStepperProjects",
                schema: "Permits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStep = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    StepId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowStepperProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowStepperProjects_FlowStepper_StepId",
                        column: x => x.StepId,
                        principalSchema: "Permits",
                        principalTable: "FlowStepper",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormInputDataSource",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataSourceId = table.Column<int>(type: "int", nullable: true),
                    FormInputId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormInputDataSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormInputDataSource_FormsDataSource_DataSourceId",
                        column: x => x.DataSourceId,
                        principalSchema: "DynamicForm",
                        principalTable: "FormsDataSource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormInputs",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    FormId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Mandatory = table.Column<bool>(type: "bit", nullable: true),
                    VerticalDataSourceDirection = table.Column<bool>(type: "bit", nullable: true),
                    ViewInFullRow = table.Column<bool>(type: "bit", nullable: true),
                    HasDataSourceFromAPI = table.Column<bool>(type: "bit", nullable: true),
                    DataSourceAPIRouting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    APIParameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnChangeAPIMethodName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnChangeParamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnChangeRefelectionInputKey = table.Column<int>(type: "int", nullable: true),
                    ShowInMainListCP = table.Column<bool>(type: "bit", nullable: true),
                    ShowInMainPortalPage = table.Column<bool>(type: "bit", nullable: true),
                    ShowInAdvancedSearch = table.Column<bool>(type: "bit", nullable: true),
                    Property = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnique = table.Column<bool>(type: "bit", nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormInputs", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_FormInputs_InputType",
                        column: x => x.Type,
                        principalSchema: "DynamicForm",
                        principalTable: "InputsType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormInputsActions",
                schema: "WorkFlow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: true),
                    EngineFormId = table.Column<int>(type: "int", nullable: true),
                    FormInputId = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: true),
                    EngineActionJobRoleId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormInputsActions", x => x.Id);
                    table.ForeignKey(
                        name: "EngineActionJobRoleNavigation",
                        column: x => x.EngineActionJobRoleId,
                        principalSchema: "WorkFlow",
                        principalTable: "EngineActionJobRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormInputsActions_EngineForms_EngineFormId",
                        column: x => x.EngineFormId,
                        principalSchema: "WorkFlow",
                        principalTable: "EngineForms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Fk_FormInputsActions_FormInputId",
                        column: x => x.FormInputId,
                        principalSchema: "DynamicForm",
                        principalTable: "FormInputs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    FormTypeId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Form_FormType",
                        column: x => x.FormTypeId,
                        principalSchema: "DynamicForm",
                        principalTable: "FormType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormsEntity",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormsEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormsEntity_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_FormsEntity_Forms_FormId",
                        column: x => x.FormId,
                        principalSchema: "DynamicForm",
                        principalTable: "Forms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormValue",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    FormId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormValue", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_FormValue_Entity",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "Fk_FormValue_Form",
                        column: x => x.FormId,
                        principalSchema: "DynamicForm",
                        principalTable: "Forms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormValueDetails",
                schema: "DynamicForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InputKey = table.Column<int>(type: "int", nullable: true),
                    InputValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormValueId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormValueDetails", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_FormInput_FormInputs",
                        column: x => x.InputKey,
                        principalSchema: "DynamicForm",
                        principalTable: "FormInputs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Fk_FormValue_FormValuDetail",
                        column: x => x.FormValueId,
                        principalSchema: "DynamicForm",
                        principalTable: "FormValue",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormValuesActions",
                schema: "WorkFlow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormValueId = table.Column<int>(type: "int", nullable: true),
                    EngineActionJobRoleId = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    ToReferenceId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReturned = table.Column<bool>(type: "bit", nullable: true),
                    IsRejected = table.Column<bool>(type: "bit", nullable: true),
                    TransferToReferenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormValuesActions", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_FormValuesActions_EngineActionJobRoleId",
                        column: x => x.EngineActionJobRoleId,
                        principalSchema: "WorkFlow",
                        principalTable: "EngineActionJobRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Fk_FormValuesActions_FormValueId",
                        column: x => x.FormValueId,
                        principalSchema: "DynamicForm",
                        principalTable: "FormValue",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GovServices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudienceEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudienceAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceChannelEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceChannelAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequirementsEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequirementsAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepsEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepsAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    IsRoot = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovServices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ParentService",
                        column: x => x.ParentID,
                        principalTable: "GovServices",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "InitiativesProjects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitiativeDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoalsAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoalsEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativesProjects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InitiativesProjects_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_InitiativesProjects_InitiativesProjectsType",
                        column: x => x.TypeId,
                        principalTable: "InitiativesProjectsType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "InitiativesProjectsBeneficiaries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InitiativesProjectId = table.Column<int>(type: "int", nullable: true),
                    BeneficiaryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativesProjectsBeneficiaries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InitiativesProjectsBeneficiaries_Beneficiaries",
                        column: x => x.BeneficiaryId,
                        principalTable: "Beneficiaries",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_InitiativesProjectsBeneficiaries_InitiativesProjects",
                        column: x => x.InitiativesProjectId,
                        principalTable: "InitiativesProjects",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "InteractionStatistics",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ReferenceMajorId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    InteractionStatisticsType = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    IsHelpfulCount = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    NotHelpfulCount = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    ItemUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionStatistics", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InteractionStatistics_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_InteractionStatistics_InteractionStatisticsType",
                        column: x => x.InteractionStatisticsType,
                        principalTable: "InteractionStatisticsType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpportunityNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpportunityType = table.Column<int>(type: "int", nullable: true),
                    OpportunityReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastInquiryDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastAdmissionDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    OpenBidDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    OpportunityDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    OpportunityUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Investments_Type",
                        column: x => x.OpportunityType,
                        principalTable: "InvestmentTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "JobAdvertisement",
                schema: "Job",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAdvertisement", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JobAdvertisement_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "JobApplicationExams",
                schema: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobApplicationId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Result = table.Column<double>(type: "float", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplicationExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplicationExams_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "Exams",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                schema: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gendar = table.Column<int>(type: "int", nullable: true),
                    IdCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainingCourses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QualificationId = table.Column<int>(type: "int", nullable: true),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    JobCareerId = table.Column<int>(type: "int", nullable: true),
                    SpecialistId = table.Column<int>(type: "int", nullable: true),
                    GradeYear = table.Column<int>(type: "int", nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: true),
                    GenderIntegration = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCareers",
                schema: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobLocationAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobLocationEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobCondationsAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobCondationsEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specifications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecificationsEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxLimit = table.Column<int>(type: "int", nullable: true),
                    JobAdvertisementId = table.Column<int>(type: "int", nullable: true),
                    QualificationId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    IsNoticeBeneficiaries = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCareers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCareers_JobAdvertisement_JobAdvertisementId",
                        column: x => x.JobAdvertisementId,
                        principalSchema: "Job",
                        principalTable: "JobAdvertisement",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "JobLookUp",
                schema: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLookUp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogInformation",
                schema: "OpenData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenData_Entity_Log",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MajorLookups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(type: "int", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorLookups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MajorLookups_MajorLookups",
                        column: x => x.ParentId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MajorLookups_MajorLookupsType",
                        column: x => x.TypeId,
                        principalTable: "MajorLookupsType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QualificationID = table.Column<int>(type: "int", nullable: true),
                    DistrictID = table.Column<int>(type: "int", nullable: true),
                    AgeID = table.Column<int>(type: "int", nullable: true),
                    GenderID = table.Column<int>(type: "int", nullable: true),
                    VolunteerFieldId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    EntityId = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((34))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Volunteers_AgeRange",
                        column: x => x.AgeID,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Volunteers_District",
                        column: x => x.DistrictID,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Volunteers_Gender",
                        column: x => x.GenderID,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Volunteers_Qualification",
                        column: x => x.QualificationID,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Volunteers_VolunteerField",
                        column: x => x.VolunteerFieldId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    MenuOrder = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((100))"),
                    ArticleId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ReferenceMajorId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    IsFirstRoot = table.Column<bool>(type: "bit", nullable: true),
                    FontIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvgIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenBlankPage = table.Column<bool>(type: "bit", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Articles",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Menu_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Menu_MenuParentID",
                        column: x => x.ParentId,
                        principalTable: "Menu",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Menu_MenuType",
                        column: x => x.TypeId,
                        principalTable: "MenuType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MobileApplications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    BriefeContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserManualUrlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserManualUrlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AndroidUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IosUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileApplications", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Multimedias",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Multimedias", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Multimedias_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsSourceAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewsSourceEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BriefeContentEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    NewsContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumpPic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OriginalPic = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    NewsDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    NewsDateH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    TagsAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ShowInHome = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID);
                    table.ForeignKey(
                        name: "FK_News_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Officials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodTo = table.Column<DateTime>(type: "date", nullable: true),
                    PeriodFrom = table.Column<DateTime>(type: "date", nullable: true),
                    CvUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ThumbPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficialOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Officials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Officials_EntityID",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OpenData",
                schema: "OpenData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((58))"),
                    IsGregorian = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenData_District",
                        column: x => x.DistrictId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OpenData_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OpenData_Type",
                        column: x => x.TypeId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OpenDataRequests",
                schema: "OpenData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenDataRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenDataRequest_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OpenDataStatistics",
                schema: "OpenData",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    StatisticType = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    FromYear = table.Column<int>(type: "int", nullable: true),
                    ToYear = table.Column<int>(type: "int", nullable: true),
                    FromMonth = table.Column<int>(type: "int", nullable: true),
                    ToMonth = table.Column<int>(type: "int", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    IsGregorian = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenDataStatistics", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OpenDataStatistics_District",
                        column: x => x.DistrictId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OpenDataStatistics_Type",
                        column: x => x.TypeId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OpenDataTemp",
                schema: "OpenData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((58))"),
                    IsConfirm = table.Column<bool>(type: "bit", nullable: true),
                    IsGregorian = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenDataTemp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenDataTemp_District",
                        column: x => x.DistrictId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OpenDataTemp_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OpenDataTemp_Type",
                        column: x => x.TypeId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderEntity",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnershipTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnershipTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RMDepartmentNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RMDepartmentNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractActive = table.Column<bool>(type: "bit", nullable: true),
                    ContractDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Partners_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PermitActions",
                schema: "Permits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: true),
                    PermitRequestId = table.Column<int>(type: "int", nullable: true),
                    StepId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitActions", x => x.Id);
                    table.ForeignKey(
                        name: "StepNavigation",
                        column: x => x.UpdatedBy,
                        principalSchema: "Permits",
                        principalTable: "FlowStepper",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermitsRequests",
                schema: "Permits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    LifeStatusCode = table.Column<int>(type: "int", nullable: true),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentitySource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermitStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PermitEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PermitDays = table.Column<int>(type: "int", nullable: true),
                    IdentityPhoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermitState = table.Column<int>(type: "int", nullable: true),
                    PersonalPhoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarLitters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarNumbers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermitType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    DeliverReferenceId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsForigin = table.Column<bool>(type: "bit", nullable: true),
                    IsCommitted = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    Attachment1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentStep = table.Column<int>(type: "int", nullable: true),
                    NextStep = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    LastUserActionJobRole = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitsRequests", x => x.Id);
                    table.ForeignKey(
                        name: "CurrentStepNavigation",
                        column: x => x.CurrentStep,
                        principalSchema: "Permits",
                        principalTable: "FlowStepper",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermitsRequests_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "NextStepNavigation",
                        column: x => x.NextStep,
                        principalSchema: "Permits",
                        principalTable: "FlowStepper",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermitsWorkSites",
                schema: "Permits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermitId = table.Column<int>(type: "int", nullable: true),
                    WorksiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitsWorkSites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermitsWorkSite_MajorLookUps",
                        column: x => x.WorksiteId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PermitsWorkSites_PermitsRequests_PermitId",
                        column: x => x.PermitId,
                        principalSchema: "Permits",
                        principalTable: "PermitsRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "Permits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailsAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailsEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProjectsUsers",
                schema: "Permits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    IsEmployee = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectsUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "Permits",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublishEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswers",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    DataSourceId = table.Column<int>(type: "int", nullable: true),
                    ExamAnswerActionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_DataSource",
                        column: x => x.DataSourceId,
                        principalSchema: "Exams",
                        principalTable: "DataSource",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_ExamAnswerAction",
                        column: x => x.ExamAnswerActionId,
                        principalSchema: "Exams",
                        principalTable: "ExamAnswerAction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswers",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    DataSourceId = table.Column<int>(type: "int", nullable: true),
                    SurveyAnswerActionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_DataSource",
                        column: x => x.DataSourceId,
                        principalSchema: "Survey",
                        principalTable: "DataSource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Mandatory = table.Column<bool>(type: "bit", nullable: true),
                    VerticalAnswersDirection = table.Column<bool>(type: "bit", nullable: true),
                    Mark = table.Column<double>(type: "float", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Exams",
                        column: x => x.ExamId,
                        principalSchema: "Exams",
                        principalTable: "Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_QuestionType_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Exams",
                        principalTable: "QuestionType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    SurveyId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsGlobal = table.Column<bool>(type: "bit", nullable: true),
                    Mandatory = table.Column<bool>(type: "bit", nullable: true),
                    VerticalAnswersDirection = table.Column<bool>(type: "bit", nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubQuestionOrder = table.Column<int>(type: "int", nullable: true),
                    GroupOrder = table.Column<int>(type: "int", nullable: true),
                    MinValue = table.Column<int>(type: "int", nullable: true),
                    MaxValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionType_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Survey",
                        principalTable: "QuestionType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionsAnswers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    QuestionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsnwerEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsAnswers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuestionsAnswers_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_QuestionsAnswers_MobileApplications",
                        column: x => x.ItemId,
                        principalTable: "MobileApplications",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ItemUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rates_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ReferenceContent",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceID = table.Column<int>(type: "int", nullable: true),
                    BriefeContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BriefeContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefWordEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Longtitude = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AddressEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mailbox = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDateRegistrationNo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceContent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReferenceContent_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReferencesMajorId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    HasContent = table.Column<bool>(type: "bit", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsPortal = table.Column<bool>(type: "bit", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.ID);
                    table.ForeignKey(
                        name: "FK_References_ReferencesParentId",
                        column: x => x.ParentId,
                        principalTable: "References",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ReferencesJobRole",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    JobRoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferencesJobRole", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReferencesJobRole_JobRole",
                        column: x => x.JobRoleId,
                        principalTable: "JobRole",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ReferencesJobRole_References",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReferenceID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Roles_References",
                        column: x => x.ReferenceID,
                        principalTable: "References",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RolesPermissionLevel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionLevelId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesPermissionLevel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RolesPermissionLevel_PermissionLevel",
                        column: x => x.PermissionLevelId,
                        principalTable: "PermissionLevel",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RolesPermissionLevel_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomainUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCardNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ReferenceID = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    NationalityId = table.Column<int>(type: "int", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    CommunicateWayId = table.Column<int>(type: "int", nullable: true),
                    YearsOfExperienceId = table.Column<int>(type: "int", nullable: true),
                    CountryOfResidenceId = table.Column<int>(type: "int", nullable: true),
                    CityOfResidenceId = table.Column<int>(type: "int", nullable: true),
                    EducationalQualificationsId = table.Column<int>(type: "int", nullable: true),
                    WorkAreaId = table.Column<int>(type: "int", nullable: true),
                    AcceptWorkOnSite = table.Column<bool>(type: "bit", nullable: true),
                    WorkInGovSectors = table.Column<bool>(type: "bit", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginWayId = table.Column<int>(type: "int", nullable: true),
                    JobRole = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_References",
                        column: x => x.ReferenceID,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Users_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ReferencesMajor",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    LoginWayId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferencesMajor", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReferencesMajor_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ReferencesMajor_LoginWay",
                        column: x => x.LoginWayId,
                        principalTable: "LoginWay",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ReferencesMajor_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ReferencesMajor_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ScientificLetters",
                schema: "ScientificLetters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ResearcherNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResearcherNameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResearchPlaceAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResearchPlaceEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DegreeId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailsAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailsEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificLetters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScientificLetters_Degree",
                        column: x => x.DegreeId,
                        principalTable: "MajorLookups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ScientificLetters_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ScientificLetters_References",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ScientificLetters_UsersActivatedBy",
                        column: x => x.ActivatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ScientificLetters_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ScientificLetters_UsersDeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ScientificLetters_UsersUpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsAuthenticated = table.Column<bool>(type: "bit", nullable: true),
                    LastOperation = table.Column<DateTime>(type: "datetime", nullable: true),
                    SystemID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Session_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    ShowInHomePage = table.Column<bool>(type: "bit", nullable: true),
                    UseCapcha = table.Column<bool>(type: "bit", nullable: true),
                    InnerOnly = table.Column<bool>(type: "bit", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Surveys_References",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Surveys_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Surveys_UsersDeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Surveys_UsersUpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TermsAndRegulations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsFinalRoot = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedBy = table.Column<int>(type: "int", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAndRegulations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TermsAndRegulations_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TermsAndRegulations_References",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TermsAndRegulations_TermsAndRegulations",
                        column: x => x.ParentId,
                        principalTable: "TermsAndRegulations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TermsAndRegulations_UsersActivatedBy",
                        column: x => x.ActivatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TermsAndRegulations_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TermsAndRegulations_UsersDeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TermsAndRegulations_UsersUpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UsersEntities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Add = table.Column<bool>(type: "bit", nullable: true),
                    Edit = table.Column<bool>(type: "bit", nullable: true),
                    Delete = table.Column<bool>(type: "bit", nullable: true),
                    List = table.Column<bool>(type: "bit", nullable: true),
                    Activate = table.Column<bool>(type: "bit", nullable: true),
                    Reports = table.Column<bool>(type: "bit", nullable: true),
                    View = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersEntities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsersEntities_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UsersEntities_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UsersEntitiesReferences",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    Add = table.Column<bool>(type: "bit", nullable: true),
                    Edit = table.Column<bool>(type: "bit", nullable: true),
                    Delete = table.Column<bool>(type: "bit", nullable: true),
                    List = table.Column<bool>(type: "bit", nullable: true),
                    Activate = table.Column<bool>(type: "bit", nullable: true),
                    Reports = table.Column<bool>(type: "bit", nullable: true),
                    View = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersEntitiesReferences", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsersEntitiesReferences_Entities",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UsersEntitiesReferences_References",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UsersEntitiesReferences_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UsersPermissionLevel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    PermissionLevelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPermissionLevel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsersPermissionLevel_PermissionLevel",
                        column: x => x.PermissionLevelId,
                        principalTable: "PermissionLevel",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UsersPermissionLevel_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowActions",
                schema: "WorkFlow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActivatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actions_References",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Actions_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Actions_UsersUpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SurveyAnswerAction",
                schema: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAnswerAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyAnswerAction_Surveys",
                        column: x => x.SurveyId,
                        principalSchema: "Survey",
                        principalTable: "Surveys",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyAnswerAction_UsersCreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionFiles_ActionId",
                schema: "Contact",
                table: "ActionFiles",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ContactId",
                schema: "Contact",
                table: "Actions",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_CreatedBy",
                schema: "Contact",
                table: "Actions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_FromUserId",
                schema: "Contact",
                table: "Actions",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_StatusId",
                schema: "Contact",
                table: "Actions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ToReferenceId",
                schema: "Contact",
                table: "Actions",
                column: "ToReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_CreatedBy",
                schema: "Orders",
                table: "Actions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_OrderId",
                schema: "Orders",
                table: "Actions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_TypeId",
                schema: "Orders",
                table: "Actions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenu_EntityId",
                table: "AdminMenu",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenu_ParentId",
                table: "AdminMenu",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenu_ReferenceId",
                table: "AdminMenu",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenu_ReferencesMajorId",
                table: "AdminMenu",
                column: "ReferencesMajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ActivatedBy",
                table: "Advertisements",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CreatedBy",
                table: "Advertisements",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_DeletedBy",
                table: "Advertisements",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_EntityId",
                table: "Advertisements",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ReferenceId",
                table: "Advertisements",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_RegionId",
                table: "Advertisements",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_UpdatedBy",
                table: "Advertisements",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AmanahAwards_ActivatedBy",
                table: "AmanahAwards",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AmanahAwards_CreatedBy",
                table: "AmanahAwards",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AmanahAwards_DeletedBy",
                table: "AmanahAwards",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AmanahAwards_EntityId",
                table: "AmanahAwards",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AmanahAwards_ReferenceId",
                table: "AmanahAwards",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_AmanahAwards_UpdatedBy",
                table: "AmanahAwards",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ActivatedBy",
                table: "Articles",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatedBy",
                table: "Articles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_EntityId",
                table: "Articles",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ReferenceId",
                table: "Articles",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UpdatedBy",
                table: "Articles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlesPublish_MajorReferenceId",
                table: "ArticlesPublish",
                column: "MajorReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlesPublish_PublishedBy",
                table: "ArticlesPublish",
                column: "PublishedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlesPublish_ReferenceId",
                table: "ArticlesPublish",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CreatedBy",
                table: "Attachments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_EntityId",
                table: "Attachments",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ItemId",
                table: "Attachments",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ReferenceId",
                table: "Attachments",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiaries_CreatedBy",
                table: "Beneficiaries",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiaries_DeletedBy",
                table: "Beneficiaries",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApprovedBy",
                table: "Comments",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedBy",
                table: "Comments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EntityId",
                table: "Comments",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReferenceId",
                table: "Comments",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RepliedBy",
                table: "Comments",
                column: "RepliedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_CreatedBy",
                schema: "Contact",
                table: "ContactUs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_EntityId",
                schema: "Contact",
                table: "ContactUs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_FirstActionId",
                schema: "Contact",
                table: "ContactUs",
                column: "FirstActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_FirstStatusId",
                schema: "Contact",
                table: "ContactUs",
                column: "FirstStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_LastActionId",
                schema: "Contact",
                table: "ContactUs",
                column: "LastActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_LastActionReference",
                schema: "Contact",
                table: "ContactUs",
                column: "LastActionReference");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_LastActionUser",
                schema: "Contact",
                table: "ContactUs",
                column: "LastActionUser");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_LastStatusId",
                schema: "Contact",
                table: "ContactUs",
                column: "LastStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_ModifiedBy",
                schema: "Contact",
                table: "ContactUs",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_ReferenceId",
                schema: "Contact",
                table: "ContactUs",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_CreatedBy",
                schema: "Exams",
                table: "DataSource",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_DeletedBy",
                schema: "Exams",
                table: "DataSource",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_QuestionId",
                schema: "Exams",
                table: "DataSource",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_UpdatedBy",
                schema: "Exams",
                table: "DataSource",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_CreatedBy",
                schema: "Survey",
                table: "DataSource",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_DeletedBy",
                schema: "Survey",
                table: "DataSource",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_QuestionId",
                schema: "Survey",
                table: "DataSource",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_UpdatedBy",
                schema: "Survey",
                table: "DataSource",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ActivatedBy",
                table: "Documents",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatedBy",
                table: "Documents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DeletedBy",
                table: "Documents",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_EntityId",
                table: "Documents",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ParentId",
                table: "Documents",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ReferenceId",
                table: "Documents",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TypeId",
                table: "Documents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UpdatedBy",
                table: "Documents",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Engine_CreatedBy",
                schema: "WorkFlow",
                table: "Engine",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Engine_ReferenceId",
                schema: "WorkFlow",
                table: "Engine",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Engine_UpdatedBy",
                schema: "WorkFlow",
                table: "Engine",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_ActionId",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_CloseStep",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "CloseStep");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_CreatedBy",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_EngineId",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "EngineId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_JobRoleId",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_NextStep",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "NextStep");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_RejectStep",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "RejectStep");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_ReturnStep",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "ReturnStep");

            migrationBuilder.CreateIndex(
                name: "IX_EngineActionJobRole_UpdatedBy",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EngineForms_FormId",
                schema: "WorkFlow",
                table: "EngineForms",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineForms_WorkFlowEngineId",
                schema: "WorkFlow",
                table: "EngineForms",
                column: "WorkFlowEngineId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_ParentId",
                table: "Entities",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_ReferenceId",
                table: "Entities",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_ReferencesMajorId",
                table: "Entities",
                column: "ReferencesMajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_TypeId",
                table: "Entities",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_ActivatedBy",
                table: "Eservices",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_CreatedBy",
                table: "Eservices",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_DeletedBy",
                table: "Eservices",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_EntityId",
                table: "Eservices",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_ParentId",
                table: "Eservices",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_ReferenceId",
                table: "Eservices",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Eservices_UpdatedBy",
                table: "Eservices",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnswerAction_CreatedBy",
                schema: "Exams",
                table: "ExamAnswerAction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnswerAction_ExamId",
                schema: "Exams",
                table: "ExamAnswerAction",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CreatedBy",
                schema: "Exams",
                table: "Exams",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_DeletedBy",
                schema: "Exams",
                table: "Exams",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_EntityId",
                schema: "Exams",
                table: "Exams",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ReferenceId",
                schema: "Exams",
                table: "Exams",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_UpdatedBy",
                schema: "Exams",
                table: "Exams",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalSites_ActivatedBy",
                table: "ExternalSites",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalSites_CreatedBy",
                table: "ExternalSites",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalSites_DeletedBy",
                table: "ExternalSites",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalSites_EntityId",
                table: "ExternalSites",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalSites_ParentId",
                table: "ExternalSites",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalSites_ReferenceId",
                table: "ExternalSites",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalSites_UpdatedBy",
                table: "ExternalSites",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_CreatedBy",
                table: "FAQ",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_ReferenceId",
                table: "FAQ",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_UpdatedBy",
                table: "FAQ",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ContactUsId",
                schema: "Contact",
                table: "Feedback",
                column: "ContactUsId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowStepperProjects_ProjectId",
                schema: "Permits",
                table: "FlowStepperProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowStepperProjects_StepId",
                schema: "Permits",
                table: "FlowStepperProjects",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputDataSource_DataSourceId",
                schema: "DynamicForm",
                table: "FormInputDataSource",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputDataSource_FormInputId",
                schema: "DynamicForm",
                table: "FormInputDataSource",
                column: "FormInputId");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputs_FormId",
                schema: "DynamicForm",
                table: "FormInputs",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputs_Type",
                schema: "DynamicForm",
                table: "FormInputs",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputsActions_ActionId",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputsActions_CreatedBy",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputsActions_EngineActionJobRoleId",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "EngineActionJobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputsActions_EngineFormId",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "EngineFormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputsActions_FormInputId",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "FormInputId");

            migrationBuilder.CreateIndex(
                name: "IX_FormInputsActions_UpdatedBy",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CreatedBy",
                schema: "DynamicForm",
                table: "Forms",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_FormTypeId",
                schema: "DynamicForm",
                table: "Forms",
                column: "FormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ReferenceId",
                schema: "DynamicForm",
                table: "Forms",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_UpdatedBy",
                schema: "DynamicForm",
                table: "Forms",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FormsDataSource_ParentId",
                schema: "DynamicForm",
                table: "FormsDataSource",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FormsEntity_EntityId",
                schema: "DynamicForm",
                table: "FormsEntity",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FormsEntity_FormId",
                schema: "DynamicForm",
                table: "FormsEntity",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValue_CreatedBy",
                schema: "DynamicForm",
                table: "FormValue",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FormValue_EntityId",
                schema: "DynamicForm",
                table: "FormValue",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValue_FormId",
                schema: "DynamicForm",
                table: "FormValue",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValue_UpdatedBy",
                schema: "DynamicForm",
                table: "FormValue",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FormValueDetails_FormValueId",
                schema: "DynamicForm",
                table: "FormValueDetails",
                column: "FormValueId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValueDetails_InputKey",
                schema: "DynamicForm",
                table: "FormValueDetails",
                column: "InputKey");

            migrationBuilder.CreateIndex(
                name: "IX_FormValuesActions_ActionId",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValuesActions_CreatedBy",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FormValuesActions_EngineActionJobRoleId",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "EngineActionJobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValuesActions_FormValueId",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "FormValueId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValuesActions_FromUserId",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormValuesActions_ToReferenceId",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "ToReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_ActivatedBy",
                table: "GovServices",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_CreatedBy",
                table: "GovServices",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_DeletedBy",
                table: "GovServices",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_ParentID",
                table: "GovServices",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_ReferenceId",
                table: "GovServices",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_GovServices_UpdatedBy",
                table: "GovServices",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjects_ActivatedBy",
                table: "InitiativesProjects",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjects_CreatedBy",
                table: "InitiativesProjects",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjects_DeletedBy",
                table: "InitiativesProjects",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjects_EntityId",
                table: "InitiativesProjects",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjects_ReferenceId",
                table: "InitiativesProjects",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjects_TypeId",
                table: "InitiativesProjects",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjects_UpdatedBy",
                table: "InitiativesProjects",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjectsBeneficiaries_BeneficiaryId",
                table: "InitiativesProjectsBeneficiaries",
                column: "BeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativesProjectsBeneficiaries_InitiativesProjectId",
                table: "InitiativesProjectsBeneficiaries",
                column: "InitiativesProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionStatistics_EntityId",
                table: "InteractionStatistics",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionStatistics_InteractionStatisticsType",
                table: "InteractionStatistics",
                column: "InteractionStatisticsType");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionStatistics_ReferenceId",
                table: "InteractionStatistics",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionStatistics_ReferenceMajorId",
                table: "InteractionStatistics",
                column: "ReferenceMajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_ActivatedBy",
                table: "Investments",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_CreatedBy",
                table: "Investments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_DeletedBy",
                table: "Investments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_OpportunityType",
                table: "Investments",
                column: "OpportunityType");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_UpdatedBy",
                table: "Investments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisement_ActivatedBy",
                schema: "Job",
                table: "JobAdvertisement",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisement_CreatedBy",
                schema: "Job",
                table: "JobAdvertisement",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisement_DeletedBy",
                schema: "Job",
                table: "JobAdvertisement",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisement_EntityId",
                schema: "Job",
                table: "JobAdvertisement",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisement_ReferenceId",
                schema: "Job",
                table: "JobAdvertisement",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisement_UpdatedBy",
                schema: "Job",
                table: "JobAdvertisement",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicationExams_CreatedBy",
                schema: "Job",
                table: "JobApplicationExams",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicationExams_DeletedBy",
                schema: "Job",
                table: "JobApplicationExams",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicationExams_ExamId",
                schema: "Job",
                table: "JobApplicationExams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicationExams_JobApplicationId",
                schema: "Job",
                table: "JobApplicationExams",
                column: "JobApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicationExams_UpdatedBy",
                schema: "Job",
                table: "JobApplicationExams",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ActivatedBy",
                schema: "Job",
                table: "JobApplications",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_DeletedBy",
                schema: "Job",
                table: "JobApplications",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_GradeId",
                schema: "Job",
                table: "JobApplications",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobCareerId",
                schema: "Job",
                table: "JobApplications",
                column: "JobCareerId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_QualificationId",
                schema: "Job",
                table: "JobApplications",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ReferenceId",
                schema: "Job",
                table: "JobApplications",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_SpecialistId",
                schema: "Job",
                table: "JobApplications",
                column: "SpecialistId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UpdatedBy",
                schema: "Job",
                table: "JobApplications",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobCareers_JobAdvertisementId",
                schema: "Job",
                table: "JobCareers",
                column: "JobAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCareers_QualificationId",
                schema: "Job",
                table: "JobCareers",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLookUp_ReferenceId",
                schema: "Job",
                table: "JobLookUp",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_LogInformation_EntityId",
                schema: "OpenData",
                table: "LogInformation",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_LogInformation_ReferenceId",
                schema: "OpenData",
                table: "LogInformation",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_MajorLookups_ParentId",
                table: "MajorLookups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MajorLookups_ReferenceId",
                table: "MajorLookups",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_MajorLookups_TypeId",
                table: "MajorLookups",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ArticleId",
                table: "Menu",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_CreatedBy",
                table: "Menu",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_EntityId",
                table: "Menu",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ParentId",
                table: "Menu",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ReferenceId",
                table: "Menu",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ReferenceMajorId",
                table: "Menu",
                column: "ReferenceMajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_TypeId",
                table: "Menu",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileApplications_ActivatedBy",
                table: "MobileApplications",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MobileApplications_CreatedBy",
                table: "MobileApplications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MobileApplications_DeletedBy",
                table: "MobileApplications",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MobileApplications_UpdatedBy",
                table: "MobileApplications",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedias_ActivatedBy",
                table: "Multimedias",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedias_CreatedBy",
                table: "Multimedias",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedias_DeletedBy",
                table: "Multimedias",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedias_EntityId",
                table: "Multimedias",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedias_ReferenceId",
                table: "Multimedias",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedias_UpdatedBy",
                table: "Multimedias",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_News_ActivatedBy",
                table: "News",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_News_CreatedBy",
                table: "News",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_News_DeletedBy",
                table: "News",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_News_EntityId",
                table: "News",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_News_ReferenceId",
                table: "News",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_News_UpdatedBy",
                table: "News",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Officials_ActivatedBy",
                table: "Officials",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Officials_CreatedBy",
                table: "Officials",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Officials_DeletedBy",
                table: "Officials",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Officials_EntityId",
                table: "Officials",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Officials_ReferenceId",
                table: "Officials",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Officials_UpdatedBy",
                table: "Officials",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OpenData_CreatedBy",
                schema: "OpenData",
                table: "OpenData",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OpenData_DistrictId",
                schema: "OpenData",
                table: "OpenData",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenData_EntityId",
                schema: "OpenData",
                table: "OpenData",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenData_ModifiedBy",
                schema: "OpenData",
                table: "OpenData",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OpenData_ReferenceId",
                schema: "OpenData",
                table: "OpenData",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenData_TypeId",
                schema: "OpenData",
                table: "OpenData",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataRequests_CreatedBy",
                schema: "OpenData",
                table: "OpenDataRequests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataRequests_EntityId",
                schema: "OpenData",
                table: "OpenDataRequests",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataRequests_ModifiedBy",
                schema: "OpenData",
                table: "OpenDataRequests",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataRequests_ReferenceId",
                schema: "OpenData",
                table: "OpenDataRequests",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataStatistics_DistrictId",
                schema: "OpenData",
                table: "OpenDataStatistics",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataStatistics_ReferenceId",
                schema: "OpenData",
                table: "OpenDataStatistics",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataStatistics_TypeId",
                schema: "OpenData",
                table: "OpenDataStatistics",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataTemp_CreatedBy",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataTemp_DistrictId",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataTemp_EntityId",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataTemp_ModifiedBy",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataTemp_ReferenceId",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenDataTemp_TypeId",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatedBy",
                schema: "Orders",
                table: "Order",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeletedBy",
                schema: "Orders",
                table: "Order",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EntityId",
                schema: "Orders",
                table: "Order",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ReferenceId",
                schema: "Orders",
                table: "Order",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_ActivatedBy",
                table: "Partners",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_CreatedBy",
                table: "Partners",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_DeletedBy",
                table: "Partners",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_EntityId",
                table: "Partners",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_ReferenceId",
                table: "Partners",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_UpdatedBy",
                table: "Partners",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermitActions_CreatedBy",
                schema: "Permits",
                table: "PermitActions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermitActions_PermitRequestId",
                schema: "Permits",
                table: "PermitActions",
                column: "PermitRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitActions_UpdatedBy",
                schema: "Permits",
                table: "PermitActions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_ActivatedBy",
                schema: "Permits",
                table: "PermitsRequests",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_CreatedBy",
                schema: "Permits",
                table: "PermitsRequests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_CurrentStep",
                schema: "Permits",
                table: "PermitsRequests",
                column: "CurrentStep");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_DeletedBy",
                schema: "Permits",
                table: "PermitsRequests",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_DeliverReferenceId",
                schema: "Permits",
                table: "PermitsRequests",
                column: "DeliverReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_EntityId",
                schema: "Permits",
                table: "PermitsRequests",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_NextStep",
                schema: "Permits",
                table: "PermitsRequests",
                column: "NextStep");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_ProjectId",
                schema: "Permits",
                table: "PermitsRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_ReferenceId",
                schema: "Permits",
                table: "PermitsRequests",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsRequests_UpdatedBy",
                schema: "Permits",
                table: "PermitsRequests",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsWorkSites_PermitId",
                schema: "Permits",
                table: "PermitsWorkSites",
                column: "PermitId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitsWorkSites_WorksiteId",
                schema: "Permits",
                table: "PermitsWorkSites",
                column: "WorksiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedBy",
                schema: "Permits",
                table: "Projects",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DeletedBy",
                schema: "Permits",
                table: "Projects",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_EntityId",
                schema: "Permits",
                table: "Projects",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UpdatedBy",
                schema: "Permits",
                table: "Projects",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsUsers_ProjectId",
                schema: "Permits",
                table: "ProjectsUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsUsers_UserId",
                schema: "Permits",
                table: "ProjectsUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishEntities_CreatedBy",
                table: "PublishEntities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PublishEntities_EntityId",
                table: "PublishEntities",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishEntities_ReferenceId",
                table: "PublishEntities",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_DataSourceId",
                schema: "Exams",
                table: "QuestionAnswers",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_ExamAnswerActionId",
                schema: "Exams",
                table: "QuestionAnswers",
                column: "ExamAnswerActionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_QuestionId",
                schema: "Exams",
                table: "QuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_DataSourceId",
                schema: "Survey",
                table: "QuestionAnswers",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_QuestionId",
                schema: "Survey",
                table: "QuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_SurveyAnswerActionId",
                schema: "Survey",
                table: "QuestionAnswers",
                column: "SurveyAnswerActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedBy",
                schema: "Exams",
                table: "Questions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DeletedBy",
                schema: "Exams",
                table: "Questions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamId",
                schema: "Exams",
                table: "Questions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TypeId",
                schema: "Exams",
                table: "Questions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UpdatedBy",
                schema: "Exams",
                table: "Questions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedBy",
                schema: "Survey",
                table: "Questions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DeletedBy",
                schema: "Survey",
                table: "Questions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SurveyId",
                schema: "Survey",
                table: "Questions",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TypeId",
                schema: "Survey",
                table: "Questions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UpdatedBy",
                schema: "Survey",
                table: "Questions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsAnswers_CreatedBy",
                table: "QuestionsAnswers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsAnswers_EntityId",
                table: "QuestionsAnswers",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsAnswers_ItemId",
                table: "QuestionsAnswers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsAnswers_ReferenceId",
                table: "QuestionsAnswers",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsAnswers_UpdatedBy",
                table: "QuestionsAnswers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_CreatedBy",
                table: "Rates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_EntityId",
                table: "Rates",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ReferenceId",
                table: "Rates",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceContent_EntityId",
                table: "ReferenceContent",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceContent_ReferenceID",
                table: "ReferenceContent",
                column: "ReferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_References_CreatedBy",
                table: "References",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_References_ParentId",
                table: "References",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_References_ReferencesMajorId",
                table: "References",
                column: "ReferencesMajorId");

            migrationBuilder.CreateIndex(
                name: "IX_References_UpdatedBy",
                table: "References",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesJobRole_JobRoleId",
                table: "ReferencesJobRole",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesJobRole_ReferenceId",
                table: "ReferencesJobRole",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesMajor_CreatedBy",
                table: "ReferencesMajor",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesMajor_EntityId",
                table: "ReferencesMajor",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesMajor_LoginWayId",
                table: "ReferencesMajor",
                column: "LoginWayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferencesMajor_UpdatedBy",
                table: "ReferencesMajor",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ReferenceID",
                table: "Roles",
                column: "ReferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_RolesPermissionLevel_PermissionLevelId",
                table: "RolesPermissionLevel",
                column: "PermissionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesPermissionLevel_RoleId",
                table: "RolesPermissionLevel",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientificLetters_ActivatedBy",
                schema: "ScientificLetters",
                table: "ScientificLetters",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ScientificLetters_CreatedBy",
                schema: "ScientificLetters",
                table: "ScientificLetters",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ScientificLetters_DegreeId",
                schema: "ScientificLetters",
                table: "ScientificLetters",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientificLetters_DeletedBy",
                schema: "ScientificLetters",
                table: "ScientificLetters",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ScientificLetters_EntityId",
                schema: "ScientificLetters",
                table: "ScientificLetters",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientificLetters_ReferenceId",
                schema: "ScientificLetters",
                table: "ScientificLetters",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientificLetters_UpdatedBy",
                schema: "ScientificLetters",
                table: "ScientificLetters",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Session_UserId",
                table: "Session",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_MajorStatusId",
                schema: "Contact",
                table: "Status",
                column: "MajorStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswerAction_CreatedBy",
                schema: "Survey",
                table: "SurveyAnswerAction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswerAction_SurveyId",
                schema: "Survey",
                table: "SurveyAnswerAction",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_CreatedBy",
                schema: "Survey",
                table: "Surveys",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_DeletedBy",
                schema: "Survey",
                table: "Surveys",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_EntityId",
                schema: "Survey",
                table: "Surveys",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_ReferenceId",
                schema: "Survey",
                table: "Surveys",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_UpdatedBy",
                schema: "Survey",
                table: "Surveys",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndRegulations_ActivatedBy",
                table: "TermsAndRegulations",
                column: "ActivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndRegulations_CreatedBy",
                table: "TermsAndRegulations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndRegulations_DeletedBy",
                table: "TermsAndRegulations",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndRegulations_EntityId",
                table: "TermsAndRegulations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndRegulations_ParentId",
                table: "TermsAndRegulations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndRegulations_ReferenceId",
                table: "TermsAndRegulations",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndRegulations_UpdatedBy",
                table: "TermsAndRegulations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReferenceID",
                table: "Users",
                column: "ReferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersEntities_EntityId",
                table: "UsersEntities",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersEntities_UserId",
                table: "UsersEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersEntitiesReferences_EntityId",
                schema: "dbo",
                table: "UsersEntitiesReferences",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersEntitiesReferences_ReferenceId",
                schema: "dbo",
                table: "UsersEntitiesReferences",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersEntitiesReferences_UserId",
                schema: "dbo",
                table: "UsersEntitiesReferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersPermissionLevel_PermissionLevelId",
                table: "UsersPermissionLevel",
                column: "PermissionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersPermissionLevel_UserId",
                table: "UsersPermissionLevel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_AgeID",
                table: "Volunteers",
                column: "AgeID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_DistrictID",
                table: "Volunteers",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_GenderID",
                table: "Volunteers",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_QualificationID",
                table: "Volunteers",
                column: "QualificationID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_VolunteerFieldId",
                table: "Volunteers",
                column: "VolunteerFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowActions_CreatedBy",
                schema: "WorkFlow",
                table: "WorkFlowActions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowActions_ReferenceId",
                schema: "WorkFlow",
                table: "WorkFlowActions",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowActions_UpdatedBy",
                schema: "WorkFlow",
                table: "WorkFlowActions",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionFiles",
                schema: "Contact",
                table: "ActionFiles",
                column: "ActionId",
                principalSchema: "Contact",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionComplain",
                schema: "Contact",
                table: "Actions",
                column: "ContactId",
                principalSchema: "Contact",
                principalTable: "ContactUs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionFromUser",
                schema: "Contact",
                table: "Actions",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserActions",
                schema: "Contact",
                table: "Actions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_References_ToReferenceId",
                schema: "Contact",
                table: "Actions",
                column: "ToReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionType",
                schema: "Orders",
                table: "Actions",
                column: "TypeId",
                principalTable: "MajorLookups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionsOrder",
                schema: "Orders",
                table: "Actions",
                column: "OrderId",
                principalSchema: "Orders",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActions",
                schema: "Orders",
                table: "Actions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminMenu_Entities",
                table: "AdminMenu",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminMenu_References",
                table: "AdminMenu",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminMenu_ReferencesMajor",
                table: "AdminMenu",
                column: "ReferencesMajorId",
                principalTable: "ReferencesMajor",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Entities",
                table: "Advertisements",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_References",
                table: "Advertisements",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Region",
                table: "Advertisements",
                column: "RegionId",
                principalTable: "MajorLookups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_UpdatedBy",
                table: "Advertisements",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_UsersActivatedBy",
                table: "Advertisements",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_UsersCreatedBy",
                table: "Advertisements",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_UsersDeletedBy",
                table: "Advertisements",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AmanahAwards_Entities",
                table: "AmanahAwards",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AmanahAwards_References",
                table: "AmanahAwards",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AmanahAwards_UsersActivatedBy",
                table: "AmanahAwards",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AmanahAwards_UsersCreatedBy",
                table: "AmanahAwards",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AmanahAwards_UsersDeletedBy",
                table: "AmanahAwards",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AmanahAwards_UsersUpdatedBy",
                table: "AmanahAwards",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Entities",
                table: "Articles",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_References",
                table: "Articles",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users",
                table: "Articles",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_UsersCreatedBy",
                table: "Articles",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_UsersPublishedBy",
                table: "Articles",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticlesPublish_References",
                table: "ArticlesPublish",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticlesPublish_ReferencesMajor",
                table: "ArticlesPublish",
                column: "MajorReferenceId",
                principalTable: "ReferencesMajor",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticlesPublish_Users",
                table: "ArticlesPublish",
                column: "PublishedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Entities",
                table: "Attachments",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_MobileApplications",
                table: "Attachments",
                column: "ItemId",
                principalTable: "MobileApplications",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Multimedias",
                table: "Attachments",
                column: "ItemId",
                principalTable: "Multimedias",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_References",
                table: "Attachments",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_UsersCreatedBy",
                table: "Attachments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiaries_UsersCreatedBy",
                table: "Beneficiaries",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiaries_UsersDeletedBy",
                table: "Beneficiaries",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Entities",
                table: "Comments",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_References",
                table: "Comments",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UsersApprovedBy",
                table: "Comments",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UsersCreatedBy",
                table: "Comments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UsersRepliedBy",
                table: "Comments",
                column: "RepliedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUs_CreatedBy",
                schema: "Contact",
                table: "ContactUs",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUs_ModifiedBy",
                schema: "Contact",
                table: "ContactUs",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUs_Users_LastActionUser",
                schema: "Contact",
                table: "ContactUs",
                column: "LastActionUser",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUs_EntityId",
                schema: "Contact",
                table: "ContactUs",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUs_ReferenceId",
                schema: "Contact",
                table: "ContactUs",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUs_References_LastActionReference",
                schema: "Contact",
                table: "ContactUs",
                column: "LastActionReference",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_Questions",
                schema: "Exams",
                table: "DataSource",
                column: "QuestionId",
                principalSchema: "Exams",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_UsersCreatedBy",
                schema: "Exams",
                table: "DataSource",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_UsersDeletedBy",
                schema: "Exams",
                table: "DataSource",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_UsersUpdatedBy",
                schema: "Exams",
                table: "DataSource",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_Questions",
                schema: "Survey",
                table: "DataSource",
                column: "QuestionId",
                principalSchema: "Survey",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_UsersCreatedBy",
                schema: "Survey",
                table: "DataSource",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_UsersDeletedBy",
                schema: "Survey",
                table: "DataSource",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_UsersUpdatedBy",
                schema: "Survey",
                table: "DataSource",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Entities",
                table: "Documents",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_References",
                table: "Documents",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_UsersActivatedBy",
                table: "Documents",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_UsersCreatedBy",
                table: "Documents",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_UsersDeletedBy",
                table: "Documents",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_UsersUpdatedBy",
                table: "Documents",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Engine_References",
                schema: "WorkFlow",
                table: "Engine",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Engine_UsersCreatedBy",
                schema: "WorkFlow",
                table: "Engine",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Engine_UsersUpdatedBy",
                schema: "WorkFlow",
                table: "Engine",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_EnginesActionsJobRole_UsersCreatedBy",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_EnginesActionsJobRole_UsersUpdatedBy",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "Fk_EnginesActionsJobRole_ActionId",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "ActionId",
                principalSchema: "WorkFlow",
                principalTable: "WorkFlowActions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk_EnginesActionsJobRole_CloseStep",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "CloseStep",
                principalSchema: "WorkFlow",
                principalTable: "WorkFlowActions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk_EnginesActionsJobRole_NextStep",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "NextStep",
                principalSchema: "WorkFlow",
                principalTable: "WorkFlowActions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk_EnginesActionsJobRole_RejectStep",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "RejectStep",
                principalSchema: "WorkFlow",
                principalTable: "WorkFlowActions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk_EnginesActionsJobRole_ReturnStep",
                schema: "WorkFlow",
                table: "EngineActionJobRole",
                column: "ReturnStep",
                principalSchema: "WorkFlow",
                principalTable: "WorkFlowActions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EngineForms_Forms_FormId",
                schema: "WorkFlow",
                table: "EngineForms",
                column: "FormId",
                principalSchema: "DynamicForm",
                principalTable: "Forms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_References",
                table: "Entities",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_ReferencesMajor",
                table: "Entities",
                column: "ReferencesMajorId",
                principalTable: "ReferencesMajor",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Eservices_References",
                table: "Eservices",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Eservices_UsersActivatedBy",
                table: "Eservices",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Eservices_UsersCreatedBy",
                table: "Eservices",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Eservices_UsersDeletedBy",
                table: "Eservices",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Eservices_UsersUpdatedBy",
                table: "Eservices",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAnswerAction_Exams",
                schema: "Exams",
                table: "ExamAnswerAction",
                column: "ExamId",
                principalSchema: "Exams",
                principalTable: "Exams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAnswerAction_UsersCreatedBy",
                schema: "Exams",
                table: "ExamAnswerAction",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_References",
                schema: "Exams",
                table: "Exams",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_UsersCreatedBy",
                schema: "Exams",
                table: "Exams",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_UsersDeletedBy",
                schema: "Exams",
                table: "Exams",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_UsersUpdatedBy",
                schema: "Exams",
                table: "Exams",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalSites_References",
                table: "ExternalSites",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalSites_UsersActivatedBy",
                table: "ExternalSites",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalSites_UsersCreatedBy",
                table: "ExternalSites",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalSites_UsersDeletedBy",
                table: "ExternalSites",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalSites_UsersUpdatedBy",
                table: "ExternalSites",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQ_CreatedBy",
                table: "FAQ",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQ_UpdatedBy",
                table: "FAQ",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQ_ReferenceId",
                table: "FAQ",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowStepperProjects_Projects_ProjectId",
                schema: "Permits",
                table: "FlowStepperProjects",
                column: "ProjectId",
                principalSchema: "Permits",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk_DataSource_FormInput",
                schema: "DynamicForm",
                table: "FormInputDataSource",
                column: "FormInputId",
                principalSchema: "DynamicForm",
                principalTable: "FormInputs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk_FormInputs_Form",
                schema: "DynamicForm",
                table: "FormInputs",
                column: "FormId",
                principalSchema: "DynamicForm",
                principalTable: "Forms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormInputsActions_UsersCreatedBy",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormInputsActions_UsersUpdatedBy",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "Fk_FormInputsActions_ActionId",
                schema: "WorkFlow",
                table: "FormInputsActions",
                column: "ActionId",
                principalSchema: "WorkFlow",
                principalTable: "WorkFlowActions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "CreatedByNavigation",
                schema: "DynamicForm",
                table: "Forms",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "UpdatedByNavigation",
                schema: "DynamicForm",
                table: "Forms",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Reference",
                schema: "DynamicForm",
                table: "Forms",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormValue_CreatedByNavigation",
                schema: "DynamicForm",
                table: "FormValue",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormValue_UpdatedByNavigation",
                schema: "DynamicForm",
                table: "FormValue",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormValuesActions_FromUserId",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormValuesActions_UsersCreatedBy",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormValuesActions_References",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "ToReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "Fk_FormValuesActions_ActionId",
                schema: "WorkFlow",
                table: "FormValuesActions",
                column: "ActionId",
                principalSchema: "WorkFlow",
                principalTable: "WorkFlowActions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatedBy",
                table: "GovServices",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GovServices_UsersActivatedBy",
                table: "GovServices",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GovServices_UsersDeletedBy",
                table: "GovServices",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ModifiedBy",
                table: "GovServices",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_RefernceId",
                table: "GovServices",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InitiativesProjects_References",
                table: "InitiativesProjects",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InitiativesProjects_UsersActivatedBy",
                table: "InitiativesProjects",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InitiativesProjects_UsersCreatedBy",
                table: "InitiativesProjects",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InitiativesProjects_UsersDeletedBy",
                table: "InitiativesProjects",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InitiativesProjects_UsersUpdatedBy",
                table: "InitiativesProjects",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InteractionStatistics_References",
                table: "InteractionStatistics",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InteractionStatistics_ReferencesMajor",
                table: "InteractionStatistics",
                column: "ReferenceMajorId",
                principalTable: "ReferencesMajor",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_UsersActivatedBy",
                table: "Investments",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_UsersCreatedBy",
                table: "Investments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_UsersDeletedBy",
                table: "Investments",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_UsersUpdatedBy",
                table: "Investments",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisement_References_ReferenceId",
                schema: "Job",
                table: "JobAdvertisement",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisement_UsersActivatedBy",
                schema: "Job",
                table: "JobAdvertisement",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisement_UsersCreatedBy",
                schema: "Job",
                table: "JobAdvertisement",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisement_UsersDeletedBy",
                schema: "Job",
                table: "JobAdvertisement",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisement_UsersUpdatedBy",
                schema: "Job",
                table: "JobAdvertisement",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicationExams_CreatedBy",
                schema: "Job",
                table: "JobApplicationExams",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicationExams_DeletedBy",
                schema: "Job",
                table: "JobApplicationExams",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicationExams_UpdatedBy",
                schema: "Job",
                table: "JobApplicationExams",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicationExams_JobApplications_JobApplicationId",
                schema: "Job",
                table: "JobApplicationExams",
                column: "JobApplicationId",
                principalSchema: "Job",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobCareers_JobCareerId",
                schema: "Job",
                table: "JobApplications",
                column: "JobCareerId",
                principalSchema: "Job",
                principalTable: "JobCareers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobLookUp_GradeId",
                schema: "Job",
                table: "JobApplications",
                column: "GradeId",
                principalSchema: "Job",
                principalTable: "JobLookUp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobLookUp_SpecialistId",
                schema: "Job",
                table: "JobApplications",
                column: "SpecialistId",
                principalSchema: "Job",
                principalTable: "JobLookUp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_MajorLookups_QualificationId",
                schema: "Job",
                table: "JobApplications",
                column: "QualificationId",
                principalTable: "MajorLookups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_References_ReferenceId",
                schema: "Job",
                table: "JobApplications",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_ActivatedBy",
                schema: "Job",
                table: "JobApplications",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_DeletedBy",
                schema: "Job",
                table: "JobApplications",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_UpdatedBy",
                schema: "Job",
                table: "JobApplications",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCareers_MajorLookups_QualificationId",
                schema: "Job",
                table: "JobCareers",
                column: "QualificationId",
                principalTable: "MajorLookups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobLookUp_References_ReferenceId",
                schema: "Job",
                table: "JobLookUp",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenData_Reference_Log",
                schema: "OpenData",
                table: "LogInformation",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MajorLookups_References",
                table: "MajorLookups",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_References",
                table: "Menu",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_ReferencesMajor",
                table: "Menu",
                column: "ReferenceMajorId",
                principalTable: "ReferencesMajor",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_Users",
                table: "Menu",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileApplications_UsersActivatedBy",
                table: "MobileApplications",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileApplications_UsersCreatedBy",
                table: "MobileApplications",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileApplications_UsersDeletedBy",
                table: "MobileApplications",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileApplications_UsersUpdatedBy",
                table: "MobileApplications",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Multimedias_References",
                table: "Multimedias",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Multimedias_UsersActivatedBy",
                table: "Multimedias",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Multimedias_UsersCreatedBy",
                table: "Multimedias",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Multimedias_UsersDeletedBy",
                table: "Multimedias",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Multimedias_UsersUpdatedBy",
                table: "Multimedias",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_News_References",
                table: "News",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_News_UsersActivatedBy",
                table: "News",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_News_UsersCreatedBy",
                table: "News",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_News_UsersDeletedBy",
                table: "News",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_News_UsersUpdatedBy",
                table: "News",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Officials_CreatedBy",
                table: "Officials",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Officials_DeletedBy",
                table: "Officials",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Officials_UpdatedBy",
                table: "Officials",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Officials_UsersActivatedBy",
                table: "Officials",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Officials_Reference",
                table: "Officials",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenData_CreatedBy",
                schema: "OpenData",
                table: "OpenData",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenData_ModifiedBy",
                schema: "OpenData",
                table: "OpenData",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenData_ReferenceId",
                schema: "OpenData",
                table: "OpenData",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenDataRequest_CreatedBy",
                schema: "OpenData",
                table: "OpenDataRequests",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenDataRequest_ModifiedBy",
                schema: "OpenData",
                table: "OpenDataRequests",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenDataRequest_ReferenceId",
                schema: "OpenData",
                table: "OpenDataRequests",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenDataStatistics_ReferenceId",
                schema: "OpenData",
                table: "OpenDataStatistics",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenDataTemp_CreatedBy",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenDataTemp_ModifiedBy",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenDataTemp_ReferenceId",
                schema: "OpenData",
                table: "OpenDataTemp",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeleteUser",
                schema: "Orders",
                table: "Order",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderUser",
                schema: "Orders",
                table: "Order",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderReference",
                schema: "Orders",
                table: "Order",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_References",
                table: "Partners",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_UsersActivatedBy",
                table: "Partners",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_UsersCreatedBy",
                table: "Partners",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_UsersDeletedBy",
                table: "Partners",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_UsersUpdatedBy",
                table: "Partners",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitActions_PermitsRequests_PermitRequestId",
                schema: "Permits",
                table: "PermitActions",
                column: "PermitRequestId",
                principalSchema: "Permits",
                principalTable: "PermitsRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PtintRequest_UsersCreatedBy",
                schema: "Permits",
                table: "PermitActions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PtintRequest_UsersUpdatedBy",
                schema: "Permits",
                table: "PermitActions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitRequest_UsersActivatedBy",
                schema: "Permits",
                table: "PermitsRequests",
                column: "ActivatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitRequest_UsersCreatedBy",
                schema: "Permits",
                table: "PermitsRequests",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitRequest_UsersDeletedBy",
                schema: "Permits",
                table: "PermitsRequests",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitRequest_UsersUpdatedBy",
                schema: "Permits",
                table: "PermitsRequests",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitsRequests_Projects_ProjectId",
                schema: "Permits",
                table: "PermitsRequests",
                column: "ProjectId",
                principalSchema: "Permits",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitsRequests_References_DeliverReferenceId",
                schema: "Permits",
                table: "PermitsRequests",
                column: "DeliverReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitsRequests_References_ReferenceId",
                schema: "Permits",
                table: "PermitsRequests",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "CreatedByNavigation",
                schema: "Permits",
                table: "Projects",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "DeletedByNavigation",
                schema: "Permits",
                table: "Projects",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "UpdatedByNavigation",
                schema: "Permits",
                table: "Projects",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsUsers_Users_UserId",
                schema: "Permits",
                table: "ProjectsUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "CreatedByNavigation",
                table: "PublishEntities",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reference",
                table: "PublishEntities",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Questions",
                schema: "Exams",
                table: "QuestionAnswers",
                column: "QuestionId",
                principalSchema: "Exams",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Questions",
                schema: "Survey",
                table: "QuestionAnswers",
                column: "QuestionId",
                principalSchema: "Survey",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_SurveyAnswerAction",
                schema: "Survey",
                table: "QuestionAnswers",
                column: "SurveyAnswerActionId",
                principalSchema: "Survey",
                principalTable: "SurveyAnswerAction",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamsQuestion_UsersCreatedBy",
                schema: "Exams",
                table: "Questions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamsQuestion_UsersDeletedBy",
                schema: "Exams",
                table: "Questions",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamsQuestion_UsersUpdatedBy",
                schema: "Exams",
                table: "Questions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Surveys",
                schema: "Survey",
                table: "Questions",
                column: "SurveyId",
                principalSchema: "Survey",
                principalTable: "Surveys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_UsersCreatedBy",
                schema: "Survey",
                table: "Questions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_UsersDeletedBy",
                schema: "Survey",
                table: "Questions",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_UsersUpdatedBy",
                schema: "Survey",
                table: "Questions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsAnswers_References",
                table: "QuestionsAnswers",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsAnswers_UsersCreatedBy",
                table: "QuestionsAnswers",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsAnswers_UsersUpdatedBy",
                table: "QuestionsAnswers",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_References",
                table: "Rates",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Users",
                table: "Rates",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceContent_References",
                table: "ReferenceContent",
                column: "ReferenceID",
                principalTable: "References",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_References_ReferencesMajor",
                table: "References",
                column: "ReferencesMajorId",
                principalTable: "ReferencesMajor",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_References_UsersCreatedBy",
                table: "References",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_References_UsersUpdatedBy",
                table: "References",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactUs_Actions_FirstActionId",
                schema: "Contact",
                table: "ContactUs");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactUs_Actions_LastActionId",
                schema: "Contact",
                table: "ContactUs");

            migrationBuilder.DropForeignKey(
                name: "FK_References_UsersCreatedBy",
                table: "References");

            migrationBuilder.DropForeignKey(
                name: "FK_References_UsersUpdatedBy",
                table: "References");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferencesMajor_UpdatedBy",
                table: "ReferencesMajor");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferencesMajor_UsersCreatedBy",
                table: "ReferencesMajor");

            migrationBuilder.DropForeignKey(
                name: "FK_Entities_References",
                table: "Entities");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferencesMajor_Entities",
                table: "ReferencesMajor");

            migrationBuilder.DropTable(
                name: "ActionFiles",
                schema: "Contact");

            migrationBuilder.DropTable(
                name: "Actions",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "AdminMenu");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "AmanahAwards");

            migrationBuilder.DropTable(
                name: "ArticlesPublish");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "EntitiesLatestUpdate");

            migrationBuilder.DropTable(
                name: "Eservices");

            migrationBuilder.DropTable(
                name: "ExternalSites");

            migrationBuilder.DropTable(
                name: "FAQ");

            migrationBuilder.DropTable(
                name: "Feedback",
                schema: "Contact");

            migrationBuilder.DropTable(
                name: "FlowStepperProjects",
                schema: "Permits");

            migrationBuilder.DropTable(
                name: "FormInputDataSource",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "FormInputsActions",
                schema: "WorkFlow");

            migrationBuilder.DropTable(
                name: "FormsEntity",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "FormValueDetails",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "FormValuesActions",
                schema: "WorkFlow");

            migrationBuilder.DropTable(
                name: "GovServices");

            migrationBuilder.DropTable(
                name: "InitiativesProjectsBeneficiaries");

            migrationBuilder.DropTable(
                name: "InteractionStatistics");

            migrationBuilder.DropTable(
                name: "Investments");

            migrationBuilder.DropTable(
                name: "JobApplicationExams",
                schema: "Job");

            migrationBuilder.DropTable(
                name: "LogInformation",
                schema: "OpenData");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Officials");

            migrationBuilder.DropTable(
                name: "OpenData",
                schema: "OpenData");

            migrationBuilder.DropTable(
                name: "OpenDataRequests",
                schema: "OpenData");

            migrationBuilder.DropTable(
                name: "OpenDataStatistics",
                schema: "OpenData");

            migrationBuilder.DropTable(
                name: "OpenDataTemp",
                schema: "OpenData");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "PermitActions",
                schema: "Permits");

            migrationBuilder.DropTable(
                name: "PermitsWorkSites",
                schema: "Permits");

            migrationBuilder.DropTable(
                name: "ProjectsUsers",
                schema: "Permits");

            migrationBuilder.DropTable(
                name: "PublishEntities");

            migrationBuilder.DropTable(
                name: "QuestionAnswers",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "QuestionAnswers",
                schema: "Survey");

            migrationBuilder.DropTable(
                name: "QuestionsAnswers");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "ReferenceContent");

            migrationBuilder.DropTable(
                name: "ReferencesJobRole");

            migrationBuilder.DropTable(
                name: "RolesPermissionLevel");

            migrationBuilder.DropTable(
                name: "ScientificLetters",
                schema: "ScientificLetters");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TermsAndRegulations");

            migrationBuilder.DropTable(
                name: "UsersEntities");

            migrationBuilder.DropTable(
                name: "UsersEntitiesReferences",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UsersPermissionLevel");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "Multimedias");

            migrationBuilder.DropTable(
                name: "DocumentsType");

            migrationBuilder.DropTable(
                name: "FormsDataSource",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "EngineForms",
                schema: "WorkFlow");

            migrationBuilder.DropTable(
                name: "FormInputs",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "EngineActionJobRole",
                schema: "WorkFlow");

            migrationBuilder.DropTable(
                name: "FormValue",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "Beneficiaries");

            migrationBuilder.DropTable(
                name: "InitiativesProjects");

            migrationBuilder.DropTable(
                name: "InteractionStatisticsType");

            migrationBuilder.DropTable(
                name: "InvestmentTypes");

            migrationBuilder.DropTable(
                name: "JobApplications",
                schema: "Job");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "MenuType");

            migrationBuilder.DropTable(
                name: "PermitsRequests",
                schema: "Permits");

            migrationBuilder.DropTable(
                name: "DataSource",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "ExamAnswerAction",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "DataSource",
                schema: "Survey");

            migrationBuilder.DropTable(
                name: "SurveyAnswerAction",
                schema: "Survey");

            migrationBuilder.DropTable(
                name: "MobileApplications");

            migrationBuilder.DropTable(
                name: "PermissionLevel");

            migrationBuilder.DropTable(
                name: "InputsType",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "JobRole");

            migrationBuilder.DropTable(
                name: "WorkFlowActions",
                schema: "WorkFlow");

            migrationBuilder.DropTable(
                name: "Engine",
                schema: "WorkFlow");

            migrationBuilder.DropTable(
                name: "Forms",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "InitiativesProjectsType");

            migrationBuilder.DropTable(
                name: "JobCareers",
                schema: "Job");

            migrationBuilder.DropTable(
                name: "JobLookUp",
                schema: "Job");

            migrationBuilder.DropTable(
                name: "FlowStepper",
                schema: "Permits");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "Permits");

            migrationBuilder.DropTable(
                name: "Questions",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "Questions",
                schema: "Survey");

            migrationBuilder.DropTable(
                name: "FormType",
                schema: "DynamicForm");

            migrationBuilder.DropTable(
                name: "JobAdvertisement",
                schema: "Job");

            migrationBuilder.DropTable(
                name: "MajorLookups");

            migrationBuilder.DropTable(
                name: "Exams",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "QuestionType",
                schema: "Exams");

            migrationBuilder.DropTable(
                name: "QuestionType",
                schema: "Survey");

            migrationBuilder.DropTable(
                name: "Surveys",
                schema: "Survey");

            migrationBuilder.DropTable(
                name: "MajorLookupsType");

            migrationBuilder.DropTable(
                name: "Actions",
                schema: "Contact");

            migrationBuilder.DropTable(
                name: "ContactUs",
                schema: "Contact");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "Contact");

            migrationBuilder.DropTable(
                name: "MajorStatus",
                schema: "Contact");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "Entities");

            migrationBuilder.DropTable(
                name: "EntitiesType");

            migrationBuilder.DropTable(
                name: "ReferencesMajor");

            migrationBuilder.DropTable(
                name: "LoginWay");
        }
    }
}
