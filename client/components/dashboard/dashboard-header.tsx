"use client"

import { Button } from "@/components/ui/button"
import { SidebarTrigger } from "@/components/ui/sidebar"
import { LanguageSwitcher } from "@/components/language-switcher"
import { ThemeToggle } from "@/components/theme-toggle"
import { Bell, Plus } from "lucide-react"
import { useTranslations } from 'next-intl'

export function DashboardHeader() {
  const t = useTranslations()

  return (
    <header className="flex items-center justify-between h-16 px-4 border-b border-border bg-background/80 backdrop-blur-sm">
      <div className="flex items-center gap-4">
        <SidebarTrigger className="md:hidden" />
        <div>
          <h1 className="text-lg font-semibold text-foreground">{t("dashboard.title")}</h1>
          <p className="text-sm text-muted-foreground">{t("dashboard.welcome")}, María</p>
        </div>
      </div>

      <div className="flex items-center gap-2">
        <Button variant="ghost" size="icon" className="relative">
          <Bell className="w-5 h-5" />
          <span className="absolute top-1 right-1 w-2 h-2 bg-destructive rounded-full" />
        </Button>
        <LanguageSwitcher />
        <ThemeToggle />
        <Button className="hidden sm:flex gap-2">
          <Plus className="w-4 h-4" />
          {t("dashboard.appointments.newAppointment")}
        </Button>
      </div>
    </header>
  )
}
