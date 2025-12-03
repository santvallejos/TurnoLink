"use client"

import type React from "react"

import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Scissors, Palette, Sparkles, Heart, Flower2, Check } from "lucide-react"
import { services, type Service } from "@/lib/mock-data"
import { cn } from "@/lib/utils"
import { useTranslations, useLocale } from 'next-intl'
import { formatCurrency } from '@/lib/i18n'

interface ServiceSelectionProps {
  selectedService: Service | null
  onSelectService: (service: Service) => void
  onNext: () => void
}

const iconMap: Record<string, React.ComponentType<{ className?: string }>> = {
  scissors: Scissors,
  palette: Palette,
  sparkles: Sparkles,
  heart: Heart,
  flower: Flower2,
}

export function ServiceSelection({ selectedService, onSelectService, onNext }: ServiceSelectionProps) {
  const t = useTranslations()
  const locale = useLocale()

  return (
    <div className="space-y-6">
      <div className="text-center">
        <h2 className="text-2xl font-bold text-foreground">{t("booking.step1.title")}</h2>
        <p className="text-muted-foreground mt-1">{t("booking.step1.subtitle")}</p>
      </div>

      <div className="grid sm:grid-cols-2 gap-4">
        {services.map((service) => {
          const Icon = iconMap[service.icon] || Scissors
          const isSelected = selectedService?.id === service.id

          return (
            <Card
              key={service.id}
              className={cn(
                "cursor-pointer transition-all hover:shadow-md",
                isSelected ? "ring-2 ring-primary border-primary" : "border-border hover:border-primary/50",
              )}
              onClick={() => onSelectService(service)}
            >
              <CardContent className="p-4">
                <div className="flex items-start justify-between">
                  <div className="flex items-start gap-3">
                    <div
                      className={cn(
                        "w-10 h-10 rounded-lg flex items-center justify-center",
                        isSelected ? "bg-primary text-primary-foreground" : "bg-primary/10 text-primary",
                      )}
                    >
                      <Icon className="w-5 h-5" />
                    </div>
                    <div>
                      <h3 className="font-semibold text-foreground">
                        {locale === "es" ? service.name : service.nameEn}
                      </h3>
                      <p className="text-sm text-muted-foreground mt-0.5">
                        {locale === "es" ? service.description : service.descriptionEn}
                      </p>
                      <div className="flex items-center gap-3 mt-2">
                        <span className="text-xs text-muted-foreground">
                          {service.duration} {t("booking.step1.minutes")}
                        </span>
                        <span className="text-sm font-semibold text-primary">
                          {formatCurrency(service.price, locale)}
                        </span>
                      </div>
                    </div>
                  </div>
                  {isSelected && (
                    <div className="w-6 h-6 rounded-full bg-primary flex items-center justify-center">
                      <Check className="w-4 h-4 text-primary-foreground" />
                    </div>
                  )}
                </div>
              </CardContent>
            </Card>
          )
        })}
      </div>

      <div className="flex justify-end">
        <Button onClick={onNext} disabled={!selectedService} size="lg">
          {t("common.next")}
        </Button>
      </div>
    </div>
  )
}
