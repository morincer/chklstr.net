﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Chklstr.Core.Utils;
using Chklstr.UI.Core.Services;
using Microsoft.Extensions.Logging;

namespace Chklstr.UI.Core.Infra;

public class JsonUserSettingsService : IUserSettingsService
{
    private readonly ILogger<JsonUserSettingsService> _logger;
    public string ConfigFile { get; }
    
    public JsonUserSettingsService(string configFile, ILogger<JsonUserSettingsService> logger)
    {
        _logger = logger;
        ConfigFile = configFile;
    }

    public Config Load()
    {
        FileUtils.EnsureDirectoryExists(Path.GetDirectoryName(ConfigFile));
        
        if (!File.Exists(ConfigFile))
        {
            _logger.LogInformation($"No config file exist at location {ConfigFile}. Creating default settings");
            return new Config();
        }

        using var fs = new FileStream(ConfigFile, FileMode.Open);

        Config? config = null;
        
        try
        {
            _logger.LogInformation($"Reading configuration file at {ConfigFile}");
            config = JsonSerializer.Deserialize<Config>(fs);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }

        if (config != null) return config;
        
        _logger.LogWarning($"Failed to read configuration - falling back to default");
        return new Config();
    }

    public void Save(Config config)
    {
        using var fs = new FileStream(ConfigFile, FileMode.OpenOrCreate);

        try
        {
            config.RecentCrafts = config.RecentCrafts
                .OrderBy(c => -c.Timestamp)
                .GroupBy(c => c.AircraftName)
                .Select(g => g.First())
                .ToList();

            JsonSerializer.Serialize(fs, config, new JsonSerializerOptions
            {
                WriteIndented = true, 
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
        
        fs.Close();
    }
}