# Instructions pour les agents

## Vue d'ensemble

BandR est une API REST pour mettre en relation des musiciens : authentification, profils, annonces, conversations et messages. Il n'y a pas de client web dans ce dépôt.

- Runtime : .NET 10 (`net10.0`), C# avec nullable et implicit usings activés.
- API : ASP.NET Core Controllers, OpenAPI et Scalar (documentation exposée uniquement en environnement `Development`).
- Données : Entity Framework Core 10, PostgreSQL via Npgsql, ASP.NET Identity avec clés `Guid`.
- Sécurité : JWT Bearer, access token et refresh token persisté sous forme de hash SHA-256.
- Validation : FluentValidation ; tests : xUnit, FluentAssertions, Testcontainers PostgreSQL et Respawn.
- Exécution locale : Docker Compose démarre PostgreSQL 16 ; le `Dockerfile` construit et publie l'API .NET 10.

Les dossiers principaux sont :

- `BandR/` : projet API.
- `BandR/Controllers/` : endpoints HTTP et récupération de l'utilisateur courant.
- `BandR/Services/` et `BandR/Services/Interfaces/` : logique applicative et contrats DI.
- `BandR/Data/` : `ApplicationDbContext` Identity/EF Core et horodatage automatique.
- `BandR/Entities/` et `BandR/Entities/Joints/` : entités EF et tables de jonction explicites.
- `BandR/EntitiesConfiguration/` : configurations EF Core et données de référence.
- `BandR/DTOs/` : contrats API immuables (`record`), organisés par domaine.
- `BandR/Validators/` : validateurs FluentValidation des DTO de mutation.
- `BandR/Extensions/` : mappings DTO/entités, extensions de requêtes, claims et inscription DI.
- `BandR/Exceptions/` : exceptions métier convertibles en `ProblemDetails` en théorie ; aucun middleware/filtre de conversion n'est enregistré dans `Program.cs`.
- `BandR/Seeds/` : jeux de données EF pour instruments, styles et tags.
- `BandR/Migrations/` : migrations EF Core et snapshot du modèle.
- `BandR.Tests/Unit/Validators/` : tests unitaires de validation.
- `BandR.Tests/IntegrationTests/` : tests des services contre un PostgreSQL Testcontainers, remis à zéro par Respawn.
- `.github/workflows/dotnet.yml` : CI sur `main` (restore, build, test).

## Conventions de code observées

- Indentation de 4 espaces ; accolades sur leur propre ligne pour les classes et méthodes. Les expressions courtes utilisent parfois le corps expression (`=>`).
- Types, méthodes et propriétés en PascalCase ; paramètres et variables locales en camelCase ; interfaces préfixées par `I`.
- Les DTO sont des `record` positionnels, groupés par fonctionnalité. Les entités sont des classes mutables avec valeurs par défaut non nulles (`string.Empty`, `[]`, `null!`).
- Architecture par couches techniques : contrôleur → interface de service → service → `ApplicationDbContext`. Les contrôleurs n'accèdent pas directement au contexte.
- Les services et contrôleurs utilisent l'injection par constructeur primaire. Les services applicatifs sont enregistrés en `Scoped` dans `Extensions/ServiceCollectionExtensions.cs`.
- Les conversions entité/DTO sont des méthodes d'extension `ToEntity`/`ToDto` dans `Extensions/`, plutôt que des mappers dédiés.
- Les relations et contraintes EF sont définies dans des `IEntityTypeConfiguration<T>` automatiquement chargées par `ApplicationDbContext`. Les relations many-to-many qui ont des attributs propres utilisent une entité de jointure ; les autres sont configurées dans le contexte.
- Les requêtes asynchrones propagent un `CancellationToken`. Les lectures complexes chargent explicitement les navigations requises ; certaines utilisent `AsSplitQuery()` et les listes d'annonces utilisent `AsNoTracking()`.
- Les listes d'annonces passent par `AnnouncementQueryFilter`, `ApplyFilters`, `ApplySort` et `ApplyPagination`. Le tri dynamique n'accepte que les propriétés publiques scalaires de l'entité.

## Développer une fonctionnalité

Pour un nouveau domaine ou endpoint, conserver l'ordre et les responsabilités observés :

1. Créer ou adapter l'entité dans `Entities/`, puis sa configuration dans `EntitiesConfiguration/`. Ajouter les `DbSet` et relations dans `ApplicationDbContext` si nécessaires.
2. Créer les DTO dans `DTOs/<Domaine>/` et les mappings dans `Extensions/`.
3. Créer les validateurs FluentValidation dans `Validators/<Domaine>/`. Leur découverte repose sur `AddValidatorsFromAssemblyContaining` dans `ServiceCollectionExtensions.cs` : vérifier que le type d'ancrage déjà enregistré permet bien de découvrir le nouveau validateur.
4. Ajouter l'interface dans `Services/Interfaces/`, son implémentation dans `Services/`, puis son enregistrement `AddScoped` dans `AddApplicationServices`.
5. Ajouter le contrôleur ou l'action, avec les attributs HTTP, `[Authorize]`/`[AllowAnonymous]` nécessaires, `CancellationToken` et les DTO en entrée/sortie.
6. Si le schéma change, générer une migration EF Core ; ne pas écrire à la main les fichiers générés. Vérifier également les seeds et leurs index uniques.
7. Ajouter les tests au même niveau que la fonctionnalité : validation dans `BandR.Tests/Unit/Validators/`, logique persistance/service dans `BandR.Tests/IntegrationTests/Services/`.

