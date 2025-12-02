using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de servicios ofrecidos por profesionales
    /// </summary>
    public interface IServiceService
    {
        /// <summary>
        /// Obtiene todos los servicios de un profesional
        /// </summary>
        Task<IEnumerable<ServiceDto>> GetServicesByUserIdAsync(Guid userId);

        /// <summary>
        /// Obtiene servicios activos de un profesional
        /// </summary>
        Task<IEnumerable<ServiceDto>> GetActiveServicesByUserIdAsync(Guid userId);

        /// <summary>
        /// Obtiene un servicio por su ID
        /// </summary>
        Task<ServiceDto?> GetServiceByIdAsync(Guid id);

        /// <summary>
        /// Crea un nuevo servicio
        /// </summary>
        Task<ServiceDto> CreateServiceAsync(Guid userId, CreateServiceDto createServiceDto);

        /// <summary>
        /// Actualiza un servicio existente
        /// </summary>
        Task<ServiceDto> UpdateServiceAsync(Guid userId, Guid serviceId, UpdateServiceDto updateServiceDto);

        /// <summary>
        /// Elimina un servicio
        /// </summary>
        Task<bool> DeleteServiceAsync(Guid userId, Guid serviceId);

        /// <summary>
        /// Obtiene todos los servicios activos (público - para clientes)
        /// </summary>
        Task<IEnumerable<ServiceDto>> GetAllActiveServicesAsync();
    }
}
