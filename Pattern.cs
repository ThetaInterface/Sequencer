using System;
using System.Collections.Generic;

namespace Sequencer.Utils;

public sealed class Pattern
{
    private readonly List<string> flow = [];

    public Pattern(string line) => flow = ExctractFlow(line);

    public static string InsertInFlow(string flow, string[] fileNames)
    {
        List<string> entries = ExctractFlow(flow); 

        for (int i = 0; i < entries.Count; i++)
        {
            if (int.TryParse(entries[i], out int number))
            {
                number--;

                if (number < fileNames.Length && number >= 0)
                    entries[i] = fileNames[number];
                
                continue;
            }

            var pattern = PatternManager.Resolve(entries[i]);

            if (pattern != null)
                entries[i] = pattern.GetFlowString();
        }

        string output = string.Empty;

        foreach (var entry in entries)
            output += entry + ',';
        
        return output;
    }

    private static List<string> ExctractFlow(string line)
    {
        List<string> flow = [];

        flow.AddRange(line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        return flow;
    }

    public string[] GetFlow() => flow.ToArray();

    public string GetFlowString()
    {
        string output = string.Empty;

        foreach (var entry in flow)
            output += entry + ',';
        
        return output;
    }
}