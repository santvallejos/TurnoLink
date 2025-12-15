import { api } from '../api/client';
import type { Service, CreateServiceRequest, UpdateServiceRequest } from '@/types';

/**
 * Service management - Handles professional services CRUD (requires authentication)
 */
export const serviceService = {
  /**
   * Get all services for the authenticated professional
   */
  async getMyServices(): Promise<Service[]> {
    return api.get<Service[]>('/api/Services/my-services');
  },

  /**
   * Get service by ID
   */
  async getById(id: string): Promise<Service> {
    return api.get<Service>(`/api/Services/${id}`);
  },

  /**
   * Get services by user slug (public)
   */
  async getByUserSlug(slug: string): Promise<Service[]> {
    return api.get<Service[]>(`/api/Services/${slug}`);
  },

  /**
   * Create a new service
   */
  async create(data: CreateServiceRequest): Promise<Service> {
    return api.post<Service>('/api/Services', data);
  },

  /**
   * Update an existing service
   */
  async update(id: string, data: UpdateServiceRequest): Promise<Service> {
    return api.put<Service>(`/api/Services/${id}`, data);
  },

  /**
   * Delete a service
   */
  async delete(id: string): Promise<void> {
    return api.delete(`/api/Services/${id}`);
  },
};
