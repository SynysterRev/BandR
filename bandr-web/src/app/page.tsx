export default function Home() {
  return (
    <main className="flex flex-1 items-center justify-center bg-stone-950 px-6 text-stone-100">
      <section className="max-w-2xl space-y-6 text-center">
        <p className="text-sm font-semibold tracking-[0.3em] text-amber-400">BANDR</p>
        <h1 className="text-4xl font-semibold tracking-tight sm:text-6xl">
          Trouve les musiciens avec qui jouer.
        </h1>
        <p className="text-lg text-stone-300">
          Profils, annonces et conversations : le frontend BandR est prêt à accueillir le MVP.
        </p>
      </section>
    </main>
  );
}
