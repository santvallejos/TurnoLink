'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { Link } from '@/lib/i18n/navigation';
import { bookingService } from '@/lib/services';
import type { Booking } from '@/types';

export default function BookingsPage() {
  const t = useTranslations();
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    bookingService.getMyBookings()
      .then(setBookings)
      .catch(() => window.location.href = '/login')
      .finally(() => setLoading(false));
  }, []);

  async function handleConfirm(id: string) {
    await bookingService.update(id, { status: 'Confirmed' });
    setBookings((prev) => prev.map((b) => b.id === id ? { ...b, status: 'Confirmed' } : b));
  }

  async function handleCancel(id: string) {
    await bookingService.cancel(id);
    setBookings((prev) => prev.map((b) => b.id === id ? { ...b, status: 'Cancelled' } : b));
  }

  if (loading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p>{t('common.loading')}</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-zinc-50 dark:bg-zinc-950">
      <header className="border-b border-zinc-200 bg-white dark:border-zinc-800 dark:bg-zinc-900">
        <nav className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <Link href="/dashboard" className="text-xl font-bold text-zinc-900 dark:text-white">
            TurnoLink
          </Link>
        </nav>
      </header>

      <main className="mx-auto max-w-6xl px-6 py-8">
        <h1 className="mb-6 text-2xl font-bold text-zinc-900 dark:text-white">
          {t('bookings.title')}
        </h1>

        {bookings.length === 0 ? (
          <p className="text-zinc-600 dark:text-zinc-400">{t('bookings.empty')}</p>
        ) : (
          <div className="overflow-x-auto rounded-lg bg-white shadow dark:bg-zinc-900">
            <table className="w-full text-left text-sm">
              <thead className="border-b border-zinc-200 dark:border-zinc-700">
                <tr>
                  <th className="px-4 py-3 font-medium">{t('bookings.client')}</th>
                  <th className="px-4 py-3 font-medium">{t('bookings.service')}</th>
                  <th className="px-4 py-3 font-medium">{t('bookings.date')}</th>
                  <th className="px-4 py-3 font-medium">Status</th>
                  <th className="px-4 py-3 font-medium"></th>
                </tr>
              </thead>
              <tbody className="divide-y divide-zinc-200 dark:divide-zinc-700">
                {bookings.map((booking) => (
                  <tr key={booking.id}>
                    <td className="px-4 py-3">{booking.clientName} {booking.clientSurname}</td>
                    <td className="px-4 py-3">{booking.serviceName}</td>
                    <td className="px-4 py-3">{new Date(booking.startTime).toLocaleString()}</td>
                    <td className="px-4 py-3">
                      <span className={`rounded-full px-2 py-1 text-xs ${
                        booking.status === 'Confirmed' ? 'bg-green-100 text-green-800' :
                        booking.status === 'Pending' ? 'bg-yellow-100 text-yellow-800' :
                        booking.status === 'Cancelled' ? 'bg-red-100 text-red-800' :
                        'bg-zinc-100 text-zinc-800'
                      }`}>
                        {t(`bookings.status.${booking.status.toLowerCase()}`)}
                      </span>
                    </td>
                    <td className="px-4 py-3">
                      {booking.status === 'Pending' && (
                        <div className="flex gap-2">
                          <button
                            onClick={() => handleConfirm(booking.id)}
                            className="text-xs text-green-600 hover:underline"
                          >
                            {t('bookings.actions.confirm')}
                          </button>
                          <button
                            onClick={() => handleCancel(booking.id)}
                            className="text-xs text-red-600 hover:underline"
                          >
                            {t('bookings.actions.cancel')}
                          </button>
                        </div>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </main>
    </div>
  );
}
