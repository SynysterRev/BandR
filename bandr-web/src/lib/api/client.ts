"use client";

import type { AccessTokenResult } from "@/types/api";

const apiUrl = process.env.NEXT_PUBLIC_API_URL;

let accessToken: string | null = null;

function getApiUrl(path: string) {
  if (!apiUrl) {
    throw new Error("NEXT_PUBLIC_API_URL is not configured.");
  }

  return `${apiUrl}${path}`;
}

export function clearAccessToken() {
  accessToken = null;
}

export async function createSession<TBody>(
  path: "/api/account/login" | "/api/account/register",
  body: TBody,
) {
  const response = await fetch(getApiUrl(path), {
    method: "POST",
    credentials: "include",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!response.ok) {
    throw new Error("Authentication failed.");
  }

  const result = (await response.json()) as AccessTokenResult;
  accessToken = result.accessToken;
  return result;
}

export async function refreshSession() {
  const response = await fetch(getApiUrl("/api/account/refresh"), {
    method: "POST",
    credentials: "include",
  });

  if (!response.ok) {
    clearAccessToken();
    return null;
  }

  const result = (await response.json()) as AccessTokenResult;
  accessToken = result.accessToken;
  return result;
}

export async function logout() {
  await fetch(getApiUrl("/api/account/logout"), {
    method: "POST",
    credentials: "include",
  });
  clearAccessToken();
}

export async function apiFetch<T>(path: string, init: RequestInit = {}, retried = false): Promise<T> {
  const headers = new Headers(init.headers);
  if (accessToken) {
    headers.set("Authorization", `Bearer ${accessToken}`);
  }

  const response = await fetch(getApiUrl(path), {
    ...init,
    headers,
    credentials: "include",
  });

  if (response.status === 401 && !retried && (await refreshSession())) {
    return apiFetch<T>(path, init, true);
  }

  if (!response.ok) {
    throw new Error(`API request failed with status ${response.status}.`);
  }

  return (await response.json()) as T;
}
