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

export type Announcement = {
  id: string;
  title: string;
  city: string;
  type: 0 | 1;
  musicianId: string;
  musicianUsername: string;
  instruments: string[];
  styles: string[];
  createdAt: string;
};

export type PagedResponse<T> = {
  data: T[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalRecords: number;
};
