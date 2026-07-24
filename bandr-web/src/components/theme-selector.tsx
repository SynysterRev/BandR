"use client";

import { useEffect, useState } from "react";

const themes = [
  ["nightwave", "Nightwave — bleu électrique"],
  ["vinyl", "Vinyl — noir & rouge"],
  ["ultraviolet", "Ultraviolet — violet scène"],
  ["evergreen", "Evergreen — vert profond"],
  ["sandstorm", "Sandstorm — sable & indigo"],
  ["cobalt", "Cobalt — bleu studio"],
  ["wine", "Wine — bordeaux & rose"],
  ["lagoon", "Lagoon — teal & écume"],
  ["gold", "Gold — noir & or"],
  ["mono", "Mono — blanc & rouge"],
] as const;

type Theme = (typeof themes)[number][0];

export function ThemeSelector() {
  const [theme, setTheme] = useState<Theme>("nightwave");

  useEffect(() => {
    const storedTheme = window.localStorage.getItem("bandr-theme") as Theme | null;
    if (storedTheme && themes.some(([value]) => value === storedTheme)) {
      setTheme(storedTheme);
      document.documentElement.dataset.theme = storedTheme;
    } else {
      document.documentElement.dataset.theme = "nightwave";
    }
  }, []);

  function selectTheme(nextTheme: Theme) {
    setTheme(nextTheme);
    document.documentElement.dataset.theme = nextTheme;
    window.localStorage.setItem("bandr-theme", nextTheme);
  }

  return (
    <label className="flex items-center gap-2 text-xs font-bold text-[var(--header-text)]">
      Thème
      <select
        className="max-w-40 rounded-full border border-[var(--header-border)] bg-[var(--header)] px-3 py-2 text-xs font-bold text-[var(--header-text)] outline-none transition hover:border-[var(--fire)]"
        onChange={(event) => selectTheme(event.target.value as Theme)}
        value={theme}
      >
        {themes.map(([value, label]) => <option key={value} value={value}>{label}</option>)}
      </select>
    </label>
  );
}
