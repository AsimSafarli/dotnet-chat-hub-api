static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/auth/register", async (IAuthService auth, RegisterDto dto) =>
        {
            var result = await auth.Register(dto);
            return Results.Ok(result);
        });

        app.MapPost("/auth/login", async (IAuthService auth, LoginDto dto) =>
        {
            var token = await auth.Login(dto);
            return token is not null
                ? Results.Ok(new { Token = token })
                : Results.Unauthorized();
        });
    }
}