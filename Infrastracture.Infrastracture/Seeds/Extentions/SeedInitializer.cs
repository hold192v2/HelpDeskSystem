using System.Text.Json;
using EFCore.BulkExtensions;
using Infrastracture.Domain.Entities;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Infrastracture.Infrastracture.Seeds.Extentions;

public class SeedInitializer
{
    private readonly AppDbContext appDbContext;
    private readonly IHostEnvironment env;

    public SeedInitializer(AppDbContext context,  IHostEnvironment env)
    {
        appDbContext = context;
        this.env = env;
    }

    public async Task Initialize()
    {
        if (await appDbContext.Regions.AnyAsync()) return;
        if (await appDbContext.FilialAreas.AnyAsync()) return;
        if (await appDbContext.Offices.AnyAsync()) return;
        if (await appDbContext.Roles.AnyAsync()) return;
        
        var regions = JsonSerializer.Deserialize<List<Region>>(await File.ReadAllTextAsync(GetSeedFilePath("regions.json")));
        var offices = JsonSerializer.Deserialize<List<Office>>(await File.ReadAllTextAsync(GetSeedFilePath("offices.json")));
        var filial = JsonSerializer.Deserialize<List<FilialArea>>(await File.ReadAllTextAsync(GetSeedFilePath("filials.json")));
        var roles = JsonSerializer.Deserialize<List<Role>>(await File.ReadAllTextAsync(GetSeedFilePath("roles.json")));
        
        await appDbContext.BulkInsertAsync(regions);
        await appDbContext.BulkInsertAsync(offices);
        await appDbContext.BulkInsertAsync(filial);
        await appDbContext.BulkInsertAsync(roles);
    }

    private string GetSeedFilePath(string filename) => 
        Path.Combine(env.ContentRootPath,"..", "Infrastracture.Infrastracture", "Seeds", filename);
}