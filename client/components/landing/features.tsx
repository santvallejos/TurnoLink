"use client"

import { BarChart3, Bell, Calendar, Shield, Smartphone, Users } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"
import { useTranslations } from 'next-intl'

export function Features() {
  const t = useTranslations()

  const features = [
    {
      icon: Calendar,
      title: t("landing.features.calendar.title"),
      description: t("landing.features.calendar.description"),
    },
    {
      icon: Bell,
      title: t("landing.features.notifications.title"),
      description: t("landing.features.notifications.description"),
    },
    {
      icon: Users,
      title: t("landing.features.clients.title"),
      description: t("landing.features.clients.description"),
    },
    {
      icon: BarChart3,
      title: t("landing.features.analytics.title"),
      description: t("landing.features.analytics.description"),
    },
    {
      icon: Smartphone,
      title: t("landing.features.mobile.title"),
      description: t("landing.features.mobile.description"),
    },
    {
      icon: Shield,
      title: t("landing.features.security.title"),
      description: t("landing.features.security.description"),
    },
  ]

  return (
    <section className="py-20 lg:py-32 bg-muted/30" id="features">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center max-w-3xl mx-auto mb-16">
          <h2 className="text-3xl sm:text-4xl font-bold text-foreground text-balance">{t("landing.features.title")}</h2>
          <p className="mt-4 text-lg text-muted-foreground text-pretty">{t("landing.features.subtitle")}</p>
        </div>

        <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-6 lg:gap-8">
          {features.map((feature, index) => (
            <Card
              key={index}
              className="group bg-card hover:shadow-lg transition-all duration-300 border-border hover:border-primary/30"
            >
              <CardContent className="p-6">
                <div className="w-12 h-12 rounded-xl bg-primary/10 flex items-center justify-center mb-4 group-hover:bg-primary/20 transition-colors">
                  <feature.icon className="w-6 h-6 text-primary" />
                </div>
                <h3 className="text-lg font-semibold text-foreground mb-2">{feature.title}</h3>
                <p className="text-muted-foreground">{feature.description}</p>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>
    </section>
  )
}
