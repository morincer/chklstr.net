using Chklstr.UI.Core.Services;

namespace Chklstr.UI.Core.Tests;

public class InMemoryUserSettings : IUserSettingsService
{

    public Config Config { get; set; } = new();
    
    public Config Load()
    {
        return Config;
    }

    public void Save(Config config)
    {
        this.Config = config;
    }
}