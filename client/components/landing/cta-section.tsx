"use client"

import Link from "next/link"
import { Button } from "@/components/ui/button"
import { ArrowRight } from "lucide-react"
import { useTranslations, useLocale } from 'next-intl'

export function CTASection() {
  const t = useTranslations()
  const locale = useLocale()

  return (
    <section className="py-20 lg:py-32 bg-muted/30">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
        <h2 className="text-3xl sm:text-4xl font-bold text-foreground text-balance">{t("landing.cta.title")}</h2>
        <p className="mt-4 text-lg text-muted-foreground max-w-2xl mx-auto text-pretty">{t("landing.cta.subtitle")}</p>
        <div className="mt-10">
          <Link href={`/${locale}/register`}>
            <Button size="lg" className="gap-2 text-base">
              {t("landing.cta.button")}
              <ArrowRight className="w-4 h-4" />
            </Button>
          </Link>
        </div>
      </div>
    </section>
  )
}
