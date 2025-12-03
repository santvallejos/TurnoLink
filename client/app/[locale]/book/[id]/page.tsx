"use client"

import Link from "next/link"
import { BookingForm } from "@/components/booking/booking-form"
import { LanguageSwitcher } from "@/components/language-switcher"
import { ThemeToggle } from "@/components/theme-toggle"
import { Calendar, ArrowLeft } from "lucide-react"
import { Button } from "@/components/ui/button"
import { useTranslations, useLocale } from 'next-intl'
import { useParams } from 'next/navigation'

export default function BookingPage() {
  const locale = useLocale()
  const t = useTranslations()
  const params = useParams()

  return (
    <div className="min-h-screen bg-background">
      {/* Header */}
      <header className="sticky top-0 z-50 bg-background/80 backdrop-blur-lg border-b border-border">
        <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <div className="flex items-center gap-4">
              <Link href={`/${locale}`}>
                <Button variant="ghost" size="icon">
                  <ArrowLeft className="w-5 h-5" />
                </Button>
              </Link>
              <Link href={`/${locale}`} className="flex items-center gap-2">
                <div className="w-8 h-8 rounded-lg bg-primary flex items-center justify-center">
                  <Calendar className="w-5 h-5 text-primary-foreground" />
                </div>
                <span className="font-bold text-lg text-foreground">{t("common.appName")}</span>
              </Link>
            </div>
            <div className="flex items-center gap-2">
              <LanguageSwitcher />
              <ThemeToggle />
            </div>
          </div>
        </div>
      </header>

      {/* Main content */}
      <main className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <BookingForm />
      </main>
    </div>
  )
}
