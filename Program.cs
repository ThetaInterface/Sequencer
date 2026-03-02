using System;
using System.Collections.Generic;
using System.IO;
using Sequencer.Utils;

namespace Sequencer;

public static class Program
{
    public static void Main()
    {
        Config whiteList = new ("white_list");
        Config patternList = new ("patterns");

        LinkManager.Initialize(whiteList.Read());
        PatternManager.Initialize(patternList.Read());

        string[] fileNames = LinkManager.Scan();

        if (fileNames.Length < 1)
        {
            PrintWarning($"'{Path.Combine(LinkManager.ShortcutDirectoryPath)}' contains no files!");

            return;
        }

        while (true)
        {
            Console.Clear();

            for (int i = 0; i < fileNames.Length; i++)
                Console.WriteLine($"{i + 1}) {fileNames[i]}");

            string? userInput = Console.ReadLine();

            if (userInput != null)
            {
                if (userInput.Equals("exit"))
                {
                    Console.Clear();

                    break;
                }
                else if (int.TryParse(userInput, out int index))
                {
                    if (index - 1 < fileNames.Length && index - 1 >= 0)
                        Processor.TryRun(fileNames[index - 1]);
                    else
                        PrintWarning($"'{userInput}' is not correct index!");
                }
                else if (userInput.Contains(','))
                {
                    string flow = Pattern.InsertInFlow(userInput, fileNames);

                    Pattern pattern = new (flow);
                    var result = Processor.TryRun(pattern, fileNames);

                    if (result != null)
                        PrintWarning(result.Message);
                }
                else
                {
                    Pattern? pattern = PatternManager.Resolve(userInput);

                    if (pattern != null)
                    {
                        var result = Processor.TryRun(pattern, fileNames);

                        if (result != null)
                            PrintWarning(result.Message);
                    }
                    else
                        PrintWarning($"'{userInput}' pattern doesn't exist! Please add it to 'patterns'!");
                }
            }
        }
    }

    private static void PrintWarning(string text)
    {
        Console.Clear();

        Console.WriteLine(text);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}