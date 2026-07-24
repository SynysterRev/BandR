# BandR

API REST permettant à des musiciens de créer un profil, publier des annonces et échanger par messagerie.

Le dépôt contient aussi `bandr-web/`, le frontend Next.js de l'application.

## Stack

- .NET 10 / ASP.NET Core Controllers
- Entity Framework Core 10 / PostgreSQL 16
- ASP.NET Identity avec JWT Bearer et refresh tokens hashés
- FluentValidation, OpenAPI et Scalar
- xUnit, Testcontainers PostgreSQL et Respawn

## Lancer le projet

Prérequis : .NET 10 SDK et Docker Desktop.

```bash
docker compose up -d
dotnet ef database update
dotnet run --project BandR
```

En environnement Development, Scalar est disponible sur `https://localhost:7294/scalar/v1`.

### Frontend

```bash
cd bandr-web
npm run dev
```

Le frontend est servi sur `http://localhost:3000` et consomme l'API définie dans `bandr-web/.env.local`.

La page d'accueil affiche publiquement les annonces actives ; la connexion est demandée uniquement pour les actions privées.

## Endpoints principaux

Les annonces actives et les profils sont consultables publiquement. Les routes d'écriture et les données personnelles nécessitent un Bearer token. `refresh` et `logout` utilisent le cookie refresh `HttpOnly`.

### Account

| Méthode | Endpoint | Description |
|---|---|---|
| POST | `/api/account/register` | Crée un compte, retourne un access token et pose un cookie refresh `HttpOnly`. |
| POST | `/api/account/login` | Authentifie un compte actif, retourne un access token et pose un cookie refresh `HttpOnly`. |
| POST | `/api/account/refresh` | Renouvelle l'access token depuis le cookie refresh. |
| POST | `/api/account/logout` | Révoque et supprime le cookie refresh. |
| DELETE | `/api/account/me` | Désactive le compte courant, ses annonces et ses conversations, sans supprimer l'historique. |
| GET | `/api/account?email={email}` | Indique si une adresse email est déjà utilisée. |

### Musicians

| Méthode | Endpoint | Description |
|---|---|---|
| GET | `/api/musicians` | Liste les profils. Public. |
| GET | `/api/musicians/{id}` | Retourne un profil. Public. |
| GET | `/api/musicians/me` | Retourne le profil du compte courant. |
| POST | `/api/musicians` | Crée le profil du compte courant. |
| PATCH | `/api/musicians/me` | Modifie le profil du compte courant. |
| GET | `/api/musicians/me/announcements` | Liste les annonces du profil courant. |
| GET | `/api/musicians/{id}/announcements` | Liste les annonces d'un profil. |

### Announcements

| Méthode | Endpoint | Description |
|---|---|---|
| GET | `/api/announcements` | Liste paginée et filtrable des annonces actives. Public. |
| GET | `/api/announcements/{id}` | Retourne une annonce active. Public. |
| POST | `/api/announcements` | Crée une annonce. |
| PATCH | `/api/announcements/{id}` | Modifie une annonce appartenant au profil courant. |
| DELETE | `/api/announcements/{id}` | Supprime une annonce appartenant au profil courant. |

### Conversations

| Méthode | Endpoint | Description |
|---|---|---|
| GET | `/api/conversations` | Liste les conversations du profil courant, actives ou inactives, avec le dernier message. |
| GET | `/api/conversations/{conversationId}` | Retourne le détail d'une conversation accessible au profil courant. |
| POST | `/api/conversations` | Crée ou retrouve une conversation avec un autre musicien. |
| POST | `/api/conversations/{conversationId}` | Envoie un message dans une conversation active. |

Une conversation devient inactive si l'un de ses participants désactive son compte. Son historique reste conservé mais l'envoi de message est refusé.

## Tests

```bash
dotnet test
```

Les tests d'intégration nécessitent Docker : ils démarrent PostgreSQL avec Testcontainers.

## Configuration locale

La connexion est lue depuis `ConnectionStrings:Default`, JWT depuis la section `Jwt` et les origines navigateur autorisées depuis `Cors:AllowedOrigins`. En développement, Next.js est autorisé sur `http://localhost:3000` et `https://localhost:3000`. En production, définir explicitement les origines du front. Utiliser les user secrets ou les variables d'environnement pour les secrets ; ne pas committer de clé JWT, mot de passe ou chaîne de connexion réelle.
