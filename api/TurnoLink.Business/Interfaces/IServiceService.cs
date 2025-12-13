using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface for service-related operations.
    /// </summary>
    public interface IServiceService
    {
        /// <summary>
        /// Gets all services of a professional
        /// </summary>
        /// <param name="userId">Professional's user ID</param>
        Task<IEnumerable<ServiceDto>> GetServicesByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets a service by its ID
        /// </summary>
        /// <param name="id">Service ID</param>
        Task<ServiceDto?> GetServiceByIdAsync(Guid id);

        /// <summary>
        /// Gets all services by slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        Task<IEnumerable<ServiceDto>> GetServicesBySlugAsync(string slug);

        /// <summary>
        /// Creates a new service
        /// </summary>
        /// <param name="userId">Professional's user ID</param>
        /// <param name="createServiceDto">DTO containing service creation data</param>
        Task<ServiceDto> CreateServiceAsync(Guid userId, CreateServiceDto createServiceDto);

        /// <summary>
        /// Updates an existing service
        /// </summary>
        /// <param name="userId">Professional's user ID</param>
        /// <param name="serviceId">Service ID</param>
        /// <param name="updateServiceDto">DTO containing service update data</param>
        Task<ServiceDto> UpdateServiceAsync(Guid userId, Guid serviceId, UpdateServiceDto updateServiceDto);

        /// <summary>
        /// Deletes a service
        /// </summary>
        /// <param name="userId">Professional's user ID</param>
        /// <param name="serviceId">Service ID</param>
        Task<bool> DeleteServiceAsync(Guid userId, Guid serviceId);
    }
}
