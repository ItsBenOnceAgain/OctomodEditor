using DataEditorUE4.Models;
using OctomodEditor.Models;
using OctomodEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctomodEditor.Parsers
{
    public class AbilityParser : OctopathModelParser<Ability>
    {
        public override string Directory => @"Ability/Database/";

        public override string FilePrefix => "AbilityData";

        public override UEDataTableObject ConvertModelToTableRow(Ability model)
        {
            var originalRow = model.OriginalRow;
            var cells = originalRow.Cells;

            cells[0].Value = model.AbilityID;
            cells[1].Value = model.DisplayName;
            cells[2].Value = model.Detail;
            cells[3].Value = model.CommandActorID;
            cells[4].Value = CommonOctomodUtilities.ConvertAbilityTypeToString(model.AbilityType);
            cells[5].Value = CommonOctomodUtilities.ConvertAbilityUseTypeToString(model.AbilityUseType);
            cells[6].Value = model.AlwaysDisable;
            cells[7].Value = CommonOctomodUtilities.ConvertAttributeTypeToString(model.AbilityAttributeType);
            cells[8].Value = model.DependWeapon;
            cells[9].Value = CommonOctomodUtilities.ConvertWeaponCategoryToString(model.RestrictWeapon);
            cells[10].Value = CommonOctomodUtilities.ConvertTargetTypeToString(model.TargetType);
            cells[11].Value = model.ExceptEnforcer;
            cells[12].Value = CommonOctomodUtilities.ConvertAbilityCostTypeToString(model.CostType);
            cells[13].Value = model.CostValue;
            cells[14].Value = model.HitRatio;
            cells[15].Value = model.CriticalRatio;
            cells[16].Value = model.AbilityRatio;
            cells[17].Value = CommonOctomodUtilities.ConvertAbilityOrderChangeTypeToString(model.OrderChange);

            var ailmentCellList = (List<UEDataTableCell>)cells[18].Value;
            for (int i = 0; i < ailmentCellList.Count; i++)
            {
                var ailment = model.Ailments[i];
                var ailmentCells = ((UEDataTableObject)ailmentCellList[i].Value).Cells;

                ailmentCells[0].Value = ailment.AilmentName;
                ailmentCells[1].Value = ailment.InvocationValue;
                ailmentCells[2].Value = ailment.InvocationTurn;
                ailmentCells[3].Value = ailment.DiseaseRate;

                ((UEDataTableObject)ailmentCellList[i].Value).Cells = ailmentCells;
            }
            cells[18].Value = ailmentCellList;

            cells[19].Value = CommonOctomodUtilities.ConvertSupportAilmentToString(model.SupportAilment);
            cells[20].Value = model.CommandEffecterID;
            cells[21].Value = model.KeepBoostEffect;
            cells[22].Value = model.EnableItemAll;
            cells[23].Value = model.EnableSkillAll;
            cells[24].Value = model.EnableConvergence;
            cells[25].Value = model.EnableDiffusion;
            cells[26].Value = model.EnableReflection;
            cells[27].Value = model.EnableCounter;
            cells[28].Value = model.EnableHideOut;
            cells[29].Value = model.EnableRepeat;
            cells[30].Value = model.EnableCover;
            cells[31].Value = model.EnableDisableMagic;
            cells[32].Value = model.EnableEnchant;
            cells[33].Value = model.EnableChaseAttack;

            originalRow.Cells = cells;
            return originalRow;
        }

        public override Ability ParseSingle(KeyValuePair<string, UEDataTableObject> row)
        {
            var cells = row.Value.Cells;

            Ability ability = new Ability();
            ability.Key = row.Key;
            ability.OriginalRow = row.Value;

            ability.AbilityID = cells[0].Value;
            ability.DisplayName = cells[1].Value;
            ability.Detail = cells[2].Value;
            ability.CommandActorID = cells[3].Value;
            ability.AbilityType = CommonOctomodUtilities.ConvertStringToAbilityType(cells[4].Value);
            ability.AbilityUseType = CommonOctomodUtilities.ConvertStringToAbilityUseType(cells[5].Value);
            ability.AlwaysDisable = cells[6].Value;
            ability.AbilityAttributeType = CommonOctomodUtilities.ConvertStringToAttributeType(cells[7].Value);
            ability.DependWeapon = cells[8].Value;
            ability.RestrictWeapon = CommonOctomodUtilities.ConvertStringToWeaponType(cells[9].Value);
            ability.TargetType = CommonOctomodUtilities.ConvertStringToTargetType(cells[10].Value);
            ability.ExceptEnforcer = cells[11].Value;
            ability.CostType = CommonOctomodUtilities.ConvertStringToAbilityCostType(cells[12].Value);
            ability.CostValue = cells[13].Value;
            ability.HitRatio = cells[14].Value;
            ability.CriticalRatio = cells[15].Value;
            ability.AbilityRatio = cells[16].Value;
            ability.OrderChange = CommonOctomodUtilities.ConvertStringToAbilityOrderChangeType(cells[17].Value);

            List<Ailment> ailments = new List<Ailment>();
            var ailmentCellList = (List<UEDataTableCell>)cells[18].Value;
            for (int i = 0; i < ailmentCellList.Count; i++)
            {
                var ailmentCells = ((UEDataTableObject)ailmentCellList[i].Value).Cells;
                Ailment ailment = new Ailment();
                ailment.AilmentName = ailmentCells[0].Value;
                ailment.InvocationValue = ailmentCells[1].Value;
                ailment.InvocationTurn = ailmentCells[2].Value;
                ailment.DiseaseRate = ailmentCells[3].Value;
                ailments.Add(ailment);
            }
            ability.Ailments = ailments.ToArray();

            ability.SupportAilment = CommonOctomodUtilities.ConvertStringToSupportAilment(cells[19].Value);
            ability.CommandEffecterID = cells[20].Value;
            ability.KeepBoostEffect = cells[21].Value;
            ability.EnableItemAll = cells[22].Value;
            ability.EnableSkillAll = cells[23].Value;
            ability.EnableConvergence = cells[24].Value;
            ability.EnableDiffusion = cells[25].Value;
            ability.EnableReflection = cells[26].Value;
            ability.EnableCounter = cells[27].Value;
            ability.EnableHideOut = cells[28].Value;
            ability.EnableRepeat = cells[29].Value;
            ability.EnableCover = cells[30].Value;
            ability.EnableDisableMagic = cells[31].Value;
            ability.EnableEnchant = cells[32].Value;
            ability.EnableChaseAttack = cells[33].Value;

            return ability;
        }
    }
}
