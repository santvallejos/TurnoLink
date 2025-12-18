import BookingPageClient from '@/components/booking/booking-page-client';

// Forzar renderizado estático para export
export const dynamic = 'force-static';

/**
 * Página de reservas para Santiago Vallejos
 * Slug: santiago-vallejos-7da3a3
 */
export default function SantiagoVallejosBookingPage() {
  return <BookingPageClient slug="santiago-vallejos-7da3a3" locale="es" />;
}
