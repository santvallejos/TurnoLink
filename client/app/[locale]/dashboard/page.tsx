"use client"

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { StatCard } from "@/components/dashboard/stat-card"
import { AppointmentsTable } from "@/components/dashboard/appointments-table"
import { WeeklyCalendar } from "@/components/dashboard/weekly-calendar"
import { Calendar, CalendarDays, DollarSign, Plus, Users } from "lucide-react"
import { useLocale, useTranslations } from 'next-intl'
import { formatCurrency } from '@/lib/i18n'

export default function DashboardPage() {
  const locale = useLocale()
  const t = useTranslations()

  const stats = [
    {
      title: t("dashboard.stats.todayAppointments"),
      value: "3",
      change: `+1 vs ${  locale === "es" ? "ayer" : "yesterday"}`,
      changeType: "positive" as const,
      icon: Calendar,
    },
    {
      title: t("dashboard.stats.weekAppointments"),
      value: "12",
      change: `+3 vs ${  locale === "es" ? "semana pasada" : "last week"}`,
      changeType: "positive" as const,
      icon: CalendarDays,
    },
    {
      title: t("dashboard.stats.totalClients"),
      value: "127",
      change: `+8 ${  locale === "es" ? "este mes" : "this month"}`,
      changeType: "positive" as const,
      icon: Users,
    },
    {
      title: t("dashboard.stats.monthRevenue"),
      value: "127",
      change: `+12% vs ${  locale === "es" ? "mes pasado" : "last month"}`,
      changeType: "positive" as const,
      icon: DollarSign,
    },
  ]

  return (
    <div className="space-y-6">
      {/* Stats Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {stats.map((stat, index) => (
          <StatCard key={index} {...stat} />
        ))}
      </div>

      {/* Calendar */}
      <Card className="bg-card border-border">
        <CardHeader className="flex flex-row items-center justify-between pb-4">
          <CardTitle className="text-lg font-semibold">
            {locale === "es" ? "Calendario Semanal" : "Weekly Calendar"}
          </CardTitle>
        </CardHeader>
        <CardContent className="pt-0">
          <WeeklyCalendar />
        </CardContent>
      </Card>

      {/* Appointments Table */}
      <Card className="bg-card border-border">
        <CardHeader className="flex flex-row items-center justify-between pb-4">
          <CardTitle className="text-lg font-semibold">{t("dashboard.appointments.title")}</CardTitle>
          <Button className="gap-2" size="sm">
            <Plus className="w-4 h-4" />
            {t("dashboard.appointments.newAppointment")}
          </Button>
        </CardHeader>
        <CardContent className="pt-0">
          <AppointmentsTable />
        </CardContent>
      </Card>
    </div>
  )
}
