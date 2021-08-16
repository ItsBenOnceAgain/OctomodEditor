using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class Ability : OctopathModel
    {
        public int AbilityID { get; set; }
        public string DisplayName { get; set; }
        public string Detail { get; set; }
        public int CommandActorID { get; set; }
        public AbilityType AbilityType { get; set; }
        public AbilityUseType AbilityUseType { get; set; }
        public bool AlwaysDisable { get; set; }
        public AttributeType AbilityAttributeType { get; set; }
        public bool DependWeapon { get; set; }
        public WeaponCategory RestrictWeapon { get; set; }
        public TargetType TargetType { get; set; }
        public bool ExceptEnforcer { get; set; }
        public AbilityCostType CostType { get; set; }
        public int CostValue { get; set; }
        public int HitRatio { get; set; }
        public int CriticalRatio { get; set; }
        public int AbilityRatio { get; set; }
        public AbilityOrderChangeType OrderChange { get; set; }
        public Ailment[] Ailments { get; set; }
        public SupportAilmentType SupportAilment { get; set; }
        public int CommandEffecterID { get; set; }
        public bool KeepBoostEffect { get; set; }
        public bool EnableItemAll { get; set; }
        public bool EnableSkillAll { get; set; }
        public bool EnableConvergence { get; set; }
        public bool EnableDiffusion { get; set; }
        public bool EnableReflection { get; set; }
        public bool EnableCounter { get; set; }
        public bool EnableHideOut { get; set; }
        public bool EnableRepeat { get; set; }
        public bool EnableCover { get; set; }
        public bool EnableDisableMagic { get; set; }
        public bool EnableEnchant { get; set; }
        public bool EnableChaseAttack { get; set; }

        public override string ToString()
        {
            string name;
            if (MainWindow.ModGameText.ContainsKey(DisplayName))
            {
                name = MainWindow.ModGameText[DisplayName].Text == string.Empty ? Key : $"{MainWindow.ModGameText[DisplayName].Text} : {Key}";
            }
            else
            {
                name = Key;
            }
            return name;
        }
    }
}
