using DataEditorUE4.Models;
using OctomodEditor.Models;
using OctomodEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctomodEditor.Parsers
{
    public class AbilitySetParser : OctopathModelParser<AbilitySet>
    {
        public override string Directory => @"Ability/Database/";

        public override string FilePrefix => "AbilitySetData";

        public override UEDataTableObject ConvertModelToTableRow(AbilitySet model)
        {
            var originalRow = model.OriginalRow;
            var cells = originalRow.Cells;

            cells[0].Value = model.AbilityBoost0ID;
            cells[1].Value = model.AbilityBoost1ID;
            cells[2].Value = model.AbilityBoost2ID;
            cells[3].Value = model.AbilityBoost3ID;

            cells[4].Value = CommonOctomodUtilities.ConvertCommandMenuTypeToString(model.MenuType);
            cells[5].Value = CommonOctomodUtilities.ConvertCommandSubMenuTypeToString(model.SubMenuType);
            cells[6].Value = CommonOctomodUtilities.ConvertAbilityIconTypeToString(model.IconType);

            cells[7].Value = model.MenuInfo;

            originalRow.Cells = cells;
            return originalRow;
        }

        public override AbilitySet ParseSingle(KeyValuePair<string, UEDataTableObject> row)
        {
            var cells = row.Value.Cells;

            AbilitySet abilitySet = new AbilitySet();
            abilitySet.Key = row.Key;
            abilitySet.OriginalRow = row.Value;

            abilitySet.AbilityBoost0ID = cells[0].Value;
            abilitySet.AbilityBoost1ID = cells[1].Value;
            abilitySet.AbilityBoost2ID = cells[2].Value;
            abilitySet.AbilityBoost3ID = cells[3].Value;

            abilitySet.MenuType = CommonOctomodUtilities.ConvertStringToCommandMenuType(cells[4].Value);
            abilitySet.SubMenuType = CommonOctomodUtilities.ConvertStringToCommandSubMenuType(cells[5].Value);
            abilitySet.IconType = CommonOctomodUtilities.ConvertStringToAbilityIconType(cells[6].Value);

            abilitySet.MenuInfo = cells[7].Value;

            return abilitySet;
        }
    }
}
