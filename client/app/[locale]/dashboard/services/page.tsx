'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { serviceService, authService } from '@/lib/services';
import type { Service, CreateServiceRequest, UpdateServiceRequest } from '@/types';
import {
  Plus,
  Briefcase,
  Clock,
  DollarSign,
  Edit,
  Trash2,
  X,
  Loader2,
  AlertCircle,
} from 'lucide-react';

export default function ServicesPage() {
  const t = useTranslations();
  const [services, setServices] = useState<Service[]>([]);
  const [loading, setLoading] = useState(true);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editingService, setEditingService] = useState<Service | null>(null);
  const [deleting, setDeleting] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [currentUserId, setCurrentUserId] = useState<string | null>(null);

  // Form state
  const [formName, setFormName] = useState('');
  const [formDescription, setFormDescription] = useState('');
  const [formDuration, setFormDuration] = useState(30);
  const [formPrice, setFormPrice] = useState(0);
  const [formIsActive, setFormIsActive] = useState(true);

  useEffect(() => {
    loadData();
  }, []);

  async function loadData() {
    try {
      const [servicesData, currentUser] = await Promise.all([
        serviceService.getMyServices(),
        authService.getCurrentUser(),
      ]);
      setServices(servicesData);
      setCurrentUserId(currentUser.userId);
    } catch {
      setError('Error loading services');
    } finally {
      setLoading(false);
    }
  }

  function openEditModal(service: Service) {
    setEditingService(service);
    setFormName(service.name);
    setFormDescription(service.description || '');
    setFormDuration(service.durationMinutes);
    setFormPrice(service.price);
    setFormIsActive(service.isActive);
  }

  function resetForm() {
    setFormName('');
    setFormDescription('');
    setFormDuration(30);
    setFormPrice(0);
    setFormIsActive(true);
    setEditingService(null);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setSaving(true);
    setError(null);

    try {
      if (editingService) {
        // Update existing service
        const updateData: UpdateServiceRequest = {
          name: formName,
          description: formDescription || undefined,
          durationMinutes: formDuration,
          price: formPrice,
          isActive: formIsActive,
        };
        const updatedService = await serviceService.update(editingService.id, updateData);
        setServices((prev) =>
          prev.map((s) => (s.id === editingService.id ? updatedService : s)),
        );
      } else {
        // Create new service
        if (!currentUserId) {
          setError('User not authenticated');
          return;
        }
        const createData: CreateServiceRequest = {
          userId: currentUserId,
          name: formName,
          description: formDescription || undefined,
          durationMinutes: formDuration,
          price: formPrice,
        };
        const newService = await serviceService.create(createData);
        setServices((prev) => [...prev, newService]);
      }

      setShowCreateModal(false);
      resetForm();
    } catch (err) {
      setError(editingService ? 'Error updating service' : 'Error creating service');
      console.error(err);
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(id: string) {
    if (!confirm(t('services.confirmDelete'))) return;

    setDeleting(id);
    setError(null);

    try {
      await serviceService.delete(id);
      setServices((prev) => prev.filter((s) => s.id !== id));
    } catch (err) {
      setError('Error deleting service');
      console.error(err);
    } finally {
      setDeleting(null);
    }
  }

  const totalServices = services.length;

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
    <>
      <div className='p-6 lg:p-8'>
        {/* Header */}
        <div className='flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-8'>
          <div>
            <h1 className='text-2xl lg:text-3xl font-bold text-foreground'>
              {t('services.title')}
            </h1>
            <p className='mt-1 text-muted-foreground'>
              {t('services.description')}
            </p>
          </div>
          <button
            onClick={() => setShowCreateModal(true)}
            className='inline-flex items-center justify-center gap-2 px-6 py-3 bg-primary text-primary-foreground rounded-xl font-semibold shadow-lg shadow-primary/25 hover:shadow-xl hover:shadow-primary/30 hover:scale-[1.02] transition-all duration-300'
          >
            <Plus className='h-5 w-5' />
            {t('services.create')}
          </button>
        </div>

        {/* Error message */}
        {error && (
          <div className='mb-6 p-4 rounded-xl bg-red-100 dark:bg-red-900/30 border border-red-200 dark:border-red-800 flex items-center gap-3'>
            <AlertCircle className='h-5 w-5 text-red-600 dark:text-red-400' />
            <p className='text-red-700 dark:text-red-400'>{error}</p>
          </div>
        )}

        {/* Stats */}
        <div className='grid gap-4 sm:grid-cols-2 mb-8'>
          <div className='rounded-2xl bg-card border border-border p-6'>
            <div className='flex items-center gap-4'>
              <div className='flex h-12 w-12 items-center justify-center rounded-xl bg-primary/10 text-primary'>
                <Briefcase className='h-6 w-6' />
              </div>
              <div>
                <p className='text-sm text-muted-foreground'>
                  {t('services.stats.total')}
                </p>
                <p className='text-2xl font-bold text-foreground'>
                  {totalServices}
                </p>
              </div>
            </div>
          </div>
          <div className='rounded-2xl bg-card border border-border p-6'>
            <div className='flex items-center gap-4'>
              <div className='flex h-12 w-12 items-center justify-center rounded-xl bg-blue-500/10 text-blue-500'>
                <Clock className='h-6 w-6' />
              </div>
              <div>
                <p className='text-sm text-muted-foreground'>
                  {t('services.stats.avgDuration')}
                </p>
                <p className='text-2xl font-bold text-foreground'>
                  {services.length > 0
                    ? Math.round(services.reduce((sum, s) => sum + s.durationMinutes, 0) / services.length)
                    : 0} min
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* Services grid */}
        {services.length === 0 ? (
          <div className='rounded-2xl bg-card border border-border p-12 text-center'>
            <div className='flex h-16 w-16 items-center justify-center rounded-full bg-muted mx-auto mb-4'>
              <Briefcase className='h-8 w-8 text-muted-foreground' />
            </div>
            <p className='text-foreground font-medium'>{t('services.empty')}</p>
            <p className='text-sm text-muted-foreground mt-1 mb-6'>
              {t('services.emptyDesc')}
            </p>
            <button
              onClick={() => setShowCreateModal(true)}
              className='inline-flex items-center gap-2 px-6 py-3 bg-primary text-primary-foreground rounded-xl font-semibold hover:bg-primary/90 transition-colors'
            >
              <Plus className='h-5 w-5' />
              {t('services.createFirst')}
            </button>
          </div>
        ) : (
          <div className='grid gap-4 sm:grid-cols-2 lg:grid-cols-3'>
            {services.map((service) => (
              <div
                key={service.id}
                className='group relative rounded-2xl bg-card border border-border p-6 hover:shadow-lg hover:border-primary/20 transition-all duration-300'
              >
                {/* Service icon */}
                <div className='flex h-14 w-14 items-center justify-center rounded-2xl bg-primary/10 text-primary mb-4 group-hover:bg-primary group-hover:text-primary-foreground transition-colors'>
                  <Briefcase className='h-7 w-7' />
                </div>

                {/* Service info */}
                <h3 className='text-lg font-semibold text-foreground mb-2'>
                  {service.name}
                </h3>
                <p className='text-sm text-muted-foreground line-clamp-2 mb-4'>
                  {service.description || t('services.noDescription')}
                </p>

                {/* Meta info */}
                <div className='flex items-center gap-4 text-sm mb-4'>
                  <div className='flex items-center gap-1.5 text-muted-foreground'>
                    <Clock className='h-4 w-4' />
                    <span>{service.durationMinutes} min</span>
                  </div>
                  <div className='flex items-center gap-1.5 text-foreground font-semibold'>
                    <DollarSign className='h-4 w-4' />
                    <span>{service.price}</span>
                  </div>
                </div>

                {/* Actions */}
                <div className='flex gap-2 pt-4 border-t border-border'>
                  <button
                    onClick={() => openEditModal(service)}
                    className='flex-1 flex items-center justify-center gap-2 px-4 py-2 text-sm font-medium rounded-lg bg-secondary text-secondary-foreground hover:bg-accent transition-colors'
                  >
                    <Edit className='h-4 w-4' />
                    {t('common.edit')}
                  </button>
                  <button
                    onClick={() => handleDelete(service.id)}
                    disabled={deleting === service.id}
                    className='p-2 text-muted-foreground hover:text-destructive rounded-lg hover:bg-destructive/10 transition-colors disabled:opacity-50'
                  >
                    {deleting === service.id ? (
                      <Loader2 className='h-4 w-4 animate-spin' />
                    ) : (
                      <Trash2 className='h-4 w-4' />
                    )}
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Create/Edit Modal */}
      {(showCreateModal || editingService) && (
        <div className='fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50'>
          <div className='w-full max-w-lg rounded-2xl bg-card border border-border p-6 shadow-xl'>
            <div className='flex items-center justify-between mb-6'>
              <h2 className='text-xl font-semibold text-foreground'>
                {editingService
                  ? t('services.editService')
                  : t('services.create')}
              </h2>
              <button
                onClick={() => {
                  setShowCreateModal(false);
                  resetForm();
                }}
                className='p-2 text-muted-foreground hover:text-foreground rounded-lg hover:bg-accent transition-colors'
              >
                <X className='h-5 w-5' />
              </button>
            </div>

            <form onSubmit={handleSubmit} className='space-y-4'>
              <div>
                <label className='block text-sm font-medium text-foreground mb-2'>
                  {t('services.form.name')}
                </label>
                <input
                  type='text'
                  value={formName}
                  onChange={(e) => setFormName(e.target.value)}
                  className='w-full rounded-xl border border-border bg-background py-3 px-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                  placeholder={t('services.form.namePlaceholder')}
                  required
                />
              </div>

              <div>
                <label className='block text-sm font-medium text-foreground mb-2'>
                  {t('services.form.description')}
                </label>
                <textarea
                  rows={3}
                  value={formDescription}
                  onChange={(e) => setFormDescription(e.target.value)}
                  className='w-full rounded-xl border border-border bg-background py-3 px-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20 resize-none'
                  placeholder={t('services.form.descriptionPlaceholder')}
                />
              </div>

              <div className='grid grid-cols-2 gap-4'>
                <div>
                  <label className='block text-sm font-medium text-foreground mb-2'>
                    {t('services.form.duration')}
                  </label>
                  <div className='relative'>
                    <Clock className='absolute left-4 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground' />
                    <input
                      type='number'
                      value={formDuration}
                      onChange={(e) => setFormDuration(Number(e.target.value))}
                      min={5}
                      className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                      required
                    />
                  </div>
                </div>

                <div>
                  <label className='block text-sm font-medium text-foreground mb-2'>
                    {t('services.form.price')}
                  </label>
                  <div className='relative'>
                    <DollarSign className='absolute left-4 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground' />
                    <input
                      type='number'
                      step='0.01'
                      value={formPrice}
                      onChange={(e) => setFormPrice(Number(e.target.value))}
                      min={0}
                      className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                      required
                    />
                  </div>
                </div>
              </div>

              {editingService && (
                <div className='flex items-center gap-3 p-4 rounded-xl bg-secondary/50'>
                  <input
                    type='checkbox'
                    id='isActive'
                    checked={formIsActive}
                    onChange={(e) => setFormIsActive(e.target.checked)}
                    className='h-5 w-5 rounded border-border text-primary focus:ring-primary'
                  />
                  <label htmlFor='isActive' className='text-sm text-foreground'>
                    {t('services.form.isActive')}
                  </label>
                </div>
              )}

              <div className='flex gap-3 pt-4'>
                <button
                  type='button'
                  onClick={() => {
                    setShowCreateModal(false);
                    resetForm();
                  }}
                  className='flex-1 px-6 py-3 text-sm font-medium rounded-xl border border-border bg-background text-foreground hover:bg-accent transition-colors'
                >
                  {t('common.cancel')}
                </button>
                <button
                  type='submit'
                  disabled={saving}
                  className='flex-1 px-6 py-3 text-sm font-medium rounded-xl bg-primary text-primary-foreground hover:bg-primary/90 transition-colors disabled:opacity-50 flex items-center justify-center gap-2'
                >
                  {saving && <Loader2 className='h-4 w-4 animate-spin' />}
                  {editingService ? t('common.save') : t('services.create')}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  );
}
