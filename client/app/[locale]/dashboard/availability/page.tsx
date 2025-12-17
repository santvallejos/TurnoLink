"use client";

import { useEffect, useState } from "react";
import { useTranslations } from "next-intl";
import { availabilityService, serviceService } from "@/lib/services";
import type {
  Availability,
  Service,
  CreateAvailabilityRequest,
  RepeatAvailability,
} from "@/types";
import {
  Plus,
  Clock,
  Calendar,
  Trash2,
  X,
  Loader2,
  AlertCircle,
} from "lucide-react";

const DAYS_OF_WEEK = [
  { value: 0, key: "sunday" },
  { value: 1, key: "monday" },
  { value: 2, key: "tuesday" },
  { value: 3, key: "wednesday" },
  { value: 4, key: "thursday" },
  { value: 5, key: "friday" },
  { value: 6, key: "saturday" },
];

export default function AvailabilityPage() {
  const t = useTranslations();
  const [availabilities, setAvailabilities] = useState<Availability[]>([]);
  const [services, setServices] = useState<Service[]>([]);
  const [loading, setLoading] = useState(true);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [creating, setCreating] = useState(false);
  const [deleting, setDeleting] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  // Form state
  const [selectedServiceId, setSelectedServiceId] = useState<string>("");
  const [selectedDay, setSelectedDay] = useState<number>(1);
  const [startTime, setStartTime] = useState<string>("09:00");
  const [isRecurring, setIsRecurring] = useState(false);
  const [repeatType, setRepeatType] = useState<RepeatAvailability>(2); // Weekly
  const [endDate, setEndDate] = useState<string>("");

  useEffect(() => {
    loadData();
  }, []);

  async function loadData() {
    try {
      const [availabilitiesData, servicesData] = await Promise.all([
        availabilityService.getMyAvailabilities(),
        serviceService.getMyServices(),
      ]);
      setAvailabilities(availabilitiesData);
      setServices(servicesData);
      if (servicesData.length > 0 && !selectedServiceId) {
        setSelectedServiceId(servicesData[0].id);
      }
    } catch {
      setError("Error loading data");
    } finally {
      setLoading(false);
    }
  }

  async function handleCreate(e: React.FormEvent) {
    e.preventDefault();
    if (!selectedServiceId) return;

    setCreating(true);
    setError(null);

    try {
      const timeSpan = `${startTime}:00`; // Convert "09:00" to "09:00:00"

      if (isRecurring && endDate) {
        await availabilityService.createRecurring({
          serviceId: selectedServiceId,
          dayOfWeek: selectedDay,
          startTime: timeSpan,
          repeat: repeatType,
          endDate: endDate,
        });
      } else {
        await availabilityService.create({
          serviceId: selectedServiceId,
          dayOfWeek: selectedDay,
          startTime: timeSpan,
        });
      }

      await loadData();
      setShowCreateModal(false);
      resetForm();
    } catch (err) {
      setError("Error creating availability");
      console.error(err);
    } finally {
      setCreating(false);
    }
  }

  async function handleDelete(id: string) {
    setDeleting(id);
    try {
      await availabilityService.delete(id);
      setAvailabilities((prev) => prev.filter((a) => a.id !== id));
    } catch (err) {
      setError("Error deleting availability");
      console.error(err);
    } finally {
      setDeleting(null);
    }
  }

  function resetForm() {
    setSelectedDay(1);
    setStartTime("09:00");
    setIsRecurring(false);
    setRepeatType(2);
    setEndDate("");
  }

  function formatTime(dateString: string) {
    return new Date(dateString).toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
    });
  }

  function formatDate(dateString: string) {
    return new Date(dateString).toLocaleDateString();
  }

  function getDayName(dateString: string) {
    const date = new Date(dateString);
    return t(`availability.days.${DAYS_OF_WEEK[date.getDay()].key}`);
  }

  // Group availabilities by service
  const availabilitiesByService = availabilities.reduce<
    Record<string, Availability[]>
  >((acc, avail) => {
    if (!acc[avail.serviceName]) {
      acc[avail.serviceName] = [];
    }
    acc[avail.serviceName].push(avail);
    return acc;
  }, {});

  if (loading) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-background">
        <div className="flex flex-col items-center gap-4">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
          <p className="text-muted-foreground">{t("common.loading")}</p>
        </div>
      </div>
    );
  }

  return (
    <>
      <div className="p-6 lg:p-8">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-8">
          <div>
            <h1 className="text-2xl lg:text-3xl font-bold text-foreground">
              {t("availability.title")}
            </h1>
            <p className="mt-1 text-muted-foreground">
              {t("availability.description")}
            </p>
          </div>
          <button
            onClick={() => setShowCreateModal(true)}
            disabled={services.length === 0}
            className="inline-flex items-center justify-center gap-2 px-6 py-3 bg-primary text-primary-foreground rounded-xl font-semibold shadow-lg shadow-primary/25 hover:shadow-xl hover:shadow-primary/30 hover:scale-[1.02] transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <Plus className="h-5 w-5" />
            {t("availability.create")}
          </button>
        </div>

        {/* Error message */}
        {error && (
          <div className="mb-6 p-4 rounded-xl bg-red-100 dark:bg-red-900/30 border border-red-200 dark:border-red-800 flex items-center gap-3">
            <AlertCircle className="h-5 w-5 text-red-600 dark:text-red-400" />
            <p className="text-red-700 dark:text-red-400">{error}</p>
          </div>
        )}

        {/* No services warning */}
        {services.length === 0 && (
          <div className="rounded-2xl bg-yellow-50 dark:bg-yellow-900/20 border border-yellow-200 dark:border-yellow-800 p-6 mb-8">
            <div className="flex items-center gap-3">
              <AlertCircle className="h-6 w-6 text-yellow-600 dark:text-yellow-400" />
              <div>
                <p className="font-medium text-yellow-800 dark:text-yellow-300">
                  {t("availability.noServicesTitle")}
                </p>
                <p className="text-sm text-yellow-700 dark:text-yellow-400">
                  {t("availability.noServicesDesc")}
                </p>
              </div>
            </div>
          </div>
        )}

        {/* Stats */}
        <div className="grid gap-4 sm:grid-cols-2 mb-8">
          <div className="rounded-2xl bg-card border border-border p-6">
            <div className="flex items-center gap-4">
              <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-primary/10 text-primary">
                <Clock className="h-6 w-6" />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">
                  {t("availability.stats.total")}
                </p>
                <p className="text-2xl font-bold text-foreground">
                  {availabilities.length}
                </p>
              </div>
            </div>
          </div>
          <div className="rounded-2xl bg-card border border-border p-6">
            <div className="flex items-center gap-4">
              <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-blue-500/10 text-blue-500">
                <Calendar className="h-6 w-6" />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">
                  {t("availability.stats.services")}
                </p>
                <p className="text-2xl font-bold text-foreground">
                  {Object.keys(availabilitiesByService).length}
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* Availabilities list */}
        {availabilities.length === 0 ? (
          <div className="rounded-2xl bg-card border border-border p-12 text-center">
            <div className="flex h-16 w-16 items-center justify-center rounded-full bg-muted mx-auto mb-4">
              <Clock className="h-8 w-8 text-muted-foreground" />
            </div>
            <p className="text-foreground font-medium">
              {t("availability.empty")}
            </p>
            <p className="text-sm text-muted-foreground mt-1 mb-6">
              {t("availability.emptyDesc")}
            </p>
            {services.length > 0 && (
              <button
                onClick={() => setShowCreateModal(true)}
                className="inline-flex items-center gap-2 px-6 py-3 bg-primary text-primary-foreground rounded-xl font-semibold hover:bg-primary/90 transition-colors"
              >
                <Plus className="h-5 w-5" />
                {t("availability.createFirst")}
              </button>
            )}
          </div>
        ) : (
          <div className="space-y-6">
            {Object.entries(availabilitiesByService).map(
              ([serviceName, serviceAvailabilities]) => (
                <div
                  key={serviceName}
                  className="rounded-2xl bg-card border border-border overflow-hidden"
                >
                  <div className="px-6 py-4 bg-secondary/50 border-b border-border">
                    <h3 className="font-semibold text-foreground">
                      {serviceName}
                    </h3>
                  </div>
                  <div className="divide-y divide-border">
                    {serviceAvailabilities.map((availability) => (
                      <div
                        key={availability.id}
                        className="flex items-center justify-between p-4 hover:bg-accent/50 transition-colors"
                      >
                        <div className="flex items-center gap-4">
                          <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-primary/10 text-primary">
                            <Clock className="h-5 w-5" />
                          </div>
                          <div>
                            <p className="font-medium text-foreground">
                              {getDayName(availability.startTime)} -{" "}
                              {formatDate(availability.startTime)}
                            </p>
                            <p className="text-sm text-muted-foreground">
                              {formatTime(availability.startTime)} -{" "}
                              {formatTime(availability.endTime)}
                            </p>
                          </div>
                        </div>
                        <button
                          onClick={() => handleDelete(availability.id)}
                          disabled={deleting === availability.id}
                          className="p-2 text-muted-foreground hover:text-destructive rounded-lg hover:bg-destructive/10 transition-colors disabled:opacity-50"
                        >
                          {deleting === availability.id ? (
                            <Loader2 className="h-4 w-4 animate-spin" />
                          ) : (
                            <Trash2 className="h-4 w-4" />
                          )}
                        </button>
                      </div>
                    ))}
                  </div>
                </div>
              )
            )}
          </div>
        )}
      </div>

      {/* Create Modal */}
      {showCreateModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50">
          <div className="w-full max-w-lg rounded-2xl bg-card border border-border p-6 shadow-xl">
            <div className="flex items-center justify-between mb-6">
              <h2 className="text-xl font-semibold text-foreground">
                {t("availability.create")}
              </h2>
              <button
                onClick={() => {
                  setShowCreateModal(false);
                  resetForm();
                }}
                className="p-2 text-muted-foreground hover:text-foreground rounded-lg hover:bg-accent transition-colors"
              >
                <X className="h-5 w-5" />
              </button>
            </div>

            <form onSubmit={handleCreate} className="space-y-4">
              {/* Service selector */}
              <div>
                <label className="block text-sm font-medium text-foreground mb-2">
                  {t("availability.form.service")}
                </label>
                <select
                  value={selectedServiceId}
                  onChange={(e) => setSelectedServiceId(e.target.value)}
                  className="w-full rounded-xl border border-border bg-background py-3 px-4 text-foreground outline-none transition-all focus:border-primary focus:ring-2 focus:ring-primary/20"
                  required
                >
                  {services.map((service) => (
                    <option key={service.id} value={service.id}>
                      {service.name} ({service.durationMinutes} min)
                    </option>
                  ))}
                </select>
              </div>

              {/* Day of week */}
              <div>
                <label className="block text-sm font-medium text-foreground mb-2">
                  {t("availability.form.dayOfWeek")}
                </label>
                <select
                  value={selectedDay}
                  onChange={(e) => setSelectedDay(Number(e.target.value))}
                  className="w-full rounded-xl border border-border bg-background py-3 px-4 text-foreground outline-none transition-all focus:border-primary focus:ring-2 focus:ring-primary/20"
                  required
                >
                  {DAYS_OF_WEEK.map((day) => (
                    <option key={day.value} value={day.value}>
                      {t(`availability.days.${day.key}`)}
                    </option>
                  ))}
                </select>
              </div>

              {/* Start time */}
              <div>
                <label className="block text-sm font-medium text-foreground mb-2">
                  {t("availability.form.startTime")}
                </label>
                <div className="relative">
                  <Clock className="absolute left-4 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground" />
                  <input
                    type="time"
                    value={startTime}
                    onChange={(e) => setStartTime(e.target.value)}
                    className="w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all focus:border-primary focus:ring-2 focus:ring-primary/20"
                    required
                  />
                </div>
              </div>

              {/* Recurring toggle */}
              <div className="flex items-center gap-3 p-4 rounded-xl bg-secondary/50">
                <input
                  type="checkbox"
                  id="isRecurring"
                  checked={isRecurring}
                  onChange={(e) => setIsRecurring(e.target.checked)}
                  className="h-5 w-5 rounded border-border text-primary focus:ring-primary"
                />
                <label
                  htmlFor="isRecurring"
                  className="text-sm text-foreground"
                >
                  {t("availability.form.recurring")}
                </label>
              </div>

              {/* Recurring options */}
              {isRecurring && (
                <>
                  <div>
                    <label className="block text-sm font-medium text-foreground mb-2">
                      {t("availability.form.repeatType")}
                    </label>
                    <select
                      value={repeatType}
                      onChange={(e) =>
                        setRepeatType(
                          Number(e.target.value) as RepeatAvailability
                        )
                      }
                      className="w-full rounded-xl border border-border bg-background py-3 px-4 text-foreground outline-none transition-all focus:border-primary focus:ring-2 focus:ring-primary/20"
                    >
                      <option value={1}>
                        {t("availability.repeat.daily")}
                      </option>
                      <option value={2}>
                        {t("availability.repeat.weekly")}
                      </option>
                      <option value={3}>
                        {t("availability.repeat.monthly")}
                      </option>
                    </select>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-foreground mb-2">
                      {t("availability.form.endDate")}
                    </label>
                    <div className="relative">
                      <Calendar className="absolute left-4 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground" />
                      <input
                        type="date"
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                        min={new Date().toISOString().split("T")[0]}
                        className="w-full rounded-xl border border-border bg-background py-3 pl-12 pr-4 text-foreground outline-none transition-all focus:border-primary focus:ring-2 focus:ring-primary/20"
                        required={isRecurring}
                      />
                    </div>
                  </div>
                </>
              )}

              {/* Actions */}
              <div className="flex gap-3 pt-4">
                <button
                  type="button"
                  onClick={() => {
                    setShowCreateModal(false);
                    resetForm();
                  }}
                  className="flex-1 px-6 py-3 text-sm font-medium rounded-xl border border-border bg-background text-foreground hover:bg-accent transition-colors"
                >
                  {t("common.cancel")}
                </button>
                <button
                  type="submit"
                  disabled={creating || !selectedServiceId}
                  className="flex-1 px-6 py-3 text-sm font-medium rounded-xl bg-primary text-primary-foreground hover:bg-primary/90 transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
                >
                  {creating && <Loader2 className="h-4 w-4 animate-spin" />}
                  {t("availability.create")}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  );
}
