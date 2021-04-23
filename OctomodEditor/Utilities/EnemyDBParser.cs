using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Utilities
{
    public static class EnemyDBParser
    {
        public const int ENEMY_ENTRY_COUNT_OFFSET = 41;
        public static Dictionary<string, Enemy> ParseEnemyObjects(bool useBaseGame = false)
        {
            string uassetPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp"))
            {
                uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";
            }
            else
            {
                uexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(uexpPath);
            int numOfEntries = BitConverter.ToInt32(allBytes, ENEMY_ENTRY_COUNT_OFFSET);
            int currentOffset = ENEMY_ENTRY_COUNT_OFFSET + 4;
            Dictionary<string, Enemy> enemyObjects = new Dictionary<string, Enemy>();

            for(int i = 0; i < numOfEntries; i++)
            {
                var enemy = ParseSingleEnemy(uassetStrings, allBytes, ref currentOffset);
                enemyObjects.Add(enemy.Key, enemy);
            }

            return enemyObjects;
        }

        public static void SaveEnemies(List<Enemy> enemies)
        {
            if (!Directory.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/"))
            {
                Directory.CreateDirectory($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/");
            }

            string uassetPath;
            if (File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset";
            }
            string oldUexpPath;
            if (File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp"))
            {
                oldUexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";
            }
            else
            {
                oldUexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";
            }

            string uexpPath;
            if (!File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp"))
            {
                var openStream = File.Create($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp");
                openStream.Close();
            }
            uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(oldUexpPath);
            foreach(var enemy in enemies)
            {
                SaveEnemy(enemy, allBytes, uassetStrings, uassetPath);
            }
            File.WriteAllBytes(uexpPath, allBytes);
        }

        private static void UpdateBytesAtOffset(byte[] updateBytes, byte[] allBytes, int currentOffset)
        {
            for(int i = 0; i < updateBytes.Length; i++)
            {
                allBytes[currentOffset + i] = updateBytes[i];
            }
        }

        public static void SaveEnemy(Enemy enemy, byte[] allBytes, Dictionary<int, string> uassetStrings, string uassetPath)
        {
            int currentOffset = enemy.Offset + 95;
            byte[] flipbookData = GetBytesFromStringWithPossibleSuffix(enemy.FlipbookPath, uassetStrings, uassetPath);
            UpdateBytesAtOffset(flipbookData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] textureData = GetBytesFromStringWithPossibleSuffix(enemy.TexturePath, uassetStrings, uassetPath);
            UpdateBytesAtOffset(textureData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] displayRankData = BitConverter.GetBytes(enemy.DisplayRank);
            UpdateBytesAtOffset(displayRankData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] enemyLevelData = BitConverter.GetBytes(enemy.EnemyLevel);
            UpdateBytesAtOffset(enemyLevelData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] damageRatioData = BitConverter.GetBytes((float)enemy.DamageRatio);
            UpdateBytesAtOffset(damageRatioData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] enemyRaceData = ConvertRaceTypeToBytes(enemy.RaceType, uassetStrings);
            UpdateBytesAtOffset(enemyRaceData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] enemySizeData = ConvertSizeTypeToBytes(enemy.Size, uassetStrings);
            UpdateBytesAtOffset(enemySizeData, allBytes, currentOffset);
            currentOffset += 32;
            byte[] isNPCData = BitConverter.GetBytes(enemy.IsNPC);
            UpdateBytesAtOffset(isNPCData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] playsSlowDeathAnimData = BitConverter.GetBytes(enemy.PlaysSlowAnimationOnDeath);
            UpdateBytesAtOffset(playsSlowDeathAnimData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] isMainEnemyData = BitConverter.GetBytes(enemy.IsMainEnemy);
            UpdateBytesAtOffset(isMainEnemyData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] isExemptFromBattleData = BitConverter.GetBytes(enemy.IsExemptFromBattle);
            UpdateBytesAtOffset(isExemptFromBattleData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] usesCatDamageData = BitConverter.GetBytes(enemy.UsesCatDamageType);
            UpdateBytesAtOffset(usesCatDamageData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] hasNoKnockbackData = BitConverter.GetBytes(enemy.HasNoKnockbackAnimation);
            UpdateBytesAtOffset(hasNoKnockbackData, allBytes, currentOffset);
            currentOffset += 76;
            byte[] hpData = BitConverter.GetBytes(enemy.HP);
            UpdateBytesAtOffset(hpData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] mpData = BitConverter.GetBytes(enemy.MP);
            UpdateBytesAtOffset(mpData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] bpData = BitConverter.GetBytes(enemy.BP);
            UpdateBytesAtOffset(bpData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] shieldsData = BitConverter.GetBytes(enemy.Shields);
            UpdateBytesAtOffset(shieldsData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] physAttackData = BitConverter.GetBytes(enemy.PhysicalAttack);
            UpdateBytesAtOffset(physAttackData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] physDefData = BitConverter.GetBytes(enemy.PhysicalDefense);
            UpdateBytesAtOffset(physDefData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] elemAttackData = BitConverter.GetBytes(enemy.ElementalAttack);
            UpdateBytesAtOffset(elemAttackData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] elemDefData = BitConverter.GetBytes(enemy.ElementalDefense);
            UpdateBytesAtOffset(elemDefData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] accuracyData = BitConverter.GetBytes(enemy.Accuracy);
            UpdateBytesAtOffset(accuracyData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] evasionData = BitConverter.GetBytes(enemy.Evasion);
            UpdateBytesAtOffset(evasionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] criticalData = BitConverter.GetBytes(enemy.Critical);
            UpdateBytesAtOffset(criticalData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] speedData = BitConverter.GetBytes(enemy.Speed);
            UpdateBytesAtOffset(speedData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] experienceData = BitConverter.GetBytes(enemy.ExperiencePoints);
            UpdateBytesAtOffset(experienceData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] jobPointsData = BitConverter.GetBytes(enemy.JobPoints);
            UpdateBytesAtOffset(jobPointsData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] moneyData = BitConverter.GetBytes(enemy.Money);
            UpdateBytesAtOffset(moneyData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] stealableMoneyData = BitConverter.GetBytes(enemy.MoneyFromCollecting);
            UpdateBytesAtOffset(stealableMoneyData, allBytes, currentOffset);
            currentOffset += 28;
            byte[] canBeCapturedData = BitConverter.GetBytes(enemy.CanBeCaptured);
            UpdateBytesAtOffset(canBeCapturedData, allBytes, currentOffset);
            currentOffset += 27;
            byte[] tameRateData = BitConverter.GetBytes((float)enemy.DefaultTameRate);
            UpdateBytesAtOffset(tameRateData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] capturedEnemyIdData = GetBytesFromStringWithPossibleSuffix(enemy.CapturedEnemyID, uassetStrings, uassetPath);
            UpdateBytesAtOffset(capturedEnemyIdData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] firstBPData = BitConverter.GetBytes(enemy.FirstBP);
            UpdateBytesAtOffset(firstBPData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] breakTypeData = GetBytesFromStringWithPossibleSuffix(enemy.BreakType, uassetStrings, uassetPath);
            UpdateBytesAtOffset(breakTypeData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] invocationValueData = BitConverter.GetBytes(enemy.InvocationValue);
            UpdateBytesAtOffset(invocationValueData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] invocationTurnData = BitConverter.GetBytes(enemy.InvocationTurn);
            UpdateBytesAtOffset(invocationTurnData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] deadTypeBytes = ConvertDeadTypeToBytes(enemy.DeadType, uassetStrings);
            UpdateBytesAtOffset(deadTypeBytes, allBytes, currentOffset);
            currentOffset += 53;
            byte[] isWeakToFireBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToFire, uassetStrings);
            UpdateBytesAtOffset(isWeakToFireBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToIceBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToIce, uassetStrings);
            UpdateBytesAtOffset(isWeakToIceBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToLightningBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToLightning, uassetStrings);
            UpdateBytesAtOffset(isWeakToLightningBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToWindBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToWind, uassetStrings);
            UpdateBytesAtOffset(isWeakToWindBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToLightBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToLight, uassetStrings);
            UpdateBytesAtOffset(isWeakToLightBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToDarkBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToDark, uassetStrings);
            UpdateBytesAtOffset(isWeakToDarkBytes, allBytes, currentOffset);
            currentOffset += 45;
            byte[] isWeakToSwordsBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToSwords, uassetStrings);
            UpdateBytesAtOffset(isWeakToSwordsBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToSpearsBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToSpears, uassetStrings);
            UpdateBytesAtOffset(isWeakToSpearsBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToDaggersBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToDaggers, uassetStrings);
            UpdateBytesAtOffset(isWeakToDaggersBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToAxesBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToAxes, uassetStrings);
            UpdateBytesAtOffset(isWeakToAxesBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToBowsBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToBows, uassetStrings);
            UpdateBytesAtOffset(isWeakToBowsBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToStavesBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToStaves, uassetStrings);
            UpdateBytesAtOffset(isWeakToStavesBytes, allBytes, currentOffset);
            currentOffset += 53;
            UpdateDiseaseResistanceBytes(enemy.AttributeResistances, allBytes, uassetStrings, ref currentOffset);
            currentOffset += 24;
            byte[] isGuardedFromStealingData = BitConverter.GetBytes(enemy.IsGuardedFromStealing);
            UpdateBytesAtOffset(isGuardedFromStealingData, allBytes, currentOffset);
            currentOffset += 27;
            byte[] itemIdData = GetBytesFromStringWithPossibleSuffix(enemy.ItemID, uassetStrings, uassetPath);
            UpdateBytesAtOffset(itemIdData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] itemDropPercentageData = BitConverter.GetBytes(enemy.ItemDropPercentage);
            UpdateBytesAtOffset(itemDropPercentageData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] aiPathData = GetBytesFromStringWithPossibleSuffix(enemy.AIPath, uassetStrings, uassetPath);
            UpdateBytesAtOffset(aiPathData, allBytes, currentOffset);
            currentOffset += 49;
            UpdateAbilityEventBytes(enemy.AbilityList, allBytes, uassetStrings, ref currentOffset);
            currentOffset += 37;
            UpdateAbilityEventBytes(enemy.BattleEvents, allBytes, uassetStrings, ref currentOffset);
            currentOffset += 49;
            UpdateVector3Bytes(enemy.DiseaseOffset, allBytes, ref currentOffset);
            currentOffset += 49;
            UpdateVector3Bytes(enemy.EnemyEffectOffset, allBytes, ref currentOffset);
            currentOffset += 49;
            UpdateVector3Bytes(enemy.StatusUIOffset, allBytes, ref currentOffset);
            currentOffset += 49;
            UpdateVector3Bytes(enemy.DamageUIOffset, allBytes, ref currentOffset);
            currentOffset += 49;
            UpdateVector2Bytes(enemy.IconL, allBytes, ref currentOffset);
            currentOffset += 49;
            UpdateVector2Bytes(enemy.PixelScaleL, allBytes, ref currentOffset);
            currentOffset += 49;
            UpdateVector2Bytes(enemy.IconS, allBytes, ref currentOffset);
            currentOffset += 49;
            UpdateVector2Bytes(enemy.PixelScaleS, allBytes, ref currentOffset);
            currentOffset += 8;
        }

        public static byte[] GetBytesFromAttributeResistanceType(Dictionary<int, string> uassetStrings, AttributeResistance resistance)
        {
            int uassetValue = 0;
            switch (resistance)
            {
                case AttributeResistance.NONE:
                    uassetValue = uassetStrings.Single(x => x.Value == "EATTRIBUTE_RESIST::NewEnumerator0").Key;
                    break;
                case AttributeResistance.WEAK:
                    uassetValue = uassetStrings.Single(x => x.Value == "EATTRIBUTE_RESIST::NewEnumerator1").Key;
                    break;
                case AttributeResistance.REDUCE:
                    uassetValue = uassetStrings.Single(x => x.Value == "EATTRIBUTE_RESIST::NewEnumerator2").Key;
                    break;
                case AttributeResistance.INVALID:
                    uassetValue = uassetStrings.Single(x => x.Value == "EATTRIBUTE_RESIST::NewEnumerator3").Key;
                    break;
                default:
                    throw new ArgumentException("Received a string that did not match an attribute resistance.");
            }
            return BitConverter.GetBytes(uassetValue);
        }

        public static Enemy ParseSingleEnemy(Dictionary<int, string> uassetStrings, byte[] allBytes, ref int currentOffset)
        {
            int enemyOffset = currentOffset;
            string key = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int enemyId = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string displayNameId = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string flipbookPath = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
            currentOffset += 37;
            string texturePath = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
            currentOffset += 37;
            int displayRank = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int enemyLevel = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            double damageRatio = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 37;
            CharacterRace enemyRace = ConvertStringToRaceType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 41;
            CharacterSize enemySize = ConvertStringToSizeType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 32;
            bool isNPC = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool playsSlowDeathAnim = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool isMainEnemy = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool isExemptFromBattle = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool usesCatDamage = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 26;
            bool hasNoKnockback = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 76;
            int hp = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int mp = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int bp = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int shields = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int physAttack = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int physDef = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int elemAttack = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int elemDef = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int accuracy = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int evasion = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int critical = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int speed = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            int experience = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int jobPoints = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int money = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int stealableMoney = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 28;
            bool canBeCaptured = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 27;
            double tameRate = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 29;
            string capturedEnemyId = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
            currentOffset += 33;
            int firstBP = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            string breakType = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
            currentOffset += 33;
            int invocationValue = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int invocationTurn = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            EnemyDeadType deadType = ConvertStringToDeadType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 53;
            bool isWeakToFire = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToIce = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToLightning = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToWind = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToLight = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToDark = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 45;
            bool isWeakToSwords = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToSpears = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToDaggers = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToAxes = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToBows = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 8;
            bool isWeakToStaves = ConvertStringToAttributeBool(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 53;
            AttributeResistance[] attributeResistances = GetDiseaseResistances(allBytes, uassetStrings, ref currentOffset);
            currentOffset += 24;
            bool isGuardedFromStealing = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 27;
            string itemId = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int itemDropPercentage = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string aiPath = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
            currentOffset += 45;
            string[] abilities = GetAbilityList(allBytes, uassetStrings, ref currentOffset);
            currentOffset += 37;
            string[] eventList = GetEventList(allBytes, uassetStrings, ref currentOffset);
            currentOffset += 49;
            Vector3 diseaseEffectOffset = GetVector3Values(allBytes, ref currentOffset);
            currentOffset += 49;
            Vector3 enemyEffectOffset = GetVector3Values(allBytes, ref currentOffset);
            currentOffset += 49;
            Vector3 statusUIOffset = GetVector3Values(allBytes, ref currentOffset);
            currentOffset += 49;
            Vector3 damageUIOffset = GetVector3Values(allBytes, ref currentOffset);
            currentOffset += 49;
            Vector2 iconL = GetVector2Values(allBytes, ref currentOffset);
            currentOffset += 49;
            Vector2 pixelScaleL = GetVector2Values(allBytes, ref currentOffset);
            currentOffset += 49;
            Vector2 iconS = GetVector2Values(allBytes, ref currentOffset);
            currentOffset += 49;
            Vector2 pixelScaleS = GetVector2Values(allBytes, ref currentOffset);
            currentOffset += 8;

            return new Enemy()
            {
                Offset = enemyOffset,
                Key = key,
                EnemyID = enemyId,
                DisplayNameID = displayNameId,
                FlipbookPath = flipbookPath,
                TexturePath = texturePath,
                DisplayRank = displayRank,
                EnemyLevel = enemyLevel,
                DamageRatio = damageRatio,
                RaceType = enemyRace,
                Size = enemySize,
                IsNPC = isNPC,
                PlaysSlowAnimationOnDeath = playsSlowDeathAnim,
                IsMainEnemy = isMainEnemy,
                IsExemptFromBattle = isExemptFromBattle,
                UsesCatDamageType = usesCatDamage,
                HasNoKnockbackAnimation = hasNoKnockback,
                HP = hp,
                MP = mp,
                BP = bp,
                Shields = shields,
                PhysicalAttack = physAttack,
                PhysicalDefense = physDef,
                ElementalAttack = elemAttack,
                ElementalDefense = elemDef,
                Accuracy = accuracy,
                Evasion = evasion,
                Critical = critical,
                Speed = speed,
                ExperiencePoints = experience,
                JobPoints = jobPoints,
                Money = money,
                MoneyFromCollecting = stealableMoney,
                CanBeCaptured = canBeCaptured,
                DefaultTameRate = tameRate,
                CapturedEnemyID = capturedEnemyId,
                FirstBP = firstBP,
                BreakType = breakType,
                InvocationValue = invocationValue,
                InvocationTurn = invocationTurn,
                DeadType = deadType,
                IsWeakToSwords = isWeakToSwords,
                IsWeakToSpears = isWeakToSpears,
                IsWeakToDaggers = isWeakToDaggers,
                IsWeakToAxes = isWeakToAxes,
                IsWeakToBows = isWeakToBows,
                IsWeakToStaves = isWeakToStaves,
                IsWeakToFire = isWeakToFire,
                IsWeakToIce = isWeakToIce,
                IsWeakToLightning = isWeakToLightning,
                IsWeakToWind = isWeakToWind,
                IsWeakToLight = isWeakToLight,
                IsWeakToDark = isWeakToDark,
                AttributeResistances = attributeResistances,
                IsGuardedFromStealing = isGuardedFromStealing,
                ItemID = itemId,
                ItemDropPercentage = itemDropPercentage,
                AIPath = aiPath,
                AbilityList = abilities,
                BattleEvents = eventList,
                DiseaseOffset = diseaseEffectOffset,
                EnemyEffectOffset = enemyEffectOffset,
                StatusUIOffset = statusUIOffset,
                DamageUIOffset = damageUIOffset,
                IconL = iconL,
                PixelScaleL = pixelScaleL,
                IconS = iconS,
                PixelScaleS = pixelScaleS
            };
        }

        public static Vector2 GetVector2Values(byte[] allBytes, ref int currentOffset)
        {
            float x = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 4;
            float y = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 4;
            return new Vector2(x, y);
        }

        public static Vector3 GetVector3Values(byte[] allBytes, ref int currentOffset)
        {
            float x = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 4;
            float y = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 4;
            float z = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 4;
            return new Vector3(x, y, z);
        }

        public static void UpdateVector3Bytes(Vector3 values, byte[] allBytes, ref int currentOffset)
        {
            byte[] xData = BitConverter.GetBytes(values.X);
            UpdateBytesAtOffset(xData, allBytes, currentOffset);
            currentOffset += 4;
            byte[] yData = BitConverter.GetBytes(values.Y);
            UpdateBytesAtOffset(yData, allBytes, currentOffset);
            currentOffset += 4;
            byte[] zData = BitConverter.GetBytes(values.Z);
            UpdateBytesAtOffset(zData, allBytes, currentOffset);
            currentOffset += 4;
        }

        public static void UpdateVector2Bytes(Vector2 values, byte[] allBytes, ref int currentOffset)
        {
            byte[] xData = BitConverter.GetBytes(values.X);
            UpdateBytesAtOffset(xData, allBytes, currentOffset);
            currentOffset += 4;
            byte[] yData = BitConverter.GetBytes(values.Y);
            UpdateBytesAtOffset(yData, allBytes, currentOffset);
            currentOffset += 4;
        }

        public static string[] GetEventList(byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset)
        {
            string[] eventList = new string[3];
            for (int i = 0; i < 3; i++)
            {
                eventList[i] = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
                currentOffset += 8;
            }
            return eventList;
        }

        public static string[] GetAbilityList(byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset)
        {
            int numOfAbilities = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 4;
            string[] abilityList = new string[numOfAbilities];
            for(int i = 0; i < numOfAbilities; i++)
            {
                abilityList[i] = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
                currentOffset += 8;
            }
            return abilityList;
        }

        public static void UpdateAbilityEventBytes(string[] abilities, byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset)
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                byte[] abilityBytes = new byte[8];
                if (uassetStrings.ContainsValue(abilities[i]))
                {
                    UpdateBytesAtOffset(BitConverter.GetBytes(uassetStrings.Single(x => x.Value == abilities[i]).Key), abilityBytes, 0);
                }
                else
                {
                    string[] abilityData = abilities[i].Split('_');
                    string abilityPrefix = string.Join("_", abilityData.ToList().Where(x => x != abilityData.Last()));
                    int abilitySuffix = int.Parse(abilityData.Last()) + 1;
                    byte[] abilityNumberValue = BitConverter.GetBytes(abilitySuffix);
                    UpdateBytesAtOffset(BitConverter.GetBytes(uassetStrings.Single(x => x.Value == abilityPrefix).Key), abilityBytes, 0);
                    UpdateBytesAtOffset(abilityNumberValue, abilityBytes, 4);
                }
                UpdateBytesAtOffset(abilityBytes, allBytes, currentOffset);
                currentOffset += 8;
            }
        }

        public static void UpdateDiseaseResistanceBytes(AttributeResistance[] resistances, byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset)
        {
            for (int i = 0; i < 12; i++)
            {
                string value;
                switch (resistances[i])
                {
                    case AttributeResistance.NONE:
                        value = "EATTRIBUTE_RESIST::NewEnumerator0";
                        break;
                    case AttributeResistance.WEAK:
                        value = "EATTRIBUTE_RESIST::NewEnumerator1";
                        break;
                    case AttributeResistance.REDUCE:
                        value = "EATTRIBUTE_RESIST::NewEnumerator2";
                        break;
                    case AttributeResistance.INVALID:
                        value = "EATTRIBUTE_RESIST::NewEnumerator3";
                        break;
                    default:
                        throw new ArgumentException("Received a string that did not match an attribute resistance.");
                }
                byte[] resistanceBytes = BitConverter.GetBytes(uassetStrings.Single(x => x.Value == value).Key);
                UpdateBytesAtOffset(resistanceBytes, allBytes, currentOffset);

                currentOffset += 8;
            }
        }

        public static AttributeResistance[] GetDiseaseResistances(byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset)
        {
            AttributeResistance[] resistances = new AttributeResistance[12];
            for(int i = 0; i < 12; i++)
            {
                string value = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
                switch (value)
                {
                    case "EATTRIBUTE_RESIST::NewEnumerator0":
                        resistances[i] = AttributeResistance.NONE;
                        break;
                    // I don't think any diseases should use this but it is an available enum anyway...
                    case "EATTRIBUTE_RESIST::NewEnumerator1":
                        resistances[i] = AttributeResistance.WEAK;
                        break;
                    case "EATTRIBUTE_RESIST::NewEnumerator2":
                        resistances[i] = AttributeResistance.REDUCE;
                        break;
                    case "EATTRIBUTE_RESIST::NewEnumerator3":
                        resistances[i] = AttributeResistance.INVALID;
                        break;
                    default:
                        throw new ArgumentException("Received a string that did not match an attribute resistance.");
                }

                currentOffset += 8;
            }
            return resistances;
        }

        public static byte[] ConvertAttributeBoolToBytes(bool attributeBool, Dictionary<int, string> uassetStrings)
        {
            string attributeString = attributeBool ? "EATTRIBUTE_RESIST::NewEnumerator1" : "EATTRIBUTE_RESIST::NewEnumerator0";
            return BitConverter.GetBytes(uassetStrings.Single(x => x.Value == attributeString).Key);
        }

        public static bool ConvertStringToAttributeBool(string attributeString)
        {
            return attributeString == "EATTRIBUTE_RESIST::NewEnumerator1";
        }

        //As of right now, it seems races went pretty much unused. This is more of a formality thing than anything.
        public static CharacterRace ConvertStringToRaceType(string raceString)
        {
            CharacterRace race;
            switch (raceString)
            {
                case "ECHARACTER_RACE::NewEnumerator0":
                    race = CharacterRace.UNKNOWN;
                    break;
                case "ECHARACTER_RACE::NewEnumerator1":
                    race = CharacterRace.HUMAN;
                    break;
                case "ECHARACTER_RACE::NewEnumerator2":
                    race = CharacterRace.BEAST;
                    break;
                case "ECHARACTER_RACE::NewEnumerator3":
                    race = CharacterRace.INSECT;
                    break;
                case "ECHARACTER_RACE::NewEnumerator4":
                    race = CharacterRace.BIRD;
                    break;
                case "ECHARACTER_RACE::NewEnumerator5":
                    race = CharacterRace.FISH;
                    break;
                case "ECHARACTER_RACE::NewEnumerator6":
                    race = CharacterRace.DRAGON;
                    break;
                case "ECHARACTER_RACE::NewEnumerator7":
                    race = CharacterRace.PLANT;
                    break;
                case "ECHARACTER_RACE::NewEnumerator8":
                    race = CharacterRace.CHIMERA;
                    break;
                case "ECHARACTER_RACE::NewEnumerator9":
                    race = CharacterRace.SHELL;
                    break;
                case "ECHARACTER_RACE::NewEnumerator10":
                    race = CharacterRace.UNDEAD;
                    break;
                case "ECHARACTER_RACE::NewEnumerator11":
                    race = CharacterRace.DEVIL;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return race;
        }

        public static byte[] ConvertRaceTypeToBytes(CharacterRace race, Dictionary<int, string> uassetStrings)
        {
            string raceString;
            switch (race)
            {
                case CharacterRace.UNKNOWN:
                    raceString = "ECHARACTER_RACE::NewEnumerator0";
                    break;
                case CharacterRace.HUMAN:
                    raceString = "ECHARACTER_RACE::NewEnumerator1";
                    break;
                case CharacterRace.BEAST:
                    raceString = "ECHARACTER_RACE::NewEnumerator2";
                    break;
                case CharacterRace.INSECT:
                    raceString = "ECHARACTER_RACE::NewEnumerator3";
                    break;
                case CharacterRace.BIRD:
                    raceString = "ECHARACTER_RACE::NewEnumerator4";
                    break;
                case CharacterRace.FISH:
                    raceString = "ECHARACTER_RACE::NewEnumerator5";
                    break;
                case CharacterRace.DRAGON:
                    raceString = "ECHARACTER_RACE::NewEnumerator6";
                    break;
                case CharacterRace.PLANT:
                    raceString = "ECHARACTER_RACE::NewEnumerator7";
                    break;
                case CharacterRace.CHIMERA:
                    raceString = "ECHARACTER_RACE::NewEnumerator8";
                    break;
                case CharacterRace.SHELL:
                    raceString = "ECHARACTER_RACE::NewEnumerator9";
                    break;
                case CharacterRace.UNDEAD:
                    raceString = "ECHARACTER_RACE::NewEnumerator10";
                    break;
                case CharacterRace.DEVIL:
                    raceString = "ECHARACTER_RACE::NewEnumerator11";
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            int value;
            try
            {
                value = uassetStrings.Single(x => x.Value == raceString).Key;
            }
            catch (KeyNotFoundException)
            {
                value = uassetStrings.Single(x => x.Value == "ECHARACTER_RACE::NewEnumerator0").Key;
            }

            return BitConverter.GetBytes(value);
        }

        public static byte[] ConvertSizeTypeToBytes(CharacterSize size, Dictionary<int, string> uassetStrings)
        {
            string sizeString;
            switch (size)
            {
                case CharacterSize.SMALL:
                    sizeString = "ECHARACTER_SIZE::NewEnumerator0";
                    break;
                case CharacterSize.MEDIUM:
                    sizeString = "ECHARACTER_SIZE::NewEnumerator1";
                    break;
                case CharacterSize.LARGE:
                    sizeString = "ECHARACTER_SIZE::NewEnumerator2";
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return BitConverter.GetBytes(uassetStrings.Single(x => x.Value == sizeString).Key);
        }

        public static CharacterSize ConvertStringToSizeType(string sizeString)
        {
            CharacterSize size;
            switch (sizeString)
            {
                case "ECHARACTER_SIZE::NewEnumerator0":
                    size = CharacterSize.SMALL;
                    break;
                case "ECHARACTER_SIZE::NewEnumerator1":
                    size = CharacterSize.MEDIUM;
                    break;
                case "ECHARACTER_SIZE::NewEnumerator2":
                    size = CharacterSize.LARGE;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return size;
        }

        public static byte[] ConvertDeadTypeToBytes(EnemyDeadType deadType, Dictionary<int, string> uassetStrings)
        {
            string deadString;
            switch (deadType)
            {
                case EnemyDeadType.DEADTYPE0:
                    deadString = "EENEMY_DEAD_TYPE::NewEnumerator0";
                    break;
                case EnemyDeadType.DEADTYPE1:
                    deadString = "EENEMY_DEAD_TYPE::NewEnumerator1";
                    break;
                case EnemyDeadType.DEADTYPE2:
                    deadString = "EENEMY_DEAD_TYPE::NewEnumerator2";
                    break;
                case EnemyDeadType.DEADTYPE3:
                    deadString = "EENEMY_DEAD_TYPE::NewEnumerator3";
                    break;
                case EnemyDeadType.DEADTYPE4:
                    deadString = "EENEMY_DEAD_TYPE::NewEnumerator4";
                    break;
                case EnemyDeadType.DEADTYPE5:
                    deadString = "EENEMY_DEAD_TYPE::NewEnumerator5";
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return BitConverter.GetBytes(uassetStrings.Single(x => x.Value == deadString).Key);
        }

        public static EnemyDeadType ConvertStringToDeadType(string deadString)
        {
            EnemyDeadType deadType;
            switch (deadString)
            {
                case "EENEMY_DEAD_TYPE::NewEnumerator0":
                    deadType = EnemyDeadType.DEADTYPE0;
                    break;
                case "EENEMY_DEAD_TYPE::NewEnumerator1":
                    deadType = EnemyDeadType.DEADTYPE1;
                    break;
                case "EENEMY_DEAD_TYPE::NewEnumerator2":
                    deadType = EnemyDeadType.DEADTYPE2;
                    break;
                case "EENEMY_DEAD_TYPE::NewEnumerator3":
                    deadType = EnemyDeadType.DEADTYPE3;
                    break;
                case "EENEMY_DEAD_TYPE::NewEnumerator4":
                    deadType = EnemyDeadType.DEADTYPE4;
                    break;
                case "EENEMY_DEAD_TYPE::NewEnumerator5":
                    deadType = EnemyDeadType.DEADTYPE5;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return deadType;
        }

        public static byte[] GetBytesFromStringWithPossibleSuffix(string stringWithPossibleSuffix, Dictionary<int, string> uassetStrings, string uassetPath)
        {
            string[] data = stringWithPossibleSuffix.Split('_');
            string prefix = string.Join("_", data.Where(y => y != data.Last()));

            byte[] byteData = new byte[8];
            if (!uassetStrings.ContainsValue(stringWithPossibleSuffix) && !uassetStrings.ContainsValue(prefix))
            {
                CommonUtilities.AddStringToUasset(uassetPath, $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset", stringWithPossibleSuffix);
                uassetStrings = CommonUtilities.ParseUAssetFile($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset");
            }

            if (uassetStrings.ContainsValue(stringWithPossibleSuffix))
            {
                UpdateBytesAtOffset(BitConverter.GetBytes(uassetStrings.Single(x => x.Value == stringWithPossibleSuffix).Key), byteData, 0);
            }
            else
            {
                byte[] numericValue = BitConverter.GetBytes(int.Parse(data[data.Length - 1]) + 1);
                UpdateBytesAtOffset(BitConverter.GetBytes(uassetStrings.Single(x => x.Value == prefix).Key), byteData, 0);
                UpdateBytesAtOffset(numericValue, byteData, 4);
            }
            return byteData;
        }
    }
}
