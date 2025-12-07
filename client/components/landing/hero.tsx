"use client"

import Link from "next/link"
import { Button } from "@/components/ui/button"
import { ArrowRight, CheckCircle2, Clock, Users } from "lucide-react"
import { useLocale, useTranslations } from 'next-intl'

export function Hero() {
  const t = useTranslations()
  const locale = useLocale()

  const highlights = [
    t('landing.hero.highlights.noCredit'),
    t('landing.hero.highlights.freeTrial'),
    t('landing.hero.highlights.cancelAnytime')
  ];

  return (
    <section className="relative pt-32 pb-20 lg:pt-40 lg:pb-32 overflow-hidden">
      {/* Background decoration */}
      <div className="absolute inset-0 -z-10">
        <div className="absolute top-1/4 left-1/4 w-96 h-96 bg-primary/10 rounded-full blur-3xl" />
        <div className="absolute bottom-1/4 right-1/4 w-96 h-96 bg-accent-foreground/10 rounded-full blur-3xl" />
      </div>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid lg:grid-cols-2 gap-12 lg:gap-20 items-center">
          {/* Left content */}
          <div className="text-center lg:text-left">
            <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-accent text-accent-foreground text-sm font-medium mb-6">
              <span className="relative flex h-2 w-2">
                <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-accent-foreground opacity-75" />
                <span className="relative inline-flex rounded-full h-2 w-2 bg-accent-foreground" />
              </span>
              {t('landing.hero.whatsappIntegration')}
            </div>

            <h1 className="text-4xl sm:text-5xl lg:text-6xl font-bold tracking-tight text-foreground text-balance">
              {t("landing.hero.title")}
            </h1>

            <p className="mt-6 text-lg sm:text-xl text-muted-foreground max-w-2xl mx-auto lg:mx-0 text-pretty">
              {t("landing.hero.subtitle")}
            </p>

            <div className="mt-10 flex flex-col sm:flex-row gap-4 justify-center lg:justify-start">
              <Link href={`/${locale}/register`}>
                <Button className="w-full sm:w-auto gap-2 text-base" size="lg">
                  {t("landing.hero.cta")}
                  <ArrowRight className="w-4 h-4" />
                </Button>
              </Link>
              <Link href={`/${locale}/login`}>
                <Button className="w-full sm:w-auto text-base bg-transparent" size="lg" variant="outline">
                  {t("landing.hero.secondaryCta")}
                </Button>
              </Link>
            </div>

            <div className="mt-8 flex flex-wrap gap-4 justify-center lg:justify-start">
              {highlights.map((item, index) => (
                <div key={index} className="flex items-center gap-2 text-sm text-muted-foreground">
                  <CheckCircle2 className="w-4 h-4 text-primary" />
                  {item}
                </div>
              ))}
            </div>
          </div>

          {/* Right content - Calendar mockup */}
          <div className="relative">
            <div className="relative bg-card rounded-2xl shadow-2xl border border-border overflow-hidden">
              {/* Calendar header */}
              <div className="bg-primary/5 px-6 py-4 border-b border-border">
                <div className="flex items-center justify-between">
                  <div>
                    <h3 className="font-semibold text-foreground">
                      {t('landing.hero.calendar.month')}
                    </h3>
                    <p className="text-sm text-muted-foreground">
                      3 {t('landing.hero.calendar.todayAppointments')}
                    </p>
                  </div>
                  <Button size="sm">+ {t('landing.hero.calendar.newButton')}</Button>
                </div>
              </div>

              {/* Calendar appointments */}
              <div className="p-6 space-y-3">
                <AppointmentCard
                  client="María García"
                  locale={locale}
                  service={t('booking.services.haircut')}
                  status="confirmed"
                  time="09:00"
                />
                <AppointmentCard
                  client="Carlos López"
                  locale={locale}
                  service={t('booking.services.coloring')}
                  status="confirmed"
                  time="10:30"
                />
                <AppointmentCard
                  client="Ana Martínez"
                  locale={locale}
                  service={t('booking.services.manicure')}
                  status="pending"
                  time="14:00"
                />
                <AppointmentCard
                  client="Pedro Sánchez"
                  locale={locale}
                  service={t('booking.services.treatment')}
                  status="confirmed"
                  time="16:00"
                />
              </div>
            </div>

            {/* Floating cards */}
            <div className="absolute -top-4 -right-4 bg-card rounded-xl shadow-lg border border-border p-4 hidden lg:flex items-center gap-3">
              <div className="w-10 h-10 rounded-full bg-green-100 dark:bg-green-900/30 flex items-center justify-center">
                <CheckCircle2 className="w-5 h-5 text-green-600 dark:text-green-400" />
              </div>
              <div>
                <p className="text-sm font-medium text-foreground">
                  {t('landing.hero.floatingCards.reminderSent')}
                </p>
                <p className="text-xs text-muted-foreground">María García</p>
              </div>
            </div>

            <div className="absolute -bottom-4 -left-4 bg-card rounded-xl shadow-lg border border-border p-4 hidden lg:flex items-center gap-3">
              <div className="w-10 h-10 rounded-full bg-primary/10 flex items-center justify-center">
                <Users className="w-5 h-5 text-primary" />
              </div>
              <div>
                <p className="text-2xl font-bold text-foreground">127</p>
                <p className="text-xs text-muted-foreground">
                  {t('landing.hero.floatingCards.activeClients')}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  )
}

function AppointmentCard({
  time,
  client,
  service,
  status,
  locale,
}: {
  time: string
  client: string
  service: string
  status: "confirmed" | "pending"
  locale: string
}) {
  const t = useTranslations('dashboard.status');
  
  return (
    <div className="flex items-center gap-4 p-3 rounded-lg bg-muted/30 hover:bg-muted/50 transition-colors">
      <div className="flex items-center gap-2 min-w-[60px]">
        <Clock className="w-4 h-4 text-muted-foreground" />
        <span className="text-sm font-medium text-foreground">{time}</span>
      </div>
      <div className="flex-1 min-w-0">
        <p className="font-medium text-foreground truncate">{client}</p>
        <p className="text-sm text-muted-foreground truncate">{service}</p>
      </div>
      <div
        className={`px-2 py-1 rounded-full text-xs font-medium ${
          status === "confirmed"
            ? "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400"
            : "bg-yellow-100 text-yellow-700 dark:bg-yellow-900/30 dark:text-yellow-400"
        }`}
      >
        {t(status)}
      </div>
    </div>
  )
}
