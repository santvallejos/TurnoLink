'use client';

import { useEffect, useState, useCallback } from 'react';
import { X, Calendar, Bell } from 'lucide-react';
import type { BookingNotification } from '@/lib/services/signalr';

interface NotificationToastProps {
  notification: BookingNotification;
  onClose: () => void;
}

/**
 * Componente Toast para mostrar notificaciones de nuevas reservas
 */
export function NotificationToast({ notification, onClose }: NotificationToastProps) {
  const [isVisible, setIsVisible] = useState(false);
  const [isLeaving, setIsLeaving] = useState(false);

  const handleClose = useCallback(() => {
    setIsLeaving(true);
    setTimeout(() => {
      onClose();
    }, 300);
  }, [onClose]);

  useEffect(() => {
    // Animación de entrada
    const showTimer = setTimeout(() => setIsVisible(true), 10);

    // Auto cerrar después de 5 segundos
    const closeTimer = setTimeout(() => {
      handleClose();
    }, 5000);

    return () => {
      clearTimeout(showTimer);
      clearTimeout(closeTimer);
    };
  }, [handleClose]);

  const formattedDate = new Date(notification.startTime).toLocaleDateString('es-ES', {
    weekday: 'short',
    day: 'numeric',
    month: 'short',
    hour: '2-digit',
    minute: '2-digit',
  });

  return (
    <div
      className={`transform transition-all duration-300 ease-out ${
        isVisible && !isLeaving
          ? 'translate-x-0 opacity-100'
          : 'translate-x-full opacity-0'
      }`}
    >
      <div className='w-80 rounded-2xl border border-border bg-card shadow-2xl overflow-hidden'>
        {/* Header */}
        <div className='flex items-center justify-between bg-primary px-4 py-3'>
          <div className='flex items-center gap-2'>
            <Bell className='h-4 w-4 text-primary-foreground' />
            <span className='text-sm font-semibold text-primary-foreground'>
              Nueva Reserva
            </span>
          </div>
          <button
            onClick={handleClose}
            className='rounded-full p-1 text-primary-foreground/80 hover:text-primary-foreground hover:bg-primary-foreground/10 transition-colors'
          >
            <X className='h-4 w-4' />
          </button>
        </div>

        {/* Content */}
        <div className='p-4'>
          <div className='flex items-start gap-3'>
            <div className='flex h-10 w-10 items-center justify-center rounded-full bg-primary/10 text-primary'>
              <Calendar className='h-5 w-5' />
            </div>
            <div className='flex-1 min-w-0'>
              <p className='font-medium text-foreground truncate'>
                {notification.clientName}
              </p>
              <p className='text-sm text-muted-foreground truncate'>
                {notification.serviceName}
              </p>
              <p className='text-xs text-muted-foreground mt-1'>
                {formattedDate}
              </p>
            </div>
          </div>
        </div>

        {/* Progress bar */}
        <div className='h-1 bg-muted'>
          <div className='h-full bg-primary' style={{ animation: 'shrink-width 5s linear forwards' }}/>
        </div>
      </div>

      <style jsx>{`
        @keyframes shrink-width {
          from {
            width: 100%;
          }
          to {
            width: 0%;
          }
        }
      `}</style>
    </div>
  );
}
