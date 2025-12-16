'use client';

import { useTranslations } from 'next-intl';
import Link from 'next/link';
import { Calendar, Twitter, Github, Linkedin, Instagram } from 'lucide-react';

const socialLinks = [
  { icon: Twitter, href: 'https://twitter.com', label: 'Twitter' },
  { icon: Github, href: 'https://github.com', label: 'GitHub' },
  { icon: Linkedin, href: 'https://linkedin.com', label: 'LinkedIn' },
  { icon: Instagram, href: 'https://instagram.com', label: 'Instagram' },
];

export default function Footer() {
  const t = useTranslations('landing.footer');
  const currentYear = new Date().getFullYear();

  return (
    <footer className='bg-card border-t border-border'>
      <div className='container mx-auto px-6 py-16'>
        <div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-12'>
          {/* Brand column */}
          <div className='lg:col-span-2'>
            <Link href='/' className='flex items-center gap-2 mb-4'>
              <div className='w-10 h-10 bg-primary rounded-xl flex items-center justify-center'>
                <Calendar className='w-5 h-5 text-primary-foreground' />
              </div>
              <span className='text-xl font-bold text-foreground'>
                TurnoLink
              </span>
            </Link>
            <p className='text-muted-foreground mb-6 max-w-sm'>
              {t('description')}
            </p>
            {/* Social links */}
            <div className='flex gap-4'>
              {socialLinks.map(({ icon: Icon, href, label }) => (
                <a
                  key={label}
                  href={href}
                  target='_blank'
                  rel='noopener noreferrer'
                  className='w-10 h-10 bg-secondary rounded-lg flex items-center justify-center text-muted-foreground hover:text-primary hover:bg-primary/10 transition-colors'
                  aria-label={label}
                >
                  <Icon className='w-5 h-5' />
                </a>
              ))}
            </div>
          </div>

          {/* Product column */}
          <div>
            <h4 className='font-semibold text-foreground mb-4'>
              {t('product.title')}
            </h4>
            <ul className='space-y-3'>
              <li>
                <Link
                  href='#features'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('product.features')}
                </Link>
              </li>
              <li>
                <Link
                  href='#pricing'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('product.pricing')}
                </Link>
              </li>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('product.integrations')}
                </Link>
              </li>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('product.changelog')}
                </Link>
              </li>
            </ul>
          </div>

          {/* Company column */}
          <div>
            <h4 className='font-semibold text-foreground mb-4'>
              {t('company.title')}
            </h4>
            <ul className='space-y-3'>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('company.about')}
                </Link>
              </li>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('company.blog')}
                </Link>
              </li>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('company.careers')}
                </Link>
              </li>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('company.contact')}
                </Link>
              </li>
            </ul>
          </div>

          {/* Legal column */}
          <div>
            <h4 className='font-semibold text-foreground mb-4'>
              {t('legal.title')}
            </h4>
            <ul className='space-y-3'>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('legal.privacy')}
                </Link>
              </li>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('legal.terms')}
                </Link>
              </li>
              <li>
                <Link
                  href='#'
                  className='text-muted-foreground hover:text-foreground transition-colors'
                >
                  {t('legal.cookies')}
                </Link>
              </li>
            </ul>
          </div>
        </div>

        {/* Bottom bar */}
        <div className='mt-12 pt-8 border-t border-border flex flex-col md:flex-row justify-between items-center gap-4'>
          <p className='text-sm text-muted-foreground'>
            © {currentYear} TurnoLink. {t('copyright')}
          </p>
          <div className='flex items-center gap-6'>
            <Link
              href='#'
              className='text-sm text-muted-foreground hover:text-foreground transition-colors'
            >
              {t('legal.privacy')}
            </Link>
            <Link
              href='#'
              className='text-sm text-muted-foreground hover:text-foreground transition-colors'
            >
              {t('legal.terms')}
            </Link>
          </div>
        </div>
      </div>
    </footer>
  );
}
