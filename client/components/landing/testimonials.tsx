'use client';

import { useTranslations } from 'next-intl';
import { Star, Quote } from 'lucide-react';

const testimonialKeys = ['1', '2', '3', '4', '5', '6'] as const;

export default function Testimonials() {
  const t = useTranslations('landing.testimonials');

  return (
    <section
      id='testimonials'
      className='py-24 bg-gradient-to-b from-secondary/20 to-background overflow-hidden'
    >
      <div className='container mx-auto px-6'>
        {/* Section header */}
        <div className='text-center max-w-3xl mx-auto mb-16'>
          <span className='inline-block px-4 py-1.5 rounded-full bg-primary/10 text-primary text-sm font-medium mb-4'>
            {t('badge')}
          </span>
          <h2 className='text-3xl sm:text-4xl lg:text-5xl font-bold text-foreground mb-6'>
            {t('title')}
          </h2>
          <p className='text-lg text-muted-foreground'>{t('subtitle')}</p>
        </div>

        {/* Testimonials grid */}
        <div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6'>
          {testimonialKeys.map((key) => (
            <div
              key={key}
              className='group relative bg-card border border-border rounded-2xl p-6 hover:border-primary/30 hover:shadow-lg transition-all duration-300'
            >
              {/* Quote icon */}
              <div className='absolute -top-3 -left-3 w-10 h-10 bg-primary/10 rounded-full flex items-center justify-center'>
                <Quote className='w-5 h-5 text-primary' />
              </div>

              {/* Stars */}
              <div className='flex gap-1 mb-4 pt-2'>
                {[...Array(5)].map((_, i) => (
                  <Star
                    key={`star-${i}`}
                    className='w-4 h-4 fill-yellow-400 text-yellow-400'
                  />
                ))}
              </div>

              {/* Quote text */}
              <p className='text-foreground leading-relaxed mb-6'>
                &quot;{t(`items.${key}.quote`)}&quot;
              </p>

              {/* Author */}
              <div className='flex items-center gap-3 pt-4 border-t border-border'>
                <div className='w-12 h-12 bg-gradient-to-br from-primary to-primary/60 rounded-full flex items-center justify-center text-white font-semibold'>
                  {t(`items.${key}.name`).charAt(0)}
                </div>
                <div>
                  <p className='font-semibold text-foreground'>
                    {t(`items.${key}.name`)}
                  </p>
                  <p className='text-sm text-muted-foreground'>
                    {t(`items.${key}.role`)}
                  </p>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
