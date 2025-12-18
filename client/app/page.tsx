'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';

/**
 * Root page - Redirects to the default locale (es)
 * Uses client-side redirect for static export compatibility
 */
export default function RootPage() {
  const router = useRouter();

  useEffect(() => {
    router.replace('/es');
  }, [router]);

  return (
    <div className="flex min-h-screen items-center justify-center">
      <p>Redirecting...</p>
    </div>
  );
}
