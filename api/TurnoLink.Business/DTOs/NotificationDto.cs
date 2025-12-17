namespace TurnoLink.Business.DTOs
{
    /// <summary>
    /// DTO para notificaciones de reserva en tiempo real
    /// </summary>
    public class BookingNotificationDto
    {
        /// <summary>
        /// Tipo de notificación
        /// </summary>
        public string Type { get; set; } = "NewBooking";

        /// <summary>
        /// ID de la reserva
        /// </summary>
        public Guid BookingId { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del servicio reservado
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Hora de inicio de la reserva
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Fecha de creación de la notificación
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Mensaje descriptivo de la notificación
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
