using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public enum AttributeResistance { NONE = 0, WEAK = 1, REDUCE = 2, INVALID = 3, ABSORB = 4 }
    public enum CharacterRace { UNKNOWN = 0, HUMAN = 1, BEAST = 2, INSECT = 3, BIRD = 4, FISH = 5, DRAGON = 6, 
                                PLANT = 7, CHIMERA = 8, SHELL = 9, UNDEAD = 10, DEVIL = 11}
    public enum CharacterSize { SMALL = 0, MEDIUM = 1, LARGE = 2, LL = 3 }
    public enum EnemyDeadType { NORMAL_S = 0, BOSS_M = 1, NORMAL_M = 2, NORMAL_L = 3, BOSS_L = 4, BOSS_EX = 5 }
    public enum ItemCategory { CONSUMABLE = 0, MATERIAL_A = 1, TRADING = 2, TREASURE = 4, EQUIPMENT = 7, INFORMATION = 8, MATERIAL_B = 9 }
    public enum ItemDisplayType { DISABLE = 3, ITEM_USE = 0, ON_HIT = 1, BATTLE_START = 2, ON_TAKE_DAMAGE = 4 }
    public enum ItemUseType { DISABLE = 0, ALWAYS = 1, FIELD_ONLY = 2, BATTLE_ONLY = 3 }
    public enum TargetType { SELF = 0, PARTY_SINGLE = 1, PARTY_ALL = 2, ENEMY_SINGLE = 3, ENEMY_ALL = 4, ALL = 5, ALL_SINGLE = 7 }
    public enum AttributeType { NONE = 0, FIRE = 1, ICE = 2, LIGHTNING = 3, WIND = 4, LIGHT = 5, DARK = 6 }
    public enum EquipmentCategory { SWORD = 0, LANCE = 1, DAGGER = 2, AXE = 3, BOW = 5, ROD = 6, SHIELD = 7, HAT = 8, 
                                    CLOTH = 9, LIGHT_HELMET = 10, LIGHT_ARMOR = 11, HEAVY_HELMET = 12, HEAVY_ARMOR = 13, ACCESSORY = 14 }
    public enum ShopType { WEAPON = 0, ITEM = 1, GENERAL = 2, INN = 3, BAR = 4, EX_BAR = 5 }
    public enum AbilityType { PHYSICS = 0, MAGIC = 1, HP_RECOVERY = 2, MP_RECOVERY = 8, REVIVE = 3, TAME_MONSTER = 7, OTHER = 6, BUFF = 9, 
                              DEBUFF = 10, REVIVE_RECOVERY = 11 }
    public enum AbilityUseType { ALWAYS = 0, BATTLE_ONLY = 1, FIELD_ONLY = 2, SUPPORT = 3 }
    public enum WeaponCategory { SWORD = 0, LANCE = 1, DAGGER = 2, AXE = 3, BOW = 4, ROD = 5, NONE = 6 }
    public enum AbilityCostType { NONE = 0, MP = 1, HP = 2, MONEY = 3, NUM = 4, BP = 5, SP = 6, MP_RATIO = 7 }
    public enum AbilityOrderChangeType { TOP = 0, SECONDLY = 1, ADD_ONE = 2, SUB_ONE = 3, END = 4, NONE = 5 }
    public enum SupportAilmentType { MERCHANT_SUPPORT_001 = 0, MERCHANT_SUPPORT_002 = 1, MERCHANT_SUPPORT_003 = 2, MERCHANT_SUPPORT_004 = 3, 
                                     THIEF_SUPPORT_001 = 4, THIEF_SUPPORT_002 = 5, THIEF_SUPPORT_003 = 6, THIEF_SUPPORT_004 = 7,
                                     FENCER_SUPPORT_001 = 8, FENCER_SUPPORT_002 = 9, FENCER_SUPPORT_003 = 10, FENCER_SUPPORT_004 = 11,
                                     HUNTER_SUPPORT_001 = 12, HUNTER_SUPPORT_002 = 13, HUNTER_SUPPORT_003 = 14, HUNTER_SUPPORT_004 = 15,
                                     PRIEST_SUPPORT_001 = 16, PRIEST_SUPPORT_002 = 17, PRIEST_SUPPORT_003 = 18, PRIEST_SUPPORT_004 = 19,
                                     DANCER_SUPPORT_001 = 20, DANCER_SUPPORT_002 = 21, DANCER_SUPPORT_003 = 22, DANCER_SUPPORT_004 = 23,
                                     PROFESSOR_SUPPORT_001 = 24, PROFESSOR_SUPPORT_002 = 25, PROFESSOR_SUPPORT_003 = 26, PROFESSOR_SUPPORT_004 = 27,
                                     ALCHEMIST_SUPPORT_001 = 28, ALCHEMIST_SUPPORT_002 = 29, ALCHEMIST_SUPPORT_003 = 30, ALCHEMIST_SUPPORT_004 = 31,
                                     WEAPON_MASTER_SUPPORT_001 = 32, WEAPON_MASTER_SUPPORT_002 = 33, WEAPON_MASTER_SUPPORT_003 = 34, WEAPON_MASTER_SUPPORT_004 = 35,
                                     WIZARD_SUPPORT_001 = 36, WIZARD_SUPPORT_002 = 37, WIZARD_SUPPORT_003 = 38, WIZARD_SUPPORT_004 = 39,
                                     ASTRONOMY_SUPPORT_001 = 40, ASTRONOMY_SUPPORT_002 = 41, ASTRONOMY_SUPPORT_003 = 42, ASTRONOMY_SUPPORT_004 = 43,
                                     RUNE_MASTER_SUPPORT_001 = 44, RUNE_MASTER_SUPPORT_002 = 45, RUNE_MASTER_SUPPORT_003 = 46, RUNE_MASTER_SUPPORT_004 = 47 }
    public enum CommandMenuType { BOOST = 6, SUB_MENU = 7, COMMAND = 0, ITEM = 13, ESCAPE = 5, MONSTER = 10, SUPPORTER = 12, DEFENSE = 14, 
                                  PREPARATION = 15, WEAPON_THROW = 16 }
    public enum CommandSubMenuType { NONE = 3, ITEM = 1, DEFAULT_JOB = 2, UNIQUE = 4, EXTEND_JOB = 5, MONSTER = 6, SUPPORTER = 7, 
                                     MATERIAL_A = 8, MATERIAL_B = 9, WEAPON = 10, MERCENARY = 11 }
    public enum AbilityIconType { NONE= 0, PHYSICS = 1, MAGIC = 2, HEAL = 3, BUFF = 4, DEBUFF = 5 }
}
