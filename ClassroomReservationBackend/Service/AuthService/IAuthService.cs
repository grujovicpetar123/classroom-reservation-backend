using ClassroomReservationBackend.Model.DTO;

namespace ClassroomReservationBackend.Service.AuthService;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task RevokeTokenAsync(string refreshToken);
}