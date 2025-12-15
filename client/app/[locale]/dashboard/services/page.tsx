'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { Link } from '@/lib/i18n/navigation';
import { serviceService } from '@/lib/services';
import type { Service } from '@/types';

export default function ServicesPage() {
  const t = useTranslations();
  const [services, setServices] = useState<Service[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    serviceService.getMyServices()
      .then(setServices)
      .catch(() => window.location.href = '/login')
      .finally(() => setLoading(false));
  }, []);

  if (loading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p>{t('common.loading')}</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-zinc-50 dark:bg-zinc-950">
      <header className="border-b border-zinc-200 bg-white dark:border-zinc-800 dark:bg-zinc-900">
        <nav className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <Link href="/dashboard" className="text-xl font-bold text-zinc-900 dark:text-white">
            TurnoLink
          </Link>
        </nav>
      </header>

      <main className="mx-auto max-w-6xl px-6 py-8">
        <div className="mb-6 flex items-center justify-between">
          <h1 className="text-2xl font-bold text-zinc-900 dark:text-white">
            {t('services.title')}
          </h1>
          <button className="rounded-md bg-blue-600 px-4 py-2 text-sm text-white hover:bg-blue-500">
            {t('services.create')}
          </button>
        </div>

        {services.length === 0 ? (
          <p className="text-zinc-600 dark:text-zinc-400">{t('services.empty')}</p>
        ) : (
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {services.map((service) => (
              <div key={service.id} className="rounded-lg bg-white p-6 shadow dark:bg-zinc-900">
                <h3 className="font-semibold text-zinc-900 dark:text-white">{service.name}</h3>
                <p className="mt-1 text-sm text-zinc-600 dark:text-zinc-400">{service.description}</p>
                <div className="mt-4 flex items-center justify-between text-sm">
                  <span className="text-zinc-600 dark:text-zinc-400">{service.durationMinutes} min</span>
                  <span className="font-medium text-zinc-900 dark:text-white">${service.price}</span>
                </div>
              </div>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}
