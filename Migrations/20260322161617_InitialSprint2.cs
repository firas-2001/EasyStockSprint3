using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyStock.Sprint3.Migrations
{
    /// <inheritdoc />
    public partial class InitialSprint2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Fournisseur",
                columns: table => new
                {
                    FournisseurId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseur", x => x.FournisseurId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    NombreArticleActuel = table.Column<int>(type: "int", nullable: false),
                    NombreArticleMinimum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Article_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Article_Fournisseur_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "Fournisseur",
                        principalColumn: "FournisseurId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PwdHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Affectation",
                columns: table => new
                {
                    AffectationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EtatSortie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EtatRetour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affectation", x => x.AffectationId);
                    table.ForeignKey(
                        name: "FK_Affectation_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Affectation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CreatedAt", "CreatedBy", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Postes de travail", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Infrastructure reseau", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 3, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Stockage et sauvegarde", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Peripheriques utilisateur", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 5, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Securite et acces", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 6, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Impression et numerisation", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 7, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Salles de reunion", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 8, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Consommables et pieces", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" }
                });

            migrationBuilder.InsertData(
                table: "Fournisseur",
                columns: new[] { "FournisseurId", "Address", "ContactEmail", "CreatedAt", "CreatedBy", "Nom", "PhoneNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "16711 Transcanada Hwy, Kirkland, QC", "commandes@ingrammicro.ca", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Ingram Micro Canada", "514-555-0101", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 2, "200 Wellington St W, Toronto, ON", "ventes@cdw.ca", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "CDW Canada", "416-555-0102", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 3, "155 Gordon Baker Rd, Toronto, ON", "pmecanada@dell.com", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Dell Technologies", "800-555-0103", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 4, "1000 De La Gauchetiere O, Montreal, QC", "comptes@hp.com", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "HP Entreprise", "514-555-0104", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 5, "4500 Boul. Matte, Brossard, QC", "reseau@ciscopartner.ca", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Cisco Partner Quebec", "450-555-0105", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 6, "173 Dufferin St, Toronto, ON", "appro@softchoice.com", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "SoftChoice", "416-555-0106", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 7, "5500 Explorer Dr, Mississauga, ON", "b2b@staples.ca", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Staples Professionnel", "877-555-0107", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 8, "151 Yonge St, Toronto, ON", "proaccounts@lenovo.ca", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Lenovo Canada", "866-555-0108", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "Description", "RoleName" },
                values: new object[,]
                {
                    { 1, "Accès complet à l'application", "Admin" },
                    { 2, "Gestion des opérations et consultation des utilisateurs", "Gestionnaire" },
                    { 3, "Gestion opérationnelle de l'inventaire et des affectations", "Operateur" }
                });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "ArticleId", "CategoryId", "CreatedAt", "CreatedBy", "Description", "FournisseurId", "Name", "NombreArticleActuel", "NombreArticleMinimum", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Portable equipe finance", 3, "Latitude 5540 - i7/16Go/512Go", 14, 5, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 2, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Portable standard equipe operations", 8, "ThinkPad T14 Gen 4", 11, 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 3, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Poste fixe reception", 3, "OptiPlex 7010 SFF", 6, 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 4, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Poste fixe comptabilite", 4, "EliteDesk 800 G9", 5, 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 5, 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Switch coeur PME 24 ports PoE", 5, "Cisco Catalyst C9200L-24P", 3, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 6, 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Point d'acces wifi bureaux", 2, "Ubiquiti UniFi U6-LR", 12, 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 7, 5, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Pare-feu principal site", 6, "Firewall FortiGate 60F", 2, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 8, 3, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "NAS sauvegarde departement", 1, "Synology DS923+ NAS", 4, 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 9, 3, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Disque pour NAS RAID", 1, "Disque WD Red 8To", 18, 6, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 10, 8, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Piece de rechange portable", 2, "SSD NVMe 1To Samsung 990", 20, 8, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 11, 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Moniteur 24 pouces bureau", 3, "Ecran Dell P2422H", 26, 10, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 12, 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Dock USB-C/TB pour portables", 3, "Station d'accueil Dell WD22TB4", 13, 5, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 13, 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Clavier bureautique USB", 7, "Clavier Logitech K280e", 44, 15, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 14, 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Souris filaire bureautique", 7, "Souris Logitech M500s", 39, 12, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 15, 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Casque calls equipe service client", 6, "Casque Jabra Evolve2 40", 17, 6, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 16, 5, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Controle acces entree principale", 6, "Lecteur badge HID Signo 20", 7, 3, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 17, 5, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Protection electrique salle serveur", 1, "Onduleur APC Smart-UPS 1500", 4, 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 18, 6, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Imprimante multifonction RH", 4, "HP LaserJet Pro MFP 4101", 3, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 19, 6, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Imprimante volume eleve", 2, "Brother HL-L6400DW", 2, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 20, 8, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Consommable imprimante HP", 7, "Cartouche toner HP 89X", 28, 10, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 21, 7, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Webcam salle et teletravail", 6, "Webcam Logitech Brio", 9, 3, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 22, 7, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Barre video salle reunion", 6, "Poly Studio USB", 3, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 23, 7, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Affichage principal salle B", 2, "TV Samsung 65 pouces 4K", 2, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 24, 8, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Patch cord postes et baies", 7, "Cable reseau Cat6 2m", 120, 40, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" },
                    { 25, 8, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Adaptateur reseau pour portables", 2, "Adaptateur USB-C vers RJ45", 31, 12, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "seed" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "CreatedAt", "CreatedBy", "Email", "PwdHash", "RoleId", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "system", "admin@easystock.local", "3EB3FE66B31E3B4D10FA70B5CAD49C7112294AF6AE4E476A1C405155D45AA121", 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "system", "admin" },
                    { 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "system", "gestionnaire@easystock.local", "14CB1392E34869C5AD54C212F8D4DB51C78A851675E5F4CDED2C550A5FC92792", 2, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "system", "gestionnaire" },
                    { 3, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "system", "operateur@easystock.local", "C92285D3BAA1271D81C07697A2E39986146D05563EF07ECBD7E291E57420BF10", 3, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "system", "operateur" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_ArticleId",
                table: "Affectation",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_ReferenceCode",
                table: "Affectation",
                column: "ReferenceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_UserId",
                table: "Affectation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_CategoryId",
                table: "Article",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_FournisseurId",
                table: "Article",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Affectation");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Fournisseur");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
