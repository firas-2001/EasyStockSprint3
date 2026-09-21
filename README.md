# EasyStock.Sprint3

EasyStock.Sprint3 est la version pédagogique du projet EasyStock enrichie avec les fonctionnalités du Sprint 3, tout en conservant intégralement les comportements validés des Sprints 1 et 2.

## Périmètre fonctionnel

### Modules conservés des Sprints 1 et 2
- Gestion des équipements
- Gestion des catégories
- Gestion des fournisseurs
- Gestion des utilisateurs
- Authentification / rôles / permissions
- Affectation et désaffectation des équipements
- Historique d'affectation

### Modules ajoutés au Sprint 3
- Enregistrement des entrées de stock
- Enregistrement des sorties de stock avec contrôle de disponibilité
- Enregistrement des retours de stock
- Alerte visuelle stock faible dans les listes d'équipements et de mouvements
- Notification courriel de stock faible, configurable
- Historique des mouvements de stock
- Filtrage de l'historique par date, équipement, type et utilisateur
- Historique des réapprovisionnements
- Rapports et exports de l'inventaire, du stock faible, des mouvements et des réapprovisionnements
- Exports CSV, Excel et PDF

## Rôles et accès
- `Admin` : accès à la gestion des utilisateurs, des rôles et des permissions
- `Gestionnaire` : accès aux équipements, catégories, fournisseurs, affectations, mouvements de stock, historique et rapports/exports
- `Operateur` : accès aux équipements, catégories, fournisseurs, affectations et mouvements de stock

## Prérequis
- .NET SDK `8.0.418`
- SQL Server local ou SQL Server Express
- Pour les notifications courriel : un serveur SMTP accessible, par exemple Gmail avec mot de passe d'application

Le SDK est verrouillé via `global.json`.

## Arborescence importante
- `EasyStock.Sprint3.csproj` : projet web MVC
- `tests/EasyStock.Sprint3.Tests/EasyStock.Sprint3.Tests.csproj` : tests unitaires
- `Migrations/` : migrations EF Core Sprint 2 + Sprint 3
- `Service/` : logique métier, y compris mouvements, historique, rapports, exports et notifications

## Configuration
### Base de données
Dans `appsettings.json` :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EasyStockSprint3;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### Notifications courriel
Le projet accepte désormais une section `Email` ou `EmailSettings`. Exemple Gmail avec `STARTTLS` :

```json
"Email": {
  "Enabled": true,
  "DisplayName": "EasyStock",
  "SmtpServer": "smtp.gmail.com",
  "Port": 587,
  "UseStartTls": true,
  "EnableSsl": false,
  "SenderEmail": "votre-adresse@gmail.com",
  "Username": "votre-adresse@gmail.com",
  "Password": "mot-de-passe-application",
  "RecipientEmail": "destinataire@gmail.com"
}
```

Les alertes de stock faible sont envoyées lors d'une sortie de stock manuelle et aussi lorsqu'une affectation fait passer un équipement au seuil minimal ou en dessous.

## Comptes par défaut
- `admin / Admin123!`
- `gestionnaire / Gestion123!`
- `operateur / Operateur123!`

## Commandes principales
### Restaurer
```powershell
dotnet restore .\EasyStock.Sprint3.csproj
```

### Compiler
```powershell
dotnet build .\EasyStock.Sprint3.csproj
```

### Lancer l'application
```powershell
dotnet run --project .\EasyStock.Sprint3.csproj
```

Exemple sur un port fixe :
```powershell
dotnet run --project .\EasyStock.Sprint3.csproj --urls http://localhost:5055
```

## Jeux de données disponibles
Le seed couvre :
- rôles système
- comptes de connexion
- 30 employés
- catégories
- fournisseurs
- équipements
- premiers mouvements de stock historiques

## Fonctionnalités effectivement opérationnelles
### Mouvements de stock
- entrée de stock avec motif et référence externe
- sortie de stock avec contrôle du stock disponible
- retour de stock
- mise à jour immédiate du stock sur l'équipement
- historisation complète dans `MouvementStock`

### Historique
- liste chronologique des mouvements
- filtres par date, équipement, type de mouvement et utilisateur
- historique distinct des réapprovisionnements

### Rapports et exports
- rapport inventaire global
- rapport stock faible
- export historique des mouvements
- export historique des réapprovisionnements
- formats disponibles : CSV, Excel, PDF

### Alertes et notification
- badge visuel `Stock faible` dans la liste des équipements
- badge visuel dans les mouvements récents
- tentative d'envoi d'un courriel configurable quand un mouvement ou une affectation place un équipement au seuil minimal ou en dessous
- l'envoi d'email n'empêche jamais l'enregistrement du mouvement

## Tests
### Exécuter tous les tests
```powershell
dotnet test .\tests\EasyStock.Sprint3.Tests\EasyStock.Sprint3.Tests.csproj
```

### Couverture ciblée sur la couche métier
```powershell
dotnet test .\tests\EasyStock.Sprint3.Tests\EasyStock.Sprint3.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Include="[EasyStock.Sprint3]EasyStock.Service.*" /p:Threshold=80 /p:ThresholdType=line /p:ThresholdStat=total
```

Résultat validé sur la couche `Service` :
- couverture méthode : `95.06%`

Le rapport Cobertura est généré dans :
- `tests/EasyStock.Sprint3.Tests/coverage.cobertura.xml`

## Validation réalisée
- build du projet web : OK
- build du projet de tests : OK
- tests unitaires : OK
- migration EF Core : OK
- `database update` sur `EasyStockSprint3` : OK
- démarrage applicatif validé sur `http://localhost:5055`

## Résumé des vérifications fonctionnelles
- entrée de stock : validée
- sortie de stock avec contrôle de disponibilité : validée
- retour de stock : validé
- historique global : validé
- filtrage historique : validé
- historique des réapprovisionnements : validé
- rapports et exports CSV / Excel / PDF : validés
- alerte UI stock faible : validée
- notification courriel : implémentée et configurable


