using System.Security.Cryptography;
using System.Text;
using EasyStock.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Employe> Employe { get; set; }
        public DbSet<Fournisseur> Fournisseur { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Affectation> Affectation { get; set; }
        public DbSet<MouvementStock> MouvementStock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var seedDate = new DateTime(2026, 3, 22, 0, 0, 0, DateTimeKind.Utc);
            var movementSeedDate = new DateTime(2026, 4, 10, 14, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Fournisseur)
                .WithMany(f => f.Articles)
                .HasForeignKey(a => a.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employe>()
                .HasIndex(e => e.EmailProfessionnel)
                .IsUnique();

            modelBuilder.Entity<Affectation>()
                .HasOne(a => a.Article)
                .WithMany(a => a.Affectations)
                .HasForeignKey(a => a.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Affectation>()
                .HasOne(a => a.Employe)
                .WithMany(e => e.Affectations)
                .HasForeignKey(a => a.EmployeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Affectation>()
                .HasIndex(a => a.ReferenceCode)
                .IsUnique();

            modelBuilder.Entity<MouvementStock>()
                .HasOne(m => m.Article)
                .WithMany(a => a.MouvementsStock)
                .HasForeignKey(m => m.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MouvementStock>()
                .HasOne(m => m.User)
                .WithMany(u => u.MouvementsStock)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MouvementStock>()
                .HasIndex(m => m.ReferenceCode)
                .IsUnique();

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Accès complet à l'administration et à la supervision" },
                new Role { RoleId = 2, RoleName = "Gestionnaire", Description = "Gestion des équipements, mouvements, historique et rapports" },
                new Role { RoleId = 3, RoleName = "Operateur", Description = "Exploitation courante des équipements, affectations et mouvements" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "admin", Email = "admin@easystock.local", PwdHash = HashPassword("Admin123!"), RoleId = 1, CreatedBy = "system", UpdatedBy = "system", CreatedAt = seedDate, UpdatedAt = seedDate },
                new User { UserId = 2, Username = "gestionnaire", Email = "gestionnaire@easystock.local", PwdHash = HashPassword("Gestion123!"), RoleId = 2, CreatedBy = "system", UpdatedBy = "system", CreatedAt = seedDate, UpdatedAt = seedDate },
                new User { UserId = 3, Username = "operateur", Email = "operateur@easystock.local", PwdHash = HashPassword("Operateur123!"), RoleId = 3, CreatedBy = "system", UpdatedBy = "system", CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<Employe>().HasData(
                new Employe { EmployeId = 1, Prenom = "Amelie", Nom = "Bouchard", EmailProfessionnel = "amelie.bouchard@easystock.local", Departement = "Finance", Poste = "Analyste comptable", IsActive = true },
                new Employe { EmployeId = 2, Prenom = "Samuel", Nom = "Tremblay", EmailProfessionnel = "samuel.tremblay@easystock.local", Departement = "TI", Poste = "Technicien support", IsActive = true },
                new Employe { EmployeId = 3, Prenom = "Nadia", Nom = "Benali", EmailProfessionnel = "nadia.benali@easystock.local", Departement = "Ressources humaines", Poste = "Conseillère RH", IsActive = true },
                new Employe { EmployeId = 4, Prenom = "Karim", Nom = "El Fassi", EmailProfessionnel = "karim.elfassi@easystock.local", Departement = "Logistique", Poste = "Coordonnateur logistique", IsActive = true },
                new Employe { EmployeId = 5, Prenom = "Chloe", Nom = "Gagnon", EmailProfessionnel = "chloe.gagnon@easystock.local", Departement = "Marketing", Poste = "Chargée de communication", IsActive = true },
                new Employe { EmployeId = 6, Prenom = "Rayan", Nom = "Mimouni", EmailProfessionnel = "rayan.mimouni@easystock.local", Departement = "Operations", Poste = "Superviseur de quart", IsActive = true },
                new Employe { EmployeId = 7, Prenom = "Fatima", Nom = "Zahraoui", EmailProfessionnel = "fatima.zahraoui@easystock.local", Departement = "Service client", Poste = "Agente service client", IsActive = true },
                new Employe { EmployeId = 8, Prenom = "Alexandre", Nom = "Roy", EmailProfessionnel = "alexandre.roy@easystock.local", Departement = "TI", Poste = "Administrateur réseau", IsActive = true },
                new Employe { EmployeId = 9, Prenom = "Myriam", Nom = "Lefebvre", EmailProfessionnel = "myriam.lefebvre@easystock.local", Departement = "Direction", Poste = "Adjointe exécutive", IsActive = true },
                new Employe { EmployeId = 10, Prenom = "Omar", Nom = "Haddad", EmailProfessionnel = "omar.haddad@easystock.local", Departement = "TI", Poste = "Développeur applicatif", IsActive = true },
                new Employe { EmployeId = 11, Prenom = "Gabriel", Nom = "Martel", EmailProfessionnel = "gabriel.martel@easystock.local", Departement = "Entrepôt", Poste = "Chef d'équipe entrepôt", IsActive = true },
                new Employe { EmployeId = 12, Prenom = "Yasmine", Nom = "Ait Said", EmailProfessionnel = "yasmine.aitsaid@easystock.local", Departement = "Achats", Poste = "Acheteuse senior", IsActive = true },
                new Employe { EmployeId = 13, Prenom = "Julien", Nom = "Cote", EmailProfessionnel = "julien.cote@easystock.local", Departement = "Maintenance", Poste = "Technicien maintenance", IsActive = true },
                new Employe { EmployeId = 14, Prenom = "Sofia", Nom = "Mansouri", EmailProfessionnel = "sofia.mansouri@easystock.local", Departement = "Qualite", Poste = "Contrôleure qualité", IsActive = true },
                new Employe { EmployeId = 15, Prenom = "Thomas", Nom = "Lavoie", EmailProfessionnel = "thomas.lavoie@easystock.local", Departement = "Ventes", Poste = "Représentant comptes PME", IsActive = true },
                new Employe { EmployeId = 16, Prenom = "Imane", Nom = "Berrada", EmailProfessionnel = "imane.berrada@easystock.local", Departement = "Juridique", Poste = "Conseillère juridique", IsActive = true },
                new Employe { EmployeId = 17, Prenom = "Nicolas", Nom = "Pelletier", EmailProfessionnel = "nicolas.pelletier@easystock.local", Departement = "Production", Poste = "Planificateur production", IsActive = true },
                new Employe { EmployeId = 18, Prenom = "Leila", Nom = "Amrani", EmailProfessionnel = "leila.amrani@easystock.local", Departement = "Finance", Poste = "Contrôleure financière", IsActive = true },
                new Employe { EmployeId = 19, Prenom = "Marc", Nom = "Desjardins", EmailProfessionnel = "marc.desjardins@easystock.local", Departement = "Direction", Poste = "Directeur opérations", IsActive = true },
                new Employe { EmployeId = 20, Prenom = "Sara", Nom = "Khoury", EmailProfessionnel = "sara.khoury@easystock.local", Departement = "Approvisionnement", Poste = "Coordonnatrice approvisionnement", IsActive = true },
                new Employe { EmployeId = 21, Prenom = "William", Nom = "Bergeron", EmailProfessionnel = "william.bergeron@easystock.local", Departement = "TI", Poste = "Analyste sécurité", IsActive = true },
                new Employe { EmployeId = 22, Prenom = "Ines", Nom = "Toumi", EmailProfessionnel = "ines.toumi@easystock.local", Departement = "Service client", Poste = "Superviseure service client", IsActive = true },
                new Employe { EmployeId = 23, Prenom = "Antoine", Nom = "Morin", EmailProfessionnel = "antoine.morin@easystock.local", Departement = "Logistique", Poste = "Analyste transport", IsActive = true },
                new Employe { EmployeId = 24, Prenom = "Meriem", Nom = "Chaoui", EmailProfessionnel = "meriem.chaoui@easystock.local", Departement = "Ressources humaines", Poste = "Partenaire d'affaires RH", IsActive = true },
                new Employe { EmployeId = 25, Prenom = "Cedric", Nom = "Fortin", EmailProfessionnel = "cedric.fortin@easystock.local", Departement = "Entrepôt", Poste = "Cariste principal", IsActive = true },
                new Employe { EmployeId = 26, Prenom = "Aya", Nom = "Meziane", EmailProfessionnel = "aya.meziane@easystock.local", Departement = "Marketing", Poste = "Graphiste numérique", IsActive = true },
                new Employe { EmployeId = 27, Prenom = "Mathieu", Nom = "Girard", EmailProfessionnel = "mathieu.girard@easystock.local", Departement = "Ventes", Poste = "Chargé de comptes grands clients", IsActive = true },
                new Employe { EmployeId = 28, Prenom = "Rim", Nom = "Boukhalfa", EmailProfessionnel = "rim.boukhalfa@easystock.local", Departement = "Qualite", Poste = "Analyste processus", IsActive = true },
                new Employe { EmployeId = 29, Prenom = "Jonathan", Nom = "Levesque", EmailProfessionnel = "jonathan.levesque@easystock.local", Departement = "Maintenance", Poste = "Électromécanicien", IsActive = true },
                new Employe { EmployeId = 30, Prenom = "Nour", Nom = "Rahmani", EmailProfessionnel = "nour.rahmani@easystock.local", Departement = "Operations", Poste = "Coordonnatrice opérations", IsActive = true }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Postes de travail", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { CategoryId = 2, Name = "Infrastructure réseau", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { CategoryId = 3, Name = "Stockage et sauvegarde", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { CategoryId = 4, Name = "Périphériques utilisateur", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { CategoryId = 5, Name = "Sécurité et accès", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { CategoryId = 6, Name = "Impression et numérisation", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { CategoryId = 7, Name = "Salles de réunion", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { CategoryId = 8, Name = "Consommables et pièces", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<Fournisseur>().HasData(
                new Fournisseur { FournisseurId = 1, Nom = "Ingram Micro Canada", ContactEmail = "commandes@ingrammicro.ca", PhoneNumber = "514-555-0101", Address = "16711 Transcanada Hwy, Kirkland, QC", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Fournisseur { FournisseurId = 2, Nom = "CDW Canada", ContactEmail = "ventes@cdw.ca", PhoneNumber = "416-555-0102", Address = "200 Wellington St W, Toronto, ON", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Fournisseur { FournisseurId = 3, Nom = "Dell Technologies", ContactEmail = "pmecanada@dell.com", PhoneNumber = "800-555-0103", Address = "155 Gordon Baker Rd, Toronto, ON", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Fournisseur { FournisseurId = 4, Nom = "HP Entreprise", ContactEmail = "comptes@hp.com", PhoneNumber = "514-555-0104", Address = "1000 De La Gauchetiere O, Montréal, QC", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Fournisseur { FournisseurId = 5, Nom = "Cisco Partner Québec", ContactEmail = "reseau@ciscopartner.ca", PhoneNumber = "450-555-0105", Address = "4500 Boul. Matte, Brossard, QC", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Fournisseur { FournisseurId = 6, Nom = "SoftChoice", ContactEmail = "appro@softchoice.com", PhoneNumber = "416-555-0106", Address = "173 Dufferin St, Toronto, ON", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Fournisseur { FournisseurId = 7, Nom = "Staples Professionnel", ContactEmail = "b2b@staples.ca", PhoneNumber = "877-555-0107", Address = "5500 Explorer Dr, Mississauga, ON", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Fournisseur { FournisseurId = 8, Nom = "Lenovo Canada", ContactEmail = "proaccounts@lenovo.ca", PhoneNumber = "866-555-0108", Address = "151 Yonge St, Toronto, ON", CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<Article>().HasData(
                new Article { ArticleId = 1, Name = "Latitude 5540 - i7/16Go/512Go", Description = "Portable équipe finance", CategoryId = 1, FournisseurId = 3, NombreArticleActuel = 14, NombreArticleMinimum = 5, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 2, Name = "ThinkPad T14 Gen 4", Description = "Portable standard équipe opérations", CategoryId = 1, FournisseurId = 8, NombreArticleActuel = 11, NombreArticleMinimum = 4, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 3, Name = "OptiPlex 7010 SFF", Description = "Poste fixe réception", CategoryId = 1, FournisseurId = 3, NombreArticleActuel = 6, NombreArticleMinimum = 2, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 4, Name = "EliteDesk 800 G9", Description = "Poste fixe comptabilité", CategoryId = 1, FournisseurId = 4, NombreArticleActuel = 5, NombreArticleMinimum = 2, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 5, Name = "Cisco Catalyst C9200L-24P", Description = "Switch cœur PME 24 ports PoE", CategoryId = 2, FournisseurId = 5, NombreArticleActuel = 3, NombreArticleMinimum = 1, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 6, Name = "Ubiquiti UniFi U6-LR", Description = "Point d'accès wifi bureaux", CategoryId = 2, FournisseurId = 2, NombreArticleActuel = 12, NombreArticleMinimum = 4, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 7, Name = "Firewall FortiGate 60F", Description = "Pare-feu principal site", CategoryId = 5, FournisseurId = 6, NombreArticleActuel = 2, NombreArticleMinimum = 1, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 8, Name = "Synology DS923+ NAS", Description = "NAS sauvegarde département", CategoryId = 3, FournisseurId = 1, NombreArticleActuel = 4, NombreArticleMinimum = 2, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 9, Name = "Disque WD Red 8To", Description = "Disque pour NAS RAID", CategoryId = 3, FournisseurId = 1, NombreArticleActuel = 18, NombreArticleMinimum = 6, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 10, Name = "SSD NVMe 1To Samsung 990", Description = "Pièce de rechange portable", CategoryId = 8, FournisseurId = 2, NombreArticleActuel = 20, NombreArticleMinimum = 8, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 11, Name = "Écran Dell P2422H", Description = "Moniteur 24 pouces bureau", CategoryId = 4, FournisseurId = 3, NombreArticleActuel = 26, NombreArticleMinimum = 10, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 12, Name = "Station d'accueil Dell WD22TB4", Description = "Dock USB-C/TB pour portables", CategoryId = 4, FournisseurId = 3, NombreArticleActuel = 13, NombreArticleMinimum = 5, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 13, Name = "Clavier Logitech K280e", Description = "Clavier bureautique USB", CategoryId = 4, FournisseurId = 7, NombreArticleActuel = 44, NombreArticleMinimum = 15, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 14, Name = "Souris Logitech M500s", Description = "Souris filaire bureautique", CategoryId = 4, FournisseurId = 7, NombreArticleActuel = 39, NombreArticleMinimum = 12, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 15, Name = "Casque Jabra Evolve2 40", Description = "Casque calls équipe service client", CategoryId = 4, FournisseurId = 6, NombreArticleActuel = 17, NombreArticleMinimum = 6, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 16, Name = "Lecteur badge HID Signo 20", Description = "Contrôle accès entrée principale", CategoryId = 5, FournisseurId = 6, NombreArticleActuel = 7, NombreArticleMinimum = 3, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 17, Name = "Onduleur APC Smart-UPS 1500", Description = "Protection électrique salle serveur", CategoryId = 5, FournisseurId = 1, NombreArticleActuel = 4, NombreArticleMinimum = 2, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 18, Name = "HP LaserJet Pro MFP 4101", Description = "Imprimante multifonction RH", CategoryId = 6, FournisseurId = 4, NombreArticleActuel = 3, NombreArticleMinimum = 1, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 19, Name = "Brother HL-L6400DW", Description = "Imprimante volume élevé", CategoryId = 6, FournisseurId = 2, NombreArticleActuel = 2, NombreArticleMinimum = 1, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 20, Name = "Cartouche toner HP 89X", Description = "Consommable imprimante HP", CategoryId = 8, FournisseurId = 7, NombreArticleActuel = 28, NombreArticleMinimum = 10, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 21, Name = "Webcam Logitech Brio", Description = "Webcam salle et télétravail", CategoryId = 7, FournisseurId = 6, NombreArticleActuel = 9, NombreArticleMinimum = 3, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 22, Name = "Poly Studio USB", Description = "Barre vidéo salle réunion", CategoryId = 7, FournisseurId = 6, NombreArticleActuel = 3, NombreArticleMinimum = 1, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 23, Name = "TV Samsung 65 pouces 4K", Description = "Affichage principal salle B", CategoryId = 7, FournisseurId = 2, NombreArticleActuel = 2, NombreArticleMinimum = 1, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 24, Name = "Câble réseau Cat6 2m", Description = "Patch cord postes et baies", CategoryId = 8, FournisseurId = 7, NombreArticleActuel = 120, NombreArticleMinimum = 40, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Article { ArticleId = 25, Name = "Adaptateur USB-C vers RJ45", Description = "Adaptateur réseau pour portables", CategoryId = 8, FournisseurId = 2, NombreArticleActuel = 31, NombreArticleMinimum = 12, CreatedBy = "seed", UpdatedBy = "seed", CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<MouvementStock>().HasData(
                new MouvementStock { MouvementStockId = 1, ReferenceCode = "ENT-20260410-00001", ArticleId = 1, UserId = 2, TypeMouvement = MouvementType.Entree, Quantite = 5, StockAvant = 9, StockApres = 14, ReferenceExterne = "PO-2026-0410", Motif = "Réapprovisionnement mensuel du parc finance", EstReapprovisionnement = true, AlerteStockFaibleDeclenchee = false, CreatedBy = "gestionnaire", CreatedAt = movementSeedDate },
                new MouvementStock { MouvementStockId = 2, ReferenceCode = "SOR-20260410-00002", ArticleId = 5, UserId = 3, TypeMouvement = MouvementType.Sortie, Quantite = 1, StockAvant = 4, StockApres = 3, ReferenceExterne = "INT-2026-0007", Motif = "Remplacement d'un switch sur un poste d'étage", EstReapprovisionnement = false, AlerteStockFaibleDeclenchee = false, CreatedBy = "operateur", CreatedAt = movementSeedDate.AddHours(2) },
                new MouvementStock { MouvementStockId = 3, ReferenceCode = "RET-20260411-00003", ArticleId = 18, UserId = 3, TypeMouvement = MouvementType.Retour, Quantite = 1, StockAvant = 2, StockApres = 3, ReferenceExterne = "RET-IMPR-104", Motif = "Retour d'une imprimante remise en stock", EstReapprovisionnement = false, AlerteStockFaibleDeclenchee = false, CreatedBy = "operateur", CreatedAt = movementSeedDate.AddDays(1) },
                new MouvementStock { MouvementStockId = 4, ReferenceCode = "ENT-20260411-00004", ArticleId = 11, UserId = 2, TypeMouvement = MouvementType.Entree, Quantite = 6, StockAvant = 20, StockApres = 26, ReferenceExterne = "PO-2026-0411", Motif = "Livraison écrans nouveaux arrivants", EstReapprovisionnement = true, AlerteStockFaibleDeclenchee = false, CreatedBy = "gestionnaire", CreatedAt = movementSeedDate.AddDays(1).AddHours(4) },
                new MouvementStock { MouvementStockId = 5, ReferenceCode = "SOR-20260412-00005", ArticleId = 7, UserId = 2, TypeMouvement = MouvementType.Sortie, Quantite = 1, StockAvant = 3, StockApres = 2, ReferenceExterne = "SEC-2026-0003", Motif = "Remplacement préventif du pare-feu principal", EstReapprovisionnement = false, AlerteStockFaibleDeclenchee = false, CreatedBy = "gestionnaire", CreatedAt = movementSeedDate.AddDays(2) }
            );
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
