'use client';

import { useTranslations } from 'next-intl';
import { Calendar, Clock, ArrowRight } from 'lucide-react';
import Link from 'next/link';

export default function Hero() {
  const t = useTranslations('landing.hero');

  return (
    <section className='relative min-h-screen flex items-center justify-center overflow-hidden bg-gradient-to-br from-background via-background to-primary/5'>
      {/* Background decoration */}
      <div className='absolute inset-0 overflow-hidden'>
        <div className='absolute -top-40 -right-40 w-80 h-80 bg-primary/10 rounded-full blur-3xl' />
        <div className='absolute -bottom-40 -left-40 w-80 h-80 bg-primary/10 rounded-full blur-3xl' />
        <div className='absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] h-[600px] bg-primary/5 rounded-full blur-3xl' />
      </div>

      {/* Grid pattern */}
      <div className='absolute inset-0 bg-[linear-gradient(rgba(139,92,246,0.03)_1px,transparent_1px),linear-gradient(90deg,rgba(139,92,246,0.03)_1px,transparent_1px)] bg-[size:64px_64px]' />

      <div className='relative z-10 container mx-auto px-6 py-20'>
        <div className='flex flex-col lg:flex-row items-center gap-12 lg:gap-20'>
          {/* Left content */}
          <div className='flex-1 text-center lg:text-left'>
            {/* Badge */}
            <div className='inline-flex items-center gap-2 px-4 py-2 rounded-full bg-primary/10 border border-primary/20 text-primary text-sm font-medium mb-8'>
              <Calendar className='w-4 h-4' />
              <span>{t('badge')}</span>
            </div>

            {/* Title */}
            <h1 className='text-4xl sm:text-5xl lg:text-6xl xl:text-7xl font-bold text-foreground leading-tight mb-6'>
              {t('titleStart')}{' '}
              <span className='text-transparent bg-clip-text bg-gradient-to-r from-primary to-primary/60'>
                {t('titleHighlight')}
              </span>{' '}
              {t('titleEnd')}
            </h1>

            {/* Subtitle */}
            <p className='text-lg sm:text-xl text-muted-foreground max-w-2xl mx-auto lg:mx-0 mb-10'>
              {t('subtitle')}
            </p>

            {/* CTA Buttons */}
            <div className='flex flex-col sm:flex-row gap-4 justify-center lg:justify-start'>
              <Link
                href='/register'
                className='group inline-flex items-center justify-center gap-2 px-8 py-4 bg-primary text-primary-foreground rounded-xl font-semibold text-lg shadow-lg shadow-primary/25 hover:shadow-xl hover:shadow-primary/30 hover:scale-105 transition-all duration-300'
              >
                {t('ctaPrimary')}
                <ArrowRight className='w-5 h-5 group-hover:translate-x-1 transition-transform' />
              </Link>
              <Link
                href='#demo'
                className='inline-flex items-center justify-center gap-2 px-8 py-4 bg-secondary text-secondary-foreground rounded-xl font-semibold text-lg border border-border hover:bg-accent hover:scale-105 transition-all duration-300'
              >
                {t('ctaSecondary')}
              </Link>
            </div>

            {/* Stats */}
            <div className='flex flex-wrap gap-8 mt-12 justify-center lg:justify-start'>
              <div className='text-center lg:text-left'>
                <p className='text-3xl font-bold text-foreground'>10k+</p>
                <p className='text-sm text-muted-foreground'>
                  {t('statsUsers')}
                </p>
              </div>
              <div className='text-center lg:text-left'>
                <p className='text-3xl font-bold text-foreground'>50k+</p>
                <p className='text-sm text-muted-foreground'>
                  {t('statsBookings')}
                </p>
              </div>
              <div className='text-center lg:text-left'>
                <p className='text-3xl font-bold text-foreground'>99.9%</p>
                <p className='text-sm text-muted-foreground'>
                  {t('statsUptime')}
                </p>
              </div>
            </div>
          </div>

          {/* Right content - Preview card */}
          <div className='flex-1 w-full max-w-lg lg:max-w-xl'>
            <div className='relative'>
              {/* Glow effect */}
              <div className='absolute -inset-4 bg-gradient-to-r from-primary/20 via-primary/10 to-primary/20 rounded-3xl blur-2xl' />
              {/* Main card */}
              <div className='relative bg-card border border-border rounded-2xl shadow-2xl overflow-hidden'>
                {/* Header */}
                <div className='bg-gradient-to-r from-primary to-primary/80 px-6 py-4'>
                  <div className='flex items-center gap-3'>
                    <div className='w-10 h-10 bg-white/20 rounded-full flex items-center justify-center'>
                      <Calendar className='w-5 h-5 text-white' />
                    </div>
                    <div>
                      <h3 className='text-white font-semibold'>
                        {t('previewTitle')}
                      </h3>
                      <p className='text-white/80 text-sm'>
                        {t('previewSubtitle')}
                      </p>
                    </div>
                  </div>
                </div>

                {/* Calendar preview */}
                <div className='p-6'>
                  {/* Mini calendar header */}
                  <div className='flex justify-between items-center mb-4'>
                    <span className='font-semibold text-foreground'>
                      {t('previewMonth')}
                    </span>
                    <div className='flex gap-2'>
                      <button className='p-1 hover:bg-accent rounded'>‹</button>
                      <button className='p-1 hover:bg-accent rounded'>›</button>
                    </div>
                  </div>

                  {/* Calendar grid */}
                  <div className='grid grid-cols-7 gap-1 mb-6'>
                    {['L', 'M', 'X', 'J', 'V', 'S', 'D'].map((day) => (
                      <div
                        key={day}
                        className='text-center text-xs text-muted-foreground py-2'
                      >
                        {day}
                      </div>
                    ))}
                    {[...Array(31)].map((_, i) => (
                      <button
                        key={`day-${i + 1}`}
                        className={`aspect-square rounded-lg text-sm flex items-center justify-center transition-colors ${
                          i === 14
                            ? 'bg-primary text-primary-foreground'
                            : i === 7 || i === 21 ? 'bg-primary/10 text-primary' : 'hover:bg-accent'
                        }`}
                      >
                        {i + 1}
                      </button>
                    ))}
                  </div>

                  {/* Time slots */}
                  <div className='space-y-2'>
                    <p className='text-sm font-medium text-foreground mb-3'>
                      {t('previewAvailable')}
                    </p>
                    <div className='flex flex-wrap gap-2'>
                      {['09:00', '10:30', '14:00', '16:30'].map((time) => (
                        <button
                          key={time}
                          className='flex items-center gap-1.5 px-3 py-2 rounded-lg border border-border hover:border-primary hover:bg-primary/5 transition-colors'
                        >
                          <Clock className='w-3.5 h-3.5 text-primary' />
                          <span className='text-sm'>{time}</span>
                        </button>
                      ))}
                    </div>
                  </div>
                </div>
              </div>

              {/* Floating notification */}
              <div className='absolute -bottom-4 -left-4 bg-card border border-border rounded-xl p-4 shadow-lg animate-bounce'>
                <div className='flex items-center gap-3'>
                  <div className='w-8 h-8 bg-green-100 dark:bg-green-900/30 rounded-full flex items-center justify-center'>
                    <svg
                      className='w-4 h-4 text-green-600'
                      fill='none'
                      viewBox='0 0 24 24'
                      stroke='currentColor'
                    >
                      <path
                        strokeLinecap='round'
                        strokeLinejoin='round'
                        strokeWidth={2}
                        d='M5 13l4 4L19 7'
                      />
                    </svg>
                  </div>
                  <div>
                    <p className='text-sm font-medium text-foreground'>
                      {t('notificationTitle')}
                    </p>
                    <p className='text-xs text-muted-foreground'>
                      {t('notificationTime')}
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Scroll indicator */}
      <div className='absolute bottom-8 left-1/2 -translate-x-1/2 animate-bounce'>
        <div className='w-6 h-10 border-2 border-muted-foreground/30 rounded-full flex justify-center pt-2'>
          <div className='w-1.5 h-1.5 bg-muted-foreground/50 rounded-full animate-pulse' />
        </div>
      </div>
    </section>
  );
}
