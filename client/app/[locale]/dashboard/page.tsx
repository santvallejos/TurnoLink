'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { Link } from '@/lib/i18n/navigation';
import { authService, bookingService, serviceService } from '@/lib/services';
import type { CurrentUser, Booking, Service } from '@/types';
import {
  CalendarCheck,
  Clock,
  Briefcase,
  TrendingUp,
  Users,
  ArrowUpRight,
  MoreHorizontal,
  Calendar,
  CheckCircle2,
  XCircle,
  AlertCircle,
  Loader2,
} from 'lucide-react';

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
        window.location.href = '/login';
      } finally {
        setLoading(false);
      }
    }
    loadData();
  }, []);

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

  const todayBookings = bookings.filter((b) => {
    const today = new Date().toDateString();
    return new Date(b.startTime).toDateString() === today;
  });

  const pendingBookings = bookings.filter((b) => b.status === 'Pending');
  const activeServices = services.filter((s) => s.isActive);

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
      default:
        return {
          icon: Clock,
          bg: 'bg-gray-100 dark:bg-gray-900/30',
          text: 'text-gray-700 dark:text-gray-400',
          dot: 'bg-gray-500',
        };
    }
  };

  return (
    <div className='p-6 lg:p-8'>
      {/* Welcome header */}
      <div className='mb-8'>
        <h1 className='text-2xl lg:text-3xl font-bold text-foreground'>
          {t('dashboard.welcome', { name: user?.name || '' })} 👋
        </h1>
        <p className='mt-1 text-muted-foreground'>{t('dashboard.subtitle')}</p>
      </div>

      {/* Stats cards */}
      <div className='grid gap-4 sm:grid-cols-2 lg:grid-cols-4 mb-8'>
        {/* Today's bookings */}
        <div className='group relative overflow-hidden rounded-2xl bg-card border border-border p-6 hover:shadow-lg hover:border-primary/20 transition-all duration-300'>
          <div className='flex items-start justify-between'>
            <div>
              <p className='text-sm font-medium text-muted-foreground'>
                {t('dashboard.summary.todayBookings')}
              </p>
              <p className='mt-2 text-3xl font-bold text-foreground'>
                {todayBookings.length}
              </p>
              <div className='mt-2 flex items-center gap-1 text-sm'>
                <ArrowUpRight className='h-4 w-4 text-green-500' />
                <span className='text-green-500 font-medium'>+12%</span>
                <span className='text-muted-foreground'>
                  {t('dashboard.vsYesterday')}
                </span>
              </div>
            </div>
            <div className='flex h-12 w-12 items-center justify-center rounded-xl bg-primary/10 text-primary group-hover:bg-primary group-hover:text-primary-foreground transition-colors'>
              <CalendarCheck className='h-6 w-6' />
            </div>
          </div>
          <div className='absolute inset-x-0 bottom-0 h-1 bg-gradient-to-r from-primary/50 to-primary opacity-0 group-hover:opacity-100 transition-opacity' />
        </div>

        {/* Pending bookings */}
        <div className='group relative overflow-hidden rounded-2xl bg-card border border-border p-6 hover:shadow-lg hover:border-yellow-500/20 transition-all duration-300'>
          <div className='flex items-start justify-between'>
            <div>
              <p className='text-sm font-medium text-muted-foreground'>
                {t('dashboard.summary.pendingBookings')}
              </p>
              <p className='mt-2 text-3xl font-bold text-foreground'>
                {pendingBookings.length}
              </p>
              <div className='mt-2 flex items-center gap-1 text-sm'>
                <Clock className='h-4 w-4 text-yellow-500' />
                <span className='text-muted-foreground'>
                  {t('dashboard.needsAttention')}
                </span>
              </div>
            </div>
            <div className='flex h-12 w-12 items-center justify-center rounded-xl bg-yellow-500/10 text-yellow-500 group-hover:bg-yellow-500 group-hover:text-white transition-colors'>
              <AlertCircle className='h-6 w-6' />
            </div>
          </div>
          <div className='absolute inset-x-0 bottom-0 h-1 bg-gradient-to-r from-yellow-500/50 to-yellow-500 opacity-0 group-hover:opacity-100 transition-opacity' />
        </div>

        {/* Active services */}
        <div className='group relative overflow-hidden rounded-2xl bg-card border border-border p-6 hover:shadow-lg hover:border-blue-500/20 transition-all duration-300'>
          <div className='flex items-start justify-between'>
            <div>
              <p className='text-sm font-medium text-muted-foreground'>
                {t('dashboard.summary.totalServices')}
              </p>
              <p className='mt-2 text-3xl font-bold text-foreground'>
                {activeServices.length}
              </p>
              <div className='mt-2 flex items-center gap-1 text-sm'>
                <span className='text-muted-foreground'>
                  {t('dashboard.servicesActive')}
                </span>
              </div>
            </div>
            <div className='flex h-12 w-12 items-center justify-center rounded-xl bg-blue-500/10 text-blue-500 group-hover:bg-blue-500 group-hover:text-white transition-colors'>
              <Briefcase className='h-6 w-6' />
            </div>
          </div>
          <div className='absolute inset-x-0 bottom-0 h-1 bg-gradient-to-r from-blue-500/50 to-blue-500 opacity-0 group-hover:opacity-100 transition-opacity' />
        </div>

        {/* Total clients */}
        <div className='group relative overflow-hidden rounded-2xl bg-card border border-border p-6 hover:shadow-lg hover:border-purple-500/20 transition-all duration-300'>
          <div className='flex items-start justify-between'>
            <div>
              <p className='text-sm font-medium text-muted-foreground'>
                {t('dashboard.summary.totalClients')}
              </p>
              <p className='mt-2 text-3xl font-bold text-foreground'>
                {new Set(bookings.map((b) => b.clientEmail)).size}
              </p>
              <div className='mt-2 flex items-center gap-1 text-sm'>
                <ArrowUpRight className='h-4 w-4 text-green-500' />
                <span className='text-green-500 font-medium'>+5</span>
                <span className='text-muted-foreground'>
                  {t('dashboard.thisMonth')}
                </span>
              </div>
            </div>
            <div className='flex h-12 w-12 items-center justify-center rounded-xl bg-purple-500/10 text-purple-500 group-hover:bg-purple-500 group-hover:text-white transition-colors'>
              <Users className='h-6 w-6' />
            </div>
          </div>
          <div className='absolute inset-x-0 bottom-0 h-1 bg-gradient-to-r from-purple-500/50 to-purple-500 opacity-0 group-hover:opacity-100 transition-opacity' />
        </div>
      </div>

      {/* Content grid */}
      <div className='grid gap-6 lg:grid-cols-3'>
        {/* Recent bookings */}
        <div className='lg:col-span-2 rounded-2xl bg-card border border-border overflow-hidden'>
          <div className='flex items-center justify-between p-6 border-b border-border'>
            <div>
              <h2 className='text-lg font-semibold text-foreground'>
                {t('dashboard.recentBookings')}
              </h2>
              <p className='text-sm text-muted-foreground'>
                {t('dashboard.recentBookingsDesc')}
              </p>
            </div>
            <Link
              href='/dashboard/bookings'
              className='flex items-center gap-1 text-sm font-medium text-primary hover:underline'
            >
              {t('dashboard.viewAll')}
              <ArrowUpRight className='h-4 w-4' />
            </Link>
          </div>

          {bookings.length === 0 ? (
            <div className='flex flex-col items-center justify-center py-12 px-6'>
              <div className='flex h-16 w-16 items-center justify-center rounded-full bg-muted mb-4'>
                <Calendar className='h-8 w-8 text-muted-foreground' />
              </div>
              <p className='text-foreground font-medium'>
                {t('bookings.empty')}
              </p>
              <p className='text-sm text-muted-foreground mt-1'>
                {t('dashboard.noBookingsYet')}
              </p>
            </div>
          ) : (
            <div className='divide-y divide-border'>
              {bookings.slice(0, 5).map((booking) => {
                const statusConfig = getStatusConfig(booking.status);
                return (
                  <div
                    key={booking.id}
                    className='flex items-center gap-4 p-4 hover:bg-accent/50 transition-colors'
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
                        {new Date(booking.startTime).toLocaleTimeString([], {
                          hour: '2-digit',
                          minute: '2-digit',
                        })}
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
                    <button className='p-2 text-muted-foreground hover:text-foreground rounded-lg hover:bg-accent transition-colors'>
                      <MoreHorizontal className='h-4 w-4' />
                    </button>
                  </div>
                );
              })}
            </div>
          )}
        </div>

        {/* Quick actions & upcoming */}
        <div className='space-y-6'>
          {/* Quick actions */}
          <div className='rounded-2xl bg-card border border-border p-6'>
            <h3 className='font-semibold text-foreground mb-4'>
              {t('dashboard.quickActions')}
            </h3>
            <div className='space-y-2'>
              <Link
                href='/dashboard/services'
                className='flex items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium bg-primary/10 text-primary hover:bg-primary/20 transition-colors'
              >
                <Briefcase className='h-5 w-5' />
                {t('dashboard.createService')}
              </Link>
              <Link
                href='/dashboard/availability'
                className='flex items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium bg-secondary text-secondary-foreground hover:bg-accent transition-colors'
              >
                <Clock className='h-5 w-5' />
                {t('dashboard.setAvailability')}
              </Link>
              <Link
                href='/dashboard/profile'
                className='flex items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium bg-secondary text-secondary-foreground hover:bg-accent transition-colors'
              >
                <TrendingUp className='h-5 w-5' />
                {t('dashboard.shareBookingPage')}
              </Link>
            </div>
          </div>

          {/* Today's schedule */}
          <div className='rounded-2xl bg-card border border-border p-6'>
            <div className='flex items-center justify-between mb-4'>
              <h3 className='font-semibold text-foreground'>
                {t('dashboard.todaySchedule')}
              </h3>
              <span className='text-xs text-muted-foreground'>
                {new Date().toLocaleDateString()}
              </span>
            </div>

            {todayBookings.length === 0 ? (
              <div className='text-center py-6'>
                <div className='flex h-12 w-12 items-center justify-center rounded-full bg-muted mx-auto mb-3'>
                  <Calendar className='h-6 w-6 text-muted-foreground' />
                </div>
                <p className='text-sm text-muted-foreground'>
                  {t('dashboard.noBookingsToday')}
                </p>
              </div>
            ) : (
              <div className='space-y-3'>
                {todayBookings.slice(0, 4).map((booking) => (
                  <div
                    key={booking.id}
                    className='flex items-center gap-3 p-3 rounded-xl bg-secondary/50'
                  >
                    <div className='text-center'>
                      <p className='text-lg font-bold text-primary'>
                        {new Date(booking.startTime).toLocaleTimeString([], {
                          hour: '2-digit',
                          minute: '2-digit',
                        })}
                      </p>
                    </div>
                    <div className='flex-1 min-w-0'>
                      <p className='text-sm font-medium text-foreground truncate'>
                        {booking.clientName}
                      </p>
                      <p className='text-xs text-muted-foreground truncate'>
                        {booking.serviceName}
                      </p>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
