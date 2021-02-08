using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Utilities
{
    public static class CommonUtilities
    {
        public const int ENTRY_COUNT_OFFSET = 117;
        public const int ENTRY_LIST_START_OFFSET = 193;
        public static Dictionary<int, string> ParseUAssetFile(string path)
        {
            byte[] allBytes = File.ReadAllBytes(path);
            long numberOfEntries = BitConverter.ToInt64(allBytes, ENTRY_COUNT_OFFSET);
            int currentOffset = ENTRY_LIST_START_OFFSET;
            Dictionary<int, string> entries = new Dictionary<int, string>();
            for (int i = 0; i < numberOfEntries; i++)
            {
                int lengthOfString = BitConverter.ToInt32(allBytes, currentOffset) - 1;
                currentOffset += 4;

                byte[] entryArray = GetSubArray(allBytes, currentOffset, lengthOfString);
                string entry = Encoding.ASCII.GetString(entryArray);
                entries.Add(i, entry);

                currentOffset += lengthOfString + 5;
            }
            return entries;
        }

        public static byte[] GetSubArray(byte[] mainArray, int startIndex, int length)
        {
            byte[] subArray = new byte[length];
            Array.Copy(mainArray, startIndex, subArray, 0, length);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(subArray);
            }
            return subArray;
        }

        public static string ParseUAssetStringWithPossibleSuffix(byte[] mainArray, int currentOffset, Dictionary<int, string> uassetStrings)
        {
            string stringValue = uassetStrings[BitConverter.ToInt32(mainArray, currentOffset)];
            currentOffset += 4;
            if (BitConverter.ToInt32(mainArray, currentOffset) != 0)
            {
                stringValue += $"_{(BitConverter.ToInt32(mainArray, currentOffset) - 1):D3}";
            }
            return stringValue;
        }
    }
}
