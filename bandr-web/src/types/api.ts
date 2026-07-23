export type AccessTokenResult = {
  accessToken: string;
  expiresAt: number;
};

export type Musician = {
  id: string;
  username: string;
  city: string;
  instruments: string[];
  styles: string[];
  tags: string[];
  bio: string | null;
  avatarUrl: string | null;
};
