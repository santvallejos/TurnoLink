'use client';

import NextLink from 'next/link';
import { usePathname } from 'next/navigation';
import { ComponentProps } from 'react';

type LinkProps = ComponentProps<typeof NextLink>;

/**
 * Link component that automatically prefixes the current locale to the href
 * Compatible with static export (doesn't use headers())
 */
export function LocaleLink({ href, ...props }: LinkProps) {
  const pathname = usePathname();
  
  // Extract current locale from pathname
  const locale = pathname.startsWith('/en') ? 'en' : 'es';
  
  // If href is a string and starts with /, prefix with locale
  const localizedHref = typeof href === 'string' && href.startsWith('/')
    ? `/${locale}${href}`
    : href;
  
  return <NextLink href={localizedHref} {...props} />;
}

export default LocaleLink;
