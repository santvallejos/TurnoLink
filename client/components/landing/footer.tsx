"use client"

import Link from "next/link"
import { Calendar } from "lucide-react"
import { useTranslations, useLocale } from 'next-intl'

export function Footer() {
  const t = useTranslations()
  const locale = useLocale()

  const footerLinks = {
    product: [
      { label: t("landing.footer.features"), href: "#features" },
      { label: t("landing.footer.pricing"), href: "#pricing" },
      { label: t("landing.footer.integrations"), href: "#" },
    ],
    company: [
      { label: t("landing.footer.about"), href: "#" },
      { label: t("landing.footer.blog"), href: "#" },
      { label: t("landing.footer.careers"), href: "#" },
    ],
    legal: [
      { label: t("landing.footer.privacy"), href: "#" },
      { label: t("landing.footer.terms"), href: "#" },
      { label: t("landing.footer.cookies"), href: "#" },
    ],
  }

  return (
    <footer className="bg-card border-t border-border">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 lg:py-16">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-8 lg:gap-12">
          {/* Brand */}
          <div className="col-span-2 md:col-span-1">
            <Link href={`/${locale}`} className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-primary flex items-center justify-center">
                <Calendar className="w-5 h-5 text-primary-foreground" />
              </div>
              <span className="font-bold text-xl text-foreground">{t("common.appName")}</span>
            </Link>
            <p className="mt-4 text-sm text-muted-foreground">
              {t('landing.footer.description')}
            </p>
          </div>

          {/* Product */}
          <div>
            <h3 className="font-semibold text-foreground mb-4">{t("landing.footer.product")}</h3>
            <ul className="space-y-3">
              {footerLinks.product.map((link, index) => (
                <li key={index}>
                  <Link
                    href={link.href}
                    className="text-sm text-muted-foreground hover:text-foreground transition-colors"
                  >
                    {link.label}
                  </Link>
                </li>
              ))}
            </ul>
          </div>

          {/* Company */}
          <div>
            <h3 className="font-semibold text-foreground mb-4">{t("landing.footer.company")}</h3>
            <ul className="space-y-3">
              {footerLinks.company.map((link, index) => (
                <li key={index}>
                  <Link
                    href={link.href}
                    className="text-sm text-muted-foreground hover:text-foreground transition-colors"
                  >
                    {link.label}
                  </Link>
                </li>
              ))}
            </ul>
          </div>

          {/* Legal */}
          <div>
            <h3 className="font-semibold text-foreground mb-4">{t("landing.footer.legal")}</h3>
            <ul className="space-y-3">
              {footerLinks.legal.map((link, index) => (
                <li key={index}>
                  <Link
                    href={link.href}
                    className="text-sm text-muted-foreground hover:text-foreground transition-colors"
                  >
                    {link.label}
                  </Link>
                </li>
              ))}
            </ul>
          </div>
        </div>

        <div className="mt-12 pt-8 border-t border-border">
          <p className="text-sm text-muted-foreground text-center">
            &copy; {new Date().getFullYear()} TurnoLink. {t("landing.footer.rights")}
          </p>
        </div>
      </div>
    </footer>
  )
}
