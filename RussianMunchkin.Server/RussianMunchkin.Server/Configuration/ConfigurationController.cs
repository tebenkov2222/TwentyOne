using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace RussianMunchkin.Server.Configuration;

public class ConfigurationController
{
    public ServerFramework.Configuration ConfigurationServer { get; private set; }
    public Repository.Configuration ConfigurationDatabase { get; private set; }
    public ConfigurationController()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false);
        var configuration = builder.Build();
        GetServerConfig(configuration);
        GetDatabaseConfig(configuration);
        Console.WriteLine($"Server = {ConfigurationServer.Host}, {ConfigurationServer.Port}, {ConfigurationServer.ConnectionKey}");
        Console.WriteLine($"Database = {ConfigurationDatabase.Host}, {ConfigurationDatabase.Port}, {ConfigurationDatabase.DataBaseName}, {ConfigurationDatabase.UserName}, {ConfigurationDatabase.Password}");
    }

    private void GetServerConfig(IConfigurationRoot configuration)
    {
        var serverSection = configuration.GetSection("Server");
        var value = serverSection.GetValue<string>("Config");
        switch (value)
        {
            case "Json":
                var jsonOption = serverSection.GetValue<string>("JsonOption");
                ConfigurationServer = serverSection.GetSection(jsonOption).Get<ServerFramework.Configuration>();
                break;
            case "Local":
                ConfigurationServer = ServerFramework.Configuration.Local;;
                break;
        }
    }
    private void GetDatabaseConfig(IConfigurationRoot configuration)
    {
        var serverSection = configuration.GetSection("Database");
        var value = serverSection.GetValue<string>("Config");
        switch (value)
        {
            case "Json":
                var jsonOption = serverSection.GetValue<string>("JsonOption");
                Console.WriteLine($"jsonOption = {jsonOption}");
                ConfigurationDatabase = serverSection.GetSection(jsonOption).Get<Repository.Configuration>();
                break;
            case "Local":
                ConfigurationDatabase = Repository.Configuration.Local;;
                break;
            case "Docker":
                ConfigurationDatabase = Repository.Configuration.Docker;;
                break;
            case "DockerLocal":
                ConfigurationDatabase = Repository.Configuration.DockerLocal;;
                break;
        }
    }
}