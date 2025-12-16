'use client';

import { useTranslations } from 'next-intl';
import {
  Calendar,
  Bell,
  Users,
  BarChart3,
  Globe,
  Smartphone,
  Shield,
  Zap,
} from 'lucide-react';

const featureIcons = {
  calendar: Calendar,
  notifications: Bell,
  clients: Users,
  analytics: BarChart3,
  multilingual: Globe,
  mobile: Smartphone,
  security: Shield,
  fast: Zap,
};

type FeatureKey = keyof typeof featureIcons;

const featureKeys: FeatureKey[] = [
  'calendar',
  'notifications',
  'clients',
  'analytics',
  'multilingual',
  'mobile',
  'security',
  'fast',
];

export default function Features() {
  const t = useTranslations('landing.features');

  return (
    <section
      id='features'
      className='py-24 bg-gradient-to-b from-background to-secondary/20'
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

        {/* Features grid */}
        <div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6'>
          {featureKeys.map((key, index) => {
            const Icon = featureIcons[key];
            return (
              <div
                key={key}
                className='group relative bg-card border border-border rounded-2xl p-6 hover:border-primary/50 hover:shadow-lg hover:shadow-primary/5 transition-all duration-300'
                style={{ animationDelay: `${index * 100}ms` }}
              >
                {/* Icon */}
                <div className='w-12 h-12 bg-primary/10 rounded-xl flex items-center justify-center mb-4 group-hover:bg-primary/20 group-hover:scale-110 transition-all duration-300'>
                  <Icon className='w-6 h-6 text-primary' />
                </div>

                {/* Content */}
                <h3 className='text-lg font-semibold text-foreground mb-2'>
                  {t(`items.${key}.title`)}
                </h3>
                <p className='text-sm text-muted-foreground leading-relaxed'>
                  {t(`items.${key}.description`)}
                </p>

                {/* Hover gradient */}
                <div className='absolute inset-0 bg-gradient-to-br from-primary/5 to-transparent opacity-0 group-hover:opacity-100 rounded-2xl transition-opacity duration-300' />
              </div>
            );
          })}
        </div>

        {/* Bottom CTA */}
        <div className='text-center mt-16'>
          <p className='text-muted-foreground mb-4'>{t('cta.text')}</p>
          <a
            href='#pricing'
            className='inline-flex items-center gap-2 text-primary font-medium hover:underline'
          >
            {t('cta.link')}
            <svg
              className='w-4 h-4'
              fill='none'
              viewBox='0 0 24 24'
              stroke='currentColor'
            >
              <path
                strokeLinecap='round'
                strokeLinejoin='round'
                strokeWidth={2}
                d='M17 8l4 4m0 0l-4 4m4-4H3'
              />
            </svg>
          </a>
        </div>
      </div>
    </section>
  );
}
