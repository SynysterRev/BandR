import { LoginForm } from "@/features/auth/login-form";
import Link from "next/link";

export default function LoginPage() {
  return (
    <main className="flex flex-1 items-center justify-center bg-stone-950 px-5 py-12">
      <section className="w-full max-w-md rounded-3xl bg-stone-50 p-7 shadow-2xl sm:p-10">
        <Link className="text-sm font-bold tracking-[0.2em] text-orange-600" href="/">BANDR</Link>
        <h1 className="mt-8 text-3xl font-black tracking-tight text-stone-950">Content de te revoir.</h1>
        <p className="mt-2 text-stone-600">Connecte-toi pour publier ou contacter des musiciens.</p>
        <div className="mt-8"><LoginForm /></div>
      </section>
    </main>
  );
}
