import type React from "react"
import { SidebarInset, SidebarProvider } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/dashboard/app-sidebar"
import { DashboardHeader } from "@/components/dashboard/dashboard-header"
import type { Locale } from "@/lib/i18n"

interface LayoutProps {
  children: React.ReactNode
  params: Promise<{ locale: Locale }>
}

export default async function DashboardLayout({ children, params }: LayoutProps) {
  const { locale } = await params
  const validLocale: Locale = locale === "en" ? "en" : "es"

  return (
    <SidebarProvider>
      <AppSidebar locale={validLocale} />
      <SidebarInset>
        <DashboardHeader locale={validLocale} />
        <main className="flex-1 p-4 md:p-6 bg-background">{children}</main>
      </SidebarInset>
    </SidebarProvider>
  )
}
