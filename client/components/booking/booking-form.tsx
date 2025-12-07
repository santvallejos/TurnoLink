"use client"

import { useState } from "react"
import { BookingStepper } from "./booking-stepper"
import { ServiceSelection } from "./service-selection"
import { DateTimeSelection } from "./datetime-selection"
import { DetailsForm } from "./details-form"
import { BookingSummary } from "./booking-summary"
import { BookingConfirmation } from "./booking-confirmation"
import type { Service } from "@/lib/mock-data"
import { useLocale } from 'next-intl'

export function BookingForm() {
  const locale = useLocale()
  const [currentStep, setCurrentStep] = useState(1)
  const [selectedService, setSelectedService] = useState<Service | null>(null)
  const [selectedDate, setSelectedDate] = useState<Date | undefined>(undefined)
  const [selectedTime, setSelectedTime] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isConfirmed, setIsConfirmed] = useState(false)

  const handleSubmit = async () => {
    setIsSubmitting(true)
    // Simulate API call
    await new Promise((resolve) => setTimeout(resolve, 2000))
    setIsSubmitting(false)
    setIsConfirmed(true)
  }

  if (isConfirmed) {
    return <BookingConfirmation />
  }

  return (
    <div className="grid lg:grid-cols-3 gap-8">
      {/* Main content */}
      <div className="lg:col-span-2 space-y-8">
        <BookingStepper currentStep={currentStep} />

        {currentStep === 1 && (
          <ServiceSelection
            selectedService={selectedService}
            onNext={() => setCurrentStep(2)}
            onSelectService={setSelectedService}
          />
        )}

        {currentStep === 2 && (
          <DateTimeSelection
            selectedDate={selectedDate}
            selectedTime={selectedTime}
            onBack={() => setCurrentStep(1)}
            onNext={() => setCurrentStep(3)}
            onSelectDate={setSelectedDate}
            onSelectTime={setSelectedTime}
          />
        )}

        {currentStep === 3 && (
          <DetailsForm
            isSubmitting={isSubmitting}
            onBack={() => setCurrentStep(2)}
            onSubmit={handleSubmit}
          />
        )}
      </div>

      {/* Sidebar summary */}
      <div className="hidden lg:block">
        <BookingSummary
          selectedDate={selectedDate}
          selectedService={selectedService}
          selectedTime={selectedTime}
        />
      </div>
    </div>
  )
}
