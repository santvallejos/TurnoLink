"use client"

import Image from "next/image"
import { Card, CardContent } from "@/components/ui/card"
import { Star } from "lucide-react"
import { testimonials } from "@/lib/mock-data"
import { useTranslations, useLocale } from 'next-intl'

export function Testimonials() {
  const t = useTranslations()
  const locale = useLocale()

  return (
    <section id="testimonials" className="py-20 lg:py-32">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center max-w-3xl mx-auto mb-16">
          <h2 className="text-3xl sm:text-4xl font-bold text-foreground text-balance">
            {t("landing.testimonials.title")}
          </h2>
          <p className="mt-4 text-lg text-muted-foreground text-pretty">{t("landing.testimonials.subtitle")}</p>
        </div>

        <div className="grid md:grid-cols-3 gap-6 lg:gap-8">
          {testimonials.map((testimonial, index) => (
            <Card key={testimonial.id} className="bg-card border-border">
              <CardContent className="p-6">
                <div className="flex gap-1 mb-4">
                  {[...Array(testimonial.rating)].map((_, i) => (
                    <Star key={i} className="w-5 h-5 fill-yellow-400 text-yellow-400" />
                  ))}
                </div>
                <p className="text-foreground mb-6 text-pretty">
                  &ldquo;{locale === "es" ? testimonial.content : testimonial.contentEn}&rdquo;
                </p>
                <div className="flex items-center gap-3">
                  <Image
                    src={
                      index === 0
                        ? "/professional-woman-portrait.png"
                        : index === 1
                          ? "/man-doctor-portrait.png"
                          : "/business-woman-portrait.png"
                    }
                    alt={testimonial.name}
                    width={48}
                    height={48}
                    className="rounded-full object-cover"
                  />
                  <div>
                    <p className="font-semibold text-foreground">{testimonial.name}</p>
                    <p className="text-sm text-muted-foreground">
                      {locale === "es" ? testimonial.role : testimonial.roleEn}
                    </p>
                  </div>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>
    </section>
  )
}
