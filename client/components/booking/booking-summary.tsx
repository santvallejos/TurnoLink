"use client"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Separator } from "@/components/ui/separator"
import { Star, Clock, CalendarDays } from "lucide-react"
import { professional, type Service } from "@/lib/mock-data"
import { useTranslations, useLocale } from 'next-intl'
import { formatCurrency, formatDate } from '@/lib/i18n'

interface BookingSummaryProps {
  selectedService: Service | null
  selectedDate: Date | undefined
  selectedTime: string | null
}

export function BookingSummary({ selectedService, selectedDate, selectedTime }: BookingSummaryProps) {
  const t = useTranslations()
  const locale = useLocale()

  return (
    <Card className="border-border sticky top-4">
      <CardHeader className="pb-4">
        <CardTitle className="text-lg">{t("booking.summary.title")}</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        {/* Professional info */}
        <div className="flex items-center gap-3">
          <Avatar className="w-12 h-12">
            <AvatarImage src="/beauty-salon-logo-elegant.jpg" alt={professional.name} />
            <AvatarFallback>MG</AvatarFallback>
          </Avatar>
          <div>
            <p className="font-semibold text-foreground">{professional.name}</p>
            <p className="text-sm text-muted-foreground">
              {locale === "es" ? professional.title : professional.titleEn}
            </p>
            <div className="flex items-center gap-1 mt-0.5">
              <Star className="w-3 h-3 fill-yellow-400 text-yellow-400" />
              <span className="text-xs text-muted-foreground">
                {professional.rating} ({professional.reviewCount})
              </span>
            </div>
          </div>
        </div>

        <Separator />

        {/* Booking details */}
        <div className="space-y-3">
          {selectedService && (
            <div className="flex justify-between">
              <span className="text-sm text-muted-foreground">{t("booking.summary.service")}</span>
              <span className="text-sm font-medium text-foreground">
                {locale === "es" ? selectedService.name : selectedService.nameEn}
              </span>
            </div>
          )}

          {selectedDate && (
            <div className="flex justify-between items-center">
              <span className="text-sm text-muted-foreground">{t("booking.summary.date")}</span>
              <div className="flex items-center gap-1">
                <CalendarDays className="w-3 h-3 text-muted-foreground" />
                <span className="text-sm font-medium text-foreground">{formatDate(selectedDate, locale)}</span>
              </div>
            </div>
          )}

          {selectedTime && (
            <div className="flex justify-between items-center">
              <span className="text-sm text-muted-foreground">{t("booking.summary.time")}</span>
              <div className="flex items-center gap-1">
                <Clock className="w-3 h-3 text-muted-foreground" />
                <span className="text-sm font-medium text-foreground">{selectedTime}</span>
              </div>
            </div>
          )}

          {selectedService && (
            <div className="flex justify-between items-center">
              <span className="text-sm text-muted-foreground">{t("booking.step1.duration")}</span>
              <span className="text-sm text-foreground">
                {selectedService.duration} {t("booking.step1.minutes")}
              </span>
            </div>
          )}
        </div>

        {selectedService && (
          <>
            <Separator />
            <div className="flex justify-between items-center">
              <span className="font-semibold text-foreground">{t("booking.summary.total")}</span>
              <span className="text-xl font-bold text-primary">{formatCurrency(selectedService.price, locale)}</span>
            </div>
          </>
        )}
      </CardContent>
    </Card>
  )
}
