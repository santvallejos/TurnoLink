using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnoLink.Business.Interfaces;
using TurnoLink.Business.DTOs;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace TurnoLink.Business.Services
{
    public class IcalDotnetService : IiCalDotnet
    {
        public async Task<string> CreateFileIcsBookingAsync(BookingDto booking)
        {
            try
            {
                // Crear un nuevo calendario
                var calendar = new Calendar
                {
                    ProductId = "-//TurnoLink//Booking Calendar//ES",
                    Version = "2.0",
                    Method = "REQUEST"
                };

                // Crear el evento
                var calendarEvent = new CalendarEvent
                {
                    Uid = booking.Id.ToString(),
                    Summary = $"Turno: {booking.ServiceName}",
                    Description = BuildDescription(booking),
                    Start = new CalDateTime(booking.StartTime),
                    End = new CalDateTime(booking.EndTime),
                    Location = booking.Location ?? string.Empty,
                    Created = new CalDateTime(booking.CreatedAt),
                    Status = booking.Status.ToUpper(),
                    Transparency = TransparencyType.Opaque
                };

                // Agregar información de contacto si está disponible
                if (!string.IsNullOrEmpty(booking.ClientEmail))
                {
                    calendarEvent.Organizer = new Organizer($"MAILTO:{booking.ClientEmail}")
                    {
                        CommonName = booking.ClientName
                    };
                }

                // Agregar el evento al calendario
                calendar.Events.Add(calendarEvent);

                // Serializar el calendario
                var serializer = new CalendarSerializer();
                return serializer.SerializeToString(calendar) ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string BuildDescription(BookingDto booking)
        {
            var description = $"Servicio: {booking.ServiceName}\n" +
                              $"Profesional: {booking.UserName}\n" +
                              $"Precio: ${booking.ServicePrice}\n";

            if (!string.IsNullOrEmpty(booking.Location))
                description += $"Dirección: {booking.Location}\n";

            if (!string.IsNullOrEmpty(booking.Notes))
                description += $"Notas: {booking.Notes}\n";

            return description;
        }
    }
}
