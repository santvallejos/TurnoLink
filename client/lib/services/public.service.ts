import { api } from '../api/client';
import type { Service, Booking, CreateBookingRequest } from '@/types';

/**
 * Public service - Endpoints accessible without authentication
 * Used for client-facing booking flow
 */
export const publicService = {
  /**
   * Get service details by ID
   */
  async getServiceById(id: string): Promise<Service> {
    return api.get<Service>(`/api/Public/services/${id}`);
  },

  /**
   * Get all services by user slug
   */
  async getServicesBySlug(slug: string): Promise<Service[]> {
    return api.get<Service[]>(`/api/Public/${slug}`);
  },

  /**
   * Create a new booking (client-facing)
   */
  async createBooking(data: CreateBookingRequest): Promise<Booking> {
    return api.post<Booking>('/api/Public/bookings', data);
  },

  /**
   * Get booking details by ID
   */
  async getBookingById(id: string): Promise<Booking> {
    return api.get<Booking>(`/api/Public/bookings/${id}`);
  },
};
