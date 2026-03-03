using System;
using System.Collections.Generic;
using System.IO;

namespace Sequencer.Utils;

public static class PatternManager
{   
    private const string FILE_NAME = "patterns";
    
    private static readonly Dictionary<string, Pattern> patterns = [];
    private static readonly string filePath = Path.Combine(Config.ProgramPath, FILE_NAME);

    public static string PatternFilePath { get { return filePath; } }
    public static string PatternFileName { get { return FILE_NAME; } }

    public static Exception? Init(string[] patternEntries)
    {
        foreach (var pattern in patternEntries)
        {
            if (pattern != null)
            {
                string[] split = pattern.Split('>', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (split.Length != 2)
                    continue;
                
                if (patterns.ContainsKey(split[0]))
                    return new Exception($"Pattern name conflict! '{split[0]}' is already existing name!");

                patterns.Add(split[0], new Pattern(split[1]));
            }
        }

        return null;
    }

    public static Pattern? Resolve(string name) => patterns.TryGetValue(name, out Pattern? value) ? value : null;
}