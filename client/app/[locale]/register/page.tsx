'use client';

import { useState } from 'react';
import { useTranslations } from 'next-intl';
import { Link } from '@/lib/i18n/navigation';
import { authService } from '@/lib/services';
import type { ApiError } from '@/types';

export default function RegisterPage() {
  const t = useTranslations('auth.register');
  const tErrors = useTranslations('auth.errors');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

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
        phoneNumber: formData.get('phone') as string || undefined,
      });
      window.location.href = '/dashboard';
    } catch (err) {
      const apiError = err as ApiError;
      setError(apiError.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 dark:bg-zinc-950">
      <div className="w-full max-w-md rounded-lg bg-white p-8 shadow-md dark:bg-zinc-900">
        <h1 className="mb-6 text-2xl font-bold text-zinc-900 dark:text-white">
          {t('title')}
        </h1>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">
                {t('name')}
              </label>
              <input
                name="name"
                type="text"
                required
                className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800 dark:text-white"
              />
            </div>
            <div>
              <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">
                {t('surname')}
              </label>
              <input
                name="surname"
                type="text"
                required
                className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800 dark:text-white"
              />
            </div>
          </div>

          <div>
            <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">
              {t('email')}
            </label>
            <input
              name="email"
              type="email"
              required
              className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800 dark:text-white"
            />
          </div>

          <div>
            <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">
              {t('phone')}
            </label>
            <input
              name="phone"
              type="tel"
              className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800 dark:text-white"
            />
          </div>

          <div>
            <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">
              {t('password')}
            </label>
            <input
              name="password"
              type="password"
              required
              className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800 dark:text-white"
            />
          </div>

          <div>
            <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">
              {t('repeatPassword')}
            </label>
            <input
              name="repeatPassword"
              type="password"
              required
              className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800 dark:text-white"
            />
          </div>

          {error && (
            <p className="text-sm text-red-600">{error}</p>
          )}

          <button
            type="submit"
            disabled={loading}
            className="w-full rounded-md bg-blue-600 py-2 text-white hover:bg-blue-500 disabled:opacity-50"
          >
            {loading ? '...' : t('submit')}
          </button>
        </form>

        <p className="mt-4 text-center text-sm text-zinc-600 dark:text-zinc-400">
          {t('hasAccount')}{' '}
          <Link href="/login" className="text-blue-600 hover:underline">
            {t('login')}
          </Link>
        </p>
      </div>
    </div>
  );
}
