import { api } from '../api/client';
import type {
  Availability,
  CreateAvailabilityRequest,
  CreateRecurringAvailabilityRequest,
  UpdateAvailabilityRequest,
} from '@/types';

/**
 * Offset UTC para Argentina (UTC-3) en minutos.
 * TODO: En el futuro, obtener dinámicamente según la zona horaria del usuario.
 */
const ARGENTINA_UTC_OFFSET = -180;

/**
 * Obtiene la fecha actual en formato YYYY-MM-DD
 */
const getTodayDate = (): string => {
  return new Date().toISOString().split('T')[0];
};

/**
 * Availability service - Handles availability slot management.
 * Todas las horas se envían al backend en hora local y se convierten a UTC usando el offset.
 */
export const availabilityService = {
  /**
   * Get availability by ID
   */
  async getById(id: string): Promise<Availability> {
    return api.get<Availability>(`/api/Availabilities/${id}`);
  },

  /**
   * Get all availabilities for the authenticated user
   */
  async getMyAvailabilities(): Promise<Availability[]> {
    return api.get<Availability[]>('/api/Availabilities/my-availabilities');
  },

  /**
   * Get availabilities by service ID (public)
   */
  async getByServiceId(serviceId: string): Promise<Availability[]> {
    return api.get<Availability[]>(`/api/Availabilities/service/${serviceId}`);
  },

  /**
   * Get available slots for a user within a date range (public)
   */
  async getByDateRange(userId: string, startDate: Date, endDate: Date): Promise<Availability[]> {
    const params = new URLSearchParams({
      startDate: startDate.toISOString(),
      endDate: endDate.toISOString(),
    });
    return api.get<Availability[]>(`/api/Availabilities/user/${userId}/range?${params}`);
  },

  /**
   * Create a non-recurring availability slot
   * @param serviceId - ID del servicio
   * @param startDate - Fecha en formato YYYY-MM-DD
   * @param startTime - Hora en formato HH:mm
   */
  async create(serviceId: string, startDate: string, startTime: string): Promise<Availability> {
    const request: CreateAvailabilityRequest = {
      serviceId,
      startDate,
      startTime,
      utcOffsetMinutes: ARGENTINA_UTC_OFFSET,
    };
    return api.post<Availability>('/api/Availabilities', request);
  },

  /**
   * Create recurring availability slots
   * @param serviceId - ID del servicio
   * @param startDate - Fecha de inicio en formato YYYY-MM-DD
   * @param startTime - Hora en formato HH:mm
   * @param repeat - Tipo de repetición (Daily, Weekly, Monthly)
   * @param endDate - Fecha de fin en formato YYYY-MM-DD
   */
  async createRecurring(
    serviceId: string,
    startDate: string,
    startTime: string,
    repeat: number,
    endDate: string
  ): Promise<Availability[]> {
    const request: CreateRecurringAvailabilityRequest = {
      serviceId,
      startDate,
      startTime,
      utcOffsetMinutes: ARGENTINA_UTC_OFFSET,
      repeat,
      endDate,
    };
    return api.post<Availability[]>('/api/Availabilities/recurring', request);
  },

  /**
   * Update an availability slot
   */
  async update(id: string, newDate?: string, newTime?: string): Promise<Availability> {
    const request: UpdateAvailabilityRequest = {
      newDate,
      newTime,
      utcOffsetMinutes: ARGENTINA_UTC_OFFSET,
    };
    return api.put<Availability>(`/api/Availabilities/${id}`, request);
  },

  /**
   * Delete an availability slot
   */
  async delete(id: string): Promise<void> {
    return api.delete(`/api/Availabilities/${id}`);
  },

  /**
   * Helper: Obtiene la fecha mínima permitida (hoy)
   */
  getMinDate: getTodayDate,
};
