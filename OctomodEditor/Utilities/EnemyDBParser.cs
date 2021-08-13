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
            if (!useBaseGame && File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset"))
            {
                uassetPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset";
            }
            else
            {
                uassetPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp"))
            {
                uexpPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";
            }
            else
            {
                uexpPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonOctomodUtilities.ParseUAssetFile(uassetPath);

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

        public static Enemy ParseSingleEnemy(Dictionary<int, string> uassetStrings, byte[] allBytes, ref int currentOffset)
        {
            int enemyOffset = currentOffset;
            string key = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int enemyId = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string displayNameId = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string flipbookPath = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 37;
            string texturePath = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 37;
            int displayRank = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int enemyLevel = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            double damageRatio = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 37;
            CharacterRace enemyRace = CommonOctomodUtilities.ConvertStringToRaceType(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 41;
            CharacterSize enemySize = CommonOctomodUtilities.ConvertStringToSizeType(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
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
            string capturedEnemyId = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int firstBP = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            string breakType = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int invocationValue = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int invocationTurn = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            EnemyDeadType deadType = CommonOctomodUtilities.ConvertStringToDeadType(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 53;
            bool isWeakToFire = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToIce = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToLightning = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToWind = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToLight = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToDark = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 45;
            bool isWeakToSwords = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToSpears = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToDaggers = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToAxes = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToBows = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 8;
            bool isWeakToStaves = ConvertStringToAttributeBool(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 53;
            AttributeResistance[] attributeResistances = GetDiseaseResistances(allBytes, uassetStrings, ref currentOffset);
            currentOffset += 24;
            bool isGuardedFromStealing = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 27;
            string itemId = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int itemDropPercentage = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string aiPath = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
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

        public static void SaveEnemies(List<Enemy> enemies)
        {
            if (!Directory.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/"))
            {
                Directory.CreateDirectory($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/");
            }

            string uassetPath;
            if (File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset"))
            {
                uassetPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset";
            }
            else
            {
                uassetPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset";
            }
            string oldUexpPath;
            if (File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp"))
            {
                oldUexpPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";
            }
            else
            {
                oldUexpPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";
            }

            string uexpPath;
            if (!File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp"))
            {
                var openStream = File.Create($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp");
                openStream.Close();
            }
            uexpPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uexp";

            Dictionary<int, string> uassetStrings = CommonOctomodUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(oldUexpPath);
            foreach(var enemy in enemies)
            {
                SaveEnemy(enemy, allBytes, uassetStrings, uassetPath, $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Character/Database/EnemyDB.uasset");
            }
            File.WriteAllBytes(uexpPath, allBytes);
        }

        public static void SaveEnemy(Enemy enemy, byte[] allBytes, Dictionary<int, string> uassetStrings, string uassetPath, string modUassetPath)
        {
            int currentOffset = enemy.Offset + 95;
            byte[] flipbookData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(enemy.FlipbookPath, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(flipbookData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] textureData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(enemy.TexturePath, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(textureData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] displayRankData = BitConverter.GetBytes(enemy.DisplayRank);
            CommonOctomodUtilities.UpdateBytesAtOffset(displayRankData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] enemyLevelData = BitConverter.GetBytes(enemy.EnemyLevel);
            CommonOctomodUtilities.UpdateBytesAtOffset(enemyLevelData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] damageRatioData = BitConverter.GetBytes((float)enemy.DamageRatio);
            CommonOctomodUtilities.UpdateBytesAtOffset(damageRatioData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] enemyRaceData = CommonOctomodUtilities.ConvertRaceTypeToBytes(enemy.RaceType, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(enemyRaceData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] enemySizeData = CommonOctomodUtilities.ConvertSizeTypeToBytes(enemy.Size, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(enemySizeData, allBytes, currentOffset);
            currentOffset += 32;
            byte[] isNPCData = BitConverter.GetBytes(enemy.IsNPC);
            CommonOctomodUtilities.UpdateBytesAtOffset(isNPCData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] playsSlowDeathAnimData = BitConverter.GetBytes(enemy.PlaysSlowAnimationOnDeath);
            CommonOctomodUtilities.UpdateBytesAtOffset(playsSlowDeathAnimData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] isMainEnemyData = BitConverter.GetBytes(enemy.IsMainEnemy);
            CommonOctomodUtilities.UpdateBytesAtOffset(isMainEnemyData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] isExemptFromBattleData = BitConverter.GetBytes(enemy.IsExemptFromBattle);
            CommonOctomodUtilities.UpdateBytesAtOffset(isExemptFromBattleData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] usesCatDamageData = BitConverter.GetBytes(enemy.UsesCatDamageType);
            CommonOctomodUtilities.UpdateBytesAtOffset(usesCatDamageData, allBytes, currentOffset);
            currentOffset += 26;
            byte[] hasNoKnockbackData = BitConverter.GetBytes(enemy.HasNoKnockbackAnimation);
            CommonOctomodUtilities.UpdateBytesAtOffset(hasNoKnockbackData, allBytes, currentOffset);
            currentOffset += 76;
            byte[] hpData = BitConverter.GetBytes(enemy.HP);
            CommonOctomodUtilities.UpdateBytesAtOffset(hpData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] mpData = BitConverter.GetBytes(enemy.MP);
            CommonOctomodUtilities.UpdateBytesAtOffset(mpData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] bpData = BitConverter.GetBytes(enemy.BP);
            CommonOctomodUtilities.UpdateBytesAtOffset(bpData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] shieldsData = BitConverter.GetBytes(enemy.Shields);
            CommonOctomodUtilities.UpdateBytesAtOffset(shieldsData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] physAttackData = BitConverter.GetBytes(enemy.PhysicalAttack);
            CommonOctomodUtilities.UpdateBytesAtOffset(physAttackData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] physDefData = BitConverter.GetBytes(enemy.PhysicalDefense);
            CommonOctomodUtilities.UpdateBytesAtOffset(physDefData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] elemAttackData = BitConverter.GetBytes(enemy.ElementalAttack);
            CommonOctomodUtilities.UpdateBytesAtOffset(elemAttackData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] elemDefData = BitConverter.GetBytes(enemy.ElementalDefense);
            CommonOctomodUtilities.UpdateBytesAtOffset(elemDefData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] accuracyData = BitConverter.GetBytes(enemy.Accuracy);
            CommonOctomodUtilities.UpdateBytesAtOffset(accuracyData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] evasionData = BitConverter.GetBytes(enemy.Evasion);
            CommonOctomodUtilities.UpdateBytesAtOffset(evasionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] criticalData = BitConverter.GetBytes(enemy.Critical);
            CommonOctomodUtilities.UpdateBytesAtOffset(criticalData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] speedData = BitConverter.GetBytes(enemy.Speed);
            CommonOctomodUtilities.UpdateBytesAtOffset(speedData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] experienceData = BitConverter.GetBytes(enemy.ExperiencePoints);
            CommonOctomodUtilities.UpdateBytesAtOffset(experienceData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] jobPointsData = BitConverter.GetBytes(enemy.JobPoints);
            CommonOctomodUtilities.UpdateBytesAtOffset(jobPointsData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] moneyData = BitConverter.GetBytes(enemy.Money);
            CommonOctomodUtilities.UpdateBytesAtOffset(moneyData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] stealableMoneyData = BitConverter.GetBytes(enemy.MoneyFromCollecting);
            CommonOctomodUtilities.UpdateBytesAtOffset(stealableMoneyData, allBytes, currentOffset);
            currentOffset += 28;
            byte[] canBeCapturedData = BitConverter.GetBytes(enemy.CanBeCaptured);
            CommonOctomodUtilities.UpdateBytesAtOffset(canBeCapturedData, allBytes, currentOffset);
            currentOffset += 27;
            byte[] tameRateData = BitConverter.GetBytes((float)enemy.DefaultTameRate);
            CommonOctomodUtilities.UpdateBytesAtOffset(tameRateData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] capturedEnemyIdData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(enemy.CapturedEnemyID, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(capturedEnemyIdData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] firstBPData = BitConverter.GetBytes(enemy.FirstBP);
            CommonOctomodUtilities.UpdateBytesAtOffset(firstBPData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] breakTypeData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(enemy.BreakType, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(breakTypeData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] invocationValueData = BitConverter.GetBytes(enemy.InvocationValue);
            CommonOctomodUtilities.UpdateBytesAtOffset(invocationValueData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] invocationTurnData = BitConverter.GetBytes(enemy.InvocationTurn);
            CommonOctomodUtilities.UpdateBytesAtOffset(invocationTurnData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] deadTypeBytes = CommonOctomodUtilities.ConvertDeadTypeToBytes(enemy.DeadType, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(deadTypeBytes, allBytes, currentOffset);
            currentOffset += 53;
            byte[] isWeakToFireBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToFire, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToFireBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToIceBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToIce, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToIceBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToLightningBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToLightning, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToLightningBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToWindBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToWind, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToWindBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToLightBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToLight, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToLightBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToDarkBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToDark, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToDarkBytes, allBytes, currentOffset);
            currentOffset += 45;
            byte[] isWeakToSwordsBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToSwords, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToSwordsBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToSpearsBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToSpears, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToSpearsBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToDaggersBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToDaggers, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToDaggersBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToAxesBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToAxes, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToAxesBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToBowsBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToBows, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToBowsBytes, allBytes, currentOffset);
            currentOffset += 8;
            byte[] isWeakToStavesBytes = ConvertAttributeBoolToBytes(enemy.IsWeakToStaves, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(isWeakToStavesBytes, allBytes, currentOffset);
            currentOffset += 53;
            UpdateDiseaseResistanceBytes(enemy.AttributeResistances, allBytes, uassetStrings, ref currentOffset, uassetPath, modUassetPath);
            currentOffset += 24;
            byte[] isGuardedFromStealingData = BitConverter.GetBytes(enemy.IsGuardedFromStealing);
            CommonOctomodUtilities.UpdateBytesAtOffset(isGuardedFromStealingData, allBytes, currentOffset);
            currentOffset += 27;
            byte[] itemIdData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(enemy.ItemID, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(itemIdData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] itemDropPercentageData = BitConverter.GetBytes(enemy.ItemDropPercentage);
            CommonOctomodUtilities.UpdateBytesAtOffset(itemDropPercentageData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] aiPathData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(enemy.AIPath, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(aiPathData, allBytes, currentOffset);
            currentOffset += 49;
            UpdateAbilityEventBytes(enemy.AbilityList, allBytes, uassetStrings, ref currentOffset, uassetPath, modUassetPath);
            currentOffset += 37;
            UpdateAbilityEventBytes(enemy.BattleEvents, allBytes, uassetStrings, ref currentOffset, uassetPath, modUassetPath);
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

        public static Vector2 GetVector2Values(byte[] allBytes, ref int currentOffset)
        {
            float x = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 4;
            float y = BitConverter.ToSingle(allBytes, currentOffset);
            currentOffset += 4;
            return new Vector2(x, y);
        }

        public static void UpdateVector2Bytes(Vector2 values, byte[] allBytes, ref int currentOffset)
        {
            byte[] xData = BitConverter.GetBytes(values.X);
            CommonOctomodUtilities.UpdateBytesAtOffset(xData, allBytes, currentOffset);
            currentOffset += 4;
            byte[] yData = BitConverter.GetBytes(values.Y);
            CommonOctomodUtilities.UpdateBytesAtOffset(yData, allBytes, currentOffset);
            currentOffset += 4;
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
            CommonOctomodUtilities.UpdateBytesAtOffset(xData, allBytes, currentOffset);
            currentOffset += 4;
            byte[] yData = BitConverter.GetBytes(values.Y);
            CommonOctomodUtilities.UpdateBytesAtOffset(yData, allBytes, currentOffset);
            currentOffset += 4;
            byte[] zData = BitConverter.GetBytes(values.Z);
            CommonOctomodUtilities.UpdateBytesAtOffset(zData, allBytes, currentOffset);
            currentOffset += 4;
        }

        public static string[] GetEventList(byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset)
        {
            string[] eventList = new string[3];
            for (int i = 0; i < 3; i++)
            {
                eventList[i] = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
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
                abilityList[i] = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
                currentOffset += 8;
            }
            return abilityList;
        }

        public static void UpdateAbilityEventBytes(string[] abilities, byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset, string uassetPath, string modUassetPath)
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                byte[] abilityBytes = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(abilities[i], uassetStrings, uassetPath, modUassetPath);
                CommonOctomodUtilities.UpdateBytesAtOffset(abilityBytes, allBytes, currentOffset);
                currentOffset += 8;
            }
        }

        public static void UpdateDiseaseResistanceBytes(AttributeResistance[] resistances, byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset, string uassetPath, string modUassetPath)
        {
            for (int i = 0; i < 12; i++)
            {
                byte[] resistanceBytes = CommonOctomodUtilities.ConvertAttributeResistanceToBytes(resistances[i], uassetStrings, uassetPath, modUassetPath);
                CommonOctomodUtilities.UpdateBytesAtOffset(resistanceBytes, allBytes, currentOffset);

                currentOffset += 8;
            }
        }

        public static AttributeResistance[] GetDiseaseResistances(byte[] allBytes, Dictionary<int, string> uassetStrings, ref int currentOffset)
        {
            AttributeResistance[] resistances = new AttributeResistance[12];
            for(int i = 0; i < 12; i++)
            {
                string value = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
                resistances[i] = CommonOctomodUtilities.ConvertStringToAttributeResistance(value);

                currentOffset += 8;
            }
            return resistances;
        }

        public static byte[] ConvertAttributeBoolToBytes(bool attributeBool, Dictionary<int, string> uassetStrings, string uassetPath, string modUassetPath)
        {
            string attributeString = attributeBool ? "EATTRIBUTE_RESIST::NewEnumerator1" : "EATTRIBUTE_RESIST::NewEnumerator0";
            return CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(attributeString, uassetStrings, uassetPath, modUassetPath);
        }

        public static bool ConvertStringToAttributeBool(string attributeString)
        {
            return attributeString == "EATTRIBUTE_RESIST::NewEnumerator1";
        }
    }
}
