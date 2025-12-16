'use client';

import { useState } from 'react';
import { useTranslations, useLocale } from 'next-intl';
import { Link } from '@/lib/i18n/navigation';
import { authService } from '@/lib/services';
import type { ApiError } from '@/types';
import {
  Calendar,
  Mail,
  Lock,
  Eye,
  EyeOff,
  ArrowRight,
  Loader2,
  User,
  Phone,
  CheckCircle2,
} from 'lucide-react';
import Image from 'next/image';

export default function RegisterPage() {
  const t = useTranslations('auth.register');
  const locale = useLocale();
  const tErrors = useTranslations('auth.errors');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showRepeatPassword, setShowRepeatPassword] = useState(false);

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError(null);
    setLoading(true);

    const formData = new FormData(e.currentTarget);
    const password = formData.get('password') as string;
    const repeatPassword = formData.get('repeatPassword') as string;

    if (password !== repeatPassword) {
      setError(tErrors('passwordMismatch'));
      setLoading(false);
      return;
    }

    try {
      await authService.register({
        name: formData.get('name') as string,
        surname: formData.get('surname') as string,
        email: formData.get('email') as string,
        password,
        repeatPassword,
        phoneNumber: (formData.get('phone') as string) || undefined,
      });
      window.location.href = `/${locale}/dashboard`;
    } catch (err) {
      const apiError = err as ApiError;
      setError(apiError.message);
    } finally {
      setLoading(false);
    }
  }

  const benefits = ['benefit1', 'benefit2', 'benefit3', 'benefit4'] as const;

  return (
    <div className='flex min-h-screen'>
      {/* Left side - Decorative */}
      <div className='relative hidden lg:block lg:w-1/2'>
        <div className='absolute inset-0 bg-gradient-to-br from-primary to-primary/80'>
          {/* Decorative elements */}
          <div className='absolute inset-0 overflow-hidden'>
            <div className='absolute -left-20 -top-20 h-64 w-64 rounded-full border border-white/10' />
            <div className='absolute -bottom-32 -right-32 h-96 w-96 rounded-full border border-white/10' />
            <div className='absolute left-1/4 top-1/3 h-48 w-48 rounded-full bg-white/5' />
            <div className='absolute bottom-1/4 right-1/4 h-32 w-32 rotate-45 bg-white/5' />
          </div>

          {/* Content */}
          <div className='relative flex h-full flex-col items-center justify-center px-12 text-center'>
            <div className='mb-8 flex h-20 w-20 items-center justify-center rounded-2xl bg-white/20 backdrop-blur-sm'>
              <Calendar className='h-10 w-10 text-white' />
            </div>
            <h2 className='mb-4 text-3xl font-bold text-white'>
              {t('welcomeTitle')}
            </h2>
            <p className='max-w-sm text-lg text-white/80'>
              {t('welcomeMessage')}
            </p>

            {/* Benefits list */}
            <div className='mt-12 space-y-4 text-left'>
              {benefits.map((key) => (
                <div key={key} className='flex items-center gap-3'>
                  <div className='flex h-6 w-6 items-center justify-center rounded-full bg-white/20'>
                    <CheckCircle2 className='h-4 w-4 text-white' />
                  </div>
                  <span className='text-white/90'>{t(key)}</span>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>

      {/* Right side - Form */}
      <div className='flex w-full flex-col justify-center px-6 py-12 lg:w-1/2 lg:px-16 xl:px-24'>
        <div className='mx-auto w-full max-w-md'>
          {/* Logo */}
          <Link href='/' className='mb-8 flex items-center'>
            <Image
              src='/link-circle-svgrepo-com.png'
              alt='TurnoLink Logo'
              width={40}
              height={40}
            />
            <span className='text-xl font-bold text-foreground'>TurnoLink</span>
          </Link>

          {/* Header */}
          <div className='mb-8'>
            <h1 className='text-3xl font-bold text-foreground'>{t('title')}</h1>
            <p className='mt-2 text-muted-foreground'>{t('subtitle')}</p>
          </div>

          {/* Form */}
          <form onSubmit={handleSubmit} className='space-y-4'>
            {/* Name fields */}
            <div className='grid grid-cols-2 gap-4'>
              <div>
                <label
                  htmlFor='name'
                  className='mb-2 block text-sm font-medium text-foreground'
                >
                  {t('name')}
                </label>
                <div className='relative'>
                  <div className='pointer-events-none absolute inset-y-0 left-0 flex items-center pl-4'>
                    <User className='h-5 w-5 text-muted-foreground' />
                  </div>
                  <input
                    id='name'
                    name='name'
                    type='text'
                    required
                    placeholder={t('namePlaceholder')}
                    className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                  />
                </div>
              </div>
              <div>
                <label
                  htmlFor='surname'
                  className='mb-2 block text-sm font-medium text-foreground'
                >
                  {t('surname')}
                </label>
                <div className='relative'>
                  <input
                    id='surname'
                    name='surname'
                    type='text'
                    required
                    placeholder={t('surnamePlaceholder')}
                    className='w-full rounded-xl border border-border bg-background py-3 px-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                  />
                </div>
              </div>
            </div>

            {/* Email field */}
            <div>
              <label
                htmlFor='email'
                className='mb-2 block text-sm font-medium text-foreground'
              >
                {t('email')}
              </label>
              <div className='relative'>
                <div className='pointer-events-none absolute inset-y-0 left-0 flex items-center pl-4'>
                  <Mail className='h-5 w-5 text-muted-foreground' />
                </div>
                <input
                  id='email'
                  name='email'
                  type='email'
                  required
                  placeholder={t('emailPlaceholder')}
                  className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                />
              </div>
            </div>

            {/* Phone field */}
            <div>
              <label
                htmlFor='phone'
                className='mb-2 block text-sm font-medium text-foreground'
              >
                {t('phone')}
              </label>
              <div className='relative'>
                <div className='pointer-events-none absolute inset-y-0 left-0 flex items-center pl-4'>
                  <Phone className='h-5 w-5 text-muted-foreground' />
                </div>
                <input
                  id='phone'
                  name='phone'
                  type='tel'
                  placeholder={t('phonePlaceholder')}
                  className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                />
              </div>
            </div>

            {/* Password field */}
            <div>
              <label
                htmlFor='password'
                className='mb-2 block text-sm font-medium text-foreground'
              >
                {t('password')}
              </label>
              <div className='relative'>
                <div className='pointer-events-none absolute inset-y-0 left-0 flex items-center pl-4'>
                  <Lock className='h-5 w-5 text-muted-foreground' />
                </div>
                <input
                  id='password'
                  name='password'
                  type={showPassword ? 'text' : 'password'}
                  required
                  placeholder='••••••••'
                  className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-12 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                />
                <button
                  type='button'
                  onClick={() => setShowPassword(!showPassword)}
                  className='absolute inset-y-0 right-0 flex items-center pr-4 text-muted-foreground hover:text-foreground'
                >
                  {showPassword ? (
                    <EyeOff className='h-5 w-5' />
                  ) : (
                    <Eye className='h-5 w-5' />
                  )}
                </button>
              </div>
            </div>

            {/* Repeat password field */}
            <div>
              <label
                htmlFor='repeatPassword'
                className='mb-2 block text-sm font-medium text-foreground'
              >
                {t('repeatPassword')}
              </label>
              <div className='relative'>
                <div className='pointer-events-none absolute inset-y-0 left-0 flex items-center pl-4'>
                  <Lock className='h-5 w-5 text-muted-foreground' />
                </div>
                <input
                  id='repeatPassword'
                  name='repeatPassword'
                  type={showRepeatPassword ? 'text' : 'password'}
                  required
                  placeholder='••••••••'
                  className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-12 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                />
                <button
                  type='button'
                  onClick={() => setShowRepeatPassword(!showRepeatPassword)}
                  className='absolute inset-y-0 right-0 flex items-center pr-4 text-muted-foreground hover:text-foreground'
                >
                  {showRepeatPassword ? (
                    <EyeOff className='h-5 w-5' />
                  ) : (
                    <Eye className='h-5 w-5' />
                  )}
                </button>
              </div>
            </div>

            {/* Error message */}
            {error && (
              <div className='rounded-lg border border-destructive/20 bg-destructive/10 px-4 py-3 text-sm text-destructive'>
                {error}
              </div>
            )}

            {/* Terms checkbox */}
            <div className='flex items-start gap-3'>
              <input
                id='terms'
                name='terms'
                type='checkbox'
                required
                className='mt-1 h-4 w-4 rounded border-border text-primary focus:ring-primary'
              />
              <label htmlFor='terms' className='text-sm text-muted-foreground'>
                {t('termsText')}{' '}
                <Link href='/terms' className='text-primary hover:underline'>
                  {t('termsLink')}
                </Link>{' '}
                {t('termsAnd')}{' '}
                <Link href='/privacy' className='text-primary hover:underline'>
                  {t('privacyLink')}
                </Link>
              </label>
            </div>

            {/* Submit button */}
            <button
              type='submit'
              disabled={loading}
              className='group flex w-full items-center justify-center gap-2 rounded-xl bg-primary px-6 py-3 font-semibold text-primary-foreground shadow-lg shadow-primary/25 transition-all duration-300 hover:scale-[1.02] hover:shadow-xl hover:shadow-primary/30 disabled:cursor-not-allowed disabled:opacity-50'
            >
              {loading ? (
                <>
                  <Loader2 className='h-5 w-5 animate-spin' />
                  {t('loading')}
                </>
              ) : (
                <>
                  {t('submit')}
                  <ArrowRight className='h-5 w-5 transition-transform group-hover:translate-x-1' />
                </>
              )}
            </button>
          </form>

          {/* Divider */}
          {/* <div className='my-6 flex items-center gap-4'>
            <div className='h-px flex-1 bg-border' />
            <span className='text-sm text-muted-foreground'>{t('or')}</span>
            <div className='h-px flex-1 bg-border' />
          </div> */}

          {/* Social login buttons */}
          {/* <button
            type='button'
            className='flex w-full items-center justify-center gap-3 rounded-xl border border-border bg-background py-3 font-medium text-foreground transition-colors hover:bg-accent'
          >
            <svg className='h-5 w-5' viewBox='0 0 24 24'>
              <path
                fill='currentColor'
                d='M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z'
              />
              <path
                fill='currentColor'
                d='M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z'
              />
              <path
                fill='currentColor'
                d='M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z'
              />
              <path
                fill='currentColor'
                d='M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z'
              />
            </svg>
            {t('googleRegister')}
          </button> */}

          {/* Login link */}
          <p className='mt-8 text-center text-sm text-muted-foreground'>
            {t('hasAccount')}{' '}
            <Link
              href='/login'
              className='font-semibold text-primary hover:underline'
            >
              {t('login')}
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}
