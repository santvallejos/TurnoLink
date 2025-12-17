import { getTranslations } from 'next-intl/server';
import type { Service } from '@/types';
import BookingForm from './booking-form';
import { Calendar, AlertCircle, ArrowLeft } from 'lucide-react';
import Link from 'next/link';

interface Props {
  params: Promise<{ slug: string }>;
}

export default async function PublicBookingPage({ params }: Props) {
  const { slug } = await params;
  const t = await getTranslations('public.booking');

  let services: Service[] = [];
  let error = null;
  let professionalName = slug; // Por defecto usa el slug

  // URL de la API para Server Components
  const API_URL = process.env.NEXT_PUBLIC_TURNOLINK_API_URL || 'http://localhost:5009';
  const fetchUrl = `${API_URL}/api/Public/${slug}`;

  console.log('=== DEBUG BOOKING PAGE ===');
  console.log('API_URL:', API_URL);
  console.log('Slug:', slug);
  console.log('Fetch URL:', fetchUrl);

  try {
    // Fetch directo desde el servidor para evitar problemas con el cliente API
    const response = await fetch(fetchUrl, {
      cache: 'no-store', // No cachear para obtener datos frescas
    });
    
    console.log('Response status:', response.status);
    console.log('Response ok:', response.ok);
    
    if (!response.ok) {
      const errorText = await response.text();
      console.log('Error response:', errorText);
      error = 'Professional not found';
    } else {
      services = await response.json();
      console.log('Services received:', services.length);
      console.log('Services data:', JSON.stringify(services, null, 2));
      // Si hay servicios, usamos el userName del primer servicio
      if (services.length > 0 && services[0].userName) {
        professionalName = services[0].userName;
      }
    }
  } catch (err) {
    console.error('=== FETCH ERROR ===');
    console.error('Error fetching services:', err);
    error = 'Professional not found';
  }
  
  console.log('Final error:', error);
  console.log('Final services count:', services.length);

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
            href='/'
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
              href='/'
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
            <Link href='/' className='font-medium text-primary hover:underline'>
              TurnoLink
            </Link>
          </p>
        </div>
      </footer>
    </div>
  );
}
