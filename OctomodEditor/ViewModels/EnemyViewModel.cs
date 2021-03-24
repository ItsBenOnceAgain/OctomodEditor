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
    public class EnemyViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, Enemy> _enemyList;
        private Enemy _currentEnemy;
        private List<Enemy> _currentEnemyList;
        public Dictionary<string, string> AllItems { get; set; }
        public Dictionary<string, string> AllFlipbookPaths { get; set; }
        public Dictionary<string, string> AllTexturePaths { get; set; }
        public Dictionary<string, string> AllAIPaths { get; set; }
        public List<string> AllBeastLoreIDs { get; set; }
        public Dictionary<string, string> AllAbilities { get; set; }
        public List<string> AllEvents { get; set; }
        public List<CharacterSize> Sizes { get; set; }
        public List<AttributeResistance> Resistances { get; set; }
        public List<CharacterRace> Races { get; set; }
        public List<EnemyDeadType> DeadTypes { get; set; }

        public Dictionary<string, Enemy> EnemyList
        {
            get
            {
                return _enemyList;
            }
            set
            {
                _enemyList = value;
                OnPropertyChanged();
            }
        }
        
        public Enemy CurrentEnemy
        {
            get
            {
                return _currentEnemy;
            }
            set
            {
                if(!(value is null))
                {
                    _currentEnemy = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Enemy> CurrentEnemyList
        {
            get
            {
                return _currentEnemyList;
            }
            set
            {
                _currentEnemyList = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public EnemyViewModel(Dictionary<string, Enemy> enemies)
        {
            EnemyList = enemies;
            CurrentEnemy = enemies.First().Value;
            Sizes = Enum.GetValues(typeof(CharacterSize)).Cast<CharacterSize>().ToList();
            Resistances = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>().ToList();
            Races = Enum.GetValues(typeof(CharacterRace)).Cast<CharacterRace>().ToList();
            DeadTypes = Enum.GetValues(typeof(EnemyDeadType)).Cast<EnemyDeadType>().ToList();

            AllItems = GetItemNames();
            AllFlipbookPaths = GetFlipbookPaths();
            AllTexturePaths = GetTexturePaths();
            AllAIPaths = GetAIPaths();
            AllBeastLoreIDs = EnemyList.Select(x => x.Value.CapturedEnemyID).Distinct().ToList();
            AllAbilities = GetAbilities();
            AllEvents = GetEvents();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Dictionary<string, string> GetItemNames()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach(var item in MainWindow.ModItemList)
            {
                items.Add(item.Value.Key, MainWindow.ModGameText[item.Value.ItemNameID]);
            }
            var finalDictionary = items.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            finalDictionary.Add("None", "None");
            return finalDictionary;
        }

        private Dictionary<string, string> GetFlipbookPaths()
        {
            Dictionary<string, string> flipbookPaths = new Dictionary<string, string>();

            foreach (var flipbookPath in EnemyList.Select(x => x.Value.FlipbookPath).Distinct())
            {
                string[] flipbookDirectories = flipbookPath.Split('/');
                flipbookPaths.Add(flipbookPath, flipbookDirectories.Last());
            }
            return flipbookPaths.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private Dictionary<string, string> GetTexturePaths()
        {
            Dictionary<string, string> texturePaths = new Dictionary<string, string>();

            foreach (var texturePath in EnemyList.Select(x => x.Value.TexturePath).Distinct())
            {
                string[] textureDirectories = texturePath.Split('/');
                texturePaths.Add(texturePath, textureDirectories.Last());
            }
            return texturePaths.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private Dictionary<string, string> GetAIPaths()
        {
            Dictionary<string, string> aiPaths = new Dictionary<string, string>();

            aiPaths.Add("None", "None");
            foreach (var aiPath in EnemyList.Where(x => x.Value.AIPath != "None").Select(x => x.Value.AIPath).Distinct())
            {
                string[] aiDirectories = aiPath.Split('/');
                aiPaths.Add(aiPath, aiDirectories.Last());
            }
            return aiPaths.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private List<string> GetEvents()
        {
            List<string> events = new List<string>();

            foreach (var enemy in EnemyList.Select(x => x.Value))
            {
                foreach (string battleEvent in enemy.BattleEvents)
                {
                    if (!events.Contains(battleEvent))
                    {
                        events.Add(battleEvent);
                    }
                }
            }
            events.Sort();
            return events;
        }

        private Dictionary<string, string> GetAbilities()
        {
            Dictionary<string, string> allAbilities = new Dictionary<string, string>();
            foreach (var enemy in EnemyList.Select(x => x.Value))
            {
                foreach (string ability in enemy.AbilityList)
                {
                    if (ability == "None")
                    {
                        if (!allAbilities.ContainsKey("None"))
                        {
                            allAbilities.Add("None", "None");
                        }
                    }
                    else
                    {
                        string textKey = $"BT_ABI_NAME_{ability.Substring(7)}";
                        string text;
                        if (MainWindow.MasterGameText.ContainsKey(textKey))
                        {
                            text = MainWindow.MasterGameText[textKey];
                        }
                        else
                        {
                            text = ability;
                        }
                        if (!allAbilities.ContainsKey(ability))
                        {
                            allAbilities.Add(ability, text);
                        }
                    }
                }
            }
            return allAbilities.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
