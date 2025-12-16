'use client';

import { useTranslations, useLocale } from 'next-intl';
import Link from 'next/link';
import { ArrowRight, Sparkles } from 'lucide-react';

export default function CTA() {
  const t = useTranslations('landing.cta');
  const locale = useLocale();

  return (
    <section className='py-24 bg-background relative overflow-hidden'>
      {/* Background decorations */}
      <div className='absolute inset-0'>
        <div className='absolute top-0 left-1/4 w-96 h-96 bg-primary/10 rounded-full blur-3xl' />
        <div className='absolute bottom-0 right-1/4 w-96 h-96 bg-primary/10 rounded-full blur-3xl' />
      </div>

      <div className='container mx-auto px-6 relative z-10'>
        <div className='bg-linear-to-br from-primary to-primary/80 rounded-3xl p-12 md:p-16 lg:p-20 text-center relative overflow-hidden'>
          {/* Decorative elements */}
          <div className='absolute top-0 left-0 w-full h-full'>
            <div className='absolute top-10 left-10 w-20 h-20 border border-white/20 rounded-full' />
            <div className='absolute bottom-10 right-10 w-32 h-32 border border-white/20 rounded-full' />
            <div className='absolute top-1/2 right-20 w-8 h-8 bg-white/10 rounded-lg rotate-45' />
            <div className='absolute bottom-1/3 left-16 w-12 h-12 bg-white/10 rounded-lg rotate-12' />
          </div>

          <div className='relative z-10'>
            {/* Badge */}
            <div className='inline-flex items-center gap-2 px-4 py-2 rounded-full bg-white/20 text-white text-sm font-medium mb-8'>
              <Sparkles className='w-4 h-4' />
              <span>{t('badge')}</span>
            </div>

            {/* Title */}
            <h2 className='text-3xl sm:text-4xl lg:text-5xl font-bold text-white mb-6 max-w-3xl mx-auto'>
              {t('title')}
            </h2>

            {/* Subtitle */}
            <p className='text-lg text-white/80 mb-10 max-w-2xl mx-auto'>
              {t('subtitle')}
            </p>

            {/* CTA Buttons */}
            <div className='flex flex-col sm:flex-row gap-4 justify-center'>
              <Link
                href={`/${locale}/register`}
                className='group inline-flex items-center justify-center gap-2 px-8 py-4 bg-white text-primary rounded-xl font-semibold text-lg hover:bg-white/90 hover:scale-105 transition-all duration-300'
              >
                {t('primaryButton')}
                <ArrowRight className='w-5 h-5 group-hover:translate-x-1 transition-transform' />
              </Link>
            </div>

            {/* Trust text */}
            <p className='text-white/60 text-sm mt-8'>{t('trustText')}</p>
          </div>
        </div>
      </div>
    </section>
  );
}
