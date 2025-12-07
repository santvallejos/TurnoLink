"use client"

import Link from "next/link"
import { Button } from "@/components/ui/button"
import { LanguageSwitcher } from "@/components/language-switcher"
import { ThemeToggle } from "@/components/theme-toggle"
import { Calendar, Menu, X } from "lucide-react"
import { useState } from "react"
import { useLocale, useTranslations } from 'next-intl'

export function Navbar() {
  const [isMenuOpen, setIsMenuOpen] = useState(false)
  const t = useTranslations()
  const locale = useLocale()

  return (
    <nav className="fixed top-0 left-0 right-0 z-50 bg-background/80 backdrop-blur-lg border-b border-border">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link className="flex items-center gap-2" href={`/${locale}`}>
            <div className="w-8 h-8 rounded-lg bg-primary flex items-center justify-center">
              <Calendar className="w-5 h-5 text-primary-foreground" />
            </div>
            <span className="font-bold text-xl text-foreground">{t("common.appName")}</span>
          </Link>

          {/* Desktop Navigation */}
          <div className="hidden md:flex items-center gap-6">
            <Link
              className="text-muted-foreground hover:text-foreground transition-colors"
              href={`/${locale}#features`}
            >
              {t("landing.footer.features")}
            </Link>
            <Link
              className="text-muted-foreground hover:text-foreground transition-colors"
              href={`/${locale}#testimonials`}
            >
              {t("landing.testimonials.title")}
            </Link>
            <Link className="text-muted-foreground hover:text-foreground transition-colors" href={`/${locale}#pricing`}>
              {t("landing.footer.pricing")}
            </Link>
          </div>

          {/* Right side */}
          <div className="hidden md:flex items-center gap-3">
            <LanguageSwitcher />
            <ThemeToggle />
            <Link href={`/${locale}/login`}>
              <Button variant="ghost">{t("common.login")}</Button>
            </Link>
            <Link href={`/${locale}/register`}>
              <Button>{t("common.getStarted")}</Button>
            </Link>
          </div>

          {/* Mobile menu button */}
          <div className="flex md:hidden items-center gap-2">
            <LanguageSwitcher />
            <ThemeToggle />
            <Button aria-label="Toggle menu" size="icon" variant="ghost" onClick={() => setIsMenuOpen(!isMenuOpen)}>
              {isMenuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
            </Button>
          </div>
        </div>

        {/* Mobile Navigation */}
        {isMenuOpen && (
          <div className="md:hidden py-4 border-t border-border">
            <div className="flex flex-col gap-4">
              <Link
                className="text-muted-foreground hover:text-foreground transition-colors"
                href={`/${locale}#features`}
                onClick={() => setIsMenuOpen(false)}
              >
                {t("landing.footer.features")}
              </Link>
              <Link
                className="text-muted-foreground hover:text-foreground transition-colors"
                href={`/${locale}#testimonials`}
                onClick={() => setIsMenuOpen(false)}
              >
                {t("landing.testimonials.title")}
              </Link>
              <Link
                className="text-muted-foreground hover:text-foreground transition-colors"
                href={`/${locale}#pricing`}
                onClick={() => setIsMenuOpen(false)}
              >
                {t("landing.footer.pricing")}
              </Link>
              <div className="flex flex-col gap-2 pt-4 border-t border-border">
                <Link href={`/${locale}/login`}>
                  <Button className="w-full" variant="ghost">
                    {t("common.login")}
                  </Button>
                </Link>
                <Link href={`/${locale}/register`}>
                  <Button className="w-full">{t("common.getStarted")}</Button>
                </Link>
              </div>
            </div>
          </div>
        )}
      </div>
    </nav>
  )
}
