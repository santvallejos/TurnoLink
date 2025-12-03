"use client"

import { cn } from "@/lib/utils"
import { appointments, services } from "@/lib/mock-data"
import { useLocale } from 'next-intl'

export function WeeklyCalendar() {
  const locale = useLocale()
  const today = new Date()
  const weekDays = []

  // Get current week starting from today
  for (let i = 0; i < 7; i++) {
    const date = new Date(today)
    date.setDate(today.getDate() + i)
    weekDays.push(date)
  }

  const dayNames =
    locale === "es"
      ? ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"]
      : ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]

  const hours = ["09:00", "10:00", "11:00", "12:00", "14:00", "15:00", "16:00", "17:00", "18:00"]

  const getAppointmentsForDay = (date: Date) => {
    return appointments.filter((apt) => {
      const aptDate = new Date(apt.date)
      return aptDate.toDateString() === date.toDateString()
    })
  }

  const getServiceName = (serviceId: string) => {
    const service = services.find((s) => s.id === serviceId)
    return service ? (locale === "es" ? service.name : service.nameEn) : ""
  }

  return (
    <div className="rounded-lg border border-border bg-card overflow-hidden">
      {/* Header with days */}
      <div className="grid grid-cols-8 border-b border-border">
        <div className="p-3 text-xs font-medium text-muted-foreground"></div>
        {weekDays.map((date, index) => {
          const isToday = date.toDateString() === today.toDateString()
          return (
            <div key={index} className={cn("p-3 text-center border-l border-border", isToday && "bg-primary/5")}>
              <p className="text-xs text-muted-foreground">{dayNames[date.getDay()]}</p>
              <p className={cn("text-lg font-semibold", isToday ? "text-primary" : "text-foreground")}>
                {date.getDate()}
              </p>
            </div>
          )
        })}
      </div>

      {/* Time slots */}
      <div className="max-h-[300px] overflow-y-auto">
        {hours.map((hour) => (
          <div key={hour} className="grid grid-cols-8 border-b border-border last:border-b-0">
            <div className="p-2 text-xs text-muted-foreground flex items-start justify-end pr-3">{hour}</div>
            {weekDays.map((date, dayIndex) => {
              const dayAppointments = getAppointmentsForDay(date)
              const appointment = dayAppointments.find((apt) => apt.time === hour)
              const isToday = date.toDateString() === today.toDateString()

              return (
                <div
                  key={dayIndex}
                  className={cn("p-1 border-l border-border min-h-[50px]", isToday && "bg-primary/5")}
                >
                  {appointment && (
                    <div
                      className={cn(
                        "rounded px-2 py-1 text-xs truncate",
                        appointment.status === "confirmed"
                          ? "bg-primary/20 text-primary border border-primary/30"
                          : "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/30 dark:text-yellow-400 border border-yellow-200 dark:border-yellow-800",
                      )}
                    >
                      <p className="font-medium truncate">{appointment.clientName.split(" ")[0]}</p>
                      <p className="truncate opacity-80">{getServiceName(appointment.serviceId)}</p>
                    </div>
                  )}
                </div>
              )
            })}
          </div>
        ))}
      </div>
    </div>
  )
}
