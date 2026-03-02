using System;
using System.Collections.Generic;

namespace Sequencer.Utils;

public static class PatternManager
{
    private static readonly Dictionary<string, Pattern> patterns = [];

    public static void Initialize(string[] patternEntries)
    {
        foreach (var pattern in patternEntries)
        {
            if (pattern != null)
            {
                string[] split = pattern.Split('>', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (split.Length != 2)
                    continue;
                
                patterns.Add(split[0], new Pattern(split[1]));
            }
        }
    }

    public static Pattern? Resolve(string name) => patterns.TryGetValue(name, out Pattern? value) ? value : null;
}