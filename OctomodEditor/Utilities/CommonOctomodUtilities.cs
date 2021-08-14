using DataEditorUE4.Utilities;
using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Utilities
{
    public static class CommonOctomodUtilities
    {
        public const int ENTRY_COUNT_OFFSET = 117;
        public const int ENTRY_LIST_START_OFFSET = 193;
        public static string BaseFilesLocation { get; set; }
        public static string ModLocation { get; set; }

        public static (string, string) GetProperUassetAndUexpReadPaths(string directory, string filePathPrefix, bool useBaseGame = false)
        {
            string uassetPath;
            if (!useBaseGame && File.Exists($"{ModLocation}/Octopath_Traveler/Content/{directory}{filePathPrefix}.uasset"))
            {
                uassetPath = $"{ModLocation}/Octopath_Traveler/Content/{directory}{filePathPrefix}.uasset";
            }
            else
            {
                uassetPath = $"{BaseFilesLocation}/Octopath_Traveler/Content/{directory}{filePathPrefix}.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{ModLocation}/Octopath_Traveler/Content/{directory}{filePathPrefix}.uexp"))
            {
                uexpPath = $"{ModLocation}/Octopath_Traveler/Content/{directory}{filePathPrefix}.uexp";
            }
            else
            {
                uexpPath = $"{BaseFilesLocation}/Octopath_Traveler/Content/{directory}{filePathPrefix}.uexp";
            }
            return (uassetPath, uexpPath);
        }

        public static (string, string) GetProperUassetAndUexpModWritePathsAndCreateDirectories(string directory, string filePathPrefix)
        {
            if (!Directory.Exists($"{ModLocation}/Octopath_Traveler/Content/{directory}"))
            {
                Directory.CreateDirectory($"{ModLocation}/Octopath_Traveler/Content/{directory}");
            }

            string uassetPath = $"{ModLocation}/Octopath_Traveler/Content/{directory}{filePathPrefix}.uasset";
            string uexpPath = $"{ModLocation}/Octopath_Traveler/Content/{directory}{filePathPrefix}.uexp";

            if (!File.Exists(uassetPath))
            {
                var openStream = File.Create(uassetPath);
                openStream.Close();
            }

            if (!File.Exists(uexpPath))
            {
                var openStream = File.Create(uassetPath);
                openStream.Close();
            }

            return (uassetPath, uexpPath);
        }

        public static bool AddSettingToConfig(KeyValuePair<string, string> setting)
        {
            try
            {
                string[] lines = File.ReadAllLines(string.Join("/", Directory.GetCurrentDirectory(), "config.octo"));
                Dictionary<string, string> settings = new Dictionary<string, string>();
                foreach (string line in lines)
                {
                    settings.Add(line.Split('=')[0], line.Split('=')[1]);
                }
                if (settings.ContainsKey(setting.Key))
                {
                    settings[setting.Key] = setting.Value;
                }
                else
                {
                    settings.Add(setting.Key, setting.Value);
                }
                List<string> newSettings = new List<string>();
                foreach(var newSetting in settings)
                {
                    newSettings.Add($"{newSetting.Key}={newSetting.Value}");
                }
                File.WriteAllLines(string.Join("/", Directory.GetCurrentDirectory(), "config.octo"), newSettings.ToArray());
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Enum to String Converters

        public static string ConvertEnumPrefixAndIndexToString(string prefix, int index)
        {
            return $"{prefix}::NewEnumerator{index}";
        }

        public static string ConvertAbilityTypeToString(AbilityType abilityType)
        {
            return ConvertEnumPrefixAndIndexToString("EABILITY_TYPE", (int)abilityType);
        }

        public static string ConvertAbilityUseTypeToString(AbilityUseType abilityUseType)
        {
            return ConvertEnumPrefixAndIndexToString("EABILITY_USE_TYPE", (int)abilityUseType);
        }

        public static string ConvertWeaponCategoryToString(WeaponCategory weaponCategory)
        {
            return ConvertEnumPrefixAndIndexToString("EWEAPON_CATEGORY", (int)weaponCategory);
        }

        public static string ConvertAbilityCostTypeToString(AbilityCostType abilityCostType)
        {
            return ConvertEnumPrefixAndIndexToString("EABILITY_COST_TYPE", (int)abilityCostType);
        }

        public static string ConvertAbilityOrderChangeTypeToString(AbilityOrderChangeType abilityOrderChangeType)
        {
            return ConvertEnumPrefixAndIndexToString("EABILITY_ORDER_CHANGE_TYPE", (int)abilityOrderChangeType);
        }

        public static string ConvertSupportAilmentToString(SupportAilmentType supportType)
        {
            return ConvertEnumPrefixAndIndexToString("ESUPPORT_AILMENT_TYPE", (int)supportType);
        }

        public static string ConvertItemCategoryToString(ItemCategory category)
        {
            return ConvertEnumPrefixAndIndexToString("EITEM_CATEGORY", (int)category);
        }

        public static string ConvertItemDisplayTypeToString(ItemDisplayType displayType)
        {
            return ConvertEnumPrefixAndIndexToString("EITEM_DISPLAY_TYPE", (int)displayType);
        }

        public static string ConvertItemUseTypeToString(ItemUseType useType)
        {
            return ConvertEnumPrefixAndIndexToString("EITEM_USE_TYPE", (int)useType);
        }

        public static string ConvertTargetTypeToString(TargetType targetType)
        {
            return ConvertEnumPrefixAndIndexToString("ETARGET_TYPE", (int)targetType);
        }

        public static string ConvertAttributeTypeToString(AttributeType attributeType)
        {
            return ConvertEnumPrefixAndIndexToString("EATTRIBUTE_TYPE", (int)attributeType);
        }

        public static string ConvertEquipmentCategoryToString(EquipmentCategory equipmentCategory)
        {
            return ConvertEnumPrefixAndIndexToString("EEQUIPMENT_CATEGORY", (int)equipmentCategory);
        }

        public static string ConvertShopTypeToString(ShopType shopType)
        {
            return ConvertEnumPrefixAndIndexToString("ESHOP_TYPE", (int)shopType);
        }

        public static string ConvertDeadTypeToString(EnemyDeadType deadType)
        {
            return ConvertEnumPrefixAndIndexToString("EENEMY_DEAD_TYPE", (int)deadType);
        }

        public static string ConvertSizeTypeToString(CharacterSize size)
        {
            return ConvertEnumPrefixAndIndexToString("ECHARACTER_SIZE", (int)size);
        }

        public static string ConvertAttributeResistanceToString(AttributeResistance resistance)
        {
            return ConvertEnumPrefixAndIndexToString("EATTRIBUTE_RESIST", (int)resistance);
        }

        public static string ConvertRaceTypeToString(CharacterRace race)
        {
            return ConvertEnumPrefixAndIndexToString("ECHARACTER_RACE", (int)race);
        }

        #endregion

        #region String to Enum Converters

        public static int GetEnumIndexFromEnumString(string enumString, string expectedPrefix = null)
        {
            string[] enumData = enumString.Split("::");
            string enumPrefix = enumData[0];
            string enumSuffix = enumData[1];
            int index;
            if (enumSuffix.StartsWith("NewEnumerator"))
            {
                index = int.Parse(enumSuffix.Replace("NewEnumerator", string.Empty));
            }
            else
            {
                throw new ArgumentException("Enum string was not in the correct format!");
            }

            if(expectedPrefix != null && expectedPrefix != enumPrefix)
            {
                throw new ArgumentException("Enum string prefix was unexpected!");
            }
            return index;
        }

        public static AbilityType ConvertStringToAbilityType(string abilityTypeString)
        {
            return (AbilityType)GetEnumIndexFromEnumString(abilityTypeString, "EABILITY_TYPE");
        }

        public static AbilityUseType ConvertStringToAbilityUseType(string abilityUseTypeString)
        {
            return (AbilityUseType)GetEnumIndexFromEnumString(abilityUseTypeString, "EABILITY_USE_TYPE");
        }

        public static WeaponCategory ConvertStringToWeaponType(string weaponCategoryString)
        {
            return (WeaponCategory)GetEnumIndexFromEnumString(weaponCategoryString, "EWEAPON_CATEGORY");
        }

        public static ItemCategory ConvertStringToItemCategory(string categoryString)
        {
            return (ItemCategory)GetEnumIndexFromEnumString(categoryString, "EITEM_CATEGORY");
        }

        public static ItemDisplayType ConvertStringToItemDisplayType(string displayTypeString)
        {
            return (ItemDisplayType)GetEnumIndexFromEnumString(displayTypeString, "EITEM_DISPLAY_TYPE");
        }

        public static ItemUseType ConvertStringToItemUseType(string useTypeString)
        {
            return (ItemUseType)GetEnumIndexFromEnumString(useTypeString, "EITEM_USE_TYPE");
        }

        public static TargetType ConvertStringToTargetType(string targetTypeString)
        {
            return (TargetType)GetEnumIndexFromEnumString(targetTypeString, "ETARGET_TYPE");
        }

        public static AttributeType ConvertStringToAttributeType(string attributeTypeString)
        {
            return (AttributeType)GetEnumIndexFromEnumString(attributeTypeString, "EATTRIBUTE_TYPE");
        }

        public static EquipmentCategory ConvertStringToItemEquipmentCategory(string equipmentCategoryString)
        {
            return (EquipmentCategory)GetEnumIndexFromEnumString(equipmentCategoryString, "EEQUIPMENT_CATEGORY");
        }

        public static AbilityCostType ConvertStringToAbilityCostType(string abilityCostTypeString)
        {
            return (AbilityCostType)GetEnumIndexFromEnumString(abilityCostTypeString, "EABILITY_COST_TYPE");
        }

        public static AbilityOrderChangeType ConvertStringToAbilityOrderChangeType(string abilityOrderChangeTypeString)
        {
            return (AbilityOrderChangeType)GetEnumIndexFromEnumString(abilityOrderChangeTypeString, "EABILITY_ORDER_CHANGE_TYPE");
        }

        public static SupportAilmentType ConvertStringToSupportAilment(string supportString)
        {
            return (SupportAilmentType)GetEnumIndexFromEnumString(supportString, "ESUPPORT_AILMENT_TYPE");
        }

        public static ShopType ConvertStringToShopType(string shopTypeString)
        {
            return (ShopType)GetEnumIndexFromEnumString(shopTypeString, "ESHOP_TYPE");
        }

        public static EnemyDeadType ConvertStringToDeadType(string deadString)
        {
            return (EnemyDeadType)GetEnumIndexFromEnumString(deadString, "EENEMY_DEAD_TYPE");
        }

        public static CharacterSize ConvertStringToSizeType(string sizeString)
        {
            return (CharacterSize)GetEnumIndexFromEnumString(sizeString, "ECHARACTER_SIZE");
        }

        public static AttributeResistance ConvertStringToAttributeResistance(string resistanceString)
        {
            return (AttributeResistance)GetEnumIndexFromEnumString(resistanceString, "EATTRIBUTE_RESIST");
        }

        //As of right now, it seems races went pretty much unused. This is more of a formality thing than anything.
        public static CharacterRace ConvertStringToRaceType(string raceString)
        {
            return (CharacterRace)GetEnumIndexFromEnumString(raceString, "ECHARACTER_RACE");
        }

        #endregion
    }
}
