'use client';

import { useState } from 'react';
import { useTranslations } from 'next-intl';
import { Link } from '@/lib/i18n/navigation';
import { authService } from '@/lib/services';
import type { ApiError } from '@/types';

export default function LoginPage() {
  const t = useTranslations('auth.login');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError(null);
    setLoading(true);

    const formData = new FormData(e.currentTarget);

    try {
      await authService.login({
        email: formData.get('email') as string,
        password: formData.get('password') as string,
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
              {t('password')}
            </label>
            <input
              name="password"
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
          {t('noAccount')}{' '}
          <Link href="/register" className="text-blue-600 hover:underline">
            {t('register')}
          </Link>
        </p>
      </div>
    </div>
  );
}
