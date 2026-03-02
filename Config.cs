using System;
using System.IO;

namespace Sequencer.Utils;

public class Config
{
    private static readonly string PROGRAM_PATH = AppDomain.CurrentDomain.BaseDirectory;

    private readonly string configPath;

    public static string ProgramPath { get { return PROGRAM_PATH; } }

    public Config(string configName)
    {
        configPath = Path.Combine(PROGRAM_PATH, configName);

        if (!File.Exists(configPath))
            File.Create(configPath).Close();
    }

    public string[] Read()
    {
        string content;

        using (StreamReader sR = new(configPath))
            content = sR.ReadToEnd();

        string[] split = content.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        
        return split;
    }
}