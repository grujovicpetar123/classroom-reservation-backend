using System.ComponentModel.DataAnnotations;

namespace ClassroomReservationBackend.Model.DTO.ReservationDTO;

public class ReservationStatusRequest
{
    [Required] public string Status{ get; set; } = string.Empty; // Approved / Rejected / Cancelled
}