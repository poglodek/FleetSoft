using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
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
            module.Module.InstallModule(builder.Services, builder.Configuration);
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
        var assemblies = ReturnAssemblies();

        var types = assemblies.SelectMany(x=>x.GetTypes())
            .Where(x => x.GetInterfaces()
                            .Contains(typeof(IModule)) 
                        && x is { IsInterface: false, IsAbstract: false, IsClass: true }
                        ).ToList();
        
        
        
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
            
            Modules.Add(new ModuleInternal(module, enabled, configuration, GetConfigPathFile(module)));
        }
    }

    private static IEnumerable<Assembly> ReturnAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var locations = assemblies.Where(x => !x.IsDynamic).Select(x => x.Location).ToArray();
        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .ToList();
        files.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));
        return assemblies;
    }

    private static string ReadConfiguration(IModule module)
    {
        var configPathFile = GetConfigPathFile(module);
        if (!File.Exists(configPathFile))
        {
            throw new FileNotFoundException($"Configuration file for module {module.ModuleName} not found.");
        } 
            
        return File.ReadAllText(configPathFile);
    }

    private static string GetConfigPathFile(IModule module)
    {
        var moduleConfigName = $"module.{module.ModuleName}.json";
        var configPathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, moduleConfigName);
        return configPathFile;
    }

    private static void LoadConfiguration(WebApplicationBuilder builder)
    {
        var configBuilder = new ConfigurationBuilder();
        
        foreach (var module in Modules.Where(x=>x.IsEnabled))
        {
            configBuilder.AddJsonFile(module.Path, optional: false, reloadOnChange: true);
        }

        var config =  configBuilder.Build();
        builder.Configuration.AddConfiguration(config);
    }

    private static bool CheckIfModuleIsEnabled(string fileContent)
    {
        var jsonObject = JsonNode.Parse(fileContent)!.AsObject();
        var fileConfig = new Dictionary<string, string>();
        
        foreach (var item in jsonObject)
        {
            fileConfig.Add(item.Key, item.Value!.ToString().Replace(CharToReplace.ToString(), ""));
        }
        
        return fileConfig.TryGetValue("Enabled", out var value) 
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
            module.Module.AddEndPoints(builder.MapGroup($"api/{module.Module.ModuleName}"));
        }

        _logger.LogInformation("Modules installed complete!");
        
        
        return builder;
    }
}