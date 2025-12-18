import BookingPageClient from '@/components/booking/booking-page-client';

// Forzar renderizado estático para export
export const dynamic = 'force-static';

/**
 * Página de reservas para Sofia Cubilla
 * Slug: sofia-cubilla-f0b49a
 */
export default function SofiaCubillaBookingPage() {
  return <BookingPageClient slug="sofia-cubilla-f0b49a" locale="es" />;
}
