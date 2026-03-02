using System;
using System.Collections.Generic;
using System.IO;

namespace Sequencer.Utils;

public sealed class Pattern
{
    private readonly List<string> flow = [];

    private readonly int fileCount = 0;
    private readonly int delayCount = 0;

    public Pattern(string line) => flow = ExctractFlow(line, out fileCount, out delayCount);

    public static string InsertInFlow(string flow, string[] fileNames)
    {
        List<string> entries = ExctractFlow(flow, out _, out _); 

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

    private static List<string> ExctractFlow(string line, out int fileCount, out int delayCount)
    {
        List<string> flow = [];

        fileCount = 0;
        delayCount = 0;

        foreach (var entry in line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (int.TryParse(entry, out _))
                delayCount++;
            else
                fileCount++;

            flow.Add(entry);
        }

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

    public int GetFileCount() => fileCount;
    public int GetDelayCount() => delayCount;
}