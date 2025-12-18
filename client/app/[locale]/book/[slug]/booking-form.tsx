'use client';

import { useState, useEffect, useMemo } from 'react';
import { useTranslations } from 'next-intl';
import { publicService, availabilityService } from '@/lib/services';
import type { Service, Availability, ApiError } from '@/types';
import {
  Clock,
  DollarSign,
  User,
  Mail,
  Phone,
  FileText,
  CheckCircle2,
  ChevronLeft,
  ChevronRight,
  Calendar,
  Loader2,
  Sparkles,
  CalendarCheck,
  PartyPopper,
} from 'lucide-react';

interface Props {
  services: Service[];
  professionalName: string;
}

// Stepper Component
function Stepper({
  currentStep,
  steps,
}: {
  currentStep: number;
  steps: string[];
}) {
  return (
    <div className='mb-8'>
      <div className='flex items-center justify-center'>
        {steps.map((step, index) => (
          <div key={step} className='flex items-center'>
            {/* Step Circle */}
            <div className='flex flex-col items-center'>
              <div
                className={`flex h-10 w-10 items-center justify-center rounded-full border-2 transition-all ${
                  index < currentStep
                    ? 'border-primary bg-primary text-primary-foreground'
                    : index === currentStep
                    ? 'border-primary bg-primary/10 text-primary'
                    : 'border-border bg-card text-muted-foreground'
                }`}
              >
                {index < currentStep ? (
                  <CheckCircle2 className='h-5 w-5' />
                ) : (
                  <span className='text-sm font-semibold'>{index + 1}</span>
                )}
              </div>
              <span
                className={`mt-2 text-xs font-medium ${
                  index <= currentStep
                    ? 'text-primary'
                    : 'text-muted-foreground'
                }`}
              >
                {step}
              </span>
            </div>

            {/* Connector Line */}
            {index < steps.length - 1 && (
              <div
                className={`mx-2 h-0.5 w-12 sm:w-20 ${
                  index < currentStep ? 'bg-primary' : 'bg-border'
                }`}
              />
            )}
          </div>
        ))}
      </div>
    </div>
  );
}

