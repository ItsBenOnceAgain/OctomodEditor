using System;
using System.Collections.Generic;
using System.Text;
using DataEditorUE4.Models;
using OctomodEditor.Models;
using OctomodEditor.Utilities;

namespace OctomodEditor.Parsers
{
    public class ItemParser : OctopathModelParser<Item>
    {
        public override string Directory => @"Item/Database/";

        public override string FilePrefix => "ItemDB";

        public override UEDataTableObject ConvertModelToTableRow(Item model)
        {
            var originalRow = model.OriginalRow;
            var cells = originalRow.Cells;

            cells[0].Value = model.ItemID;
            cells[1].Value = model.ItemNameID;
            cells[2].Value = model.DetailTextID;
            cells[3].Value = model.IconLabelID;
            cells[4].Value = CommonOctomodUtilities.ConvertItemCategoryToString(model.Category);
            cells[5].Value = model.SortCategory;
            cells[6].Value = CommonOctomodUtilities.ConvertItemDisplayTypeToString(model.DisplayType);
            cells[7].Value = CommonOctomodUtilities.ConvertItemUseTypeToString(model.UseType);
            cells[8].Value = CommonOctomodUtilities.ConvertTargetTypeToString(model.TargetType);
            cells[9].Value = CommonOctomodUtilities.ConvertItemAttributeTypeToString(model.AttributeType);

            var ailmentCellList = (List<UEDataTableCell>)cells[10].Value;
            for (int i = 0; i < ailmentCellList.Count; i++)
            {
                var ailmentCells = ((UEDataTableObject)ailmentCellList[i].Value).Cells;
                Ailment ailment = model.Ailments[i];

                ailmentCells[0].Value = ailment.AilmentName;
                ailmentCells[1].Value = ailment.InvocationValue;
                ailmentCells[2].Value = ailment.InvocationTurn;
                ailmentCells[3].Value = ailment.DiseaseRate;

                ((UEDataTableObject)ailmentCellList[i].Value).Cells = ailmentCells;
            }
            cells[10].Value = ailmentCellList;

            cells[11].Value = model.IsValuable;
            cells[12].Value = model.BuyPrice;
            cells[13].Value = model.SellPrice;
            cells[14].Value = CommonOctomodUtilities.ConvertItemEquipmentCategoryToString(model.EquipmentCategory);

            var equipmentRevisionParams = ((UEDataTableObject)cells[15].Value).Cells;
            equipmentRevisionParams[0].Value = model.HPRevision;
            equipmentRevisionParams[1].Value = model.MPRevision;
            equipmentRevisionParams[2].Value = model.BPRevision;
            equipmentRevisionParams[3].Value = model.SPRevision;
            equipmentRevisionParams[4].Value = model.PAttackRevision;
            equipmentRevisionParams[5].Value = model.PDefenseRevision;
            equipmentRevisionParams[6].Value = model.MAttackRevision;
            equipmentRevisionParams[7].Value = model.MDefenseRevision;
            equipmentRevisionParams[8].Value = model.AccuracyRevision;
            equipmentRevisionParams[9].Value = model.EvasionRevision;
            equipmentRevisionParams[10].Value = model.CriticalRevision;
            equipmentRevisionParams[11].Value = model.SpeedRevision;
            ((UEDataTableObject)cells[15].Value).Cells = equipmentRevisionParams;

            var attributeResistanceCellList = (List<UEDataTableCell>)cells[16].Value;
            for (int i = 0; i < attributeResistanceCellList.Count; i++)
            {
                attributeResistanceCellList[i].Value = CommonOctomodUtilities.ConvertAttributeResistanceToString(model.AttributeResistances[i]);
            }
            cells[16].Value = attributeResistanceCellList;

            List<UEDataTableCell> diseaseResistanceCellList = (List<UEDataTableCell>)cells[17].Value;
            diseaseResistanceCellList[0].Value = model.ResistPoison;
            diseaseResistanceCellList[1].Value = model.ResistSilence;
            diseaseResistanceCellList[2].Value = model.ResistBlindness;
            diseaseResistanceCellList[3].Value = model.ResistConfusion;
            diseaseResistanceCellList[4].Value = model.ResistSleep;
            diseaseResistanceCellList[5].Value = model.ResistTerror;
            diseaseResistanceCellList[6].Value = model.ResistUnconciousness;
            diseaseResistanceCellList[7].Value = model.ResistInstantDeath;
            diseaseResistanceCellList[8].Value = model.ResistTransform;
            diseaseResistanceCellList[9].Value = model.ResistDebuff;
            cells[17].Value = diseaseResistanceCellList;

            cells[18].Value = model.CommandEffecterID;

            originalRow.Cells = cells;

            return originalRow;
        }

