import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

/**
 * Middleware mínimo para export estático
 * El middleware de next-intl no es compatible con static export
 * La redirección de locale se maneja en app/page.tsx
 */
export function middleware(request: NextRequest) {
  // Permitir que todas las requests pasen sin modificación
  return NextResponse.next();
}

export const config = {
  // Solo aplicar a la raíz si es necesario
  matcher: [],
};
