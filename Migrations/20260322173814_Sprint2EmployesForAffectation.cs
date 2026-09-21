using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyStock.Sprint3.Migrations
{
    /// <inheritdoc />
    public partial class Sprint2EmployesForAffectation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affectation_User_UserId",
                table: "Affectation");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Affectation",
                newName: "EmployeId");

            migrationBuilder.RenameIndex(
                name: "IX_Affectation_UserId",
                table: "Affectation",
                newName: "IX_Affectation_EmployeId");

            migrationBuilder.CreateTable(
                name: "Employe",
                columns: table => new
                {
                    EmployeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailProfessionnel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Departement = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Poste = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employe", x => x.EmployeId);
                });

            migrationBuilder.InsertData(
                table: "Employe",
                columns: new[] { "EmployeId", "Departement", "EmailProfessionnel", "IsActive", "Nom", "Poste", "Prenom" },
                values: new object[,]
                {
                    { 1, "Finance", "amelie.bouchard@easystock.local", true, "Bouchard", "Analyste comptable", "Amelie" },
                    { 2, "TI", "samuel.tremblay@easystock.local", true, "Tremblay", "Technicien support", "Samuel" },
                    { 3, "Ressources humaines", "nadia.benali@easystock.local", true, "Benali", "Conseillere RH", "Nadia" },
                    { 4, "Logistique", "karim.elfassi@easystock.local", true, "El Fassi", "Coordonnateur logistique", "Karim" },
                    { 5, "Marketing", "chloe.gagnon@easystock.local", true, "Gagnon", "Chargee de communication", "Chloe" },
                    { 6, "Operations", "rayan.mimouni@easystock.local", true, "Mimouni", "Superviseur de quart", "Rayan" },
                    { 7, "Service client", "fatima.zahraoui@easystock.local", true, "Zahraoui", "Agente service client", "Fatima" },
                    { 8, "TI", "alexandre.roy@easystock.local", true, "Roy", "Administrateur reseau", "Alexandre" },
                    { 9, "Direction", "myriam.lefebvre@easystock.local", true, "Lefebvre", "Adjointe executive", "Myriam" },
                    { 10, "TI", "omar.haddad@easystock.local", true, "Haddad", "Developpeur applicatif", "Omar" },
                    { 11, "Entrepot", "gabriel.martel@easystock.local", true, "Martel", "Chef d'equipe entrepot", "Gabriel" },
                    { 12, "Achats", "yasmine.aitsaid@easystock.local", true, "Ait Said", "Acheteuse senior", "Yasmine" },
                    { 13, "Maintenance", "julien.cote@easystock.local", true, "Cote", "Technicien maintenance", "Julien" },
                    { 14, "Qualite", "sofia.mansouri@easystock.local", true, "Mansouri", "Controleure qualite", "Sofia" },
                    { 15, "Ventes", "thomas.lavoie@easystock.local", true, "Lavoie", "Representant comptes PME", "Thomas" },
                    { 16, "Juridique", "imane.berrada@easystock.local", true, "Berrada", "Conseillere juridique", "Imane" },
                    { 17, "Production", "nicolas.pelletier@easystock.local", true, "Pelletier", "Planificateur production", "Nicolas" },
                    { 18, "Finance", "leila.amrani@easystock.local", true, "Amrani", "Controleure financiere", "Leila" },
                    { 19, "Direction", "marc.desjardins@easystock.local", true, "Desjardins", "Directeur operations", "Marc" },
                    { 20, "Approvisionnement", "sara.khoury@easystock.local", true, "Khoury", "Coordonnatrice approvisionnement", "Sara" },
                    { 21, "TI", "william.bergeron@easystock.local", true, "Bergeron", "Analyste securite", "William" },
                    { 22, "Service client", "ines.toumi@easystock.local", true, "Toumi", "Superviseure service client", "Ines" },
                    { 23, "Logistique", "antoine.morin@easystock.local", true, "Morin", "Analyste transport", "Antoine" },
                    { 24, "Ressources humaines", "meriem.chaoui@easystock.local", true, "Chaoui", "Partenaire d'affaires RH", "Meriem" },
                    { 25, "Entrepot", "cedric.fortin@easystock.local", true, "Fortin", "Cariste principal", "Cedric" },
                    { 26, "Marketing", "aya.meziane@easystock.local", true, "Meziane", "Graphiste numerique", "Aya" },
                    { 27, "Ventes", "mathieu.girard@easystock.local", true, "Girard", "Charge de comptes grands clients", "Mathieu" },
                    { 28, "Qualite", "rim.boukhalfa@easystock.local", true, "Boukhalfa", "Analyste processus", "Rim" },
                    { 29, "Maintenance", "jonathan.levesque@easystock.local", true, "Levesque", "Electromecanicien", "Jonathan" },
                    { 30, "Operations", "nour.rahmani@easystock.local", true, "Rahmani", "Coordonnatrice operations", "Nour" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Employe_EmailProfessionnel",
                table: "Employe",
                column: "EmailProfessionnel",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Affectation_Employe_EmployeId",
                table: "Affectation",
                column: "EmployeId",
                principalTable: "Employe",
                principalColumn: "EmployeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affectation_Employe_EmployeId",
                table: "Affectation");

            migrationBuilder.DropTable(
                name: "Employe");

            migrationBuilder.RenameColumn(
                name: "EmployeId",
                table: "Affectation",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Affectation_EmployeId",
                table: "Affectation",
                newName: "IX_Affectation_UserId");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "Description",
                value: "Accès complet à l'application");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Description",
                value: "Gestion des opérations et consultation des utilisateurs");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "Description",
                value: "Gestion opérationnelle de l'inventaire et des affectations");

            migrationBuilder.AddForeignKey(
                name: "FK_Affectation_User_UserId",
                table: "Affectation",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
