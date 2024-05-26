using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace ApiShared;

public static class ModuleInstallation
{
    private static List<ModuleInternal> _modules = new();
    private const char CharToReplace = '"';
    
    
    
    public static WebApplicationBuilder InstallModules(this WebApplicationBuilder builder)
    {
        Console.WriteLine("Installing modules...");
        LoadModules();

        Console.WriteLine($"Modules Found ({_modules.Count})");

        Console.WriteLine("Loading configuration...");
        LoadConfiguration(builder);
        Console.WriteLine($"Configuration loaded.");
        
        Console.WriteLine("Installing services...");

        foreach (var module in _modules)
        {
            if (!module.IsEnabled)
            {
                Console.WriteLine($"Module '{module.Module.ModuleName}' is disabled. Skipping installation....");
                continue;
            }
            
            Console.WriteLine($"Installing {module.Module.ModuleName} services...");
            module.Module.InstallModule(builder.Services);
            Console.WriteLine($"Services installed for {module.Module.ModuleName}.");
        }

        Console.WriteLine("Services installed.");
        
        return builder;
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
            
            _modules.Add(new ModuleInternal(module,enabled,configuration));
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
        
        foreach (var module in _modules.Where(x=>x.IsEnabled))
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
        foreach (var module in _modules.Where(x=>x.IsEnabled))
        {
            Console.WriteLine($"Installing {module.Module.ModuleName}...");
            module.Module.InstallModule(builder);

            Console.WriteLine($"Adding endpoints for {module.Module.ModuleName}...");
            module.Module.AddEndPoints(builder.MapGroup(module.Module.ModuleName));
        }

        Console.WriteLine("Modules installed complete!");
        
        
        return builder;
    }
}