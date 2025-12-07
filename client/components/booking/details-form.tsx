"use client"

import type React from "react"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Checkbox } from "@/components/ui/checkbox"
import { Loader2 } from "lucide-react"
import { useTranslations } from 'next-intl'

interface DetailsFormProps {
  onSubmit: (data: FormData) => void
  onBack: () => void
  isSubmitting: boolean
}

interface FormData {
  name: string
  email: string
  phone: string
  notes: string
  whatsapp: boolean
}

export function DetailsForm({ onSubmit, onBack, isSubmitting }: DetailsFormProps) {
  const t = useTranslations()
  const [formData, setFormData] = useState<FormData>({
    name: "",
    email: "",
    phone: "",
    notes: "",
    whatsapp: false,
  })

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSubmit(formData)
  }

  return (
    <div className="space-y-6">
      <div className="text-center">
        <h2 className="text-2xl font-bold text-foreground">{t("booking.step3.title")}</h2>
        <p className="text-muted-foreground mt-1">{t("booking.step3.subtitle")}</p>
      </div>

      <form className="space-y-4 max-w-md mx-auto" onSubmit={handleSubmit}>
        <div className="space-y-2">
          <Label htmlFor="name">{t("booking.step3.name")}</Label>
          <Input
            required
            disabled={isSubmitting}
            id="name"
            placeholder={t("booking.step3.namePlaceholder")}
            type="text"
            value={formData.name}
            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="email">{t("booking.step3.email")}</Label>
          <Input
            required
            disabled={isSubmitting}
            id="email"
            placeholder={t("booking.step3.emailPlaceholder")}
            type="email"
            value={formData.email}
            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="phone">{t("booking.step3.phone")}</Label>
          <Input
            required
            disabled={isSubmitting}
            id="phone"
            placeholder={t("booking.step3.phonePlaceholder")}
            type="tel"
            value={formData.phone}
            onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="notes">{t("booking.step3.notes")}</Label>
          <Textarea
            disabled={isSubmitting}
            id="notes"
            placeholder={t("booking.step3.notesPlaceholder")}
            rows={3}
            value={formData.notes}
            onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
          />
        </div>

        <div className="flex items-center gap-2">
          <Checkbox
            checked={formData.whatsapp}
            disabled={isSubmitting}
            id="whatsapp"
            onCheckedChange={(checked) => setFormData({ ...formData, whatsapp: checked as boolean })}
          />
          <Label className="text-sm text-muted-foreground cursor-pointer" htmlFor="whatsapp">
            {t("booking.step3.whatsapp")}
          </Label>
        </div>

        <div className="flex justify-between pt-4">
          <Button disabled={isSubmitting} size="lg" type="button" variant="outline" onClick={onBack}>
            {t("common.back")}
          </Button>
          <Button disabled={isSubmitting} size="lg" type="submit">
            {isSubmitting ? (
              <>
                <Loader2 className="w-4 h-4 mr-2 animate-spin" />
                {t("common.loading")}
              </>
            ) : (
              t("booking.step3.confirmBooking")
            )}
          </Button>
        </div>
      </form>
    </div>
  )
}
