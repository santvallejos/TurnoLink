import { getRequestConfig } from 'next-intl/server';

type Locale = 'es' | 'en';

/**
 * Configuración de next-intl para carga de mensajes
 * Para static export, usamos un locale por defecto
 */
export default getRequestConfig(async () => {
  // Para static export, devolvemos un locale por defecto
  // Los layouts específicos de cada locale cargan sus propios mensajes
  const locale: Locale = 'es';

  return {
    locale,
    messages: (await import(`../../messages/${locale}.json`)).default,
  };
});
