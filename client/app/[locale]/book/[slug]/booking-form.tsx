'use client';

import { useState, useEffect } from 'react';
import { useTranslations } from 'next-intl';
import { publicService, availabilityService } from '@/lib/services';
import type { Service, Availability, ApiError } from '@/types';

interface Props {
  services: Service[];
}

export default function BookingForm({ services }: Props) {
  const t = useTranslations('public.booking');
  const [selectedService, setSelectedService] = useState<string>('');
  const [slots, setSlots] = useState<Availability[]>([]);
  const [selectedSlot, setSelectedSlot] = useState<string>('');
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  // Load availability when service is selected
  useEffect(() => {
    if (!selectedService) {
      setSlots([]);
      return;
    }
    availabilityService.getByServiceId(selectedService).then(setSlots);
  }, [selectedService]);

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError(null);
    setLoading(true);

    const formData = new FormData(e.currentTarget);

    try {
      await publicService.createBooking({
        serviceId: selectedService,
        availabilityId: selectedSlot,
        clientName: formData.get('name') as string,
        clientSurname: formData.get('surname') as string,
        clientEmail: formData.get('email') as string,
        clientPhone: formData.get('phone') as string || undefined,
        notes: formData.get('notes') as string || undefined,
      });
      setSuccess(true);
    } catch (err) {
      const apiError = err as ApiError;
      setError(apiError.message);
    } finally {
      setLoading(false);
    }
  }

  if (success) {
    return (
      <div className="rounded-lg bg-green-50 p-8 text-center dark:bg-green-900/20">
        <h2 className="text-xl font-bold text-green-800 dark:text-green-400">
          {t('success')}
        </h2>
        <p className="mt-2 text-green-700 dark:text-green-500">
          {t('successMessage')}
        </p>
      </div>
    );
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      {/* Select Service */}
      <div>
        <label className="mb-2 block font-medium text-zinc-900 dark:text-white">
          {t('selectService')}
        </label>
        <div className="grid gap-3">
          {services.map((service) => (
            <label
              key={service.id}
              className={`cursor-pointer rounded-lg border p-4 ${
                selectedService === service.id
                  ? 'border-blue-600 bg-blue-50 dark:bg-blue-900/20'
                  : 'border-zinc-200 dark:border-zinc-700'
              }`}
            >
              <input
                type="radio"
                name="service"
                value={service.id}
                checked={selectedService === service.id}
                onChange={(e) => setSelectedService(e.target.value)}
                className="sr-only"
              />
              <div className="flex items-center justify-between">
                <div>
                  <p className="font-medium text-zinc-900 dark:text-white">{service.name}</p>
                  <p className="text-sm text-zinc-600 dark:text-zinc-400">{service.durationMinutes} min</p>
                </div>
                <span className="font-semibold text-zinc-900 dark:text-white">${service.price}</span>
              </div>
            </label>
          ))}
        </div>
      </div>

      {/* Select Slot */}
      {slots.length > 0 && (
        <div>
          <label className="mb-2 block font-medium text-zinc-900 dark:text-white">
            {t('selectSlot')}
          </label>
          <div className="grid grid-cols-2 gap-2 sm:grid-cols-3">
            {slots.map((slot) => (
              <button
                key={slot.id}
                type="button"
                onClick={() => setSelectedSlot(slot.id)}
                className={`rounded-md border px-3 py-2 text-sm ${
                  selectedSlot === slot.id
                    ? 'border-blue-600 bg-blue-600 text-white'
                    : 'border-zinc-200 dark:border-zinc-700'
                }`}
              >
                {new Date(slot.startTime).toLocaleString()}
              </button>
            ))}
          </div>
        </div>
      )}

      {/* Client Details */}
      {selectedSlot && (
        <div className="space-y-4 rounded-lg border border-zinc-200 p-4 dark:border-zinc-700">
          <h3 className="font-medium text-zinc-900 dark:text-white">{t('yourDetails')}</h3>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">{t('name')}</label>
              <input
                name="name"
                type="text"
                required
                className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800"
              />
            </div>
            <div>
              <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">{t('surname')}</label>
              <input
                name="surname"
                type="text"
                required
                className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800"
              />
            </div>
          </div>

          <div>
            <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">{t('email')}</label>
            <input
              name="email"
              type="email"
              required
              className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800"
            />
          </div>

          <div>
            <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">{t('phone')}</label>
            <input
              name="phone"
              type="tel"
              className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800"
            />
          </div>

          <div>
            <label className="mb-1 block text-sm text-zinc-600 dark:text-zinc-400">{t('notes')}</label>
            <textarea
              name="notes"
              rows={3}
              className="w-full rounded-md border border-zinc-300 px-3 py-2 dark:border-zinc-700 dark:bg-zinc-800"
            />
          </div>
        </div>
      )}

      {error && <p className="text-sm text-red-600">{error}</p>}

      {selectedSlot && (
        <button
          type="submit"
          disabled={loading}
          className="w-full rounded-md bg-blue-600 py-3 font-medium text-white hover:bg-blue-500 disabled:opacity-50"
        >
          {loading ? '...' : t('submit')}
        </button>
      )}
    </form>
  );
}
