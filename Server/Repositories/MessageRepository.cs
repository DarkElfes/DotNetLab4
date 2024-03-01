using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.Messages;
using Server.Repositories.IRepositories;

namespace Server.Repositories;

public record MessageRepository(
    ApplicationDbContext _db
    ) : IRepository<ChatMessage>
{
    public async Task<ChatMessage?> GetByIdAsync(int id)
        => await _db.ChatMessages.FirstOrDefaultAsync(c => c.Id == id);
    public async Task<List<ChatMessage>> GetAllAsync()
        => await _db.ChatMessages.ToListAsync();


    public async Task<ChatMessage> CreateAsync(ChatMessage entity)
    {
        await _db.ChatMessages.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
    public async Task<ChatMessage?> UpdateAsync(ChatMessage entity)
    {
        _db.ChatMessages.UpdateRange(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        if (await GetByIdAsync(id) is ChatMessage chatMessage)
        {
            _db.ChatMessages.Remove(chatMessage);
            await _db.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
