using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace ApiShared;

public static class ModuleInstallation
{
    private static readonly List<ModuleInternal> Modules = new();
    private const char CharToReplace = '"';
    private static ILogger _logger = null!;
    
    
    public static WebApplicationBuilder InstallModules(this WebApplicationBuilder builder)
    {
        CreateLogger();
        
        _logger.LogInformation("Installing modules...");
        
        LoadModules();

        _logger.LogInformation($"Modules Found ({Modules.Count})");

        _logger.LogInformation("Loading configuration...");
        LoadConfiguration(builder);
        _logger.LogInformation($"Configuration loaded.");
        
        _logger.LogInformation("Installing services...");

        foreach (var module in Modules)
        {
            if (!module.IsEnabled)
            {
                _logger.LogInformation($"Module '{module.Module.ModuleName}' is disabled. Skipping installation....");
                continue;
            }
            
            _logger.LogInformation($"Installing {module.Module.ModuleName} services...");
            module.Module.InstallModule(builder.Services);
            _logger.LogInformation($"Services installed for {module.Module.ModuleName}.");
        }

        _logger.LogInformation("Services installed.");
       
        return builder;
    }

    private static void CreateLogger()
    {
        _logger = LoggerFactory.Create(x =>
        {
            x.AddConsole();
            x.AddDebug();
        }).CreateLogger("ModuleInstallation");
    }

    private static void LoadModules()
    {
        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.GetInterfaces()
                            .Contains(typeof(IModule)) 
                        && x is { IsInterface: false, IsAbstract: false, IsClass: true }).ToList();
        
        
        foreach (var type in types)
        {
            if (Activator.CreateInstance(type) is not IModule module)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(module.ModuleName))
            {
                throw new Exception($"Module {module.GetType().Name} does not have a ModuleName defined.");
            }
            
            
            var configuration = ReadConfiguration(module);
            var enabled = CheckIfModuleIsEnabled(configuration);
            
            Modules.Add(new ModuleInternal(module,enabled,configuration));
        }
    }

    private static string ReadConfiguration(IModule module)
    {
        var moduleConfigName = $"module.{module.ModuleName}.json";
        if (!File.Exists(moduleConfigName))
        {
            throw new FileNotFoundException($"Configuration file for module {module.ModuleName} not found.");
        } 
            
        return File.ReadAllText(moduleConfigName);
    }
    
    private static void LoadConfiguration(WebApplicationBuilder builder)
    {
        var configBuilder = new ConfigurationBuilder();
        
        foreach (var module in Modules.Where(x=>x.IsEnabled))
        {
            configBuilder.AddJsonFile(module.Configuration);
        }

        var config =  configBuilder.Build();
        builder.Configuration.AddConfiguration(config);
    }

    private static bool CheckIfModuleIsEnabled(string fileContent)
    {
        var fileConfig = JsonSerializer.Deserialize<Dictionary<string, string>>(fileContent);
        
        return fileConfig is not null
               && fileConfig.TryGetValue("Enabled", out var value) 
               && !string.IsNullOrWhiteSpace(value) 
               && value.Equals("true".Replace(CharToReplace.ToString(),""), StringComparison.InvariantCultureIgnoreCase);
    }
    
    
    

    public static WebApplication UseModules(this WebApplication builder)
    {
        foreach (var module in Modules.Where(x=>x.IsEnabled))
        {
            _logger.LogInformation($"Installing {module.Module.ModuleName}...");
            module.Module.InstallModule(builder);

            _logger.LogInformation($"Adding endpoints for {module.Module.ModuleName}...");
            module.Module.AddEndPoints(builder.MapGroup(module.Module.ModuleName));
        }

        _logger.LogInformation("Modules installed complete!");
        
        
        return builder;
    }
}