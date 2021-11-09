using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestBackend.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "edm");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "edm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "edm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Owner",
                schema: "edm",
                columns: table => new
                {
                    IdOwner = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.IdOwner);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "edm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "edm",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "edm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "edm",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "edm",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "edm",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "edm",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "edm",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "edm",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "edm",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "edm",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "edm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    IdUser = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalSchema: "edm",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                schema: "edm",
                columns: table => new
                {
                    IdProperty = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CodeInternal = table.Column<string>(maxLength: 20, nullable: false),
                    Year = table.Column<int>(nullable: false),
                    IdOwner = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.IdProperty);
                    table.ForeignKey(
                        name: "FK_Properties_Owner_IdOwner",
                        column: x => x.IdOwner,
                        principalSchema: "edm",
                        principalTable: "Owner",
                        principalColumn: "IdOwner",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImage",
                schema: "edm",
                columns: table => new
                {
                    IdProperyImage = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    File = table.Column<string>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    IdProperty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImage", x => x.IdProperyImage);
                    table.ForeignKey(
                        name: "FK_PropertyImage_Properties_IdProperty",
                        column: x => x.IdProperty,
                        principalSchema: "edm",
                        principalTable: "Properties",
                        principalColumn: "IdProperty",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTrace",
                schema: "edm",
                columns: table => new
                {
                    IdPropertyTrace = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSale = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IdProperty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTrace", x => x.IdPropertyTrace);
                    table.ForeignKey(
                        name: "FK_PropertyTrace_Properties_IdProperty",
                        column: x => x.IdProperty,
                        principalSchema: "edm",
                        principalTable: "Properties",
                        principalColumn: "IdProperty",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "edm",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("286b1076-392e-442f-8226-9c5675cecea8"), "a0349e28-21bd-4093-8ee8-37f8b3f89799", "Asistente", "ASISTENTE" });

            migrationBuilder.InsertData(
                schema: "edm",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("83747102-420d-462f-b038-410d634d02c6"), "b467f111-b5c1-4d43-864b-468597d92904", "Administrador", "ADMINISTRADOR" });

            migrationBuilder.InsertData(
                schema: "edm",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("993b6e19-492a-4e18-815d-d0bd731f31d6"), 0, "42b1ff90-0258-4a33-af2e-97dced88b387", "edwinman1991@gmail.com", true, false, null, "EDWINMAN1991@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEDv8+LH80/PBMGfK+opAEi+QzEBO9m9xZ4xucM373ASs/YAeZ/iHepwtEdtT7RhIvA==", null, false, "7226ebeb-243c-4264-8a31-eac414f0e5ab", false, "admin" });

            migrationBuilder.InsertData(
                schema: "edm",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("993b6e19-492a-4e18-815d-d0bd731f31d6"), new Guid("83747102-420d-462f-b038-410d634d02c6") });

            migrationBuilder.InsertData(
                schema: "edm",
                table: "Person",
                columns: new[] { "Id", "Address", "Email", "FirstName", "IdUser", "LastName", "Phone" },
                values: new object[] { 1, "", "edwinman1991@gmail.com", "Edwin", new Guid("993b6e19-492a-4e18-815d-d0bd731f31d6"), "Mancilla", "3207299734" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "edm",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "edm",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "edm",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "edm",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "edm",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "edm",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "edm",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Person_IdUser",
                schema: "edm",
                table: "Person",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CodeInternal",
                schema: "edm",
                table: "Properties",
                column: "CodeInternal",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_IdOwner",
                schema: "edm",
                table: "Properties",
                column: "IdOwner");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImage_IdProperty",
                schema: "edm",
                table: "PropertyImage",
                column: "IdProperty");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTrace_IdProperty",
                schema: "edm",
                table: "PropertyTrace",
                column: "IdProperty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "PropertyImage",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "PropertyTrace",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "Properties",
                schema: "edm");

            migrationBuilder.DropTable(
                name: "Owner",
                schema: "edm");
        }
    }
}