        public override Item ParseSingle(KeyValuePair<string, UEDataTableObject> row)
        {
            var cells = row.Value.Cells;

            Item item = new Item();
            item.Key = row.Key;
            item.OriginalRow = row.Value;

            item.ItemID = cells[0].Value;
            item.ItemNameID = cells[1].Value;
            item.DetailTextID = cells[2].Value;
            item.IconLabelID = cells[3].Value;
            item.Category = CommonOctomodUtilities.ConvertStringToItemCategory(cells[4].Value);
            item.SortCategory = cells[5].Value;
            item.DisplayType = CommonOctomodUtilities.ConvertStringToItemDisplayType(cells[6].Value);
            item.UseType = CommonOctomodUtilities.ConvertStringToItemUseType(cells[7].Value);
            item.TargetType = CommonOctomodUtilities.ConvertStringToTargetType(cells[8].Value);
            item.AttributeType = CommonOctomodUtilities.ConvertStringToItemAttributeType(cells[9].Value);

            List<Ailment> ailments = new List<Ailment>();
            var ailmentCellList = (List<UEDataTableCell>)cells[10].Value;
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
            item.Ailments = ailments.ToArray();

            item.IsValuable = cells[11].Value;
            item.BuyPrice = cells[12].Value;
            item.SellPrice = cells[13].Value;
            item.EquipmentCategory = CommonOctomodUtilities.ConvertStringToItemEquipmentCategory(cells[14].Value);
            
            var equipmentRevisionParams = ((UEDataTableObject)cells[15].Value).Cells;
            item.HPRevision = equipmentRevisionParams[0].Value;
            item.MPRevision = equipmentRevisionParams[1].Value;
            item.BPRevision = equipmentRevisionParams[2].Value;
            item.SPRevision = equipmentRevisionParams[3].Value;
            item.PAttackRevision = equipmentRevisionParams[4].Value;
            item.PDefenseRevision = equipmentRevisionParams[5].Value;
            item.MAttackRevision = equipmentRevisionParams[6].Value;
            item.MDefenseRevision = equipmentRevisionParams[7].Value;
            item.AccuracyRevision = equipmentRevisionParams[8].Value;
            item.EvasionRevision = equipmentRevisionParams[9].Value;
            item.CriticalRevision = equipmentRevisionParams[10].Value;
            item.SpeedRevision = equipmentRevisionParams[11].Value;

            var attributeResistanceCellList = (List<UEDataTableCell>)cells[16].Value;
            List<AttributeResistance> resistances = new List<AttributeResistance>();
            for(int i = 0; i < attributeResistanceCellList.Count; i++)
            {
                var resistance = CommonOctomodUtilities.ConvertStringToAttributeResistance(attributeResistanceCellList[i].Value);
                resistances.Add(resistance);
            }
            item.AttributeResistances = resistances.ToArray();

            List<UEDataTableCell> diseaseResistanceCellList = (List<UEDataTableCell>)cells[17].Value;
            item.ResistPoison = diseaseResistanceCellList[0].Value;
            item.ResistSilence = diseaseResistanceCellList[1].Value;
            item.ResistBlindness = diseaseResistanceCellList[2].Value;
            item.ResistConfusion = diseaseResistanceCellList[3].Value;
            item.ResistSleep = diseaseResistanceCellList[4].Value;
            item.ResistTerror = diseaseResistanceCellList[5].Value;
            item.ResistUnconciousness = diseaseResistanceCellList[6].Value;
            item.ResistInstantDeath = diseaseResistanceCellList[7].Value;
            item.ResistTransform = diseaseResistanceCellList[8].Value;
            item.ResistDebuff = diseaseResistanceCellList[9].Value;

            item.CommandEffecterID = cells[18].Value;

            return item;
        }
    }
}
