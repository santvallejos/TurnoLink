using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Servicio de gestión de servicios ofrecidos por profesionales
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
            var services = await _serviceRepository.GetServicesByUserIdAsync(userId);
            return services.Select(MapToDto);
        }

        public async Task<IEnumerable<ServiceDto>> GetActiveServicesByUserIdAsync(Guid userId)
        {
            var services = await _serviceRepository.GetActiveServicesByUserIdAsync(userId);
            return services.Select(MapToDto);
        }

        public async Task<ServiceDto?> GetServiceByIdAsync(Guid id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            return service == null ? null : MapToDto(service);
        }

        public async Task<ServiceDto> CreateServiceAsync(Guid userId, CreateServiceDto createServiceDto)
        {
            // Verificar que el usuario existe
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

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

        public async Task<ServiceDto> UpdateServiceAsync(Guid userId, Guid serviceId, UpdateServiceDto updateServiceDto)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
            {
                throw new InvalidOperationException("Servicio no encontrado");
            }

            // Verificar que el servicio pertenece al usuario
            if (service.UserId != userId)
            {
                throw new UnauthorizedAccessException("No tienes permiso para modificar este servicio");
            }

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

        public async Task<bool> DeleteServiceAsync(Guid userId, Guid serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
            {
                throw new InvalidOperationException("Servicio no encontrado");
            }

            // Verificar que el servicio pertenece al usuario
            if (service.UserId != userId)
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar este servicio");
            }

            _serviceRepository.Remove(service);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllActiveServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services.Where(s => s.IsActive).Select(MapToDto);
        }

        private static ServiceDto MapToDto(Service service)
        {
            return new ServiceDto
            {
                Id = service.Id,
                UserId = service.UserId,
                Name = service.Name,
                Description = service.Description,
                DurationMinutes = service.DurationMinutes,
                Price = service.Price,
                IsActive = service.IsActive,
                CreatedAt = service.CreatedAt,
                UserName = service.User?.FullName ?? string.Empty
            };
        }
    }
}
