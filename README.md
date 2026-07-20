# BandR

API REST permettant à des musiciens de créer un profil, publier des annonces et échanger par messagerie.

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

## Endpoints principaux

Toutes les routes sauf `register`, `login` et la vérification d'email nécessitent un Bearer token.

### Account

| Méthode | Endpoint | Description |
|---|---|---|
| POST | `/api/account/register` | Crée un compte et retourne des tokens. |
| POST | `/api/account/login` | Authentifie un compte actif. |
| POST | `/api/account/refresh` | Renouvelle une paire de tokens. |
| POST | `/api/account/logout` | Révoque un refresh token. |
| DELETE | `/api/account/me` | Désactive le compte courant, ses annonces et ses conversations, sans supprimer l'historique. |
| GET | `/api/account?email={email}` | Indique si une adresse email est déjà utilisée. |

### Musicians

| Méthode | Endpoint | Description |
|---|---|---|
| GET | `/api/musicians` | Liste les profils. |
| GET | `/api/musicians/{id}` | Retourne un profil. |
| GET | `/api/musicians/me` | Retourne le profil du compte courant. |
| POST | `/api/musicians` | Crée le profil du compte courant. |
| PATCH | `/api/musicians/me` | Modifie le profil du compte courant. |
| GET | `/api/musicians/me/announcements` | Liste les annonces du profil courant. |
| GET | `/api/musicians/{id}/announcements` | Liste les annonces d'un profil. |

### Announcements

| Méthode | Endpoint | Description |
|---|---|---|
| GET | `/api/announcements` | Liste paginée et filtrable des annonces actives. |
| GET | `/api/announcements/{id}` | Retourne une annonce active. |
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

La connexion est lue depuis `ConnectionStrings:Default` et JWT depuis la section `Jwt`. Utiliser les user secrets ou les variables d'environnement pour les secrets ; ne pas committer de clé JWT, mot de passe ou chaîne de connexion réelle.
