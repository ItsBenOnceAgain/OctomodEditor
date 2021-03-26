using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Utilities
{
    public class AbilityDataParser
    {
        public const int ENTRY_COUNT_OFFSET = 0x29;

        public static Dictionary<string, Ability> ParseAbilityObjects(bool useBaseGame = false)
        {
            string uassetPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Ability/Database/AbilityData.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Ability/Database/AbilityData.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Ability/Database/AbilityData.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Ability/Database/AbilityData.uexp"))
            {
                uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Ability/Database/AbilityData.uexp";
            }
            else
            {
                uexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Ability/Database/AbilityData.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(uexpPath);
            int numOfEntries = BitConverter.ToInt32(allBytes, ENTRY_COUNT_OFFSET);
            int currentOffset = ENTRY_COUNT_OFFSET + 4;
            Dictionary<string, Ability> abilityObjects = new Dictionary<string, Ability>();

            for (int i = 0; i < numOfEntries; i++)
            {
                var item = ParseSingleAbility(uassetStrings, allBytes, ref currentOffset);
                abilityObjects.Add(item.Key, item);
            }

            return abilityObjects;
        }

        public static Ability ParseSingleAbility(Dictionary<int, string> uassetStrings, byte[] allBytes, ref int currentOffset)
        {
            int abilityOffset = currentOffset;
            string key = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int abilityId = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string displayName = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string detail = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string commandActor = ConvertIntToCommandActor(BitConverter.ToInt32(allBytes, currentOffset), uassetStrings);
            currentOffset += 37;
            AbilityType abilityType = CommonUtilities.ConvertStringToAbilityType(CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 41;
            AbilityUseType useType = CommonUtilities.ConvertStringToUseType(CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 32;
            bool alwaysDisable = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 35;
            AttributeType attribute = CommonUtilities.ConvertStringToItemAttributeType(CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 32;
            bool dependWeapon = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 35;
            WeaponCategory restrictWeapon = CommonUtilities.ConvertStringToWeaponType(CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 41;
            TargetType targetType = CommonUtilities.ConvertStringToTargetType(CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 32;
            bool exceptEnforcer = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 35;
            AbilityCostType costType = CommonUtilities.ConvertStringToAbilityCostType(CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 33;
            int costValue = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int hitRatio = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int criticalRatio = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int abilityRatio = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            AbilityOrderChangeType orderChange = CommonUtilities.ConvertStringToAbilityOrderChangeType(CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 41;
            int numOfAilments = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 78;
            Ailment[] ailments = new Ailment[numOfAilments];
            for (int i = 0; i < ailments.Length; i++)
            {
                string ailmentName = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
                currentOffset += 33;
                int invocationValue = BitConverter.ToInt32(allBytes, currentOffset);
                currentOffset += 29;
                int invocationTurn = BitConverter.ToInt32(allBytes, currentOffset);
                currentOffset += 29;
                int diseaseRate = BitConverter.ToInt32(allBytes, currentOffset);
                ailments[i] = new Ailment() { AilmentName = ailmentName, InvocationValue = invocationValue, InvocationTurn = invocationTurn, DiseaseRate = diseaseRate };
                currentOffset += i == ailments.Length - 1 ? 45 : 37;
            }
            SupportAilmentType supportAilment = CommonUtilities.ConvertStringToSupportAilment(CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 33;
            string commandEffecter = ConvertIntToCommandEffecter(BitConverter.ToInt32(allBytes, currentOffset), uassetStrings);
            currentOffset += 28;
            bool keepBosstEffect = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableItemAll = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableSkillAll = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableConvergence = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableDiffusion = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableReflection = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableCounter = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableHideOut = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableRepeat = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableCover = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableDisableMagic = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableEnchant = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool enableChaseAttack = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 10;

            return new Ability()
            {
                Offset = abilityOffset,
                Key = key,
                AbilityID = abilityId,
                DisplayName = displayName,
                Detail = detail,
                CommandActor = commandActor,
                AbilityType = abilityType,
                AbilityUseType = useType,
                AlwaysDisable = alwaysDisable,
                AbilityAttributeType = attribute,
                DependWeapon = dependWeapon,
                RestrictWeapon = restrictWeapon,
                TargetType = targetType,
                ExceptEnforcer = exceptEnforcer,
                CostType = costType,
                CostValue = costValue,
                HitRatio = hitRatio,
                CriticalRatio = criticalRatio,
                AbilityRatio = abilityRatio,
                OrderChange = orderChange,
                Ailments = ailments,
                SupportAilment = supportAilment,
                CommandEffecter = commandEffecter,
                KeepBoostEffect = keepBosstEffect,
                EnableItemAll = enableItemAll,
                EnableSkillAll = enableSkillAll,
                EnableConvergence = enableConvergence,
                EnableDiffusion = enableDiffusion,
                EnableReflection = enableReflection,
                EnableCounter = enableCounter,
                EnableHideOut = enableHideOut,
                EnableRepeat = enableRepeat,
                EnableCover = enableCover,
                EnableDisableMagic = enableDisableMagic,
                EnableEnchant = enableEnchant,
                EnableChaseAttack = enableChaseAttack
            };
        }

        private static string ConvertIntToCommandEffecter(int v, Dictionary<int, string> uassetStrings)
        {
            return null;
        }

        private static string ConvertIntToCommandActor(int v, Dictionary<int, string> uassetStrings)
        {
            return null;
        }
    }
}
