// API Response Types for TurnoLink

// ============================================
// AUTH TYPES
// ============================================

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  surname: string;
  email: string;
  password: string;
  repeatPassword: string;
  phoneNumber?: string;
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
}

export interface CurrentUser {
  userId: string;
  email: string;
  name: string;
  message: string;
}

// ============================================
// USER TYPES
// ============================================

export interface User {
  name: string;
  surname: string;
  email: string;
  phoneNumber?: string;
  slug: string;
  isActive: boolean;
}

export interface UpdateUserRequest {
  name?: string;
  surname?: string;
  phoneNumber?: string;
  isActive?: boolean;
}

// ============================================
// SERVICE TYPES
// ============================================

export interface Service {
  id: string;
  userName: string;
  name: string;
  description?: string;
  durationMinutes: number;
  price: number;
  isActive: boolean;
}

export interface CreateServiceRequest {
  userId: string;
  name: string;
  description?: string;
  durationMinutes: number;
  price: number;
}

export interface UpdateServiceRequest {
  name?: string;
  description?: string;
  durationMinutes?: number;
  price?: number;
  isActive?: boolean;
}

// ============================================
// BOOKING TYPES
// ============================================

export interface Booking {
  id: string;
  clientName: string;
  clientSurname: string;
  clientEmail?: string;
  clientPhone?: string;
  userId: string;
  serviceId: string;
  userName: string;
  serviceName: string;
  servicePrice: number;
  startTime: string;
  endTime: string;
  status: BookingStatus;
  notes?: string;
  createdAt: string;
}

export type BookingStatus = 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed';

export interface CreateBookingRequest {
  serviceId: string;
  availabilityId: string;
  clientName: string;
  clientSurname: string;
  clientEmail: string;
  clientPhone?: string;
  notes?: string;
}

export interface UpdateBookingRequest {
  availabilityId?: string;
  status?: BookingStatus;
  notes?: string;
}

// ============================================
// AVAILABILITY TYPES
// ============================================

export interface Availability {
  id: string;
  serviceId: string;
  startTime: string;
  endTime: string;
  serviceName: string;
}

export interface CreateAvailabilityRequest {
  serviceId: string;
  dayOfWeek: number; // 0=Sunday, 1=Monday, ..., 6=Saturday
  startTime: string; // TimeSpan format: "09:00:00"
  startDate?: string; // Optional: specific date to start
}

export enum RepeatAvailability {
  None = 0,
  Daily = 1,
  Weekly = 2,
  Monthly = 3,
}

export interface CreateRecurringAvailabilityRequest {
  serviceId: string;
  dayOfWeek: number;
  startTime: string;
  repeat: RepeatAvailability;
  endDate: string;
  startDate?: string;
}

export interface UpdateAvailabilityRequest {
  dayOfWeek?: number;
  startTime?: string;
  newDate?: string;
}

// ============================================
// API ERROR TYPE
// ============================================

export interface ApiError {
  message: string;
  status: number;
}
