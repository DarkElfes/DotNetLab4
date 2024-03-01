﻿using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.Chats;
using Server.Repositories.IRepositories;

namespace Server.Repositories;

public record ChatRepository<T>(
    ApplicationDbContext _db
    ) : IRepository<T> where T : BaseChat
{
    public async Task<T?> GetByIdAsync(int id)
        => await _db.BaseChats
            .OfType<T>()
            .Include(g => g.Users)
            .Include(m => m.Messages)
            .FirstOrDefaultAsync(g => g.Id == id);
    public async Task<List<T>> GetAllAsync()
        => await _db.BaseChats
            .OfType<T>()
            .Include(u => u.Users)
            .Include(m => m.Messages)
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
