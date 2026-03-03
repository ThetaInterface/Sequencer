using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sequencer.Utils;

public static class LinkManager
{
    private const string FOLDER_NAME = "shortcuts";
    private const string WHITE_LIST_FILE_NAME = "white_list";

    private static readonly string shortcutDir = Path.Combine(Config.ProgramPath, FOLDER_NAME);
    private static string[] extensionWhiteList = [];

    public static string ShortcutDirectoryPath { get { return shortcutDir; } }
    public static string ShortcutFolderName { get { return FOLDER_NAME; } }
    public static string WhiteListFileName { get { return WHITE_LIST_FILE_NAME; } }

    public static void Init(string[] extensionEntries)
    {
        extensionWhiteList = (string[])extensionEntries.Clone();

        for (int i = 0; i < extensionWhiteList.Length; i++)
        {
            if (!extensionWhiteList[i].StartsWith('.'))
                extensionWhiteList[i] = "." + extensionWhiteList[i];
        }
    }

    public static string[] Scan()
    {
        string[] paths;

        if (!Directory.Exists(shortcutDir))
            Directory.CreateDirectory(shortcutDir);

        paths = Directory.GetFiles(shortcutDir);

        if (paths.Length > 0)
        {
            for (int i = 0; i < paths.Length; i++)
                paths[i] = Path.GetFileName(paths[i]);

            if (extensionWhiteList?.Length > 0)
            {
                List<string> sorted = [];

                foreach (string path in paths)
                    if (extensionWhiteList.Contains(Path.GetExtension(path)))
                        sorted.Add(path);
                
                paths = sorted.ToArray();
            }
        }

        return paths;
    }
}