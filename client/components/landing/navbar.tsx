'use client';

import { useState, useEffect } from 'react';
import { useTranslations, useLocale } from 'next-intl';
import { Menu, X } from 'lucide-react';
import Link from 'next/link';
import Image from 'next/image';

export default function Navbar() {
  const t = useTranslations('navbar');
  const locale = useLocale();
  const [isScrolled, setIsScrolled] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  const navLinks = [
    { href: '/', label: t('home') },
    { href: '#features', label: t('about') },
    { href: '#pricing', label: t('pricing') },
  ];

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 20);
    };
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  return (
    <nav
      className={`fixed top-0 left-0 right-0 z-50 transition-all duration-300 ${
        isScrolled
          ? 'bg-background/80 backdrop-blur-xl border-b border-border shadow-sm'
          : 'bg-transparent'
      }`}
    >
      <div className='container mx-auto px-6'>
        <div className='flex items-center justify-between h-20'>
          {/* Logo */}
          <Link href='/' className='flex items-center'>
            <Image
              src='/link-circle-svgrepo-com.png'
              alt='TurnoLink Logo'
              width={40}
              height={40}
            />
            <span className='text-xl font-bold text-foreground'>TurnoLink</span>
          </Link>

          {/* Desktop Navigation */}
          <div className='hidden md:flex items-center gap-8'>
            {navLinks.map((link) => (
              <Link
                key={link.href}
                href={link.href}
                className='text-muted-foreground hover:text-foreground transition-colors font-medium'
              >
                {link.label}
              </Link>
            ))}
          </div>

          {/* Desktop CTA */}
          <div className='hidden md:flex items-center gap-4'>
            <Link
              href={`/${locale}/login`}
              className='px-4 py-2 text-muted-foreground hover:text-foreground transition-colors font-medium'
            >
              {t('login')}
            </Link>
            <Link
              href={`/${locale}/register`}
              className='px-6 py-2.5 bg-primary text-primary-foreground rounded-xl font-semibold hover:bg-primary/90 shadow-lg shadow-primary/25 transition-all duration-300'
            >
              {t('register')}
            </Link>
          </div>

          {/* Mobile menu button */}
          <button
            onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
            className='md:hidden p-2 text-foreground'
            aria-label='Toggle menu'
          >
            {isMobileMenuOpen ? (
              <X className='w-6 h-6' />
            ) : (
              <Menu className='w-6 h-6' />
            )}
          </button>
        </div>

        {/* Mobile Navigation */}
        {isMobileMenuOpen && (
          <div className='md:hidden absolute top-20 left-0 right-0 bg-background border-b border-border shadow-lg'>
            <div className='container mx-auto px-6 py-6 space-y-4'>
              {navLinks.map((link) => (
                <Link
                  key={link.href}
                  href={link.href}
                  onClick={() => setIsMobileMenuOpen(false)}
                  className='block text-lg text-muted-foreground hover:text-foreground transition-colors font-medium py-2'
                >
                  {link.label}
                </Link>
              ))}
              <div className='pt-4 space-y-3 border-t border-border'>
                <Link
                  href={`/${locale}/login`}
                  onClick={() => setIsMobileMenuOpen(false)}
                  className='block w-full text-center py-3 text-foreground font-medium border border-border rounded-xl hover:bg-accent transition-colors'
                >
                  {t('login')}
                </Link>
                <Link
                  href={`/${locale}/register`}
                  onClick={() => setIsMobileMenuOpen(false)}
                  className='block w-full text-center py-3 bg-primary text-primary-foreground rounded-xl font-semibold hover:bg-primary/90 transition-colors'
                >
                  {t('register')}
                </Link>
              </div>
            </div>
          </div>
        )}
      </div>
    </nav>
  );
}
