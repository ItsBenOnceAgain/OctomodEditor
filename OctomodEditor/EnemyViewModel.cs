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
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
