using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyStock.Sprint3.Migrations
{
    /// <inheritdoc />
    public partial class Sprint3StockMouvements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MouvementStock",
                columns: table => new
                {
                    MouvementStockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TypeMouvement = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    StockAvant = table.Column<int>(type: "int", nullable: false),
                    StockApres = table.Column<int>(type: "int", nullable: false),
                    ReferenceExterne = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Motif = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EstReapprovisionnement = table.Column<bool>(type: "bit", nullable: false),
                    AlerteStockFaibleDeclenchee = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MouvementStock", x => x.MouvementStockId);
                    table.ForeignKey(
                        name: "FK_MouvementStock_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MouvementStock_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 1,
                column: "Description",
                value: "Portable équipe finance");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 2,
                column: "Description",
                value: "Portable standard équipe opérations");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 3,
                column: "Description",
                value: "Poste fixe réception");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 4,
                column: "Description",
                value: "Poste fixe comptabilité");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 5,
                column: "Description",
                value: "Switch cœur PME 24 ports PoE");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 6,
                column: "Description",
                value: "Point d'accès wifi bureaux");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 8,
                column: "Description",
                value: "NAS sauvegarde département");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 10,
                column: "Description",
                value: "Pièce de rechange portable");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 11,
                column: "Name",
                value: "Écran Dell P2422H");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 15,
                column: "Description",
                value: "Casque calls équipe service client");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 16,
                column: "Description",
                value: "Contrôle accès entrée principale");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 17,
                column: "Description",
                value: "Protection électrique salle serveur");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 19,
                column: "Description",
                value: "Imprimante volume élevé");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 21,
                column: "Description",
                value: "Webcam salle et télétravail");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 22,
                column: "Description",
                value: "Barre vidéo salle réunion");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 24,
                column: "Name",
                value: "Câble réseau Cat6 2m");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 25,
                column: "Description",
                value: "Adaptateur réseau pour portables");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Name",
                value: "Infrastructure réseau");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "Name",
                value: "Périphériques utilisateur");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "Name",
                value: "Sécurité et accès");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 6,
                column: "Name",
                value: "Impression et numérisation");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 7,
                column: "Name",
                value: "Salles de réunion");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 8,
                column: "Name",
                value: "Consommables et pièces");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 3,
                column: "Poste",
                value: "Conseillère RH");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 5,
                column: "Poste",
                value: "Chargée de communication");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 8,
                column: "Poste",
                value: "Administrateur réseau");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 9,
                column: "Poste",
                value: "Adjointe exécutive");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 10,
                column: "Poste",
                value: "Développeur applicatif");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 11,
                columns: new[] { "Departement", "Poste" },
                values: new object[] { "Entrepôt", "Chef d'équipe entrepôt" });

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 14,
                column: "Poste",
                value: "Contrôleure qualité");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 15,
                column: "Poste",
                value: "Représentant comptes PME");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 16,
                column: "Poste",
                value: "Conseillère juridique");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 18,
                column: "Poste",
                value: "Contrôleure financière");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 19,
                column: "Poste",
                value: "Directeur opérations");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 21,
                column: "Poste",
                value: "Analyste sécurité");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 25,
                column: "Departement",
                value: "Entrepôt");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 26,
                column: "Poste",
                value: "Graphiste numérique");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 27,
                column: "Poste",
                value: "Chargé de comptes grands clients");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 29,
                column: "Poste",
                value: "Électromécanicien");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 30,
                column: "Poste",
                value: "Coordonnatrice opérations");

            migrationBuilder.UpdateData(
                table: "Fournisseur",
                keyColumn: "FournisseurId",
                keyValue: 4,
                column: "Address",
                value: "1000 De La Gauchetiere O, Montréal, QC");

            migrationBuilder.UpdateData(
                table: "Fournisseur",
                keyColumn: "FournisseurId",
                keyValue: 5,
                column: "Nom",
                value: "Cisco Partner Québec");

            migrationBuilder.InsertData(
                table: "MouvementStock",
                columns: new[] { "MouvementStockId", "AlerteStockFaibleDeclenchee", "ArticleId", "CreatedAt", "CreatedBy", "EstReapprovisionnement", "Motif", "Quantite", "ReferenceCode", "ReferenceExterne", "StockApres", "StockAvant", "TypeMouvement", "UserId" },
                values: new object[,]
                {
                    { 1, false, 1, new DateTime(2026, 4, 10, 14, 0, 0, 0, DateTimeKind.Utc), "gestionnaire", true, "Réapprovisionnement mensuel du parc finance", 5, "ENT-20260410-00001", "PO-2026-0410", 14, 9, "Entree", 2 },
                    { 2, false, 5, new DateTime(2026, 4, 10, 16, 0, 0, 0, DateTimeKind.Utc), "operateur", false, "Remplacement d'un switch sur un poste d'étage", 1, "SOR-20260410-00002", "INT-2026-0007", 3, 4, "Sortie", 3 },
                    { 3, false, 18, new DateTime(2026, 4, 11, 14, 0, 0, 0, DateTimeKind.Utc), "operateur", false, "Retour d'une imprimante remise en stock", 1, "RET-20260411-00003", "RET-IMPR-104", 3, 2, "Retour", 3 },
                    { 4, false, 11, new DateTime(2026, 4, 11, 18, 0, 0, 0, DateTimeKind.Utc), "gestionnaire", true, "Livraison écrans nouveaux arrivants", 6, "ENT-20260411-00004", "PO-2026-0411", 26, 20, "Entree", 2 },
                    { 5, false, 7, new DateTime(2026, 4, 12, 14, 0, 0, 0, DateTimeKind.Utc), "gestionnaire", false, "Remplacement préventif du pare-feu principal", 1, "SOR-20260412-00005", "SEC-2026-0003", 2, 3, "Sortie", 2 }
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "Description",
                value: "Accès complet à l'administration et à la supervision");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Description",
                value: "Gestion des équipements, mouvements, historique et rapports");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "Description",
                value: "Exploitation courante des équipements, affectations et mouvements");

            migrationBuilder.CreateIndex(
                name: "IX_MouvementStock_ArticleId",
                table: "MouvementStock",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MouvementStock_ReferenceCode",
                table: "MouvementStock",
                column: "ReferenceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MouvementStock_UserId",
                table: "MouvementStock",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MouvementStock");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 1,
                column: "Description",
                value: "Portable equipe finance");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 2,
                column: "Description",
                value: "Portable standard equipe operations");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 3,
                column: "Description",
                value: "Poste fixe reception");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 4,
                column: "Description",
                value: "Poste fixe comptabilite");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 5,
                column: "Description",
                value: "Switch coeur PME 24 ports PoE");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 6,
                column: "Description",
                value: "Point d'acces wifi bureaux");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 8,
                column: "Description",
                value: "NAS sauvegarde departement");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 10,
                column: "Description",
                value: "Piece de rechange portable");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 11,
                column: "Name",
                value: "Ecran Dell P2422H");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 15,
                column: "Description",
                value: "Casque calls equipe service client");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 16,
                column: "Description",
                value: "Controle acces entree principale");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 17,
                column: "Description",
                value: "Protection electrique salle serveur");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 19,
                column: "Description",
                value: "Imprimante volume eleve");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 21,
                column: "Description",
                value: "Webcam salle et teletravail");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 22,
                column: "Description",
                value: "Barre video salle reunion");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 24,
                column: "Name",
                value: "Cable reseau Cat6 2m");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "ArticleId",
                keyValue: 25,
                column: "Description",
                value: "Adaptateur reseau pour portables");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Name",
                value: "Infrastructure reseau");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "Name",
                value: "Peripheriques utilisateur");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "Name",
                value: "Securite et acces");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 6,
                column: "Name",
                value: "Impression et numerisation");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 7,
                column: "Name",
                value: "Salles de reunion");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 8,
                column: "Name",
                value: "Consommables et pieces");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 3,
                column: "Poste",
                value: "Conseillere RH");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 5,
                column: "Poste",
                value: "Chargee de communication");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 8,
                column: "Poste",
                value: "Administrateur reseau");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 9,
                column: "Poste",
                value: "Adjointe executive");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 10,
                column: "Poste",
                value: "Developpeur applicatif");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 11,
                columns: new[] { "Departement", "Poste" },
                values: new object[] { "Entrepot", "Chef d'equipe entrepot" });

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 14,
                column: "Poste",
                value: "Controleure qualite");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 15,
                column: "Poste",
                value: "Representant comptes PME");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 16,
                column: "Poste",
                value: "Conseillere juridique");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 18,
                column: "Poste",
                value: "Controleure financiere");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 19,
                column: "Poste",
                value: "Directeur operations");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 21,
                column: "Poste",
                value: "Analyste securite");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 25,
                column: "Departement",
                value: "Entrepot");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 26,
                column: "Poste",
                value: "Graphiste numerique");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 27,
                column: "Poste",
                value: "Charge de comptes grands clients");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 29,
                column: "Poste",
                value: "Electromecanicien");

            migrationBuilder.UpdateData(
                table: "Employe",
                keyColumn: "EmployeId",
                keyValue: 30,
                column: "Poste",
                value: "Coordonnatrice operations");

            migrationBuilder.UpdateData(
                table: "Fournisseur",
                keyColumn: "FournisseurId",
                keyValue: 4,
                column: "Address",
                value: "1000 De La Gauchetiere O, Montreal, QC");

            migrationBuilder.UpdateData(
                table: "Fournisseur",
                keyColumn: "FournisseurId",
                keyValue: 5,
                column: "Nom",
                value: "Cisco Partner Quebec");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "Description",
                value: "Accès complet à l'administration des comptes");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Description",
                value: "Gestion des équipements, fournisseurs, catégories et affectations");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "Description",
                value: "Exploitation courante des équipements et affectations");
        }
    }
}
