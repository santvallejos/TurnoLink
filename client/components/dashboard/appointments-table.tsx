"use client"

import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Badge } from "@/components/ui/badge"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { MoreHorizontal } from "lucide-react"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { appointments, services } from "@/lib/mock-data"
import { useTranslations, useLocale } from 'next-intl'
import { formatShortDate } from '@/lib/i18n'

export function AppointmentsTable() {
  const t = useTranslations()
  const locale = useLocale()

  const getServiceName = (serviceId: string) => {
    const service = services.find((s) => s.id === serviceId)
    if (!service) return ""
    return locale === "es" ? service.name : service.nameEn
  }

  const getStatusBadge = (status: string) => {
    const variants: Record<string, "default" | "secondary" | "destructive" | "outline"> = {
      confirmed: "default",
      pending: "secondary",
      cancelled: "destructive",
      completed: "outline",
    }

    const statusKey = `dashboard.status.${status}` as const

    return <Badge variant={variants[status] || "default"}>{t(statusKey)}</Badge>
  }

  const getInitials = (name: string) => {
    return name
      .split(" ")
      .map((n) => n[0])
      .join("")
      .toUpperCase()
  }

  return (
    <div className="rounded-lg border border-border bg-card">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>{t("dashboard.appointments.client")}</TableHead>
            <TableHead>{t("dashboard.appointments.service")}</TableHead>
            <TableHead>{t("dashboard.appointments.date")}</TableHead>
            <TableHead>{t("dashboard.appointments.time")}</TableHead>
            <TableHead>{t("dashboard.appointments.status")}</TableHead>
            <TableHead className="w-[50px]"></TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {appointments.slice(0, 8).map((appointment) => (
            <TableRow key={appointment.id} className="hover:bg-muted/50">
              <TableCell>
                <div className="flex items-center gap-3">
                  <Avatar className="w-8 h-8">
                    <AvatarImage
                      src={`/.jpg?height=32&width=32&query=${appointment.clientName}`}
                    />
                    <AvatarFallback className="text-xs">{getInitials(appointment.clientName)}</AvatarFallback>
                  </Avatar>
                  <div>
                    <p className="font-medium text-foreground">{appointment.clientName}</p>
                    <p className="text-xs text-muted-foreground">{appointment.clientEmail}</p>
                  </div>
                </div>
              </TableCell>
              <TableCell className="text-foreground">{getServiceName(appointment.serviceId)}</TableCell>
              <TableCell className="text-muted-foreground">{formatShortDate(appointment.date, locale)}</TableCell>
              <TableCell className="text-foreground font-medium">{appointment.time}</TableCell>
              <TableCell>{getStatusBadge(appointment.status)}</TableCell>
              <TableCell>
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button variant="ghost" size="icon" className="h-8 w-8">
                      <MoreHorizontal className="h-4 w-4" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    <DropdownMenuItem>{t("common.edit")}</DropdownMenuItem>
                    <DropdownMenuItem className="text-destructive">{t("common.cancel")}</DropdownMenuItem>
                  </DropdownMenuContent>
                </DropdownMenu>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  )
}
