using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Service to manage services.
    /// </summary>
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly TurnoLinkDbContext _context;

        public ServiceService(IServiceRepository serviceRepository, IUserRepository userRepository, TurnoLinkDbContext context)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IEnumerable<ServiceDto>> GetServicesByUserIdAsync(Guid userId)
        {
            try
            {
                var services = await _serviceRepository.GetServicesByUserIdAsync(userId);

                if (services == null)
                    throw new InvalidOperationException("User not found");

                return services.Select(MapToDto);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving services by user ID.", e);
            }
        }

        public async Task<ServiceDto?> GetServiceByIdAsync(Guid id)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(id);

                if (service == null)
                    throw new InvalidOperationException("Service not found");

                return MapToDto(service);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving the service by ID.", e);
            }
        }

        public async Task<IEnumerable<ServiceDto>> GetServicesBySlugAsync(string slug)
        {
            try
            {
                var services = await _serviceRepository.GetServicesBySlug(slug);

                // Fix: Check if services is null or empty
                return services.Select(MapToDto);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving services by slug.", e);
            }
        }

        public async Task<ServiceDto> CreateServiceAsync(Guid userId, CreateServiceDto createServiceDto)
        {
            try
            {
                // Verify that the user exists
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found");

                var service = new Service
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = createServiceDto.Name,
                    Description = createServiceDto.Description,
                    DurationMinutes = createServiceDto.DurationMinutes,
                    Price = createServiceDto.Price,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _serviceRepository.AddAsync(service);
                await _context.SaveChangesAsync();

                return MapToDto(service);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while creating the service.", e);
            }
        }

        public async Task<ServiceDto> UpdateServiceAsync(Guid userId, Guid serviceId, UpdateServiceDto updateServiceDto)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(serviceId);
                if (service == null)
                    throw new InvalidOperationException("Service not found");

                // Check that the service belongs to the user
                if (service.UserId != userId)
                    throw new UnauthorizedAccessException("You do not have permission to modify this service");

                if (!string.IsNullOrWhiteSpace(updateServiceDto.Name))
                    service.Name = updateServiceDto.Name;

                if (!string.IsNullOrWhiteSpace(updateServiceDto.Description))
                    service.Description = updateServiceDto.Description;

                if (updateServiceDto.DurationMinutes.HasValue)
                    service.DurationMinutes = updateServiceDto.DurationMinutes.Value;

                if (updateServiceDto.Price.HasValue)
                    service.Price = updateServiceDto.Price.Value;

                if (updateServiceDto.IsActive.HasValue)
                    service.IsActive = updateServiceDto.IsActive.Value;

                _serviceRepository.Update(service);
                await _context.SaveChangesAsync();

                return MapToDto(service);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while updating the service.", e);
            }
        }

        public async Task<bool> DeleteServiceAsync(Guid userId, Guid serviceId)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(serviceId);
                if (service == null)
                    throw new InvalidOperationException("Service not found");

                // Check that the service belongs to the user
                if (service.UserId != userId)
                    throw new UnauthorizedAccessException("You do not have permission to delete this service");

                _serviceRepository.Remove(service);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while deleting the service.", e);
            }
        }

        /// <summary>
        /// Maps a Service entity to a ServiceDto
        /// </summary>
        /// <param name="service">Entity Service</param>
        /// <returns>DTO of Service</returns>
        private static ServiceDto MapToDto(Service service)
        {
            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                DurationMinutes = service.DurationMinutes,
                Price = service.Price,
                IsActive = service.IsActive,
                UserName = $"{service.User?.Name}  {service.User?.Surname}" ?? string.Empty
            };
        }
    }
}