// Calendar Component
function CalendarPicker({
  selectedDate,
  onSelectDate,
  availableDates,
}: {
  selectedDate: Date | null;
  onSelectDate: (date: Date) => void;
  availableDates: string[];
}) {
  const t = useTranslations('public.booking');
  const [currentMonth, setCurrentMonth] = useState(new Date());

  const daysInMonth = useMemo(() => {
    const year = currentMonth.getFullYear();
    const month = currentMonth.getMonth();
    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    const days: (Date | null)[] = [];

    // Add empty slots for days before the first day of the month
    for (let i = 0; i < firstDay.getDay(); i++) {
      days.push(null);
    }

    // Add all days of the month
    for (let i = 1; i <= lastDay.getDate(); i++) {
      days.push(new Date(year, month, i));
    }

    return days;
  }, [currentMonth]);

  const monthNames = [
    t('months.january'),
    t('months.february'),
    t('months.march'),
    t('months.april'),
    t('months.may'),
    t('months.june'),
    t('months.july'),
    t('months.august'),
    t('months.september'),
    t('months.october'),
    t('months.november'),
    t('months.december'),
  ];

  const dayNames = [
    t('days.sun'),
    t('days.mon'),
    t('days.tue'),
    t('days.wed'),
    t('days.thu'),
    t('days.fri'),
    t('days.sat'),
  ];

  const isDateAvailable = (date: Date) => {
    const dateStr = date.toISOString().split('T')[0];
    return availableDates.includes(dateStr);
  };

  const isToday = (date: Date) => {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  };

  const isSelected = (date: Date) => {
    if (!selectedDate) return false;
    return (
      date.getDate() === selectedDate.getDate() &&
      date.getMonth() === selectedDate.getMonth() &&
      date.getFullYear() === selectedDate.getFullYear()
    );
  };

  const isPastDate = (date: Date) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return date < today;
  };

  return (
    <div className='rounded-2xl border border-border bg-card p-4 sm:p-6'>
      {/* Month Navigation */}
      <div className='mb-4 flex items-center justify-between'>
        <button
          type='button'
          onClick={() =>
            setCurrentMonth(
              new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1)
            )
          }
          className='rounded-lg p-2 text-muted-foreground transition-colors hover:bg-accent hover:text-foreground'
        >
          <ChevronLeft className='h-5 w-5' />
        </button>
        <h3 className='text-lg font-semibold text-foreground'>
          {monthNames[currentMonth.getMonth()]} {currentMonth.getFullYear()}
        </h3>
        <button
          type='button'
          onClick={() =>
            setCurrentMonth(
              new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1)
            )
          }
          className='rounded-lg p-2 text-muted-foreground transition-colors hover:bg-accent hover:text-foreground'
        >
          <ChevronRight className='h-5 w-5' />
        </button>
      </div>

      {/* Day Names */}
      <div className='mb-2 grid grid-cols-7 gap-1'>
        {dayNames.map((day) => (
          <div
            key={day}
            className='py-2 text-center text-xs font-medium text-muted-foreground'
          >
            {day}
          </div>
        ))}
      </div>

      {/* Calendar Grid */}
      <div className='grid grid-cols-7 gap-1'>
        {daysInMonth.map((date, index) => (
          <div key={index} className='aspect-square p-0.5'>
            {date && (
              <button
                type='button'
                disabled={isPastDate(date) || !isDateAvailable(date)}
                onClick={() => onSelectDate(date)}
                className={`flex h-full w-full items-center justify-center rounded-lg text-sm font-medium transition-all ${
                  isSelected(date)
                    ? 'bg-primary text-primary-foreground shadow-lg shadow-primary/25'
                    : isToday(date)
                    ? 'border-2 border-primary bg-primary/10 text-primary'
                    : isPastDate(date) || !isDateAvailable(date)
                    ? 'cursor-not-allowed text-muted-foreground/50'
                    : 'text-foreground hover:bg-accent'
                }`}
              >
                {date.getDate()}
              </button>
            )}
          </div>
        ))}
      </div>

      {/* Legend */}
      <div className='mt-4 flex flex-wrap items-center justify-center gap-4 border-t border-border pt-4 text-xs'>
        <div className='flex items-center gap-1.5'>
          <div className='h-3 w-3 rounded-full bg-primary' />
          <span className='text-muted-foreground'>{t('selected')}</span>
        </div>
        <div className='flex items-center gap-1.5'>
          <div className='h-3 w-3 rounded-full border-2 border-primary' />
          <span className='text-muted-foreground'>{t('today')}</span>
        </div>
        <div className='flex items-center gap-1.5'>
          <div className='h-3 w-3 rounded-full bg-muted' />
          <span className='text-muted-foreground'>{t('unavailable')}</span>
        </div>
      </div>
    </div>
  );
}

