'use client';

import { useTranslations } from 'next-intl';
import { Check, X } from 'lucide-react';
import Link from 'next/link';

type PlanKey = 'free' | 'pro' | 'business';

const plans: { key: PlanKey; popular: boolean }[] = [
  { key: 'free', popular: false },
  { key: 'pro', popular: true },
  { key: 'business', popular: false },
];

export default function Pricing() {
  const t = useTranslations('landing.pricing');

  return (
    <section id='pricing' className='py-24 bg-background'>
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

        {/* Pricing cards */}
        <div className='grid grid-cols-1 md:grid-cols-3 gap-8 max-w-6xl mx-auto'>
          {plans.map(({ key, popular }) => (
            <div
              key={key}
              className={`relative bg-card rounded-2xl border ${
                popular
                  ? 'border-primary shadow-xl shadow-primary/10 scale-105'
                  : 'border-border'
              } overflow-hidden transition-all duration-300 hover:shadow-lg`}
            >
              {/* Popular badge */}
              {popular && (
                <div className='absolute top-0 right-0'>
                  <div className='bg-primary text-primary-foreground text-xs font-bold px-4 py-1 rounded-bl-xl'>
                    {t('popularBadge')}
                  </div>
                </div>
              )}

              <div className='p-8'>
                {/* Plan name */}
                <h3 className='text-xl font-semibold text-foreground mb-2'>
                  {t(`plans.${key}.name`)}
                </h3>
                <p className='text-sm text-muted-foreground mb-6'>
                  {t(`plans.${key}.description`)}
                </p>

                {/* Price */}
                <div className='mb-6'>
                  <span className='text-4xl font-bold text-foreground'>
                    {t(`plans.${key}.price`)}
                  </span>
                  {key !== 'free' && (
                    <span className='text-muted-foreground'>
                      {t('perMonth')}
                    </span>
                  )}
                </div>

                {/* CTA Button */}
                <Link
                  href='/register'
                  className={`block w-full text-center py-3 rounded-xl font-semibold transition-all duration-300 ${
                    popular
                      ? 'bg-primary text-primary-foreground hover:bg-primary/90 shadow-lg shadow-primary/25'
                      : 'bg-secondary text-secondary-foreground hover:bg-accent'
                  }`}
                >
                  {t(`plans.${key}.cta`)}
                </Link>

                {/* Features list */}
                <ul className='mt-8 space-y-4'>
                  {[1, 2, 3, 4, 5].map((i) => {
                    const featureKey = `plans.${key}.features.${i}`;
                    const feature = t.raw(featureKey) as
                      | { text: string; included: boolean }
                      | undefined;

                    if (!feature) return null;

                    return (
                      <li key={i} className='flex items-start gap-3'>
                        {feature.included ? (
                          <div className='w-5 h-5 bg-green-100 dark:bg-green-900/30 rounded-full flex items-center justify-center flex-shrink-0 mt-0.5'>
                            <Check className='w-3 h-3 text-green-600' />
                          </div>
                        ) : (
                          <div className='w-5 h-5 bg-muted rounded-full flex items-center justify-center flex-shrink-0 mt-0.5'>
                            <X className='w-3 h-3 text-muted-foreground' />
                          </div>
                        )}
                        <span
                          className={`text-sm ${
                            feature.included
                              ? 'text-foreground'
                              : 'text-muted-foreground'
                          }`}
                        >
                          {feature.text}
                        </span>
                      </li>
                    );
                  })}
                </ul>
              </div>
            </div>
          ))}
        </div>

        {/* FAQ hint */}
        <div className='text-center mt-16'>
          <p className='text-muted-foreground'>
            {t('faqHint')}{' '}
            <a href='#faq' className='text-primary hover:underline'>
              {t('faqLink')}
            </a>
          </p>
        </div>
      </div>
    </section>
  );
}
