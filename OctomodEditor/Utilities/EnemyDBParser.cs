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
        public static Dictionary<string, Enemy> ParseEnemyObjects(string uassetPath, string uexpPath)
        {
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

        public static CharacterSize ConvertStringToSizeType(string raceString)
        {
            CharacterSize size;
            switch (raceString)
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

        public static EnemyDeadType ConvertStringToDeadType(string raceString)
        {
            EnemyDeadType deadType;
            switch (raceString)
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
    }
}
