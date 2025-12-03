export interface Service {
  id: string
  name: string
  nameEn: string
  description: string
  descriptionEn: string
  duration: number
  price: number
  icon: string
}

export interface Appointment {
  id: string
  clientName: string
  clientEmail: string
  clientPhone: string
  serviceId: string
  date: Date
  time: string
  status: "confirmed" | "pending" | "cancelled" | "completed"
  notes?: string
}

export interface Testimonial {
  id: string
  name: string
  role: string
  roleEn: string
  content: string
  contentEn: string
  avatar: string
  rating: number
}

export interface Professional {
  id: string
  name: string
  title: string
  titleEn: string
  avatar: string
  rating: number
  reviewCount: number
}

export const services: Service[] = [
  {
    id: "1",
    name: "Corte de Cabello",
    nameEn: "Haircut",
    description: "Corte profesional adaptado a tu estilo",
    descriptionEn: "Professional cut adapted to your style",
    duration: 30,
    price: 25,
    icon: "scissors",
  },
  {
    id: "2",
    name: "Coloración Completa",
    nameEn: "Full Coloring",
    description: "Coloración profesional con productos premium",
    descriptionEn: "Professional coloring with premium products",
    duration: 90,
    price: 75,
    icon: "palette",
  },
  {
    id: "3",
    name: "Manicura",
    nameEn: "Manicure",
    description: "Cuidado completo de manos y uñas",
    descriptionEn: "Complete hand and nail care",
    duration: 45,
    price: 35,
    icon: "sparkles",
  },
  {
    id: "4",
    name: "Tratamiento Facial",
    nameEn: "Facial Treatment",
    description: "Limpieza e hidratación profunda",
    descriptionEn: "Deep cleansing and hydration",
    duration: 60,
    price: 55,
    icon: "heart",
  },
  {
    id: "5",
    name: "Masaje Relajante",
    nameEn: "Relaxing Massage",
    description: "Masaje corporal completo de relajación",
    descriptionEn: "Full body relaxation massage",
    duration: 60,
    price: 65,
    icon: "flower",
  },
]

const today = new Date()
const tomorrow = new Date(today)
tomorrow.setDate(tomorrow.getDate() + 1)
const dayAfter = new Date(today)
dayAfter.setDate(dayAfter.getDate() + 2)

export const appointments: Appointment[] = [
  {
    id: "1",
    clientName: "María García",
    clientEmail: "maria@email.com",
    clientPhone: "+34 612 345 678",
    serviceId: "1",
    date: today,
    time: "09:00",
    status: "confirmed",
  },
  {
    id: "2",
    clientName: "Carlos López",
    clientEmail: "carlos@email.com",
    clientPhone: "+34 623 456 789",
    serviceId: "2",
    date: today,
    time: "10:30",
    status: "confirmed",
  },
  {
    id: "3",
    clientName: "Ana Martínez",
    clientEmail: "ana@email.com",
    clientPhone: "+34 634 567 890",
    serviceId: "3",
    date: today,
    time: "14:00",
    status: "pending",
  },
  {
    id: "4",
    clientName: "Pedro Sánchez",
    clientEmail: "pedro@email.com",
    clientPhone: "+34 645 678 901",
    serviceId: "4",
    date: tomorrow,
    time: "09:30",
    status: "confirmed",
  },
  {
    id: "5",
    clientName: "Laura Fernández",
    clientEmail: "laura@email.com",
    clientPhone: "+34 656 789 012",
    serviceId: "5",
    date: tomorrow,
    time: "11:00",
    status: "confirmed",
  },
  {
    id: "6",
    clientName: "Miguel Torres",
    clientEmail: "miguel@email.com",
    clientPhone: "+34 667 890 123",
    serviceId: "1",
    date: tomorrow,
    time: "16:00",
    status: "pending",
  },
  {
    id: "7",
    clientName: "Isabel Ruiz",
    clientEmail: "isabel@email.com",
    clientPhone: "+34 678 901 234",
    serviceId: "2",
    date: dayAfter,
    time: "10:00",
    status: "confirmed",
  },
  {
    id: "8",
    clientName: "David González",
    clientEmail: "david@email.com",
    clientPhone: "+34 689 012 345",
    serviceId: "3",
    date: dayAfter,
    time: "12:30",
    status: "confirmed",
  },
  {
    id: "9",
    clientName: "Carmen Díaz",
    clientEmail: "carmen@email.com",
    clientPhone: "+34 690 123 456",
    serviceId: "4",
    date: dayAfter,
    time: "15:00",
    status: "pending",
  },
  {
    id: "10",
    clientName: "Javier Moreno",
    clientEmail: "javier@email.com",
    clientPhone: "+34 701 234 567",
    serviceId: "5",
    date: dayAfter,
    time: "17:30",
    status: "confirmed",
  },
]

export const testimonials: Testimonial[] = [
  {
    id: "1",
    name: "Elena Rodríguez",
    role: "Propietaria de Salón de Belleza",
    roleEn: "Beauty Salon Owner",
    content:
      "TurnoLink ha transformado completamente la gestión de mi salón. Ahora puedo dedicar más tiempo a mis clientes en lugar de gestionar citas.",
    contentEn:
      "TurnoLink has completely transformed how I manage my salon. Now I can spend more time with my clients instead of managing appointments.",
    avatar: "/professional-woman-portrait.png",
    rating: 5,
  },
  {
    id: "2",
    name: "Roberto Medina",
    role: "Fisioterapeuta",
    roleEn: "Physiotherapist",
    content:
      "Las notificaciones automáticas han reducido mis ausencias en un 70%. Una herramienta imprescindible para cualquier profesional.",
    contentEn: "Automatic notifications have reduced my no-shows by 70%. An essential tool for any professional.",
    avatar: "/man-doctor-portrait.png",
    rating: 5,
  },
  {
    id: "3",
    name: "Sofía Navarro",
    role: "Consultora de Negocios",
    roleEn: "Business Consultant",
    content:
      "La interfaz es intuitiva y mis clientes pueden reservar fácilmente. El mejor sistema de gestión de citas que he usado.",
    contentEn:
      "The interface is intuitive and my clients can book easily. The best appointment management system I've used.",
    avatar: "/business-woman-portrait.png",
    rating: 5,
  },
]

export const professional: Professional = {
  id: "1",
  name: "Studio María García",
  title: "Salón de Belleza & Spa",
  titleEn: "Beauty Salon & Spa",
  avatar: "/beauty-salon-logo-elegant.jpg",
  rating: 4.9,
  reviewCount: 127,
}

export const timeSlots = [
  "09:00",
  "09:30",
  "10:00",
  "10:30",
  "11:00",
  "11:30",
  "12:00",
  "12:30",
  "14:00",
  "14:30",
  "15:00",
  "15:30",
  "16:00",
  "16:30",
  "17:00",
  "17:30",
  "18:00",
  "18:30",
]

export const occupiedSlots: Record<string, string[]> = {
  [today.toISOString().split("T")[0]]: ["09:00", "10:30", "14:00"],
  [tomorrow.toISOString().split("T")[0]]: ["09:30", "11:00", "16:00"],
  [dayAfter.toISOString().split("T")[0]]: ["10:00", "12:30", "15:00"],
}
