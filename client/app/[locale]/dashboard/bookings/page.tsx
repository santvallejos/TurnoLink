'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { authService, bookingService } from '@/lib/services';
import type { CurrentUser, Booking } from '@/types';
import DashboardLayout from '@/components/dashboard/dashboard-layout';
import {
  Search,
  Calendar,
  CheckCircle2,
  XCircle,
  AlertCircle,
  Clock,
  Loader2,
  Mail,
  Phone,
  CalendarCheck,
} from 'lucide-react';

type StatusFilter = 'all' | 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed';

export default function BookingsPage() {
  const t = useTranslations();
  const [user, setUser] = useState<CurrentUser | null>(null);
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState<StatusFilter>('all');
  const [selectedBooking, setSelectedBooking] = useState<Booking | null>(null);

  useEffect(() => {
    async function loadData() {
      try {
        const [userData, bookingsData] = await Promise.all([
          authService.getCurrentUser(),
          bookingService.getMyBookings(),
        ]);
        setUser(userData);
        setBookings(bookingsData);
      } catch {
        window.location.href = '/login';
      } finally {
        setLoading(false);
      }
    }
    loadData();
  }, []);

  async function handleConfirm(id: string) {
    await bookingService.update(id, { status: 'Confirmed' });
    setBookings((prev) =>
      prev.map((b) => (b.id === id ? { ...b, status: 'Confirmed' } : b))
    );
  }

  async function handleCancel(id: string) {
    await bookingService.cancel(id);
    setBookings((prev) =>
      prev.map((b) => (b.id === id ? { ...b, status: 'Cancelled' } : b))
    );
  }

  const getStatusConfig = (status: string) => {
    switch (status) {
      case 'Confirmed':
        return {
          icon: CheckCircle2,
          bg: 'bg-green-100 dark:bg-green-900/30',
          text: 'text-green-700 dark:text-green-400',
          dot: 'bg-green-500',
        };
      case 'Pending':
        return {
          icon: AlertCircle,
          bg: 'bg-yellow-100 dark:bg-yellow-900/30',
          text: 'text-yellow-700 dark:text-yellow-400',
          dot: 'bg-yellow-500',
        };
      case 'Cancelled':
        return {
          icon: XCircle,
          bg: 'bg-red-100 dark:bg-red-900/30',
          text: 'text-red-700 dark:text-red-400',
          dot: 'bg-red-500',
        };
      case 'Completed':
        return {
          icon: CheckCircle2,
          bg: 'bg-blue-100 dark:bg-blue-900/30',
          text: 'text-blue-700 dark:text-blue-400',
          dot: 'bg-blue-500',
        };
      default:
        return {
          icon: Clock,
          bg: 'bg-gray-100 dark:bg-gray-900/30',
          text: 'text-gray-700 dark:text-gray-400',
          dot: 'bg-gray-500',
        };
    }
  };

  const filteredBookings = bookings.filter((booking) => {
    const matchesSearch =
      booking.clientName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      booking.clientSurname.toLowerCase().includes(searchTerm.toLowerCase()) ||
      booking.serviceName.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus =
      statusFilter === 'all' || booking.status === statusFilter;
    return matchesSearch && matchesStatus;
  });

  const statusCounts = {
    all: bookings.length,
    Pending: bookings.filter((b) => b.status === 'Pending').length,
    Confirmed: bookings.filter((b) => b.status === 'Confirmed').length,
    Cancelled: bookings.filter((b) => b.status === 'Cancelled').length,
    Completed: bookings.filter((b) => b.status === 'Completed').length,
  };

  if (loading) {
    return (
      <div className='flex min-h-screen items-center justify-center bg-background'>
        <div className='flex flex-col items-center gap-4'>
          <Loader2 className='h-8 w-8 animate-spin text-primary' />
          <p className='text-muted-foreground'>{t('common.loading')}</p>
        </div>
      </div>
    );
  }

  return (
    <DashboardLayout user={user}>
      <div className='p-6 lg:p-8'>
        {/* Header */}
        <div className='mb-8'>
          <h1 className='text-2xl lg:text-3xl font-bold text-foreground'>
            {t('bookings.title')}
          </h1>
          <p className='mt-1 text-muted-foreground'>
            {t('bookings.description')}
          </p>
        </div>

        {/* Filters */}
        <div className='mb-6 flex flex-col sm:flex-row gap-4'>
          {/* Search */}
          <div className='relative flex-1'>
            <Search className='absolute left-4 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground' />
            <input
              type='text'
              placeholder={t('bookings.searchPlaceholder')}
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
            />
          </div>

          {/* Status tabs */}
          <div className='flex gap-2 overflow-x-auto pb-2 sm:pb-0'>
            {(
              ['all', 'Pending', 'Confirmed', 'Cancelled'] as StatusFilter[]
            ).map((status) => (
              <button
                key={status}
                onClick={() => setStatusFilter(status)}
                className={`flex items-center gap-2 px-4 py-2 rounded-xl text-sm font-medium whitespace-nowrap transition-colors ${
                  statusFilter === status
                    ? 'bg-primary text-primary-foreground'
                    : 'bg-secondary text-secondary-foreground hover:bg-accent'
                }`}
              >
                {t(`bookings.filter.${status}`)}
                <span
                  className={`px-2 py-0.5 rounded-full text-xs ${
                    statusFilter === status
                      ? 'bg-primary-foreground/20 text-primary-foreground'
                      : 'bg-muted text-muted-foreground'
                  }`}
                >
                  {statusCounts[status]}
                </span>
              </button>
            ))}
          </div>
        </div>

        {/* Content */}
        <div className='grid gap-6 lg:grid-cols-3'>
          {/* Bookings list */}
          <div className='lg:col-span-2'>
            <div className='rounded-2xl bg-card border border-border overflow-hidden'>
              {filteredBookings.length === 0 ? (
                <div className='flex flex-col items-center justify-center py-16 px-6'>
                  <div className='flex h-16 w-16 items-center justify-center rounded-full bg-muted mb-4'>
                    <Calendar className='h-8 w-8 text-muted-foreground' />
                  </div>
                  <p className='text-foreground font-medium'>
                    {t('bookings.empty')}
                  </p>
                  <p className='text-sm text-muted-foreground mt-1'>
                    {t('bookings.emptyDesc')}
                  </p>
                </div>
              ) : (
                <div className='divide-y divide-border'>
                  {filteredBookings.map((booking) => {
                    const statusConfig = getStatusConfig(booking.status);
                    const isSelected = selectedBooking?.id === booking.id;
                    return (
                      <div
                        key={booking.id}
                        onClick={() => setSelectedBooking(booking)}
                        className={`flex items-center gap-4 p-4 cursor-pointer transition-colors ${
                          isSelected
                            ? 'bg-primary/5 border-l-2 border-l-primary'
                            : 'hover:bg-accent/50'
                        }`}
                      >
                        <div className='flex h-12 w-12 items-center justify-center rounded-full bg-primary/10 text-primary font-semibold'>
                          {booking.clientName.charAt(0)}
                          {booking.clientSurname.charAt(0)}
                        </div>
                        <div className='flex-1 min-w-0'>
                          <p className='font-medium text-foreground truncate'>
                            {booking.clientName} {booking.clientSurname}
                          </p>
                          <p className='text-sm text-muted-foreground truncate'>
                            {booking.serviceName}
                          </p>
                        </div>
                        <div className='hidden sm:block text-right'>
                          <p className='text-sm font-medium text-foreground'>
                            {new Date(booking.startTime).toLocaleDateString()}
                          </p>
                          <p className='text-xs text-muted-foreground'>
                            {new Date(booking.startTime).toLocaleTimeString(
                              [],
                              {
                                hour: '2-digit',
                                minute: '2-digit',
                              }
                            )}
                          </p>
                        </div>
                        <div
                          className={`flex items-center gap-1.5 px-3 py-1.5 rounded-full text-xs font-medium ${statusConfig.bg} ${statusConfig.text}`}
                        >
                          <span
                            className={`h-1.5 w-1.5 rounded-full ${statusConfig.dot}`}
                          />
                          {t(`bookings.status.${booking.status.toLowerCase()}`)}
                        </div>
                      </div>
                    );
                  })}
                </div>
              )}
            </div>
          </div>

          {/* Booking details sidebar */}
          <div className='lg:col-span-1'>
            {selectedBooking ? (
              <div className='rounded-2xl bg-card border border-border p-6 sticky top-24'>
                <div className='flex items-center justify-between mb-6'>
                  <h3 className='font-semibold text-foreground'>
                    {t('bookings.details')}
                  </h3>
                  <button
                    onClick={() => setSelectedBooking(null)}
                    className='p-1 text-muted-foreground hover:text-foreground'
                  >
                    <XCircle className='h-5 w-5' />
                  </button>
                </div>

                {/* Client info */}
                <div className='flex items-center gap-4 mb-6'>
                  <div className='flex h-16 w-16 items-center justify-center rounded-full bg-primary text-primary-foreground text-xl font-semibold'>
                    {selectedBooking.clientName.charAt(0)}
                    {selectedBooking.clientSurname.charAt(0)}
                  </div>
                  <div>
                    <p className='text-lg font-semibold text-foreground'>
                      {selectedBooking.clientName}{' '}
                      {selectedBooking.clientSurname}
                    </p>
                    <div
                      className={`inline-flex items-center gap-1.5 px-2 py-1 rounded-full text-xs font-medium ${
                        getStatusConfig(selectedBooking.status).bg
                      } ${getStatusConfig(selectedBooking.status).text}`}
                    >
                      <span
                        className={`h-1.5 w-1.5 rounded-full ${
                          getStatusConfig(selectedBooking.status).dot
                        }`}
                      />
                      {t(
                        `bookings.status.${selectedBooking.status.toLowerCase()}`
                      )}
                    </div>
                  </div>
                </div>

                {/* Contact info */}
                <div className='space-y-3 mb-6'>
                  <div className='flex items-center gap-3 text-sm'>
                    <Mail className='h-4 w-4 text-muted-foreground' />
                    <span className='text-foreground'>
                      {selectedBooking.clientEmail}
                    </span>
                  </div>
                  {selectedBooking.clientPhone && (
                    <div className='flex items-center gap-3 text-sm'>
                      <Phone className='h-4 w-4 text-muted-foreground' />
                      <span className='text-foreground'>
                        {selectedBooking.clientPhone}
                      </span>
                    </div>
                  )}
                </div>

                {/* Booking info */}
                <div className='space-y-4 p-4 rounded-xl bg-secondary/50 mb-6'>
                  <div className='flex items-center justify-between'>
                    <span className='text-sm text-muted-foreground'>
                      {t('bookings.service')}
                    </span>
                    <span className='text-sm font-medium text-foreground'>
                      {selectedBooking.serviceName}
                    </span>
                  </div>
                  <div className='flex items-center justify-between'>
                    <span className='text-sm text-muted-foreground'>
                      {t('bookings.date')}
                    </span>
                    <span className='text-sm font-medium text-foreground'>
                      {new Date(selectedBooking.startTime).toLocaleDateString()}
                    </span>
                  </div>
                  <div className='flex items-center justify-between'>
                    <span className='text-sm text-muted-foreground'>
                      {t('bookings.time')}
                    </span>
                    <span className='text-sm font-medium text-foreground'>
                      {new Date(selectedBooking.startTime).toLocaleTimeString(
                        [],
                        {
                          hour: '2-digit',
                          minute: '2-digit',
                        }
                      )}
                    </span>
                  </div>
                </div>

                {/* Actions */}
                {selectedBooking.status === 'Pending' && (
                  <div className='flex gap-3'>
                    <button
                      onClick={() => handleConfirm(selectedBooking.id)}
                      className='flex-1 flex items-center justify-center gap-2 px-4 py-3 bg-green-600 text-white rounded-xl font-medium hover:bg-green-700 transition-colors'
                    >
                      <CheckCircle2 className='h-4 w-4' />
                      {t('bookings.actions.confirm')}
                    </button>
                    <button
                      onClick={() => handleCancel(selectedBooking.id)}
                      className='flex-1 flex items-center justify-center gap-2 px-4 py-3 bg-red-600 text-white rounded-xl font-medium hover:bg-red-700 transition-colors'
                    >
                      <XCircle className='h-4 w-4' />
                      {t('bookings.actions.cancel')}
                    </button>
                  </div>
                )}
              </div>
            ) : (
              <div className='rounded-2xl bg-card border border-border p-6 text-center'>
                <div className='flex h-16 w-16 items-center justify-center rounded-full bg-muted mx-auto mb-4'>
                  <CalendarCheck className='h-8 w-8 text-muted-foreground' />
                </div>
                <p className='text-foreground font-medium'>
                  {t('bookings.selectBooking')}
                </p>
                <p className='text-sm text-muted-foreground mt-1'>
                  {t('bookings.selectBookingDesc')}
                </p>
              </div>
            )}
          </div>
        </div>
      </div>
    </DashboardLayout>
  );
}
