import { ApiError } from '@/types';

type HttpMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';

interface RequestOptions {
  method?: HttpMethod;
  body?: unknown;
  headers?: Record<string, string>;
}

/**
 * Gets the stored auth token from localStorage
 */
function getAuthToken(): string | null {
  if (typeof window === 'undefined') return null;
  return localStorage.getItem('turnolink_token');
}

/**
 * Stores the auth token in localStorage
 */
export function setAuthToken(token: string): void {
  if (typeof window !== 'undefined') {
    localStorage.setItem('turnolink_token', token);
  }
}

/**
 * Removes the auth token from localStorage
 */
export function clearAuthToken(): void {
  if (typeof window !== 'undefined') {
    localStorage.removeItem('turnolink_token');
  }
}

/**
 * Base fetch wrapper with error handling and auth
 */
async function request<T>(endpoint: string, options: RequestOptions = {}): Promise<T> {
  const { method = 'GET', body, headers = {} } = options;

  const token = getAuthToken();
  const requestHeaders: Record<string, string> = {
    'Content-Type': 'application/json',
    ...headers,
  };

  if (token) {
    requestHeaders['Authorization'] = `Bearer ${token}`;
  }

  const fullUrl = `${endpoint}`;

  // Debug logging
  console.log('=== API REQUEST ===');
  console.log('URL:', fullUrl);
  console.log('Method:', method);
  console.log('Body:', body ? JSON.stringify(body, null, 2) : 'none');

  try {
    const response = await fetch(fullUrl, {
      method,
      headers: requestHeaders,
      body: body ? JSON.stringify(body) : undefined,
    });

    console.log('Response status:', response.status);

    // Handle no content response
    if (response.status === 204) {
      return undefined as T;
    }

    const data = await response.json();
    console.log('Response data:', data);

    if (!response.ok) {
      const error: ApiError = {
        message: data.message || 'An error occurred',
        status: response.status,
      };
      console.error('API Error:', error);
      throw error;
    }

    return data;
  } catch (err) {
    console.error('=== FETCH ERROR ===', err);
    throw err;
  }
}

// HTTP method helpers
export const api = {
  get: <T>(endpoint: string) => request<T>(endpoint),
  post: <T>(endpoint: string, body?: unknown) => request<T>(endpoint, { method: 'POST', body }),
  put: <T>(endpoint: string, body?: unknown) => request<T>(endpoint, { method: 'PUT', body }),
  patch: <T>(endpoint: string, body?: unknown) => request<T>(endpoint, { method: 'PATCH', body }),
  delete: <T>(endpoint: string) => request<T>(endpoint, { method: 'DELETE' }),
};
