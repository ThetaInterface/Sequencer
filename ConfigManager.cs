using System;
using System.Collections.Generic;
using System.IO;

namespace Sequencer.Utils;

public static class ConfigManager
{
    private const string FILE_NAME = "config.ini";
    private static readonly Dictionary<EConfigOption, string> DEFAULT_CONFIGURATION = new ()
    {
        {EConfigOption.SeparatorSymbol, ","},
        {EConfigOption.ExitPhrase, "exit"},
        {EConfigOption.ShowExtension, "false"},
        {EConfigOption.AutoClose, "true"},
        {EConfigOption.RunAsChild, "NaN"}
    };
    
    private static readonly int DEFAULT_ENTRY_COUNT = DEFAULT_CONFIGURATION.Count;

    private static readonly string configFilePath = Path.Combine(Config.ProgramPath, FILE_NAME);
    private static readonly Dictionary<EConfigOption, string> configData = [];

    public static string ConfigFilePath { get { return configFilePath; } }
    public static string ConfigFileName { get { return FILE_NAME; } }

    public static Exception? Init(string[] content)
    {
        if (content.Length < DEFAULT_ENTRY_COUNT)
        {
            var result = Write(defaultConfig: true);

            if (result != null)
                return result;
        }

        foreach (var entry in content)
        {
            string[] pair = entry.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (pair.Length != 2)
                return new Exception($"Uncomplete entry in config file! '{entry}'");
            
            if (!Enum.TryParse(pair[0], out EConfigOption option))
                return new Exception($"Unknow name in config file! '{pair[0]}'");

            configData.Add(option, pair[1]);
        }

        return null;
    }

    public static string GetEntry(EConfigOption key)
    {
        if (configData.TryGetValue(key, out string? value))
            return value;
        else
            return ResolveNameByDefault(key);
    }

    private static string ResolveNameByDefault(EConfigOption key)
    {
        if (DEFAULT_CONFIGURATION.TryGetValue(key, out  string? value))
            return value;
        else
            throw new NullReferenceException($"Default configuration doesn't have '{key}' option!");
    }

    public static Exception? Write(bool defaultConfig = false)
    {
        string content = string.Empty;

        if (!defaultConfig)
            content = BuildConfigFromData(configData);
        else if (defaultConfig)
            content = BuildConfigFromData(DEFAULT_CONFIGURATION);
        
        try 
        {
            using (StreamWriter sW = new (configFilePath))
                sW.WriteLine(content);
        }
        catch
        {
            return new Exception("Can not write config!");
        }

        return null;
    }

    private static string BuildConfigFromData(Dictionary<EConfigOption, string> data)
    {
        string content = string.Empty;

        foreach (var pair in data)
            content += $"{pair.Key}={pair.Value}\n";

        return content;
    }
}