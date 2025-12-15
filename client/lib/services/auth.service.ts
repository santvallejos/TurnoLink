import { api, setAuthToken, clearAuthToken } from '../api/client';
import type { AuthResponse, CurrentUser, LoginRequest, RegisterRequest } from '@/types';

/**
 * Authentication service - Handles login, register, and session management
 */
export const authService = {
  /**
   * Login with email and password
   * Stores token automatically on success
   */
  async login(credentials: LoginRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/api/Auth/login', credentials);
    setAuthToken(response.token);
    return response;
  },

  /**
   * Register a new user account
   * Stores token automatically on success
   */
  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/api/Auth/register', data);
    setAuthToken(response.token);
    return response;
  },

  /**
   * Get current authenticated user info
   * Requires valid token
   */
  async getCurrentUser(): Promise<CurrentUser> {
    return api.get<CurrentUser>('/api/Auth/me');
  },

  /**
   * Logout - Clears stored token
   */
  logout(): void {
    clearAuthToken();
  },
};
