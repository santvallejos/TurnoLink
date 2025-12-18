'use client';

import { useState, useEffect } from 'react';
import { useTranslations } from 'next-intl';
import type { Service } from '@/types';
import BookingForm from '@/components/booking/booking-form';
import { Calendar, AlertCircle, ArrowLeft, Loader2 } from 'lucide-react';
import Link from 'next/link';

interface Props {
  slug: string;
  locale: string;
}

/**
 * Componente cliente para la página de booking público
 * Carga los servicios del profesional dinámicamente desde el cliente
 */
export default function BookingPageClient({ slug, locale }: Props) {
  const t = useTranslations('public.booking');
  const [services, setServices] = useState<Service[]>([]);
  const [professionalName, setProfessionalName] = useState(slug);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchServices() {
      const API_URL = process.env.NEXT_PUBLIC_TURNOLINK_API_URL || 'http://localhost:5000';
      const fetchUrl = `${API_URL}/api/Public/${slug}`;

      try {
        const response = await fetch(fetchUrl);

        if (!response.ok) {
          setError('Professional not found');
          return;
        }

        const data: Service[] = await response.json();
        setServices(data);

        // Si hay servicios, usamos el userName del primer servicio
        if (data.length > 0 && data[0].userName) {
          setProfessionalName(data[0].userName);
        }
      } catch (err) {
        console.error('Error fetching services:', err);
        setError('Professional not found');
      } finally {
        setLoading(false);
      }
    }

    fetchServices();
  }, [slug]);

  // Loading State
  if (loading) {
    return (
      <div className='flex min-h-screen flex-col items-center justify-center bg-gradient-to-br from-background via-background to-primary/5 px-4'>
        <Loader2 className='h-12 w-12 animate-spin text-primary' />
        <p className='mt-4 text-muted-foreground'>{t('loading') || 'Loading...'}</p>
      </div>
    );
  }

  // Error State
  if (error || services.length === 0) {
    return (
      <div className='flex min-h-screen flex-col items-center justify-center bg-gradient-to-br from-background via-background to-primary/5 px-4'>
        <div className='w-full max-w-md text-center'>
          {/* Error Icon */}
          <div className='mx-auto mb-6 flex h-20 w-20 items-center justify-center rounded-full bg-red-100 dark:bg-red-900/30'>
            <AlertCircle className='h-10 w-10 text-red-600 dark:text-red-400' />
          </div>

          {/* Error Message */}
          <h1 className='mb-2 text-2xl font-bold text-foreground'>
            {t('notFound')}
          </h1>
          <p className='mb-8 text-muted-foreground'>{t('notFoundMessage')}</p>

          {/* Back Button */}
          <Link
            href={`/${locale}`}
            className='inline-flex items-center gap-2 rounded-xl bg-primary px-6 py-3 font-medium text-primary-foreground transition-all hover:bg-primary/90'
          >
            <ArrowLeft className='h-4 w-4' />
            {t('backHome')}
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className='min-h-screen bg-gradient-to-br from-background via-background to-primary/5'>
      {/* Header */}
      <header className='border-b border-border bg-card/80 backdrop-blur-md'>
        <div className='mx-auto max-w-4xl px-4 py-4 sm:px-6'>
          <div className='flex items-center justify-between'>
            {/* Logo/Brand */}
            <Link
              href={`/${locale}`}
              className='flex items-center gap-3 transition-opacity hover:opacity-80'
            >
              <div className='flex h-10 w-10 items-center justify-center rounded-xl bg-gradient-to-br from-primary to-primary/80 shadow-lg shadow-primary/25'>
                <Calendar className='h-5 w-5 text-primary-foreground' />
              </div>
              <span className='text-xl font-bold text-foreground'>
                TurnoLink
              </span>
            </Link>

            {/* Professional Info */}
            <div className='text-right'>
              <p className='text-xs text-muted-foreground'>
                {t('bookingWith')}
              </p>
              <p className='font-semibold text-foreground capitalize'>
                {professionalName}
              </p>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className='mx-auto max-w-4xl px-4 py-8 sm:px-6 lg:py-12'>
        {/* Page Title */}
        <div className='mb-8 text-center'>
          <h1 className='text-2xl font-bold text-foreground sm:text-3xl'>
            {t('title')}
          </h1>
          <p className='mt-2 text-muted-foreground'>{t('subtitle')}</p>
        </div>

        {/* Booking Form */}
        <BookingForm services={services} professionalName={professionalName} />
      </main>

      {/* Footer */}
      <footer className='border-t border-border bg-card/50 py-6'>
        <div className='mx-auto max-w-4xl px-4 text-center sm:px-6'>
          <p className='text-sm text-muted-foreground'>
            {t('poweredBy')}{' '}
            <Link href={`/${locale}`} className='font-medium text-primary hover:underline'>
              TurnoLink
            </Link>
          </p>
        </div>
      </footer>
    </div>
  );
}
