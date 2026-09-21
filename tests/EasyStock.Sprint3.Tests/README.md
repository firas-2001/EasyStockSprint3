# EasyStock.Sprint3.Tests

Tests unitaires de la couche métier Sprint 3, en complément des services déjà couverts des Sprints 1 et 2.

## Périmètre testé
- `ArticleService`
- `CategoryService`
- `FournisseurService`
- `EmployeService`
- `UserService`
- `AuthService`
- `AffectationService`
- `StockMovementService`
- `HistoryService`
- `ReportService`
- `ExportService`
- `NotificationService`

## Exécuter les tests
```powershell
dotnet test .\tests\EasyStock.Sprint3.Tests\EasyStock.Sprint3.Tests.csproj
```

## Couverture ciblée sur la couche métier
```powershell
dotnet test .\tests\EasyStock.Sprint3.Tests\EasyStock.Sprint3.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Include="[EasyStock.Sprint3]EasyStock.Service.*" /p:Threshold=80 /p:ThresholdType=line /p:ThresholdStat=total
```

Résultat validé :
- 70 tests réussis
- 88.08% de couverture en lignes sur `EasyStock.Service.*`