export default function BookingForm({ services, professionalName }: Props) {
  const t = useTranslations('public.booking');
  const [currentStep, setCurrentStep] = useState(0);
  const [selectedService, setSelectedService] = useState<Service | null>(null);
  const [slots, setSlots] = useState<Availability[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [selectedSlot, setSelectedSlot] = useState<Availability | null>(null);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [loadingSlots, setLoadingSlots] = useState(false);

  // Client details state
  const [clientName, setClientName] = useState('');
  const [clientSurname, setClientSurname] = useState('');
  const [clientEmail, setClientEmail] = useState('');
  const [clientPhone, setClientPhone] = useState('');
  const [notes, setNotes] = useState('');

  const steps = [
    t('steps.service'),
    t('steps.datetime'),
    t('steps.details'),
    t('steps.confirm'),
  ];

  // Get available dates from slots
  const availableDates = useMemo(() => {
    return [
      ...new Set(
        slots.map(
          (slot) => new Date(slot.startTimeUtc).toISOString().split('T')[0]
        ),
      ),
    ];
  }, [slots]);

  // Get slots for selected date
  const slotsForSelectedDate = useMemo(() => {
    if (!selectedDate) return [];
    const dateStr = selectedDate.toISOString().split('T')[0];
    return slots.filter(
      (slot) => new Date(slot.startTimeUtc).toISOString().split('T')[0] === dateStr
    );
  }, [slots, selectedDate]);

  // Load availability when service is selected
  useEffect(() => {
    if (!selectedService) {
      setSlots([]);
      return;
    }
    setLoadingSlots(true);
    availabilityService
      .getByServiceId(selectedService.id)
      .then(setSlots)
      .finally(() => setLoadingSlots(false));
  }, [selectedService]);

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      await publicService.createBooking({
        serviceId: selectedService!.id,
        availabilityId: selectedSlot!.id,
        clientName: clientName,
        clientSurname: clientSurname,
        clientEmail: clientEmail,
        clientPhone: clientPhone || undefined,
        notes: notes || undefined,
      });
      setSuccess(true);
    } catch (err) {
      const apiError = err as ApiError;
      setError(apiError.message);
    } finally {
      setLoading(false);
    }
  }

  // Success State
  if (success) {
    return (
      <div className='mx-auto max-w-lg'>
        <div className='rounded-2xl border border-border bg-card p-8 text-center shadow-xl'>
          {/* Success Animation */}
          <div className='relative mx-auto mb-6 flex h-24 w-24 items-center justify-center'>
            <div className='absolute inset-0 animate-ping rounded-full bg-green-500/20' />
            <div className='relative flex h-20 w-20 items-center justify-center rounded-full bg-gradient-to-br from-green-500 to-emerald-600 shadow-lg shadow-green-500/30'>
              <CheckCircle2 className='h-10 w-10 text-white' />
            </div>
          </div>

          {/* Confetti Icon */}
          <PartyPopper className='mx-auto mb-4 h-8 w-8 text-yellow-500' />

          <h2 className='mb-2 text-2xl font-bold text-foreground'>
            {t('success')}
          </h2>
          <p className='mb-6 text-muted-foreground'>{t('successMessage')}</p>

          {/* Booking Summary */}
          <div className='mb-6 rounded-xl bg-accent/50 p-4 text-left'>
            <h3 className='mb-3 font-semibold text-foreground'>
              {t('bookingSummary')}
            </h3>
            <div className='space-y-2 text-sm'>
              <div className='flex justify-between'>
                <span className='text-muted-foreground'>{t('service')}:</span>
                <span className='font-medium text-foreground'>
                  {selectedService?.name}
                </span>
              </div>
              <div className='flex justify-between'>
                <span className='text-muted-foreground'>{t('date')}:</span>
                <span className='font-medium text-foreground'>
                  {selectedSlot &&
                    new Date(selectedSlot.startTimeUtc).toLocaleDateString()}
                </span>
              </div>
              <div className='flex justify-between'>
                <span className='text-muted-foreground'>{t('time')}:</span>
                <span className='font-medium text-foreground'>
                  {selectedSlot &&
                    new Date(selectedSlot.startTimeUtc).toLocaleTimeString([], {
                      hour: '2-digit',
                      minute: '2-digit',
                    })}
                </span>
              </div>
              <div className='flex justify-between'>
                <span className='text-muted-foreground'>
                  {t('professional')}:
                </span>
                <span className='font-medium capitalize text-foreground'>
                  {professionalName}
                </span>
              </div>
            </div>
          </div>

          <p className='text-sm text-muted-foreground'>
            {t('emailConfirmation')}
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className='mx-auto max-w-2xl'>
      {/* Stepper */}
      <Stepper currentStep={currentStep} steps={steps} />

      <form onSubmit={handleSubmit}>
        {/* Step 1: Select Service */}
        {currentStep === 0 && (
          <div className='space-y-4'>
            <div className='mb-6 text-center'>
              <Sparkles className='mx-auto mb-2 h-8 w-8 text-primary' />
              <h2 className='text-xl font-semibold text-foreground'>
                {t('selectService')}
              </h2>
              <p className='text-sm text-muted-foreground'>
                {t('selectServiceDesc')}
              </p>
            </div>

            <div className='grid gap-4'>
              {services.map((service) => (
                <button
                  key={service.id}
                  type='button'
                  onClick={() => {
                    setSelectedService(service);
                    setSelectedDate(null);
                    setSelectedSlot(null);
                  }}
                  className={`group relative overflow-hidden rounded-2xl border p-5 text-left transition-all ${
                    selectedService?.id === service.id
                      ? 'border-primary bg-primary/5 shadow-lg shadow-primary/10'
                      : 'border-border bg-card hover:border-primary/50 hover:shadow-md'
                  }`}
                >
                  {/* Selected Indicator */}
                  {selectedService?.id === service.id && (
                    <div className='absolute right-4 top-4'>
                      <div className='flex h-6 w-6 items-center justify-center rounded-full bg-primary'>
                        <CheckCircle2 className='h-4 w-4 text-primary-foreground' />
                      </div>
                    </div>
                  )}

                  <div className='flex items-start gap-4'>
                    {/* Service Icon */}
                    <div
                      className={`flex h-12 w-12 shrink-0 items-center justify-center rounded-xl ${
                        selectedService?.id === service.id
                          ? 'bg-primary text-primary-foreground'
                          : 'bg-accent text-muted-foreground group-hover:bg-primary/10 group-hover:text-primary'
                      }`}
                    >
                      <CalendarCheck className='h-6 w-6' />
                    </div>

                    {/* Service Info */}
                    <div className='flex-1'>
                      <h3 className='font-semibold text-foreground'>
                        {service.name}
                      </h3>
                      {service.description && (
                        <p className='mt-1 text-sm text-muted-foreground line-clamp-2'>
                          {service.description}
                        </p>
                      )}
                      <div className='mt-3 flex flex-wrap items-center gap-4 text-sm'>
                        <div className='flex items-center gap-1.5 text-muted-foreground'>
                          <Clock className='h-4 w-4' />
                          <span>
                            {service.durationMinutes} {t('minutes')}
                          </span>
                        </div>
                        <div className='flex items-center gap-1.5 font-semibold text-primary'>
                          <DollarSign className='h-4 w-4' />
                          <span>${service.price}</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </button>
              ))}
            </div>

            {/* Next Button */}
            <div className='mt-6 flex justify-end'>
              <button
                type='button'
                disabled={!selectedService}
                onClick={() => setCurrentStep(1)}
                className='inline-flex items-center gap-2 rounded-xl bg-primary px-6 py-3 font-medium text-primary-foreground transition-all hover:bg-primary/90 disabled:cursor-not-allowed disabled:opacity-50'
              >
                {t('next')}
                <ChevronRight className='h-4 w-4' />
              </button>
            </div>
          </div>
        )}

        {/* Step 2: Select Date & Time */}
        {currentStep === 1 && (
          <div className='space-y-6'>
            <div className='mb-6 text-center'>
              <Calendar className='mx-auto mb-2 h-8 w-8 text-primary' />
              <h2 className='text-xl font-semibold text-foreground'>
                {t('selectDateTime')}
              </h2>
              <p className='text-sm text-muted-foreground'>
                {t('selectDateTimeDesc')}
              </p>
            </div>

            {loadingSlots ? (
              <div className='flex items-center justify-center py-12'>
                <Loader2 className='h-8 w-8 animate-spin text-primary' />
              </div>
            ) : (
              <div className='grid gap-6 lg:grid-cols-2'>
                {/* Calendar */}
                <CalendarPicker
                  selectedDate={selectedDate}
                  onSelectDate={(date) => {
                    setSelectedDate(date);
                    setSelectedSlot(null);
                  }}
                  availableDates={availableDates}
                />

                {/* Time Slots */}
                <div className='rounded-2xl border border-border bg-card p-4 sm:p-6'>
                  <h3 className='mb-4 flex items-center gap-2 font-semibold text-foreground'>
                    <Clock className='h-5 w-5 text-primary' />
                    {t('availableSlots')}
                  </h3>

                  {!selectedDate ? (
                    <div className='flex flex-col items-center justify-center py-8 text-center'>
                      <Calendar className='mb-3 h-12 w-12 text-muted-foreground/50' />
                      <p className='text-sm text-muted-foreground'>
                        {t('selectDateFirst')}
                      </p>
                    </div>
                  ) : slotsForSelectedDate.length === 0 ? (
                    <div className='flex flex-col items-center justify-center py-8 text-center'>
                      <Clock className='mb-3 h-12 w-12 text-muted-foreground/50' />
                      <p className='text-sm text-muted-foreground'>
                        {t('noSlotsAvailable')}
                      </p>
                    </div>
                  ) : (
                    <div className='grid grid-cols-2 gap-2 sm:grid-cols-3'>
                      {slotsForSelectedDate.map((slot) => {
                        const time = new Date(slot.startTimeUtc);
                        return (
                          <button
                            key={slot.id}
                            type='button'
                            onClick={() => setSelectedSlot(slot)}
                            className={`rounded-xl px-3 py-3 text-sm font-medium transition-all ${
                              selectedSlot?.id === slot.id
                                ? 'bg-primary text-primary-foreground shadow-lg shadow-primary/25'
                                : 'border border-border bg-accent/50 text-foreground hover:border-primary hover:bg-primary/10'
                            }`}
                          >
                            {time.toLocaleTimeString([], {
                              hour: '2-digit',
                              minute: '2-digit',
                            })}
                          </button>
                        );
                      })}
                    </div>
                  )}
                </div>
              </div>
            )}

            {/* Navigation Buttons */}
            <div className='mt-6 flex justify-between'>
              <button
                type='button'
                onClick={() => setCurrentStep(0)}
                className='inline-flex items-center gap-2 rounded-xl border border-border px-6 py-3 font-medium text-foreground transition-all hover:bg-accent'
              >
                <ChevronLeft className='h-4 w-4' />
                {t('back')}
              </button>
              <button
                type='button'
                disabled={!selectedSlot}
                onClick={() => setCurrentStep(2)}
                className='inline-flex items-center gap-2 rounded-xl bg-primary px-6 py-3 font-medium text-primary-foreground transition-all hover:bg-primary/90 disabled:cursor-not-allowed disabled:opacity-50'
              >
                {t('next')}
                <ChevronRight className='h-4 w-4' />
              </button>
            </div>
          </div>
        )}

        {/* Step 3: Client Details */}
        {currentStep === 2 && (
          <div className='space-y-6'>
            <div className='mb-6 text-center'>
              <User className='mx-auto mb-2 h-8 w-8 text-primary' />
              <h2 className='text-xl font-semibold text-foreground'>
                {t('yourDetails')}
              </h2>
              <p className='text-sm text-muted-foreground'>
                {t('yourDetailsDesc')}
              </p>
            </div>

            <div className='rounded-2xl border border-border bg-card p-6'>
              <div className='space-y-4'>
                {/* Name Fields */}
                <div className='grid gap-4 sm:grid-cols-2'>
                  <div>
                    <label className='mb-2 flex items-center gap-2 text-sm font-medium text-foreground'>
                      <User className='h-4 w-4 text-muted-foreground' />
                      {t('name')} *
                    </label>
                    <input
                      name='name'
                      type='text'
                      required
                      value={clientName}
                      onChange={(e) => setClientName(e.target.value)}
                      placeholder={t('namePlaceholder')}
                      className='w-full rounded-xl border border-border bg-background px-4 py-3 text-foreground placeholder:text-muted-foreground focus:border-primary focus:outline-none focus:ring-2 focus:ring-primary/20'
                    />
                  </div>
                  <div>
                    <label className='mb-2 flex items-center gap-2 text-sm font-medium text-foreground'>
                      <User className='h-4 w-4 text-muted-foreground' />
                      {t('surname')} *
                    </label>
                    <input
                      name='surname'
                      type='text'
                      required
                      value={clientSurname}
                      onChange={(e) => setClientSurname(e.target.value)}
                      placeholder={t('surnamePlaceholder')}
                      className='w-full rounded-xl border border-border bg-background px-4 py-3 text-foreground placeholder:text-muted-foreground focus:border-primary focus:outline-none focus:ring-2 focus:ring-primary/20'
                    />
                  </div>
                </div>

                {/* Email */}
                <div>
                  <label className='mb-2 flex items-center gap-2 text-sm font-medium text-foreground'>
                    <Mail className='h-4 w-4 text-muted-foreground' />
                    {t('email')} *
                  </label>
                  <input
                    name='email'
                    type='email'
                    required
                    value={clientEmail}
                    onChange={(e) => setClientEmail(e.target.value)}
                    placeholder={t('emailPlaceholder')}
                    className='w-full rounded-xl border border-border bg-background px-4 py-3 text-foreground placeholder:text-muted-foreground focus:border-primary focus:outline-none focus:ring-2 focus:ring-primary/20'
                  />
                </div>

                {/* Phone */}
                <div>
                  <label className='mb-2 flex items-center gap-2 text-sm font-medium text-foreground'>
                    <Phone className='h-4 w-4 text-muted-foreground' />
                    {t('phone')}
                  </label>
                  <input
                    name='phone'
                    type='tel'
                    value={clientPhone}
                    onChange={(e) => setClientPhone(e.target.value)}
                    placeholder={t('phonePlaceholder')}
                    className='w-full rounded-xl border border-border bg-background px-4 py-3 text-foreground placeholder:text-muted-foreground focus:border-primary focus:outline-none focus:ring-2 focus:ring-primary/20'
                  />
                </div>

                {/* Notes */}
                <div>
                  <label className='mb-2 flex items-center gap-2 text-sm font-medium text-foreground'>
                    <FileText className='h-4 w-4 text-muted-foreground' />
                    {t('notes')}
                  </label>
                  <textarea
                    name='notes'
                    rows={3}
                    value={notes}
                    onChange={(e) => setNotes(e.target.value)}
                    placeholder={t('notesPlaceholder')}
                    className='w-full resize-none rounded-xl border border-border bg-background px-4 py-3 text-foreground placeholder:text-muted-foreground focus:border-primary focus:outline-none focus:ring-2 focus:ring-primary/20'
                  />
                </div>
              </div>
            </div>

            {/* Navigation Buttons */}
            <div className='mt-6 flex justify-between'>
              <button
                type='button'
                onClick={() => setCurrentStep(1)}
                className='inline-flex items-center gap-2 rounded-xl border border-border px-6 py-3 font-medium text-foreground transition-all hover:bg-accent'
              >
                <ChevronLeft className='h-4 w-4' />
                {t('back')}
              </button>
              <button
                type='button'
                disabled={!clientName || !clientSurname || !clientEmail}
                onClick={() => setCurrentStep(3)}
                className='inline-flex items-center gap-2 rounded-xl bg-primary px-6 py-3 font-medium text-primary-foreground transition-all hover:bg-primary/90 disabled:cursor-not-allowed disabled:opacity-50'
              >
                {t('next')}
                <ChevronRight className='h-4 w-4' />
              </button>
            </div>
          </div>
        )}

        {/* Step 4: Confirmation */}
        {currentStep === 3 && (
          <div className='space-y-6'>
            <div className='mb-6 text-center'>
              <CheckCircle2 className='mx-auto mb-2 h-8 w-8 text-primary' />
              <h2 className='text-xl font-semibold text-foreground'>
                {t('confirmBooking')}
              </h2>
              <p className='text-sm text-muted-foreground'>
                {t('confirmBookingDesc')}
              </p>
            </div>

            {/* Booking Summary Card */}
            <div className='rounded-2xl border border-border bg-card overflow-hidden'>
              {/* Header */}
              <div className='bg-gradient-to-r from-primary to-primary/80 px-6 py-4'>
                <h3 className='font-semibold text-primary-foreground'>
                  {t('bookingSummary')}
                </h3>
              </div>

              {/* Content */}
              <div className='divide-y divide-border'>
                {/* Service */}
                <div className='flex items-center justify-between p-4'>
                  <div className='flex items-center gap-3'>
                    <div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
                      <CalendarCheck className='h-5 w-5 text-primary' />
                    </div>
                    <div>
                      <p className='text-sm text-muted-foreground'>
                        {t('service')}
                      </p>
                      <p className='font-medium text-foreground'>
                        {selectedService?.name}
                      </p>
                    </div>
                  </div>
                  <span className='font-semibold text-primary'>
                    ${selectedService?.price}
                  </span>
                </div>

                {/* Date & Time */}
                <div className='flex items-center gap-3 p-4'>
                  <div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
                    <Calendar className='h-5 w-5 text-primary' />
                  </div>
                  <div>
                    <p className='text-sm text-muted-foreground'>
                      {t('dateTime')}
                    </p>
                    <p className='font-medium text-foreground'>
                      {selectedSlot &&
                        new Date(selectedSlot.startTimeUtc).toLocaleDateString(
                          undefined,
                          {
                            weekday: 'long',
                            year: 'numeric',
                            month: 'long',
                            day: 'numeric',
                          }
                        )}
                    </p>
                    <p className='text-sm text-muted-foreground'>
                      {selectedSlot &&
                        new Date(selectedSlot.startTimeUtc).toLocaleTimeString(
                          [],
                          { hour: '2-digit', minute: '2-digit' }
                        )}
                      {' - '}
                      {selectedService?.durationMinutes} {t('minutes')}
                    </p>
                  </div>
                </div>

                {/* Duration */}
                <div className='flex items-center gap-3 p-4'>
                  <div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
                    <Clock className='h-5 w-5 text-primary' />
                  </div>
                  <div>
                    <p className='text-sm text-muted-foreground'>
                      {t('duration')}
                    </p>
                    <p className='font-medium text-foreground'>
                      {selectedService?.durationMinutes} {t('minutes')}
                    </p>
                  </div>
                </div>

                {/* Professional */}
                <div className='flex items-center gap-3 p-4'>
                  <div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
                    <User className='h-5 w-5 text-primary' />
                  </div>
                  <div>
                    <p className='text-sm text-muted-foreground'>
                      {t('professional')}
                    </p>
                    <p className='font-medium capitalize text-foreground'>
                      {professionalName}
                    </p>
                  </div>
                </div>
              </div>
            </div>

            {/* Error Message */}
            {error && (
              <div className='rounded-xl border border-red-200 bg-red-50 p-4 dark:border-red-800 dark:bg-red-900/20'>
                <p className='text-sm text-red-600 dark:text-red-400'>
                  {error}
                </p>
              </div>
            )}

            {/* Navigation Buttons */}
            <div className='mt-6 flex justify-between'>
              <button
                type='button'
                onClick={() => setCurrentStep(2)}
                className='inline-flex items-center gap-2 rounded-xl border border-border px-6 py-3 font-medium text-foreground transition-all hover:bg-accent'
              >
                <ChevronLeft className='h-4 w-4' />
                {t('back')}
              </button>
              <button
                type='submit'
                disabled={loading}
                className='inline-flex items-center gap-2 rounded-xl bg-primary px-8 py-3 font-medium text-primary-foreground transition-all hover:bg-primary/90 disabled:cursor-not-allowed disabled:opacity-50'
              >
                {loading ? (
                  <>
                    <Loader2 className='h-4 w-4 animate-spin' />
                    {t('processing')}
                  </>
                ) : (
                  <>
                    <CheckCircle2 className='h-4 w-4' />
                    {t('submit')}
                  </>
                )}
              </button>
            </div>
          </div>
        )}
      </form>
    </div>
  );
}
