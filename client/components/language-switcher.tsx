"use client"

import { useRouter, usePathname } from "next/navigation"
import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { Globe } from "lucide-react"
import { useLocale } from 'next-intl'
import type { Locale } from '@/i18n/config'

export function LanguageSwitcher() {
  const router = useRouter()
  const pathname = usePathname()
  const currentLocale = useLocale() as Locale

  const switchLocale = (newLocale: Locale) => {
    const segments = pathname.split("/")
    if (segments[1] === "es" || segments[1] === "en") {
      segments[1] = newLocale
    } else {
      segments.splice(1, 0, newLocale)
    }
    router.push(segments.join("/"))
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" size="sm" className="gap-2">
          <Globe className="h-4 w-4" />
          <span className="uppercase">{currentLocale}</span>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuItem onClick={() => switchLocale("es")} className={currentLocale === "es" ? "bg-accent" : ""}>
          <span className="mr-2">🇪🇸</span>
          Español
        </DropdownMenuItem>
        <DropdownMenuItem onClick={() => switchLocale("en")} className={currentLocale === "en" ? "bg-accent" : ""}>
          <span className="mr-2">🇬🇧</span>
          English
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
