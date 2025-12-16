'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { authService, serviceService } from '@/lib/services';
import type { CurrentUser, Service } from '@/types';
import DashboardLayout from '@/components/dashboard/dashboard-layout';
import {
  Plus,
  Search,
  Briefcase,
  Clock,
  DollarSign,
  MoreHorizontal,
  Edit,
  Trash2,
  ToggleLeft,
  ToggleRight,
  X,
  Loader2,
} from 'lucide-react';

export default function ServicesPage() {
  const t = useTranslations();
  const [user, setUser] = useState<CurrentUser | null>(null);
  const [services, setServices] = useState<Service[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editingService, setEditingService] = useState<Service | null>(null);

  useEffect(() => {
    async function loadData() {
      try {
        const [userData, servicesData] = await Promise.all([
          authService.getCurrentUser(),
          serviceService.getMyServices(),
        ]);
        setUser(userData);
        setServices(servicesData);
      } catch {
        window.location.href = '/login';
      } finally {
        setLoading(false);
      }
    }
    loadData();
  }, []);

  const filteredServices = services.filter(
    (service) =>
      service.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      service.description?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const activeServices = services.filter((s) => s.isActive).length;
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
    <DashboardLayout user={user}>
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

        {/* Stats */}
        <div className='grid gap-4 sm:grid-cols-3 mb-8'>
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
              <div className='flex h-12 w-12 items-center justify-center rounded-xl bg-green-500/10 text-green-500'>
                <ToggleRight className='h-6 w-6' />
              </div>
              <div>
                <p className='text-sm text-muted-foreground'>
                  {t('services.stats.active')}
                </p>
                <p className='text-2xl font-bold text-foreground'>
                  {activeServices}
                </p>
              </div>
            </div>
          </div>
          <div className='rounded-2xl bg-card border border-border p-6'>
            <div className='flex items-center gap-4'>
              <div className='flex h-12 w-12 items-center justify-center rounded-xl bg-orange-500/10 text-orange-500'>
                <ToggleLeft className='h-6 w-6' />
              </div>
              <div>
                <p className='text-sm text-muted-foreground'>
                  {t('services.stats.inactive')}
                </p>
                <p className='text-2xl font-bold text-foreground'>
                  {totalServices - activeServices}
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* Search */}
        <div className='relative mb-6'>
          <Search className='absolute left-4 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground' />
          <input
            type='text'
            placeholder={t('services.searchPlaceholder')}
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
          />
        </div>

        {/* Services grid */}
        {filteredServices.length === 0 ? (
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
            {filteredServices.map((service) => (
              <div
                key={service.id}
                className='group relative rounded-2xl bg-card border border-border p-6 hover:shadow-lg hover:border-primary/20 transition-all duration-300'
              >
                {/* Status badge */}
                <div className='absolute top-4 right-4'>
                  <span
                    className={`px-2 py-1 rounded-full text-xs font-medium ${
                      service.isActive
                        ? 'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400'
                        : 'bg-gray-100 text-gray-700 dark:bg-gray-900/30 dark:text-gray-400'
                    }`}
                  >
                    {service.isActive
                      ? t('services.active')
                      : t('services.inactive')}
                  </span>
                </div>

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
                    onClick={() => setEditingService(service)}
                    className='flex-1 flex items-center justify-center gap-2 px-4 py-2 text-sm font-medium rounded-lg bg-secondary text-secondary-foreground hover:bg-accent transition-colors'
                  >
                    <Edit className='h-4 w-4' />
                    {t('common.edit')}
                  </button>
                  <button className='p-2 text-muted-foreground hover:text-destructive rounded-lg hover:bg-destructive/10 transition-colors'>
                    <Trash2 className='h-4 w-4' />
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
                  setEditingService(null);
                }}
                className='p-2 text-muted-foreground hover:text-foreground rounded-lg hover:bg-accent transition-colors'
              >
                <X className='h-5 w-5' />
              </button>
            </div>

            <form className='space-y-4'>
              <div>
                <label className='block text-sm font-medium text-foreground mb-2'>
                  {t('services.form.name')}
                </label>
                <input
                  type='text'
                  defaultValue={editingService?.name}
                  className='w-full rounded-xl border border-border bg-background py-3 px-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                  placeholder={t('services.form.namePlaceholder')}
                />
              </div>

              <div>
                <label className='block text-sm font-medium text-foreground mb-2'>
                  {t('services.form.description')}
                </label>
                <textarea
                  rows={3}
                  defaultValue={editingService?.description}
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
                      defaultValue={editingService?.durationMinutes || 30}
                      className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
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
                      defaultValue={editingService?.price || 0}
                      className='w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all placeholder:text-muted-foreground focus:border-primary focus:ring-2 focus:ring-primary/20'
                    />
                  </div>
                </div>
              </div>

              <div className='flex items-center gap-3 p-4 rounded-xl bg-secondary/50'>
                <input
                  type='checkbox'
                  id='isActive'
                  defaultChecked={editingService?.isActive ?? true}
                  className='h-5 w-5 rounded border-border text-primary focus:ring-primary'
                />
                <label htmlFor='isActive' className='text-sm text-foreground'>
                  {t('services.form.isActive')}
                </label>
              </div>

              <div className='flex gap-3 pt-4'>
                <button
                  type='button'
                  onClick={() => {
                    setShowCreateModal(false);
                    setEditingService(null);
                  }}
                  className='flex-1 px-6 py-3 text-sm font-medium rounded-xl border border-border bg-background text-foreground hover:bg-accent transition-colors'
                >
                  {t('common.cancel')}
                </button>
                <button
                  type='submit'
                  className='flex-1 px-6 py-3 text-sm font-medium rounded-xl bg-primary text-primary-foreground hover:bg-primary/90 transition-colors'
                >
                  {editingService ? t('common.save') : t('services.create')}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </DashboardLayout>
  );
}
