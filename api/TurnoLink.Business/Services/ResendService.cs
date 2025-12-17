using System;
using System.Text;
using Resend;
using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Services
{
    public class ResendService
    {
        private readonly IResend _resendClient;

        public ResendService(IResend resendClient)
        {
            _resendClient = resendClient;
        }

        /// <summary>
        /// Sends booking confirmation email to the client
        /// </summary>
        public async Task SendClientConfirmationEmailAsync(BookingDto booking, string stringIcs)
        {
            var icsBytes = Encoding.UTF8.GetBytes(stringIcs);
            var icsBase64 = Convert.ToBase64String(icsBytes);

            var htmlContent = GenerateClientConfirmationHtml(booking);

            var message = new EmailMessage
            {
                From = "TurnoLink <turnolink@santvallejos.dev>",
                To = [booking.ClientEmail ?? string.Empty],
                Subject = "✅ Confirmación de Turno - TurnoLink",
                HtmlBody = htmlContent,
                Attachments =
                [
                    new EmailAttachment
                    {
                        Filename = $"turno_{booking.Id}.ics",
                        ContentType = "text/calendar",
                        Content = icsBase64
                    }
                ]
            };

            await _resendClient.EmailSendAsync(message);
        }

        /// <summary>
        /// Sends new booking notification email to the professional
        /// </summary>
        public async Task SendProfessionalNotificationEmailAsync(BookingDto booking, string userEmail)
        {
            var htmlContent = GenerateProfessionalNotificationHtml(booking);

            var message = new EmailMessage
            {
                From = "TurnoLink <onboarding@santvallejos.dev>",
                To = [userEmail],
                Subject = $"📅 Nueva Reserva - {booking.ClientName} - TurnoLink",
                HtmlBody = htmlContent
            };

            await _resendClient.EmailSendAsync(message);
        }

        /// <summary>
        /// Generates HTML email for client confirmation
        /// </summary>
        public string GenerateClientConfirmationHtml(BookingDto booking)
        {
            var locationSection = !string.IsNullOrEmpty(booking.Location)
                ? $@"
                    <tr>
                        <td style=""padding: 16px 20px; border-bottom: 1px solid #e5e7eb;"">
                            <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td width=""40"" valign=""top"">
                                        <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #10b981 0%, #059669 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                            📍
                                        </div>
                                    </td>
                                    <td style=""padding-left: 12px;"">
                                        <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Dirección</p>
                                        <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 500;"">{booking.Location}</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>"
                : "";

            var phoneSection = !string.IsNullOrEmpty(booking.UserPhone)
                ? $@"
                    <tr>
                        <td style=""padding: 16px 20px;"">
                            <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td width=""40"" valign=""top"">
                                        <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                            📞
                                        </div>
                                    </td>
                                    <td style=""padding-left: 12px;"">
                                        <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Teléfono de contacto</p>
                                        <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 500;"">{booking.UserPhone}</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>"
                : "";

            return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirmación de Turno - TurnoLink</title>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f3f4f6; -webkit-font-smoothing: antialiased;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f3f4f6; padding: 40px 20px;"">
        <tr>
            <td align=""center"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width: 600px; background-color: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);"">
                    
                    <!-- Header con Logo -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 50%, #a855f7 100%); padding: 40px 30px; text-align: center;"">
                            <div style=""background-color: rgba(255, 255, 255, 0.2); border-radius: 50px; padding: 8px 20px; display: inline-block;"">
                                <span style=""color: #ffffff; font-size: 14px; font-weight: 600;"">✓ Turno Confirmado</span>
                            </div>
                        </td>
                    </tr>
                    
                    <!-- Contenido Principal -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <!-- Saludo -->
                            <h1 style=""color: #1f2937; margin: 0 0 8px 0; font-size: 26px; font-weight: 700;"">
                                ¡Hola, {booking.ClientName}! 👋
                            </h1>
                            <p style=""color: #6b7280; font-size: 16px; margin: 0 0 30px 0; line-height: 1.6;"">
                                Tu turno ha sido confirmado exitosamente. A continuación encontrarás todos los detalles de tu reserva.
                            </p>
                            
                            <!-- Tarjeta de Detalles -->
                            <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f9fafb; border-radius: 12px; overflow: hidden; margin-bottom: 24px;"">
                                <!-- Servicio -->
                                <tr>
                                    <td style=""padding: 16px 20px; border-bottom: 1px solid #e5e7eb;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""40"" valign=""top"">
                                                    <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                                        💼
                                                    </div>
                                                </td>
                                                <td style=""padding-left: 12px;"">
                                                    <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Servicio</p>
                                                    <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 600;"">{booking.ServiceName}</p>
                                                </td>
                                                <td align=""right"" valign=""middle"">
                                                    <span style=""background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%); color: #ffffff; padding: 6px 14px; border-radius: 20px; font-size: 14px; font-weight: 600;"">${booking.ServicePrice}</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <!-- Fecha y Hora -->
                                <tr>
                                    <td style=""padding: 16px 20px; border-bottom: 1px solid #e5e7eb;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""40"" valign=""top"">
                                                    <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                                        📅
                                                    </div>
                                                </td>
                                                <td style=""padding-left: 12px;"">
                                                    <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Fecha y Hora</p>
                                                    <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 500;"">{booking.StartTime:dddd, dd 'de' MMMM 'de' yyyy}</p>
                                                    <p style=""margin: 2px 0 0 0; font-size: 14px; color: #6b7280;"">{booking.StartTime:HH:mm} - {booking.EndTime:HH:mm} hs</p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <!-- Profesional -->
                                <tr>
                                    <td style=""padding: 16px 20px; border-bottom: 1px solid #e5e7eb;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""40"" valign=""top"">
                                                    <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                                        👤
                                                    </div>
                                                </td>
                                                <td style=""padding-left: 12px;"">
                                                    <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Profesional</p>
                                                    <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 500;"">{booking.UserName}</p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <!-- Dirección (condicional) -->
                                {locationSection}
                                
                                <!-- Teléfono (condicional) -->
                                {phoneSection}
                            </table>
                            
                            <!-- Nota del Calendario -->
                            <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background: linear-gradient(135deg, #eff6ff 0%, #f5f3ff 100%); border-radius: 12px; border-left: 4px solid #6366f1; margin-bottom: 24px;"">
                                <tr>
                                    <td style=""padding: 20px;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""50"" valign=""top"">
                                                    <div style=""font-size: 32px;"">📎</div>
                                                </td>
                                                <td>
                                                    <p style=""margin: 0 0 4px 0; font-size: 14px; font-weight: 600; color: #4338ca;"">Archivo adjunto</p>
                                                    <p style=""margin: 0; font-size: 13px; color: #6b7280; line-height: 1.5;"">
                                                        Se incluye el archivo <strong style=""color: #4338ca;"">turno_{booking.Id}.ics</strong> para agregar este turno a tu calendario favorito (Google Calendar, Outlook, Apple Calendar, etc.)
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            
                            <!-- Mensaje de ayuda -->
                            <p style=""color: #9ca3af; font-size: 13px; margin: 0; text-align: center; line-height: 1.6;"">
                                Si tienes alguna pregunta o necesitas modificar tu turno, por favor contacta directamente al profesional.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #1f2937; padding: 30px; text-align: center;"">
                            <p style=""color: #9ca3af; margin: 0 0 8px 0; font-size: 13px;"">
                                Gracias por confiar en nosotros
                            </p>
                            <p style=""color: #ffffff; margin: 0; font-size: 16px; font-weight: 600;"">
                                TurnoLink
                            </p>
                            <p style=""color: #6b7280; margin: 16px 0 0 0; font-size: 12px;"">
                                © {DateTime.Now.Year} TurnoLink. Todos los derechos reservados.
                            </p>
                        </td>
                    </tr>
                </table>
                
                <!-- Texto legal -->
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width: 600px;"">
                    <tr>
                        <td style=""padding: 20px; text-align: center;"">
                            <p style=""color: #9ca3af; font-size: 11px; margin: 0; line-height: 1.6;"">
                                Este correo fue enviado automáticamente. Por favor, no respondas a este mensaje.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }

        /// <summary>
        /// Generates HTML email for professional notification
        /// </summary>
        public string GenerateProfessionalNotificationHtml(BookingDto booking)
        {
            var clientPhoneSection = !string.IsNullOrEmpty(booking.ClientPhone)
                ? $@"
                    <tr>
                        <td style=""padding: 16px 20px;"">
                            <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td width=""40"" valign=""top"">
                                        <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                            📞
                                        </div>
                                    </td>
                                    <td style=""padding-left: 12px;"">
                                        <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Teléfono del cliente</p>
                                        <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 500;"">{booking.ClientPhone}</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>"
                : "";

            var notesSection = !string.IsNullOrEmpty(booking.Notes)
                ? $@"
                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #fef3c7; border-radius: 12px; border-left: 4px solid #f59e0b; margin-bottom: 24px;"">
                        <tr>
                            <td style=""padding: 20px;"">
                                <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                    <tr>
                                        <td width=""50"" valign=""top"">
                                            <div style=""font-size: 32px;"">📝</div>
                                        </td>
                                        <td>
                                            <p style=""margin: 0 0 4px 0; font-size: 14px; font-weight: 600; color: #92400e;"">Notas del cliente</p>
                                            <p style=""margin: 0; font-size: 13px; color: #78350f; line-height: 1.5;"">{booking.Notes}</p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>"
                : "";

            return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Nueva Reserva - TurnoLink</title>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f3f4f6; -webkit-font-smoothing: antialiased;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f3f4f6; padding: 40px 20px;"">
        <tr>
            <td align=""center"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width: 600px; background-color: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);"">
                    
                    <!-- Header -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #10b981 0%, #059669 50%, #047857 100%); padding: 40px 30px; text-align: center;"">
                            <div style=""background-color: rgba(255, 255, 255, 0.2); border-radius: 50px; padding: 8px 20px; display: inline-block;"">
                                <span style=""color: #ffffff; font-size: 14px; font-weight: 600;"">🔔 Nueva Reserva</span>
                            </div>
                        </td>
                    </tr>
                    
                    <!-- Contenido Principal -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <h1 style=""color: #1f2937; margin: 0 0 8px 0; font-size: 26px; font-weight: 700;"">
                                ¡Nueva reserva recibida! 🎉
                            </h1>
                            <p style=""color: #6b7280; font-size: 16px; margin: 0 0 30px 0; line-height: 1.6;"">
                                <strong style=""color: #1f2937;"">{booking.ClientName}</strong> ha reservado un turno contigo. Aquí están los detalles:
                            </p>
                            
                            <!-- Tarjeta de Detalles del Cliente -->
                            <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f0fdf4; border-radius: 12px; overflow: hidden; margin-bottom: 24px; border: 1px solid #bbf7d0;"">
                                <tr>
                                    <td style=""padding: 16px 20px; background-color: #dcfce7; border-bottom: 1px solid #bbf7d0;"">
                                        <p style=""margin: 0; font-size: 14px; font-weight: 600; color: #166534;"">👤 Información del Cliente</p>
                                    </td>
                                </tr>
                                <!-- Nombre -->
                                <tr>
                                    <td style=""padding: 16px 20px; border-bottom: 1px solid #bbf7d0;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""40"" valign=""top"">
                                                    <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #10b981 0%, #059669 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                                        👤
                                                    </div>
                                                </td>
                                                <td style=""padding-left: 12px;"">
                                                    <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Nombre completo</p>
                                                    <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 600;"">{booking.ClientName}</p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- Email -->
                                <tr>
                                    <td style=""padding: 16px 20px; border-bottom: 1px solid #bbf7d0;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""40"" valign=""top"">
                                                    <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                                        ✉️
                                                    </div>
                                                </td>
                                                <td style=""padding-left: 12px;"">
                                                    <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Email</p>
                                                    <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 500;"">{booking.ClientEmail}</p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                {clientPhoneSection}
                            </table>
                            
                            <!-- Tarjeta de Detalles de la Reserva -->
                            <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f9fafb; border-radius: 12px; overflow: hidden; margin-bottom: 24px;"">
                                <tr>
                                    <td style=""padding: 16px 20px; background-color: #f3f4f6; border-bottom: 1px solid #e5e7eb;"">
                                        <p style=""margin: 0; font-size: 14px; font-weight: 600; color: #374151;"">📋 Detalles de la Reserva</p>
                                    </td>
                                </tr>
                                <!-- Servicio -->
                                <tr>
                                    <td style=""padding: 16px 20px; border-bottom: 1px solid #e5e7eb;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""40"" valign=""top"">
                                                    <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                                        💼
                                                    </div>
                                                </td>
                                                <td style=""padding-left: 12px;"">
                                                    <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Servicio</p>
                                                    <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 600;"">{booking.ServiceName}</p>
                                                </td>
                                                <td align=""right"" valign=""middle"">
                                                    <span style=""background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: #ffffff; padding: 6px 14px; border-radius: 20px; font-size: 14px; font-weight: 600;"">${booking.ServicePrice}</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- Fecha y Hora -->
                                <tr>
                                    <td style=""padding: 16px 20px;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""40"" valign=""top"">
                                                    <div style=""width: 36px; height: 36px; background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); border-radius: 10px; text-align: center; line-height: 36px; font-size: 18px;"">
                                                        📅
                                                    </div>
                                                </td>
                                                <td style=""padding-left: 12px;"">
                                                    <p style=""margin: 0; font-size: 12px; color: #6b7280; text-transform: uppercase; letter-spacing: 0.5px;"">Fecha y Hora</p>
                                                    <p style=""margin: 4px 0 0 0; font-size: 15px; color: #1f2937; font-weight: 500;"">{booking.StartTime:dddd, dd 'de' MMMM 'de' yyyy}</p>
                                                    <p style=""margin: 2px 0 0 0; font-size: 14px; color: #6b7280;"">{booking.StartTime:HH:mm} - {booking.EndTime:HH:mm} hs</p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            
                            {notesSection}
                            
                            <!-- Mensaje de acción -->
                            <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background: linear-gradient(135deg, #eff6ff 0%, #f0fdf4 100%); border-radius: 12px; border-left: 4px solid #10b981; margin-bottom: 24px;"">
                                <tr>
                                    <td style=""padding: 20px;"">
                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                            <tr>
                                                <td width=""50"" valign=""top"">
                                                    <div style=""font-size: 32px;"">💡</div>
                                                </td>
                                                <td>
                                                    <p style=""margin: 0 0 4px 0; font-size: 14px; font-weight: 600; color: #166534;"">Recordatorio</p>
                                                    <p style=""margin: 0; font-size: 13px; color: #6b7280; line-height: 1.5;"">
                                                        El cliente ya ha recibido un correo de confirmación con los detalles del turno y un archivo para agregar al calendario.
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            
                            <p style=""color: #9ca3af; font-size: 13px; margin: 0; text-align: center; line-height: 1.6;"">
                                Ingresa a tu panel de TurnoLink para gestionar esta y otras reservas.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #1f2937; padding: 30px; text-align: center;"">
                            <p style=""color: #9ca3af; margin: 0 0 8px 0; font-size: 13px;"">Gestiona tus turnos fácilmente</p>
                            <p style=""color: #ffffff; margin: 0; font-size: 16px; font-weight: 600;"">TurnoLink</p>
                            <p style=""color: #6b7280; margin: 16px 0 0 0; font-size: 12px;"">© {DateTime.Now.Year} TurnoLink. Todos los derechos reservados.</p>
                        </td>
                    </tr>
                </table>
                
                <!-- Texto legal -->
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width: 600px;"">
                    <tr>
                        <td style=""padding: 20px; text-align: center;"">
                            <p style=""color: #9ca3af; font-size: 11px; margin: 0; line-height: 1.6;"">
                                Este correo fue enviado automáticamente. Por favor, no respondas a este mensaje.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }
    }
}