namespace TurnoLink.Business.DTOs
{
    public class CreateServiceDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateServiceDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? DurationMinutes { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}