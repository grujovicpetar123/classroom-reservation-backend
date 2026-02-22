using ClassroomReservationBackend.Model.DTO.ClassroomDTO;
using ClassroomReservationBackend.Model.DTO;
using ClassroomReservationBackend.Model.DTO.ClassroomDTO;

namespace ClassroomReservationBackend.Service.ClassroomService;

public interface IClassroomService
{
    Task<IEnumerable<ClassroomResponse>> GetAllAsync();
    Task<ClassroomResponse> GetByIdAsync(Guid id);
    Task<ClassroomResponse> CreateAsync(CreateClassroomRequest request);
    Task<ClassroomResponse> UpdateAsync(Guid id, UpdateClassroomRequest request);
    Task DeleteAsync(Guid id);
}