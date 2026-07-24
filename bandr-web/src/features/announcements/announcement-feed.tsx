"use client";

import { apiFetch } from "@/features/auth/api";
import { ThemeSelector } from "@/components/theme-selector";
import type { Announcement, PagedResponse } from "@/types/api";
import { useEffect, useMemo, useState } from "react";

const styles = ["Tous", "Rock", "Metal", "Jazz", "Pop", "Electro"];

function announcementTypeLabel(type: Announcement["type"]) {
  return type === 0 ? "Cherche musicien" : "Cherche groupe";
}

function formatDate(date: string) {
  return new Intl.DateTimeFormat("fr-FR", { day: "numeric", month: "short" }).format(new Date(date));
}

export function AnnouncementFeed() {
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [query, setQuery] = useState("");
  const [selectedStyle, setSelectedStyle] = useState("Tous");
  const [status, setStatus] = useState<"loading" | "ready" | "error">("loading");

  useEffect(() => {
    apiFetch<PagedResponse<Announcement>>("/api/Announcements?pageNumber=1&pageSize=24")
      .then((response) => {
        setAnnouncements(response.data);
        setStatus("ready");
      })
      .catch(() => setStatus("error"));
  }, []);

  const filteredAnnouncements = useMemo(() => {
    const normalizedQuery = query.trim().toLocaleLowerCase();

    return announcements.filter((announcement) => {
      const matchesQuery =
        !normalizedQuery ||
        [announcement.title, announcement.city, announcement.musicianUsername, ...announcement.instruments]
          .join(" ")
          .toLocaleLowerCase()
          .includes(normalizedQuery);
      const matchesStyle =
        selectedStyle === "Tous" || announcement.styles.some((style) => style.toLocaleLowerCase() === selectedStyle.toLocaleLowerCase());

      return matchesQuery && matchesStyle;
    });
  }, [announcements, query, selectedStyle]);

  return (
    <section className="w-full">
      <header className="sticky top-0 z-10 border-b border-[var(--header-border)] bg-[var(--header)] text-[var(--header-text)] shadow-lg">
        <div className="mx-auto flex max-w-7xl items-center justify-between gap-3 px-5 py-3 sm:px-8">
          <a className="text-sm font-black tracking-[0.25em]" href="/">BANDR</a>
          <div className="flex items-center gap-2">
            <ThemeSelector />
            <a className="rounded-full bg-[var(--fire)] px-4 py-2 text-sm font-bold text-white transition hover:brightness-110" href="/login">
              Se connecter
            </a>
          </div>
        </div>
      </header>
      <div className="mx-auto w-full max-w-7xl px-5 py-10 sm:px-8 lg:py-14">
      <div className="mb-8 flex flex-col gap-5 lg:flex-row lg:items-end lg:justify-between">
        <div>
          <p className="mb-2 text-sm font-bold tracking-[0.22em] text-[var(--fire)]">DÉCOUVRIR</p>
          <h1 className="max-w-xl text-4xl font-black tracking-[-0.06em] text-[var(--foreground)] sm:text-6xl">
            Trouve les musiciens avec qui jouer.
          </h1>
        </div>
      </div>

      <label className="block rounded-2xl border border-[var(--border)] bg-[var(--surface)] p-1 shadow-sm focus-within:border-[var(--fire)]">
        <span className="sr-only">Rechercher une annonce</span>
        <input
          className="w-full rounded-xl bg-transparent px-4 py-3 text-[var(--foreground)] outline-none placeholder:text-[var(--muted)]"
          onChange={(event) => setQuery(event.target.value)}
          placeholder="Ville, instrument ou nom de musicien"
          value={query}
        />
      </label>

      <div className="my-5 flex gap-2 overflow-x-auto pb-1">
        {styles.map((style) => (
          <button
            className={`shrink-0 rounded-full border px-4 py-2 text-sm font-semibold transition ${
              selectedStyle === style
                ? "border-[var(--fire)] bg-[var(--fire)] text-white"
                : "border-[var(--border)] bg-[var(--surface)] text-[var(--foreground)] hover:border-[var(--fire)] hover:text-[var(--fire)]"
            }`}
            key={style}
            onClick={() => setSelectedStyle(style)}
            type="button"
          >
            {style}
          </button>
        ))}
      </div>

      <div className="mb-5 flex items-center justify-between text-sm text-[var(--muted)]">
        <p>{status === "ready" ? `${filteredAnnouncements.length} annonce${filteredAnnouncements.length > 1 ? "s" : ""}` : "Annonces"}</p>
        <p>Plus récent</p>
      </div>

      {status === "loading" && <p className="rounded-2xl border border-dashed border-[var(--border)] p-8 text-[var(--muted)]">Chargement des annonces…</p>}
      {status === "error" && <p className="rounded-2xl border border-[var(--fire)] bg-[var(--fire-soft)] p-8 text-[var(--foreground)]">Les annonces ne sont pas disponibles pour le moment.</p>}
      {status === "ready" && filteredAnnouncements.length === 0 && (
        <p className="rounded-2xl border border-dashed border-[var(--border)] p-8 text-[var(--muted)]">Aucune annonce ne correspond à cette recherche.</p>
      )}
      {status === "ready" && filteredAnnouncements.length > 0 && (
        <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-3">
          {filteredAnnouncements.map((announcement) => (
            <article className="group rounded-2xl border border-[var(--border)] bg-[var(--surface)] p-5 shadow-sm transition hover:-translate-y-1 hover:border-[var(--fire)] hover:shadow-lg" key={announcement.id}>
              <div className="mb-7 flex items-start justify-between gap-4">
                <span className="rounded-full bg-[var(--fire-soft)] px-3 py-1 text-xs font-bold text-[var(--fire)]">{announcementTypeLabel(announcement.type)}</span>
                <time className="text-xs font-medium text-[var(--muted)]">{formatDate(announcement.createdAt)}</time>
              </div>
              <h2 className="text-xl font-bold tracking-tight text-[var(--foreground)]">{announcement.title}</h2>
              <p className="mt-2 text-sm text-[var(--muted)]">{announcement.musicianUsername} · {announcement.city}</p>
              <div className="mt-6 flex flex-wrap gap-2">
                {[...announcement.instruments, ...announcement.styles].slice(0, 4).map((item) => (
                  <span className="rounded-md bg-[var(--surface-muted)] px-2 py-1 text-xs font-semibold text-[var(--foreground)]" key={item}>{item}</span>
                ))}
              </div>
            </article>
          ))}
        </div>
      )}
      </div>
    </section>
  );
}
