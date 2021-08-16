using System;
using System.Collections.Generic;
using System.Text;

namespace OctomodEditor.Models
{
    public class AbilitySet : OctopathModel
    {
        public string AbilityBoost0ID { get; set; }
        public string AbilityBoost1ID { get; set; }
        public string AbilityBoost2ID { get; set; }
        public string AbilityBoost3ID { get; set; }
        public CommandMenuType MenuType { get; set; }
        public CommandSubMenuType SubMenuType { get; set; }
        public AbilityIconType IconType { get; set; }
        public int MenuInfo { get; set; }
    }
}
