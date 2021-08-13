using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Utilities
{
    public static class GameTextParser
    {
        public const int ENEMY_ENTRY_COUNT_OFFSET = 41;

        public static Dictionary<string, string> ParseGameText(string language, bool useMasterFile = false)
        {
            string uassetPath;
            if (!useMasterFile && File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/GameText/Database/GameText{language}.uasset"))
            {
                uassetPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/GameText/Database/GameText{language}.uasset";
            }
            else
            {
                uassetPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/GameText/Database/GameText{language}.uasset";
            }

            string uexpPath;
            if (!useMasterFile && File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/GameText/Database/GameText{language}.uexp"))
            {
                uexpPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/GameText/Database/GameText{language}.uexp";
            }
            else
            {
                uexpPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/GameText/Database/GameText{language}.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonOctomodUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(uexpPath);
            int numOfEntries = BitConverter.ToInt32(allBytes, ENEMY_ENTRY_COUNT_OFFSET);
            int currentOffset = ENEMY_ENTRY_COUNT_OFFSET + 4;

            Dictionary<string, string> gameText = new Dictionary<string, string>();

            for(int i = 0; i < numOfEntries; i++)
            {
                string key = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
                currentOffset += 42;
                int keyLength = BitConverter.ToInt32(allBytes, currentOffset);
                if(keyLength > 0)
                {
                    currentOffset += keyLength + 4;
                    int length = BitConverter.ToInt32(allBytes, currentOffset);
                    currentOffset += 4;
                    string value = "";
                    if (length < 0)
                    {
                        length *= -2;
                        string unparsedValue = Encoding.ASCII.GetString(allBytes, currentOffset, length - 1);
                        for (int j = 0; j < unparsedValue.Length; j += 2)
                        {
                            value += unparsedValue[j];
                        }
                        gameText.Add(key, value);
                        currentOffset += length + 8;
                    }
                    else if (length > 0)
                    {
                        value = Encoding.ASCII.GetString(allBytes, currentOffset, length - 1);
                        gameText.Add(key, value);
                        currentOffset += length + 8;
                    }
                }
                else
                {
                    gameText.Add(key, "");
                    currentOffset += 4;
                }
            }
            return gameText;
        }
    }
}
