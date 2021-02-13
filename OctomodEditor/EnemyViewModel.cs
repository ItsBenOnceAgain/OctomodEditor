using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor
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
                if(value != null)
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

            items.Add("None", "None");
            foreach (var itemId in EnemyList.Where(x => x.Value.ItemID != "None").Select(x => x.Value.ItemID).Distinct())
            {
                if (MainWindow.MasterGameText.ContainsKey($"TX_NA_{itemId}"))
                {
                    items.Add(itemId, MainWindow.MasterGameText[$"TX_NA_{itemId}"]);
                }
                else
                {
                    string[] idInfo = itemId.Split('_');
                    int identifier = int.Parse(idInfo[2]);
                    if (idInfo[1] == "MB")
                    {
                        identifier += 16;
                    }
                    items.Add(itemId, MainWindow.MasterGameText[$"MIX_ITM_NA_{identifier:D3}"]);
                }
            }
            return items;
        }

        private Dictionary<string, string> GetFlipbookPaths()
        {
            Dictionary<string, string> flipbookPaths = new Dictionary<string, string>();

            foreach (var flipbookPath in EnemyList.Select(x => x.Value.FlipbookPath).Distinct())
            {
                string[] flipbookDirectories = flipbookPath.Split('/');
                flipbookPaths.Add(flipbookPath, flipbookDirectories.Last());
            }
            return flipbookPaths;
        }

        private Dictionary<string, string> GetTexturePaths()
        {
            Dictionary<string, string> texturePaths = new Dictionary<string, string>();

            foreach (var texturePath in EnemyList.Select(x => x.Value.TexturePath).Distinct())
            {
                string[] textureDirectories = texturePath.Split('/');
                texturePaths.Add(texturePath, textureDirectories.Last());
            }
            return texturePaths;
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
            return aiPaths;
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
            return allAbilities;
        }
    }
}
