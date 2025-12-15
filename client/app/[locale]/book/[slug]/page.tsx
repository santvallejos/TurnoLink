import { getTranslations } from 'next-intl/server';
import { publicService } from '@/lib/services';
import type { Service } from '@/types';
import BookingForm from './booking-form';

interface Props {
  params: Promise<{ slug: string }>;
}

export default async function PublicBookingPage({ params }: Props) {
  const { slug } = await params;
  const t = await getTranslations('public.booking');

  let services: Service[] = [];
  let error = null;

  try {
    services = await publicService.getServicesBySlug(slug);
  } catch {
    error = 'Professional not found';
  }

  if (error || services.length === 0) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p className="text-zinc-600">Professional not found: {slug}</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-zinc-50 dark:bg-zinc-950">
      <header className="border-b border-zinc-200 bg-white dark:border-zinc-800 dark:bg-zinc-900">
        <div className="mx-auto max-w-2xl px-6 py-4">
          <h1 className="text-xl font-bold text-zinc-900 dark:text-white">
            {t('title')}
          </h1>
        </div>
      </header>

      <main className="mx-auto max-w-2xl px-6 py-8">
        <BookingForm services={services} />
      </main>
    </div>
  );
}
