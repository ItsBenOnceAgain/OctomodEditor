using DataEditorUE4.Models;
using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.ViewModels
{
    public class AbilityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, Ability> abilityList;
        private Ability _currentAbility;
        private List<Ability> _currentAbilityList;
        public List<AbilityType> AbilityTypes { get; set; }
        public List<AbilityUseType> AbilityUseTypes { get; set; }
        public List<AttributeType> AttributeTypes { get; set; }
        public List<WeaponCategory> WeaponCategories { get; set; }
        public List<TargetType> TargetTypes { get; set; }
        public List<AbilityCostType> AbilityCostTypes { get; set; }
        public List<AbilityOrderChangeType> AbilityOrderChangeTypes { get; set; }
        public List<SupportAilmentType> SupportAilmentTypes { get; set; }
        public List<string> EffectNames { get; set; }

        public Dictionary<string, Ability> AbilityList
        {
            get
            {
                return abilityList;
            }
            set
            {
                abilityList = value;
                OnPropertyChanged();
            }
        }

        public Ability CurrentAbility
        {
            get
            {
                return _currentAbility;
            }
            set
            {
                if (!(value is null))
                {
                    _currentAbility = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Ability> CurrentAbilityList
        {
            get
            {
                return _currentAbilityList;
            }
            set
            {
                _currentAbilityList = value;
                OnPropertyChanged();
            }
        }

        public AbilityViewModel(Dictionary<string, Ability> abilities)
        {
            AbilityList = abilities;
            CurrentAbility = abilities.First().Value;

            AbilityTypes = Enum.GetValues(typeof(AbilityType)).Cast<AbilityType>().ToList();
            AbilityUseTypes = Enum.GetValues(typeof(AbilityUseType)).Cast<AbilityUseType>().ToList();
            AttributeTypes = Enum.GetValues(typeof(AttributeType)).Cast<AttributeType>().ToList();
            WeaponCategories = Enum.GetValues(typeof(WeaponCategory)).Cast<WeaponCategory>().ToList();
            TargetTypes = Enum.GetValues(typeof(TargetType)).Cast<TargetType>().ToList();
            AbilityCostTypes = Enum.GetValues(typeof(AbilityCostType)).Cast<AbilityCostType>().ToList();
            AbilityOrderChangeTypes = Enum.GetValues(typeof(AbilityOrderChangeType)).Cast<AbilityOrderChangeType>().ToList();
            SupportAilmentTypes = Enum.GetValues(typeof(SupportAilmentType)).Cast<SupportAilmentType>().ToList();

            EffectNames = GetEffectNames();
        }

        private List<string> GetEffectNames()
        {
            List<string> effects = new List<string>();

            foreach (var ability in AbilityList.Select(x => x.Value))
            {
                foreach (var ailment in ability.Ailments)
                {
                    if (!effects.Contains(ailment.AilmentName))
                    {
                        effects.Add(ailment.AilmentName);
                    }
                }
            }
            effects.Sort();
            return effects;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
