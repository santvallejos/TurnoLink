// Re-export all services for convenient imports
export { authService } from './auth';
export { userService } from './user';
export { serviceService } from './service';
export { bookingService } from './booking';
export { availabilityService } from './availability';
export { publicService } from './public';
export { signalRService } from './signalr';
export type { BookingNotification } from './signalr';

// Re-export token utilities
export { setAuthToken, clearAuthToken } from '../api/client';
