'use client';

import { useEffect, useState, useCallback } from 'react';
import { useTranslations } from 'next-intl';
import { LocaleLink as Link } from '@/components/ui/locale-link';
import { usePathname } from 'next/navigation';
import { authService, signalRService, type BookingNotification } from '@/lib/services';
import { NotificationToast } from '@/components/ui/notification-toast';
import type { CurrentUser } from '@/types';
import {
  Calendar,
  LayoutDashboard,
  CalendarCheck,
  Briefcase,
  Clock,
  LogOut,
  Menu,
  X,
  ChevronRight,
  Bell,
  Loader2,
} from 'lucide-react';

const navItems = [
  { href: '/dashboard', icon: LayoutDashboard, labelKey: 'dashboard' },
  { href: '/dashboard/bookings', icon: CalendarCheck, labelKey: 'bookings' },
  { href: '/dashboard/services', icon: Briefcase, labelKey: 'services' },
  { href: '/dashboard/availability', icon: Clock, labelKey: 'availability' },
];

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const t = useTranslations('nav');
  const tCommon = useTranslations('common');
  const pathname = usePathname();
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [user, setUser] = useState<CurrentUser | null>(null);
  const [loading, setLoading] = useState(true);
  const [notifications, setNotifications] = useState<BookingNotification[]>([]);

  useEffect(() => {
    async function loadUser() {
      try {
        const userData = await authService.getCurrentUser();
        setUser(userData);
      } catch {
        window.location.href = '/login';
      } finally {
        setLoading(false);
      }
    }
    loadUser();
  }, []);

  // Conectar SignalR cuando el usuario esté autenticado
  useEffect(() => {
    if (!user) return;

    // Iniciar conexión SignalR
    signalRService.start();

    // Registrar callback para notificaciones
    const unsubscribe = signalRService.onNotification((notification) => {
      setNotifications((prev) => [...prev, notification]);
    });

    // Cleanup al desmontar
    return () => {
      unsubscribe();
      signalRService.stop();
    };
  }, [user]);

  const removeNotification = useCallback((index: number) => {
    setNotifications((prev) => prev.filter((_, i) => i !== index));
  }, []);

  function handleLogout() {
    signalRService.stop();
    authService.logout();
    window.location.href = '/';
  }

  const isActive = (href: string) => {
    if (href === '/dashboard') {
      return (
        pathname === '/dashboard' ||
        pathname === '/es/dashboard' ||
        pathname === '/en/dashboard'
      );
    }
    return pathname.includes(href.replace('/dashboard', ''));
  };

  if (loading) {
    return (
      <div className='flex min-h-screen items-center justify-center bg-background'>
        <div className='flex flex-col items-center gap-4'>
          <Loader2 className='h-8 w-8 animate-spin text-primary' />
          <p className='text-muted-foreground'>{tCommon('loading')}</p>
        </div>
      </div>
    );
  }

  return (
    <div className='flex min-h-screen bg-background'>
      {/* Notifications Container */}
      <div className='fixed top-4 right-4 z-100 flex flex-col gap-3'>
        {notifications.map((notification, index) => (
          <NotificationToast
            key={`${notification.bookingId}-${index}`}
            notification={notification}
            onClose={() => removeNotification(index)}
          />
        ))}
      </div>

      {/* Mobile sidebar overlay */}
      {sidebarOpen && (
        <div
          className='fixed inset-0 z-40 bg-black/50 lg:hidden'
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Sidebar */}
      <aside
        className={`fixed inset-y-0 left-0 z-50 flex w-72 flex-col bg-card border-r border-border transition-transform duration-300 lg:static lg:translate-x-0 ${
          sidebarOpen ? 'translate-x-0' : '-translate-x-full'
        }`}
      >
        {/* Logo */}
        <div className='flex h-16 items-center justify-between px-6 border-b border-border'>
          <Link href='/' className='flex items-center gap-2'>
            <div className='flex h-9 w-9 items-center justify-center rounded-xl bg-primary'>
              <Calendar className='h-5 w-5 text-primary-foreground' />
            </div>
            <span className='text-lg font-bold text-foreground'>TurnoLink</span>
          </Link>
          <button
            onClick={() => setSidebarOpen(false)}
            className='p-1 text-muted-foreground hover:text-foreground lg:hidden'
          >
            <X className='h-5 w-5' />
          </button>
        </div>

        {/* Navigation */}
        <nav className='flex-1 overflow-y-auto p-4'>
          <ul className='space-y-1'>
            {navItems.map((item) => {
              const Icon = item.icon;
              const active = isActive(item.href);
              return (
                <li key={item.href}>
                  <Link
                    href={item.href}
                    onClick={() => setSidebarOpen(false)}
                    className={`flex items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium transition-all duration-200 ${
                      active
                        ? 'bg-primary text-primary-foreground shadow-lg shadow-primary/25'
                        : 'text-muted-foreground hover:bg-accent hover:text-foreground'
                    }`}
                  >
                    <Icon className='h-5 w-5' />
                    {t(item.labelKey)}
                    {active && <ChevronRight className='ml-auto h-4 w-4' />}
                  </Link>
                </li>
              );
            })}
          </ul>
        </nav>

        {/* User section */}
        <div className='border-t border-border p-4'>
          <div className='flex items-center gap-3 rounded-xl bg-secondary/50 p-3'>
            <div className='flex h-10 w-10 items-center justify-center rounded-full bg-primary text-primary-foreground font-semibold'>
              {user?.name?.charAt(0) || 'U'}
            </div>
            <div className='flex-1 min-w-0'>
              <p className='truncate text-sm font-medium text-foreground'>
                {user?.name || 'Usuario'}
              </p>
              <p className='truncate text-xs text-muted-foreground'>
                {user?.email || 'email@ejemplo.com'}
              </p>
            </div>
          </div>
          <button
            onClick={handleLogout}
            className='mt-3 flex w-full items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium text-destructive hover:bg-destructive/10 transition-colors'
          >
            <LogOut className='h-5 w-5' />
            {t('logout')}
          </button>
        </div>
      </aside>

      {/* Main content */}
      <div className='flex flex-1 flex-col'>
        {/* Top header */}
        <header className='sticky top-0 z-30 flex h-16 items-center gap-4 border-b border-border bg-background/95 backdrop-blur supports-backdrop-filter:bg-background/60 px-6'>
          <button
            onClick={() => setSidebarOpen(true)}
            className='p-2 text-muted-foreground hover:text-foreground lg:hidden'
          >
            <Menu className='h-5 w-5' />
          </button>

          {/* Search (placeholder) */}
          <div className='flex-1'>
            {/* You can add a search bar here later */}
          </div>

          {/* Right actions */}
          <div className='flex items-center gap-3'>
            <button className='relative p-2 text-muted-foreground hover:text-foreground rounded-lg hover:bg-accent transition-colors'>
              <Bell className='h-5 w-5' />
              <span className='absolute top-1.5 right-1.5 h-2 w-2 rounded-full bg-primary' />
            </button>
            <div className='hidden sm:flex items-center gap-2 pl-3 border-l border-border'>
              <div className='flex h-8 w-8 items-center justify-center rounded-full bg-primary text-primary-foreground text-sm font-semibold'>
                {user?.name?.charAt(0) || 'U'}
              </div>
              <span className='text-sm font-medium text-foreground'>
                {user?.name || 'Usuario'}
              </span>
            </div>
          </div>
        </header>

        {/* Page content */}
        <main className='flex-1 overflow-y-auto'>{children}</main>
      </div>
    </div>
  );
}
