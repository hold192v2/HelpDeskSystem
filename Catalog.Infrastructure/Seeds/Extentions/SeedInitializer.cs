using System.Text.Json;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Context;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Catalog.Infrastructure.Seeds.Extentions;

public class SeedInitializer
{
    private readonly AppDbContext _appDbContext;
    private readonly IHostEnvironment _env;
    
    public SeedInitializer(AppDbContext context,  IHostEnvironment env)
    {
        _appDbContext = context;
        _env = env;
    }

    public async Task Initialize()
    {
        if (await _appDbContext.Categories.AnyAsync()) return;
        if (await _appDbContext.Priorities.AnyAsync()) return;
        if (await _appDbContext.Statuses.AnyAsync()) return;
        
        var categories = JsonSerializer.Deserialize<List<Category>>(await File.ReadAllTextAsync(GetSeedFilePath("categories.json")));
        var priorities = JsonSerializer.Deserialize<List<Priority>>(await File.ReadAllTextAsync(GetSeedFilePath("priorities.json")));
        var statuses = JsonSerializer.Deserialize<List<Status>>(await File.ReadAllTextAsync(GetSeedFilePath("statuses.json")));
        
        await _appDbContext.BulkInsertAsync(categories);
        await _appDbContext.BulkInsertAsync(priorities);
        await _appDbContext.BulkInsertAsync(statuses);
    }
    
    private string GetSeedFilePath(string filename) => 
        Path.Combine(_env.ContentRootPath,"..", "Catalog.Infrastructure", "Seeds", filename);
}