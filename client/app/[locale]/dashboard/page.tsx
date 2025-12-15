'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { Link } from '@/lib/i18n/navigation';
import { authService, bookingService, serviceService } from '@/lib/services';
import type { CurrentUser, Booking, Service } from '@/types';

export default function DashboardPage() {
  const t = useTranslations();
  const [user, setUser] = useState<CurrentUser | null>(null);
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [services, setServices] = useState<Service[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function loadData() {
      try {
        const [userData, bookingsData, servicesData] = await Promise.all([
          authService.getCurrentUser(),
          bookingService.getMyBookings(),
          serviceService.getMyServices(),
        ]);
        setUser(userData);
        setBookings(bookingsData);
        setServices(servicesData);
      } catch {
        // Not authenticated, redirect to login
        window.location.href = '/login';
      } finally {
        setLoading(false);
      }
    }
    loadData();
  }, []);

  function handleLogout() {
    authService.logout();
    window.location.href = '/';
  }

  if (loading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p className="text-zinc-600">{t('common.loading')}</p>
      </div>
    );
  }

  const todayBookings = bookings.filter((b) => {
    const today = new Date().toDateString();
    return new Date(b.startTime).toDateString() === today;
  });

  const pendingBookings = bookings.filter((b) => b.status === 'Pending');

  return (
    <div className="min-h-screen bg-zinc-50 dark:bg-zinc-950">
      {/* Header */}
      <header className="border-b border-zinc-200 bg-white dark:border-zinc-800 dark:bg-zinc-900">
        <nav className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <Link href="/" className="text-xl font-bold text-zinc-900 dark:text-white">
            TurnoLink
          </Link>
          <div className="flex items-center gap-6">
            <Link href="/dashboard" className="text-sm text-zinc-600 hover:text-zinc-900 dark:text-zinc-400">
              {t('nav.dashboard')}
            </Link>
            <Link href="/dashboard/services" className="text-sm text-zinc-600 hover:text-zinc-900 dark:text-zinc-400">
              {t('nav.services')}
            </Link>
            <Link href="/dashboard/bookings" className="text-sm text-zinc-600 hover:text-zinc-900 dark:text-zinc-400">
              {t('nav.bookings')}
            </Link>
            <button
              onClick={handleLogout}
              className="text-sm text-red-600 hover:text-red-500"
            >
              {t('nav.logout')}
            </button>
          </div>
        </nav>
      </header>

      {/* Main */}
      <main className="mx-auto max-w-6xl px-6 py-8">
        <h1 className="mb-8 text-2xl font-bold text-zinc-900 dark:text-white">
          {t('dashboard.welcome', { name: user?.name || '' })}
        </h1>

        {/* Summary Cards */}
        <div className="mb-8 grid gap-4 sm:grid-cols-3">
          <div className="rounded-lg bg-white p-6 shadow dark:bg-zinc-900">
            <p className="text-sm text-zinc-600 dark:text-zinc-400">
              {t('dashboard.summary.todayBookings')}
            </p>
            <p className="text-3xl font-bold text-zinc-900 dark:text-white">
              {todayBookings.length}
            </p>
          </div>
          <div className="rounded-lg bg-white p-6 shadow dark:bg-zinc-900">
            <p className="text-sm text-zinc-600 dark:text-zinc-400">
              {t('dashboard.summary.pendingBookings')}
            </p>
            <p className="text-3xl font-bold text-zinc-900 dark:text-white">
              {pendingBookings.length}
            </p>
          </div>
          <div className="rounded-lg bg-white p-6 shadow dark:bg-zinc-900">
            <p className="text-sm text-zinc-600 dark:text-zinc-400">
              {t('dashboard.summary.totalServices')}
            </p>
            <p className="text-3xl font-bold text-zinc-900 dark:text-white">
              {services.filter((s) => s.isActive).length}
            </p>
          </div>
        </div>

        {/* Recent Bookings */}
        <section className="rounded-lg bg-white p-6 shadow dark:bg-zinc-900">
          <h2 className="mb-4 text-lg font-semibold text-zinc-900 dark:text-white">
            {t('bookings.title')}
          </h2>
          {bookings.length === 0 ? (
            <p className="text-zinc-600 dark:text-zinc-400">{t('bookings.empty')}</p>
          ) : (
            <ul className="divide-y divide-zinc-200 dark:divide-zinc-700">
              {bookings.slice(0, 5).map((booking) => (
                <li key={booking.id} className="flex items-center justify-between py-3">
                  <div>
                    <p className="font-medium text-zinc-900 dark:text-white">
                      {booking.clientName} {booking.clientSurname}
                    </p>
                    <p className="text-sm text-zinc-600 dark:text-zinc-400">
                      {booking.serviceName} - {new Date(booking.startTime).toLocaleString()}
                    </p>
                  </div>
                  <span className={`rounded-full px-2 py-1 text-xs ${
                    booking.status === 'Confirmed' ? 'bg-green-100 text-green-800' :
                    booking.status === 'Pending' ? 'bg-yellow-100 text-yellow-800' :
                    booking.status === 'Cancelled' ? 'bg-red-100 text-red-800' :
                    'bg-zinc-100 text-zinc-800'
                  }`}>
                    {t(`bookings.status.${booking.status.toLowerCase()}`)}
                  </span>
                </li>
              ))}
            </ul>
          )}
        </section>
      </main>
    </div>
  );
}
