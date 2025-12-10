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
                var calendar = new Calendar();
                
                // Configurar propiedades del calendario
                calendar.ProductId = "-//TurnoLink//Booking Calendar//ES";
                calendar.Version = "2.0";
                calendar.Method = "REQUEST";

                // Crear el evento
                var calendarEvent = new CalendarEvent
                {
                    Uid = booking.Id.ToString(),
                    Summary = $"Turno: {booking.ServiceName}",
                    Description = $"Servicio: {booking.ServiceName}\n" +
                                  $"Profesional: {booking.UserName}\n" +
                                  $"Precio: ${booking.ServicePrice}\n" +
                                  (string.IsNullOrEmpty(booking.Notes) ? "" : $"Notas: {booking.Notes}\n"),
                    Start = new CalDateTime(booking.StartTime),
                    End = new CalDateTime(booking.EndTime),
                    // Location = "Service location",
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

                // Agregar recordatorio (15 minutos antes)
                //calendarEvent.Alarms.Add(new Alarm
                //{
                    //Trigger = new Trigger(new Ical.Net.DataTypes.Duration(0, 0, -15, 0)),
                    //Action = "DISPLAY",
                    //Description = $"Recordatorio: Reserva de {booking.ServiceName} en 15 minutos"
                //});

                // Agregar el evento al calendario
                calendar.Events.Add(calendarEvent);

                // Serializar el calendario
                var serializer = new CalendarSerializer();
                return serializer.SerializeToString(calendar);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
