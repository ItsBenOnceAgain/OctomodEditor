using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class Enemy
    {
        public int Offset { get; set; }
        public string Key { get; set; }
        public int EnemyID { get; set; }
        public string DisplayNameID { get; set; }
        public string FlipbookPath { get; set; }
        public string TexturePath { get; set; }
        public int DisplayRank { get; set; }
        public int EnemyLevel { get; set; }
        public double DamageRatio { get; set; }
        public CharacterRace RaceType { get; set; }
        public CharacterSize Size { get; set; }
        public bool IsNPC { get; set; }
        public bool PlaysSlowAnimationOnDeath { get; set; }
        public bool IsMainEnemy { get; set; }
        public bool IsNotPartOfBattle { get; set; }
        public bool UsesCatDamageType { get; set; }
        public bool HasNoKnockbackAnimation { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int BP { get; set; }
        public int Shields { get; set; }
        public int PhysicalAttack { get; set; }
        public int PhysicalDefense { get; set; }
        public int ElementalAttack { get; set; }
        public int ElementalDefense { get; set; }
        public int Accuracy { get; set; }
        public int Evasion { get; set; }
        public int Critical { get; set; }
        public int Speed { get; set; }
        public int ExperiencePoints { get; set; }
        public int JobPoints { get; set; }
        public int Money { get; set; }
        public int MoneyFromCollecting { get; set; }
        public bool CanBeCaptured { get; set; }
        public double DefaultTameRate { get; set; }
        public string InvadeMonsterID { get; set; }
        public int FirstBP { get; set; }
        public string BreakType { get; set; }
        public int InvocationValue { get; set; }
        public int InvocationTurn { get; set; }
        public EnemyDeadType DeadType { get; set; }
        public bool ResistsSwords { get; set; }
        public bool ResistsSpears { get; set; }
        public bool ResistsDaggers { get; set; }
        public bool ResistsAxes { get; set; }
        public bool ResistsBows { get; set; }
        public bool ResistsStaves { get; set; }
        public bool ResistsFire { get; set; }
        public bool ResistsIce { get; set; }
        public bool ResistsLightning { get; set; }
        public bool ResistsWind { get; set; }
        public bool ResistsLight { get; set; }
        public bool ResistsDark { get; set; }
        public bool[] AttributeResistances { get; set; }
        public bool GuardedFromStealing { get; set; }
        public string ItemID { get; set; }
        public int ItemDropPercentage { get; set; }
        public string AIPath { get; set; }
        public string[] AbilityList { get; set; }
        public string[] BattleEvents { get; set; }
        public Vector3 DiseaseOffset { get; set; }
        public Vector3 EffectOffset1 { get; set; }
        public Vector3 EffectOffset2 { get; set; }
        public Vector3 DamageUIOffset { get; set; }
        public Vector2 IconL { get; set; }
        public Vector2 PixelScaleL { get; set; }
        public Vector2 IconS { get; set; }
        public Vector2 PixelScaleS { get; set; }
    }

    public enum CharacterRace { RACE0, RACE1 }

    public enum CharacterSize { SMALL, MEDIUM, LARGE }

    public enum EnemyDeadType { DEADTYPE0, DEADTYPE1, DEADTYPE2, DEADTYPE3, DEADTYPE4, }
}
