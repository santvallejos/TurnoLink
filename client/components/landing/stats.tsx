"use client"

import { useTranslations } from 'next-intl'

export function Stats() {
  const t = useTranslations()

  const stats = [
    { value: "50K+", label: t("landing.stats.appointments") },
    { value: "2,500+", label: t("landing.stats.users") },
    { value: "99%", label: t("landing.stats.satisfaction") },
  ]

  return (
    <section className="py-16 bg-primary">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-3 gap-8">
          {stats.map((stat, index) => (
            <div key={index} className="text-center">
              <p className="text-3xl sm:text-4xl lg:text-5xl font-bold text-primary-foreground">{stat.value}</p>
              <p className="mt-2 text-sm sm:text-base text-primary-foreground/80">{stat.label}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  )
}
