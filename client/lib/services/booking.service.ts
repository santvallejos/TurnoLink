import { api } from '../api/client';
import type { Booking, UpdateBookingRequest } from '@/types';

/**
 * Booking service - Handles booking management for professionals (requires authentication)
 */
export const bookingService = {
  /**
   * Get all bookings for the authenticated professional
   */
  async getMyBookings(): Promise<Booking[]> {
    return api.get<Booking[]>('/api/Bookings/my-bookings');
  },

  /**
   * Get bookings within a date range
   */
  async getByDateRange(startDate: Date, endDate: Date): Promise<Booking[]> {
    const params = new URLSearchParams({
      startDate: startDate.toISOString(),
      endDate: endDate.toISOString(),
    });
    return api.get<Booking[]>(`/api/Bookings/my-bookings/range?${params}`);
  },

  /**
   * Get booking by ID
   */
  async getById(id: string): Promise<Booking> {
    return api.get<Booking>(`/api/Bookings/${id}`);
  },

  /**
   * Update booking status or details
   */
  async update(id: string, data: UpdateBookingRequest): Promise<Booking> {
    return api.patch<Booking>(`/api/Bookings/${id}`, data);
  },

  /**
   * Cancel a booking
   */
  async cancel(id: string): Promise<void> {
    return api.post(`/api/Bookings/${id}/cancel`);
  },
};
