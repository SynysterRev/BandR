using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BandR.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "instruments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsValidated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instruments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "styles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsValidated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_styles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsValidated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "musicians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musicians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_musicians_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_musicians_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MusicianId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_announcements_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_announcements_musicians_MusicianId",
                        column: x => x.MusicianId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "musician_instruments",
                columns: table => new
                {
                    MusicianId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musician_instruments", x => new { x.MusicianId, x.InstrumentId });
                    table.ForeignKey(
                        name: "FK_musician_instruments_instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_musician_instruments_musicians_MusicianId",
                        column: x => x.MusicianId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "musician_styles",
                columns: table => new
                {
                    MusiciansId = table.Column<Guid>(type: "uuid", nullable: false),
                    StylesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musician_styles", x => new { x.MusiciansId, x.StylesId });
                    table.ForeignKey(
                        name: "FK_musician_styles_musicians_MusiciansId",
                        column: x => x.MusiciansId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_musician_styles_styles_StylesId",
                        column: x => x.StylesId,
                        principalTable: "styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "musician_tags",
                columns: table => new
                {
                    MusiciansId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musician_tags", x => new { x.MusiciansId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_musician_tags_musicians_MusiciansId",
                        column: x => x.MusiciansId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_musician_tags_tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "announcement_instruments",
                columns: table => new
                {
                    AnnouncementId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcement_instruments", x => new { x.AnnouncementId, x.InstrumentId });
                    table.ForeignKey(
                        name: "FK_announcement_instruments_announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_announcement_instruments_instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "announcement_styles",
                columns: table => new
                {
                    AnnouncementsId = table.Column<Guid>(type: "uuid", nullable: false),
                    StylesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcement_styles", x => new { x.AnnouncementsId, x.StylesId });
                    table.ForeignKey(
                        name: "FK_announcement_styles_announcements_AnnouncementsId",
                        column: x => x.AnnouncementsId,
                        principalTable: "announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_announcement_styles_styles_StylesId",
                        column: x => x.StylesId,
                        principalTable: "styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "announcement_tags",
                columns: table => new
                {
                    AnnouncementsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcement_tags", x => new { x.AnnouncementsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_announcement_tags_announcements_AnnouncementsId",
                        column: x => x.AnnouncementsId,
                        principalTable: "announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_announcement_tags_tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnnouncementId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_conversations_announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "announcements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_messages_conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_musicians_SenderId",
                        column: x => x.SenderId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "musician_conversation",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MusicianId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musician_conversation", x => new { x.MusicianId, x.ConversationId });
                    table.ForeignKey(
                        name: "FK_musician_conversation_conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_musician_conversation_musicians_MusicianId",
                        column: x => x.MusicianId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "instruments",
                columns: new[] { "Id", "CreatedAt", "IsValidated", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Acoustic Guitar", null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Electric Guitar", null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Bass Guitar", null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Violin", null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Cello", null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Ukulele", null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Piano", null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Keyboard", null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Synthesizer", null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Organ", null },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Drums", null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Percussion", null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Cajon", null },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Djembe", null },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Saxophone", null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Trumpet", null },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Flute", null },
                    { new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Clarinet", null },
                    { new Guid("40000000-0000-0000-0000-000000000005"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Trombone", null },
                    { new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Harmonica", null },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Vocals", null },
                    { new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "DJ Controller", null },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Turntables", null },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Drum Machine", null },
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Accordion", null },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Banjo", null },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Mandolin", null }
                });

            migrationBuilder.InsertData(
                table: "styles",
                columns: new[] { "Id", "CreatedAt", "IsValidated", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Rock", null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Hard Rock", null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Punk Rock", null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Alternative Rock", null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Indie Rock", null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Metal", null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Heavy Metal", null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Death Metal", null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Black Metal", null },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Pop", null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Pop Rock", null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Electro Pop", null },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Electronic", null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "House", null },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Techno", null },
                    { new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Dubstep", null },
                    { new Guid("40000000-0000-0000-0000-000000000005"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Trance", null },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Hip Hop", null },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Rap", null },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Trap", null },
                    { new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Jazz", null },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Blues", null },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Soul", null },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Funk", null },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "R&B", null },
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Folk", null },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Country", null },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Bluegrass", null },
                    { new Guid("80000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Reggae", null },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Ska", null },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Latin", null },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Salsa", null },
                    { new Guid("90000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Classical", null },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Opera", null },
                    { new Guid("a0000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Experimental", null },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Ambient", null },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "World Music", null }
                });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "Id", "CreatedAt", "IsValidated", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Beginner", null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Intermediate", null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Advanced", null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Professional", null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Looking for a band", null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Looking for a jam session", null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Open to collaborations", null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Available quickly", null },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Studio", null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Live / Stage", null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Rehearsal", null },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Recording", null },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Cover", null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Original compositions", null },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), true, "Improvisation", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_announcement_instruments_InstrumentId",
                table: "announcement_instruments",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_announcement_styles_StylesId",
                table: "announcement_styles",
                column: "StylesId");

            migrationBuilder.CreateIndex(
                name: "IX_announcement_tags_TagsId",
                table: "announcement_tags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_announcements_LocationId",
                table: "announcements",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_announcements_MusicianId",
                table: "announcements",
                column: "MusicianId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_conversations_AnnouncementId",
                table: "conversations",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_instruments_Name",
                table: "instruments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_messages_ConversationId",
                table: "messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_SenderId",
                table: "messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_musician_conversation_ConversationId",
                table: "musician_conversation",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_musician_instruments_InstrumentId",
                table: "musician_instruments",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_musician_styles_StylesId",
                table: "musician_styles",
                column: "StylesId");

            migrationBuilder.CreateIndex(
                name: "IX_musician_tags_TagsId",
                table: "musician_tags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_musicians_AppUserId",
                table: "musicians",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_musicians_LocationId",
                table: "musicians",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_styles_Name",
                table: "styles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_Name",
                table: "tags",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcement_instruments");

            migrationBuilder.DropTable(
                name: "announcement_styles");

            migrationBuilder.DropTable(
                name: "announcement_tags");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "musician_conversation");

            migrationBuilder.DropTable(
                name: "musician_instruments");

            migrationBuilder.DropTable(
                name: "musician_styles");

            migrationBuilder.DropTable(
                name: "musician_tags");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "conversations");

            migrationBuilder.DropTable(
                name: "instruments");

            migrationBuilder.DropTable(
                name: "styles");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropTable(
                name: "musicians");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "locations");
        }
    }
}
