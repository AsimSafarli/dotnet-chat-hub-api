using Microsoft.EntityFrameworkCore;

class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokenService;

    public AuthService(AppDbContext db, TokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<string> Register(RegisterDto dto)
    {
        bool emailVar = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailVar) return "This email is already registered!";

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return "Registration successful!";
    }

    public async Task<string?> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;

        bool sifreDogru = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!sifreDogru) return null;

        return _tokenService.GenerateToken(user);
    }
}