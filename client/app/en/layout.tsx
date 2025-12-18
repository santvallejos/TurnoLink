import { NextIntlClientProvider } from 'next-intl';
import messages from '@/messages/en.json';

interface Props {
  children: React.ReactNode;
}

/**
 * Layout para la versión en inglés
 * Provee el contexto de next-intl para todos los componentes hijos
 */
export default function EnLocaleLayout({ children }: Props) {
  return (
    <NextIntlClientProvider locale="en" messages={messages}>
      {children}
    </NextIntlClientProvider>
  );
}
