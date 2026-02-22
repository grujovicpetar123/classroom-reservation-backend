using System.ComponentModel.DataAnnotations;

namespace ClassroomReservationBackend.Model.DTO.ReservationDTO;

public class UpdateReservationRequest
{
    [MaxLength(200)] public string? Title{ get; set; }

    public string? Description{ get; set; }

    public DateTime? StartTime{ get; set; }

    public DateTime? EndTime{ get; set; }

    [MaxLength(50)] public string? Purpose{ get; set; }

    public int? AttendeeCount{ get; set; }
}