using Microsoft.EntityFrameworkCore;

public class RoomService : IRoomService
{
    private readonly AppDbContext _db;

    public RoomService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Room>> GetAll()
    {
        return await _db.Rooms.ToListAsync();
    }

    public async Task<Room> Create(string name)
    {
        var room = new Room { Name = name };
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
        return room;
    }

    public async Task<bool> Delete(int roomId)
    {
        var room = await _db.Rooms.FindAsync(roomId);
        if (room == null) return false;

        _db.Rooms.Remove(room);
        await _db.SaveChangesAsync();
        return true;
    }
}