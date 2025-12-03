"use client"

import { cn } from "@/lib/utils"
import { Check } from "lucide-react"
import { useTranslations } from 'next-intl'

interface BookingStepperProps {
  currentStep: number
}

export function BookingStepper({ currentStep }: BookingStepperProps) {
  const t = useTranslations()

  const steps = [
    { number: 1, label: t("booking.steps.service") },
    { number: 2, label: t("booking.steps.datetime") },
    { number: 3, label: t("booking.steps.details") },
  ]

  return (
    <div className="flex items-center justify-center gap-2 sm:gap-4">
      {steps.map((step, index) => (
        <div key={step.number} className="flex items-center">
          <div className="flex items-center gap-2">
            <div
              className={cn(
                "w-8 h-8 rounded-full flex items-center justify-center text-sm font-medium transition-colors",
                currentStep > step.number
                  ? "bg-primary text-primary-foreground"
                  : currentStep === step.number
                    ? "bg-primary text-primary-foreground"
                    : "bg-muted text-muted-foreground",
              )}
            >
              {currentStep > step.number ? <Check className="w-4 h-4" /> : step.number}
            </div>
            <span
              className={cn(
                "text-sm font-medium hidden sm:block",
                currentStep >= step.number ? "text-foreground" : "text-muted-foreground",
              )}
            >
              {step.label}
            </span>
          </div>
          {index < steps.length - 1 && (
            <div
              className={cn("w-8 sm:w-16 h-0.5 mx-2 sm:mx-4", currentStep > step.number ? "bg-primary" : "bg-muted")}
            />
          )}
        </div>
      ))}
    </div>
  )
}
