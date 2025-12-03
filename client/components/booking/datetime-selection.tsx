"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Calendar } from "@/components/ui/calendar"
import { Card, CardContent } from "@/components/ui/card"
import { timeSlots, occupiedSlots } from "@/lib/mock-data"
import { cn } from "@/lib/utils"
import { useTranslations, useLocale } from 'next-intl'

interface DateTimeSelectionProps {
  selectedDate: Date | undefined
  selectedTime: string | null
  onSelectDate: (date: Date | undefined) => void
  onSelectTime: (time: string) => void
  onNext: () => void
  onBack: () => void
}

export function DateTimeSelection({
  selectedDate,
  selectedTime,
  onSelectDate,
  onSelectTime,
  onNext,
  onBack,
}: DateTimeSelectionProps) {
  const t = useTranslations()
  const locale = useLocale()
  const [month, setMonth] = useState<Date>(new Date())

  const getOccupiedSlots = (date: Date | undefined) => {
    if (!date) return []
    const dateKey = date.toISOString().split("T")[0]
    return occupiedSlots[dateKey] || []
  }

  const occupied = getOccupiedSlots(selectedDate)

  // Disable past dates
  const disabledDays = { before: new Date() }

  return (
    <div className="space-y-6">
      <div className="text-center">
        <h2 className="text-2xl font-bold text-foreground">{t("booking.step2.title")}</h2>
        <p className="text-muted-foreground mt-1">{t("booking.step2.subtitle")}</p>
      </div>

      <div className="grid md:grid-cols-2 gap-6">
        {/* Calendar */}
        <Card className="border-border">
          <CardContent className="p-4">
            <p className="text-sm font-medium text-foreground mb-3">{t("booking.step2.selectDate")}</p>
            <Calendar
              mode="single"
              selected={selectedDate}
              onSelect={onSelectDate}
              month={month}
              onMonthChange={setMonth}
              disabled={disabledDays}
              className="rounded-md"
              locale={locale === "es" ? undefined : undefined}
            />
          </CardContent>
        </Card>

        {/* Time slots */}
        <Card className="border-border">
          <CardContent className="p-4">
            <p className="text-sm font-medium text-foreground mb-3">{t("booking.step2.selectTime")}</p>
            {selectedDate ? (
              <div className="grid grid-cols-3 gap-2">
                {timeSlots.map((time) => {
                  const isOccupied = occupied.includes(time)
                  const isSelected = selectedTime === time

                  return (
                    <Button
                      key={time}
                      variant={isSelected ? "default" : "outline"}
                      size="sm"
                      disabled={isOccupied}
                      onClick={() => onSelectTime(time)}
                      className={cn(
                        "transition-all",
                        isOccupied && "opacity-50 cursor-not-allowed line-through",
                        isSelected && "ring-2 ring-primary ring-offset-2",
                      )}
                    >
                      {time}
                    </Button>
                  )
                })}
              </div>
            ) : (
              <div className="flex items-center justify-center h-48 text-muted-foreground">
                {locale === "es" ? "Selecciona una fecha primero" : "Select a date first"}
              </div>
            )}

            <div className="flex items-center gap-4 mt-4 pt-4 border-t border-border">
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded bg-primary" />
                <span className="text-xs text-muted-foreground">{t("booking.step2.available")}</span>
              </div>
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded bg-muted" />
                <span className="text-xs text-muted-foreground">{t("booking.step2.unavailable")}</span>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="flex justify-between">
        <Button variant="outline" onClick={onBack} size="lg">
          {t("common.back")}
        </Button>
        <Button onClick={onNext} disabled={!selectedDate || !selectedTime} size="lg">
          {t("common.next")}
        </Button>
      </div>
    </div>
  )
}
