using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task SendEmailAsync(BookingDto booking, string stringIcs)
        {
            var icsBytes = Encoding.UTF8.GetBytes(stringIcs);
            var icsBase64 = Convert.ToBase64String(icsBytes);

            // Generar HTML del email
            var htmlContent = GenerateBookingConfirmationHtml(booking);


            var message = new EmailMessage
            {
                From = $"Acme <onboarding@resend.dev>",
                To = new[] { booking.ClientEmail ?? string.Empty },
                Subject = "✅ Confirmación de Turno - TurnoLink",
                HtmlBody = htmlContent,
                Attachments = [
                    new EmailAttachment
                    {
                        Filename = $"turno_{booking.Id}.ics",
                        ContentType = "text/calendar",
                        Content = icsBase64
                    }
                ]
            };

            await _resendClient.EmailSendAsync( message );
        }

        public string GenerateBookingConfirmationHtml(BookingDto booking)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                </head>
                <body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;"">
                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f4f4f4; padding: 20px 0;"">
                        <tr>
                            <td align=""center"">
                                <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #ffffff; border-radius: 8px; overflow: hidden;"">
                                    
                                    <tr>
                                        <td style=""padding: 40px 30px;"">
                                            <h1 style=""color: #2c3e50; margin: 0 0 20px 0; font-size: 28px;"">
                                                ✅ Turno Confirmado
                                            </h1>
                                            
                                            <p style=""color: #555; font-size: 16px; margin: 0 0 30px 0;"">
                                                Hola {booking.ClientName}, tu turno ha sido confirmado exitosamente.
                                            </p>
                                            
                                            <table width=""100%"" cellpadding=""15"" cellspacing=""0"" style=""background-color: #f8f9fa; border-radius: 6px; margin-bottom: 30px;"">
                                                <tr>
                                                    <td style=""color: #666; font-size: 14px; border-bottom: 1px solid #e0e0e0;"">
                                                        <strong style=""color: #333;"">📋 Servicio:</strong><br>
                                                        {booking.ServiceName}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style=""color: #666; font-size: 14px; border-bottom: 1px solid #e0e0e0;"">
                                                        <strong style=""color: #333;"">📅 Fecha y Hora:</strong><br>
                                                        {booking.StartTime:dddd, dd 'de' MMMM 'de' yyyy 'a las' HH:mm}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style=""color: #666; font-size: 14px;"">
                                                        <strong style=""color: #333;"">👤 Profesional:</strong><br>
                                                        {booking.UserName}
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                            <div style=""background-color: #e3f2fd; border-left: 4px solid #2196f3; padding: 15px; margin: 20px 0; border-radius: 4px;"">
                                                <p style=""color: #1565c0; margin: 0; font-size: 14px;"">
                                                    <strong>📎 Archivo adjunto:</strong> Se incluye el archivo <strong>turno_{booking.Id}.ics</strong> 
                                                    para agregar este turno a tu calendario favorito (Google Calendar, Outlook, Apple Calendar, etc.)
                                                </p>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style=""background-color: #2c3e50; padding: 20px; text-align: center;"">
                                            <p style=""color: #ffffff; margin: 0; font-size: 14px;"">
                                                © 2025 TurnoLink
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