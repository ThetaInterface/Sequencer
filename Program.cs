using System;
using System.IO;
using Sequencer.Utils;

using static Sequencer.Utils.EConfigOption;

namespace Sequencer;

public static class Program
{
    private const bool DEBUG = false;

    public static void Main()
    {
        Config whiteList = new (LinkManager.WhiteListFileName);
        Config patternList = new (PatternManager.PatternFileName);
        Config config = new (ConfigManager.ConfigFileName);

        LinkManager.Init(whiteList.Read());

        Exception? result = PatternManager.Init(patternList.Read());
        if (result != null)
        {
            PrintWarning($"{result.Message}");

            return;
        }

        ConfigManager.Init(config.Read());

        string separatorSymbol = ConfigManager.GetEntry(SeparatorSymbol);
        string exitPhrase = ConfigManager.GetEntry(ExitPhrase);

        bool showExtensions = ConfigManager.GetEntry(ShowExtension).Contains("true");
        bool autoClose = ConfigManager.GetEntry(AutoClose).Contains("true");
        
        string[] fileNames = LinkManager.Scan();

        if (fileNames.Length < 1)
        {
            PrintWarning($"'{Path.Combine(LinkManager.ShortcutDirectoryPath)}' contains no files!");

            return;
        }

        while (true)
        {
            if (!DEBUG)
                Console.Clear();

            for (int i = 0; i < fileNames.Length; i++)
                Console.WriteLine($"{i + 1}) {(showExtensions ? fileNames[i] : Path.GetFileNameWithoutExtension(fileNames[i]))}");

            string? userInput = Console.ReadLine();

            if (userInput != null)
            {
                if (userInput.Equals(exitPhrase))
                {
                    if (!DEBUG)
                        Console.Clear();

                    break;
                }
                else if (int.TryParse(userInput, out int index))
                {
                    if (index - 1 < fileNames.Length && index - 1 >= 0)
                    {
                        Processor.TryRun(fileNames[index - 1]);

                        if (autoClose)
                        {
                            if (!DEBUG)
                                Console.Clear();

                            return;
                        }
                    }
                    else
                        PrintWarning($"'{userInput}' is not correct index!");
                }
                else if (userInput.Contains(separatorSymbol))
                {
                    string flow = Pattern.InsertInFlow(userInput, fileNames);

                    Pattern pattern = new (flow);
                    result = Processor.TryRun(pattern);

                    if (result != null)
                        PrintWarning(result.Message);
                    else
                        if (autoClose)
                        {
                            if (!DEBUG)
                                Console.Clear();

                            return;
                        }
                }
                else
                {
                    Pattern? pattern = PatternManager.Resolve(userInput);

                    if (pattern != null)
                    {
                        result = Processor.TryRun(pattern);

                        if (result != null)
                            PrintWarning(result.Message);
                        else
                            if (autoClose)
                            {
                                if (!DEBUG)
                                    Console.Clear();

                                return;
                            }
                    }
                    else
                        PrintWarning($"'{userInput}' pattern doesn't exist! Please add it to 'patterns'!");
                }
            }
        }
    }

    private static void PrintWarning(string text)
    {
        if (!DEBUG)
            Console.Clear();

        Console.WriteLine(text);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}