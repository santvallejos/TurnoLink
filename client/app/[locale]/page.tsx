'use client';

import { useTranslations } from 'next-intl';
import { Link } from '@/lib/i18n/navigation';

export default function HomePage() {
  const t = useTranslations();

  return (
    <div className="min-h-screen bg-gradient-to-b from-zinc-50 to-white dark:from-zinc-950 dark:to-black">
      {/* Header */}
      <header className="border-b border-zinc-200 dark:border-zinc-800">
        <nav className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <Link href="/" className="text-xl font-bold text-zinc-900 dark:text-white">
            TurnoLink
          </Link>
          <div className="flex items-center gap-4">
            <Link
              href="/login"
              className="text-sm text-zinc-600 hover:text-zinc-900 dark:text-zinc-400 dark:hover:text-white"
            >
              {t('nav.login')}
            </Link>
            <Link
              href="/register"
              className="rounded-full bg-zinc-900 px-4 py-2 text-sm text-white hover:bg-zinc-700 dark:bg-white dark:text-zinc-900 dark:hover:bg-zinc-200"
            >
              {t('nav.register')}
            </Link>
          </div>
        </nav>
      </header>

      {/* Hero */}
      <main className="mx-auto max-w-6xl px-6 py-24">
        <div className="text-center">
          <h1 className="text-4xl font-bold tracking-tight text-zinc-900 dark:text-white sm:text-6xl">
            Gestiona tus reservas
            <br />
            <span className="text-blue-600">de forma simple</span>
          </h1>
          <p className="mx-auto mt-6 max-w-2xl text-lg text-zinc-600 dark:text-zinc-400">
            TurnoLink te permite crear tu página de reservas personalizada 
            y permitir a tus clientes agendar citas fácilmente.
          </p>
          <div className="mt-10 flex items-center justify-center gap-4">
            <Link
              href="/register"
              className="rounded-full bg-blue-600 px-6 py-3 text-sm font-semibold text-white hover:bg-blue-500"
            >
              {t('nav.register')}
            </Link>
            <Link
              href="/login"
              className="rounded-full border border-zinc-300 px-6 py-3 text-sm font-semibold text-zinc-900 hover:bg-zinc-50 dark:border-zinc-700 dark:text-white dark:hover:bg-zinc-800"
            >
              {t('nav.login')}
            </Link>
          </div>
        </div>
      </main>

      {/* Language Switcher */}
      <footer className="fixed bottom-4 right-4">
        <div className="flex gap-2 rounded-full bg-zinc-100 p-1 dark:bg-zinc-800">
          <Link href="/" locale="es" className="rounded-full px-3 py-1 text-sm hover:bg-white dark:hover:bg-zinc-700">
            ES
          </Link>
          <Link href="/" locale="en" className="rounded-full px-3 py-1 text-sm hover:bg-white dark:hover:bg-zinc-700">
            EN
          </Link>
        </div>
      </footer>
    </div>
  );
}
