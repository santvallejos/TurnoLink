"use client"

import esMessages from "@/messages/es.json"
import enMessages from "@/messages/en.json"

export type Locale = "es" | "en"

export const locales: Locale[] = ["es", "en"]
export const defaultLocale: Locale = "es"

const messages = {
  es: esMessages,
  en: enMessages,
}

export function getMessages(locale: Locale) {
  return messages[locale] || messages[defaultLocale]
}

export function getNestedValue(obj: Record<string, unknown>, path: string): string {
  const keys = path.split(".")
  let current: unknown = obj

  for (const key of keys) {
    if (current && typeof current === "object" && key in current) {
      current = (current as Record<string, unknown>)[key]
    } else {
      return path
    }
  }

  return typeof current === "string" ? current : path
}

export function useTranslations(locale: Locale) {
  const msgs = getMessages(locale)

  return function t(key: string): string {
    return getNestedValue(msgs as Record<string, unknown>, key)
  }
}

export function formatDate(date: Date, locale: Locale): string {
  return new Intl.DateTimeFormat(locale === "es" ? "es-ES" : "en-US", {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric",
  }).format(date)
}

export function formatTime(date: Date, locale: Locale): string {
  return new Intl.DateTimeFormat(locale === "es" ? "es-ES" : "en-US", {
    hour: "2-digit",
    minute: "2-digit",
  }).format(date)
}

export function formatShortDate(date: Date, locale: Locale): string {
  return new Intl.DateTimeFormat(locale === "es" ? "es-ES" : "en-US", {
    month: "short",
    day: "numeric",
  }).format(date)
}

export function formatCurrency(amount: number, locale: Locale): string {
  return new Intl.NumberFormat(locale === "es" ? "es-ES" : "en-US", {
    style: "currency",
    currency: "EUR",
  }).format(amount)
}
