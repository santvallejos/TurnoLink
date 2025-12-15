import { api } from '../api/client';
import type {
  Availability,
  CreateAvailabilityRequest,
  CreateRecurringAvailabilityRequest,
  UpdateAvailabilityRequest,
} from '@/types';

/**
 * Availability service - Handles availability slot management
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
   */
  async create(data: CreateAvailabilityRequest): Promise<Availability> {
    return api.post<Availability>('/api/Availabilities', data);
  },

  /**
   * Create a recurring availability slot
   */
  async createRecurring(data: CreateRecurringAvailabilityRequest): Promise<Availability> {
    return api.post<Availability>('/recurring', data);
  },

  /**
   * Update an availability slot
   */
  async update(id: string, data: UpdateAvailabilityRequest): Promise<Availability> {
    return api.put<Availability>(`/api/Availabilities/${id}`, data);
  },

  /**
   * Delete an availability slot
   */
  async delete(id: string): Promise<void> {
    return api.delete(`/api/Availabilities/${id}`);
  },
};
