public interface IRoomService
{
    Task<List<Room>> GetAll();
    Task<Room> Create(string name);
    Task<bool> Delete(int roomId);
}