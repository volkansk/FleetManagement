namespace FleetManagement.Core.DTOs.Output
{
    public class ErrorResponseDto
    {
        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
