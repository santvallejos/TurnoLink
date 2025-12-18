import type { NextConfig } from 'next';
import createNextIntlPlugin from 'next-intl/plugin';

// Plugin de next-intl con configuración simplificada para static export
const withNextIntl = createNextIntlPlugin('./lib/i18n/request.ts');

const nextConfig: NextConfig = {
  output: 'export',
  // Deshabilitar trailing slash para mejor compatibilidad
  trailingSlash: true,
  // Configuración de imágenes para `next/image`.
  // Para static export, usamos unoptimized
  images: {
    unoptimized: true,
    // Formatos modernos preferidos
    formats: ['image/avif', 'image/webp'],
    // Tamaños de dispositivo usados por Next.js para generar srcset
    deviceSizes: [640, 750, 828, 1080, 1200, 1920],
    // Tamaños de imagen adicionales cuando se usan imágenes pequeñas (icons, avatars)
    imageSizes: [16, 32, 48, 64, 96],
    // Ejemplo de remotePatterns: habilita patrones remotos más flexibles
    remotePatterns: [
      {
        protocol: 'https',
        hostname: '**',
        pathname: '/**',
      },
    ],
  },
};

export default withNextIntl(nextConfig);
