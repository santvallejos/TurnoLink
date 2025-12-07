"use client"

import Link from "next/link"
import { Button } from "@/components/ui/button"
import { Card, CardContent } from "@/components/ui/card"
import { ArrowRight, CheckCircle2 } from "lucide-react"
import { useLocale, useTranslations } from 'next-intl'

export function BookingConfirmation() {
  const t = useTranslations()
  const locale = useLocale()

  return (
    <div className="flex items-center justify-center min-h-[400px]">
      <Card className="max-w-md w-full border-border">
        <CardContent className="p-8 text-center">
          <div className="w-16 h-16 rounded-full bg-green-100 dark:bg-green-900/30 flex items-center justify-center mx-auto mb-6">
            <CheckCircle2 className="w-8 h-8 text-green-600 dark:text-green-400" />
          </div>
          <h2 className="text-2xl font-bold text-foreground mb-2">{t("booking.confirmation.title")}</h2>
          <p className="text-muted-foreground mb-6">{t("booking.confirmation.message")}</p>
          <div className="flex flex-col sm:flex-row gap-3 justify-center">
            <Link href={`/${locale}`}>
              <Button className="w-full sm:w-auto bg-transparent" variant="outline">
                {locale === "es" ? "Volver al inicio" : "Back to home"}
              </Button>
            </Link>
            <Link href={`/${locale}/book/1`}>
              <Button className="w-full sm:w-auto gap-2">
                {locale === "es" ? "Nueva reserva" : "New booking"}
                <ArrowRight className="w-4 h-4" />
              </Button>
            </Link>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}