Nommage des tests : classes `...Tests`, méthodes `Action_ShouldResult_WhenCondition`. Les tests d'intégration partagent `TestDatabaseFixture`, démarrent PostgreSQL 16 Alpine, appliquent les migrations, puis réinitialisent les tables avec Respawn avant chaque test.

Commandes observées :

```powershell
docker compose up -d
dotnet ef database update
dotnet run --project BandR
dotnet test
```

Les tests d'intégration nécessitent Docker. La CI utilise `dotnet restore`, `dotnet build --no-restore`, puis `dotnet test --no-build --verbosity normal`.

## Erreurs, authentification et configuration

- Les services signalent les ressources introuvables ou les accès interdits par les exceptions imbriquées `MusicianException`, `AnnouncementException` et `ConversationException`. Ces classes renseignent un `ProblemDetails`, mais l'application n'enregistre actuellement aucun middleware ou filtre qui les intercepte.
- L'authentification est configurée dans `AddApplicationServices`. Les réponses JWT 401/403 y sont explicitement produites sous `application/problem+json`.
- Utiliser `User.GetUserId()` pour extraire l'identifiant Identity depuis le claim `NameIdentifier`. Les opérations qui appartiennent à un musicien récupèrent d'abord le profil correspondant à cet utilisateur.
- La configuration de connexion est lue via `ConnectionStrings:Default`; JWT via la section `Jwt` (`SecretKey`, `Issuer`, `Audience`, durées). `appsettings.Development.json` contient une chaîne locale et une clé JWT vide. Le projet possède aussi un `UserSecretsId`.
- Ne pas committer de vraie clé JWT, de mot de passe ou de chaîne de connexion sensible. Préférer les user secrets ou les variables de configuration ASP.NET Core. Le dépôt ne contient pas de fichier `.env` ni de mécanisme dédié pour le charger.

## Ne pas faire

- Ne pas contourner la couche service depuis les contrôleurs ni exposer les entités EF directement dans les réponses API.
- Ne pas modifier manuellement les fichiers EF générés dans `Migrations/`, notamment `*.Designer.cs` et `ApplicationDbContextModelSnapshot.cs`. Créer une migration après une évolution de modèle.
- Ne pas modifier les données seed (`Seeds/`) sans vérifier les index uniques sur les noms et sans migration associée.
- Ne pas retirer ou changer les versions des dépendances Identity, EF Core, Npgsql, JWT, FluentValidation, Testcontainers ou Respawn sans raison explicite et vérification de build/tests : elles structurent l'authentification, le modèle et les tests.
- Ne pas supposer que les validateurs sont exécutés automatiquement : ils sont inscrits dans le conteneur DI, mais aucun appel explicite à `Validate` ni intégration MVC FluentValidation n'apparaît dans les contrôleurs ou `Program.cs`.

## Points d'attention constatés

- Le README ne reflète pas entièrement le code : il annonce la messagerie « in progress », mais contrôleur et service de conversations existent ; il liste des suppressions/profils qui ne correspondent pas toutes aux actions présentes dans les contrôleurs. Traiter les contrôleurs et tests comme référence d'implémentation.
- `MusiciansController` ne publie pas d'action `PUT`/`DELETE`, tandis que les méthodes correspondantes existent dans `IMusicianService`/`MusicianService`. Le même décalage existe pour `UpdateAnnouncementAsync`, sans action HTTP `PUT`/`PATCH` dans `AnnouncementsController`.
- `LocationConfiguration` rend `Country` et `PostalCode` obligatoires, alors que les créations de `Location` dans les services ne renseignent que `City`. Toute évolution de ce flux doit être testée contre PostgreSQL et les migrations actuelles.
- Les exceptions `AnnouncementException` internes héritent de `MusicianException` plutôt que de `AnnouncementException`, bien qu'elles soient déclarées dans cette dernière. Ne pas restructurer cette hiérarchie sans mesurer l'impact sur les captures d'exceptions et les tests.
- Les configurations `InstrumentConfiguration`, `StyleConfiguration` et `TagConfiguration` importent le namespace `Bandix.Infrastructure.Persistence.Seeds`, différent du nom de projet `BandR`; préserver ou corriger cette dépendance uniquement avec build/migration de contrôle.
