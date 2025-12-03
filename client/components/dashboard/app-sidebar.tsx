"use client"

import Link from "next/link"
import { usePathname } from "next/navigation"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Calendar, LayoutDashboard, Users, Scissors, Settings, LogOut, ChevronUp } from "lucide-react"
import { useTranslations, useLocale } from 'next-intl'

export function AppSidebar() {
  const t = useTranslations()
  const locale = useLocale()
  const pathname = usePathname()

  const navItems = [
    {
      title: t("dashboard.nav.dashboard"),
      href: `/${locale}/dashboard`,
      icon: LayoutDashboard,
    },
    {
      title: t("dashboard.nav.appointments"),
      href: `/${locale}/dashboard/appointments`,
      icon: Calendar,
    },
    {
      title: t("dashboard.nav.clients"),
      href: `/${locale}/dashboard/clients`,
      icon: Users,
    },
    {
      title: t("dashboard.nav.services"),
      href: `/${locale}/dashboard/services`,
      icon: Scissors,
    },
    {
      title: t("dashboard.nav.settings"),
      href: `/${locale}/dashboard/settings`,
      icon: Settings,
    },
  ]

  return (
    <Sidebar variant="inset">
      <SidebarHeader className="border-b border-sidebar-border">
        <Link href={`/${locale}/dashboard`} className="flex items-center gap-2 px-2 py-1">
          <div className="w-8 h-8 rounded-lg bg-sidebar-primary flex items-center justify-center">
            <Calendar className="w-5 h-5 text-sidebar-primary-foreground" />
          </div>
          <span className="font-bold text-lg text-sidebar-foreground">{t("common.appName")}</span>
        </Link>
      </SidebarHeader>

      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupContent>
            <SidebarMenu>
              {navItems.map((item) => (
                <SidebarMenuItem key={item.href}>
                  <SidebarMenuButton asChild isActive={pathname === item.href} tooltip={item.title}>
                    <Link href={item.href}>
                      <item.icon className="w-4 h-4" />
                      <span>{item.title}</span>
                    </Link>
                  </SidebarMenuButton>
                </SidebarMenuItem>
              ))}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter className="border-t border-sidebar-border">
        <SidebarMenu>
          <SidebarMenuItem>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <SidebarMenuButton className="w-full">
                  <Avatar className="w-6 h-6">
                    <AvatarImage src="/beauty-salon-logo-elegant.jpg" alt="User" />
                    <AvatarFallback>MG</AvatarFallback>
                  </Avatar>
                  <div className="flex-1 text-left">
                    <p className="text-sm font-medium text-sidebar-foreground">María García</p>
                    <p className="text-xs text-muted-foreground">Studio María</p>
                  </div>
                  <ChevronUp className="w-4 h-4" />
                </SidebarMenuButton>
              </DropdownMenuTrigger>
              <DropdownMenuContent side="top" align="start" className="w-56">
                <DropdownMenuItem>
                  <Settings className="w-4 h-4 mr-2" />
                  {t("dashboard.nav.settings")}
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                <DropdownMenuItem asChild>
                  <Link href={`/${locale}`}>
                    <LogOut className="w-4 h-4 mr-2" />
                    {t("common.logout")}
                  </Link>
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
    </Sidebar>
  )
}
