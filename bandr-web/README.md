# BandR Web

Frontend Next.js du projet BandR.

## Démarrage local

1. Copier `.env.example` vers `.env.local` si nécessaire.
2. Lancer `npm run dev`.

Le frontend appelle l'API via `NEXT_PUBLIC_API_URL`. Les requêtes utilisent les cookies navigateur pour le refresh token ; le refresh token n'est jamais stocké ni accessible en JavaScript.
