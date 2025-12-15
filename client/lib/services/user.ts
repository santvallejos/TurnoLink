import { api } from '../api/client';
import type { User, UpdateUserRequest } from '@/types';

/**
 * User service - Handles user CRUD operations (requires authentication)
 */
export const userService = {
  /**
   * Get all users
   */
  async getAll(): Promise<User[]> {
    return api.get<User[]>('/api/Users');
  },

  /**
   * Get user by ID
   */
  async getById(id: string): Promise<User> {
    return api.get<User>(`/api/Users/${id}`);
  },

  /**
   * Get user by email
   */
  async getByEmail(email: string): Promise<User> {
    return api.get<User>(`/api/Users/email/${encodeURIComponent(email)}`);
  },

  /**
   * Update user
   */
  async update(id: string, data: UpdateUserRequest): Promise<User> {
    return api.put<User>(`/api/Users/${id}`, data);
  },

  /**
   * Delete user
   */
  async delete(id: string): Promise<void> {
    return api.delete(`/api/Users/${id}`);
  },
};
