import { NextIntlClientProvider } from 'next-intl';
import messages from '@/messages/es.json';

interface Props {
  children: React.ReactNode;
}

/**
 * Layout para la versión en español
 * Provee el contexto de next-intl para todos los componentes hijos
 */
export default function EsLocaleLayout({ children }: Props) {
  return (
    <NextIntlClientProvider locale="es" messages={messages}>
      {children}
    </NextIntlClientProvider>
  );
}
