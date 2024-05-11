using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Models.Chats;
using Server.Repositories.IRepositories;

namespace Server.Repositories;

public record ChatRepository<T>(
    ApplicationDbContext _db
    ) : IChatRepository<T> where T : BaseChat
{
    public async Task<T?> GetByIdAsync(int id)
        => await _db.BaseChats
            .OfType<T>()
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id.Equals(id));

    public async Task<List<T>> GetAllAsync()
        => await _db.BaseChats
            .OfType<T>()
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .ToListAsync();

    public async Task<List<T>> GetByUser(ApplicationUser user)
        => await _db.BaseChats
            .OfType<T>()
            .Where(u => u.Users.Any(u => u.Equals(user)))
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .ToListAsync();



    public async Task<T> CreateAsync(T entity)
    {
        await _db.BaseChats.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
    public async Task<T?> UpdateAsync(T entity)
    {
        _db.BaseChats.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        if (await GetByIdAsync(id) is T removedGroup)
        {
            _db.BaseChats.Remove(removedGroup);
            await _db.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
