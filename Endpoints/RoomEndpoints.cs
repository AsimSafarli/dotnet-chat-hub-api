public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this WebApplication app)
    {
        app.MapGet("/rooms", async (IRoomService rooms) =>
            Results.Ok(await rooms.GetAll()));

        app.MapPost("/rooms", async (IRoomService rooms, string name) =>
        {
            var room = await rooms.Create(name);
            return Results.Created($"/rooms/{room.Id}", room);
        });

        app.MapDelete("/rooms/{id}", async (IRoomService rooms, int id) =>
        {
            var silindi = await rooms.Delete(id);
            return silindi ? Results.Ok("Deleted.") : Results.NotFound();
        });
    }
}