"use client";

import { createSession } from "@/features/auth/api";
import { useRouter } from "next/navigation";
import { FormEvent, useState } from "react";

export function LoginForm() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      await createSession("/api/account/login", { email, password });
      router.push("/");
    } catch {
      setError("Email ou mot de passe incorrect.");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <form className="space-y-5" onSubmit={handleSubmit}>
      <label className="block space-y-2 text-sm font-semibold text-stone-800">
        Email
        <input
          autoComplete="email"
          className="w-full rounded-xl border border-stone-300 bg-white px-4 py-3 font-normal outline-none transition focus:border-orange-600"
          onChange={(event) => setEmail(event.target.value)}
          required
          type="email"
          value={email}
        />
      </label>
      <label className="block space-y-2 text-sm font-semibold text-stone-800">
        Mot de passe
        <input
          autoComplete="current-password"
          className="w-full rounded-xl border border-stone-300 bg-white px-4 py-3 font-normal outline-none transition focus:border-orange-600"
          onChange={(event) => setPassword(event.target.value)}
          required
          type="password"
          value={password}
        />
      </label>
      {error && <p className="rounded-xl bg-orange-50 p-3 text-sm text-orange-800">{error}</p>}
      <button
        className="w-full rounded-xl bg-stone-950 px-4 py-3 font-bold text-stone-50 transition hover:bg-orange-600 disabled:cursor-not-allowed disabled:opacity-60"
        disabled={isSubmitting}
        type="submit"
      >
        {isSubmitting ? "Connexion…" : "Se connecter"}
      </button>
    </form>
  );
}
