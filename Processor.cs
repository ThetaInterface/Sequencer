using System.IO;
using System.Diagnostics;
using System.Threading;
using System;

namespace Sequencer.Utils;

public static class Processor
{
    public static Exception? TryRun(string path)
    {
        path = Path.Combine(LinkManager.ShortcutDirectoryPath, path);

        if (!File.Exists(path))
            return new Exception($"{path} wasn't found!");

        ProcessStartInfo startInfo;

        if (ConfigManager.GetEntry(EConfigOption.RunAsChild).Contains(Path.GetFileName(path)))
        {
            startInfo = new (path) 
            {
                UseShellExecute = true
            };
        }
        else
        {
            startInfo = new () 
            {
                FileName = "cmd.exe",
                Arguments = $"/c start \"\" \"{path}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        try 
        {
            Process.Start(startInfo);
        }
        catch
        {
            return new Exception($"Unable to run {path}!");
        }

        return null;
    }

    public static Exception? TryRun(Pattern pattern)
    {   
        string[] flow = pattern.GetFlow();
        
        foreach (var entry in flow)
        {
            if (int.TryParse(entry, out int delay))
                Thread.Sleep(delay);
            else
            {
                var result = TryRun(entry);

                if (result != null)
                    return result;
            }
        }

        return null;
    }
}