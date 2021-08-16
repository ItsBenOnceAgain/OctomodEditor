using DataEditorUE4.Utilities;
using DataEditorUE4.Models;
using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctomodEditor.Utilities;

namespace OctomodEditor.Parsers
{
    public class EnemyParser : OctopathModelParser<Enemy>
    {
        public override string Directory => @"Character/Database/";
        public override string FilePrefix => "EnemyDB";

        public override Enemy ParseSingle(KeyValuePair<string, UEDataTableObject> row)
        {
            var cells = row.Value.Cells;

            Enemy enemy = new Enemy();
            enemy.Key = row.Key;
            enemy.OriginalRow = row.Value;

            enemy.EnemyID = cells[0].Value;
            enemy.DisplayNameID = cells[1].Value;
            enemy.FlipbookPath = cells[2].Value;
            enemy.TexturePath = cells[3].Value;
            enemy.DisplayRank = cells[4].Value;
            enemy.EnemyLevel = cells[5].Value;
            enemy.DamageRatio = cells[6].Value;
            enemy.RaceType = CommonOctomodUtilities.ConvertStringToRaceType(cells[7].Value);
            enemy.Size = CommonOctomodUtilities.ConvertStringToSizeType(cells[8].Value);
            enemy.IsNPC = cells[9].Value;
            enemy.PlaysSlowAnimationOnDeath = cells[10].Value;
            enemy.IsMainEnemy = cells[11].Value;
            enemy.IsExemptFromBattle = cells[12].Value;
            enemy.UsesCatDamageType = cells[13].Value;
            enemy.HasNoKnockbackAnimation = cells[14].Value;

            var paramsCells = ((UEDataTableObject)cells[15].Value).Cells;
            enemy.HP = paramsCells[0].Value;
            enemy.MP = paramsCells[1].Value;
            enemy.BP = paramsCells[2].Value;
            enemy.Shields = paramsCells[3].Value;
            enemy.PhysicalAttack = paramsCells[4].Value;
            enemy.PhysicalDefense = paramsCells[5].Value;
            enemy.ElementalAttack = paramsCells[6].Value;
            enemy.ElementalDefense = paramsCells[7].Value;
            enemy.Accuracy = paramsCells[8].Value;
            enemy.Evasion = paramsCells[9].Value;
            enemy.Critical = paramsCells[10].Value;
            enemy.Speed = paramsCells[11].Value;

            enemy.ExperiencePoints = cells[16].Value;
            enemy.JobPoints = cells[17].Value;
            enemy.Money = cells[18].Value;
            enemy.MoneyFromCollecting = cells[19].Value;
            enemy.CanBeCaptured = cells[20].Value;
            enemy.DefaultTameRate = cells[21].Value;
            enemy.CapturedEnemyID = cells[22].Value;
            enemy.FirstBP = cells[23].Value;
            enemy.BreakType = cells[24].Value;
            enemy.InvocationValue = cells[25].Value;
            enemy.InvocationTurn = cells[26].Value;
            enemy.DeadType = CommonOctomodUtilities.ConvertStringToDeadType(cells[27].Value);
            
            var elementalResistanceCells = (List<UEDataTableCell>)cells[28].Value;
            enemy.IsWeakToFire = ConvertStringToAttributeBool(elementalResistanceCells[1].Value);
            enemy.IsWeakToIce = ConvertStringToAttributeBool(elementalResistanceCells[2].Value);
            enemy.IsWeakToLightning = ConvertStringToAttributeBool(elementalResistanceCells[3].Value);
            enemy.IsWeakToWind = ConvertStringToAttributeBool(elementalResistanceCells[4].Value);
            enemy.IsWeakToLight = ConvertStringToAttributeBool(elementalResistanceCells[5].Value);
            enemy.IsWeakToDark = ConvertStringToAttributeBool(elementalResistanceCells[6].Value);

            var physicalResistanceCells = (List<UEDataTableCell>)cells[29].Value;
            enemy.IsWeakToSwords = ConvertStringToAttributeBool(physicalResistanceCells[0].Value);
            enemy.IsWeakToSpears = ConvertStringToAttributeBool(physicalResistanceCells[1].Value);
            enemy.IsWeakToDaggers = ConvertStringToAttributeBool(physicalResistanceCells[2].Value);
            enemy.IsWeakToAxes = ConvertStringToAttributeBool(physicalResistanceCells[3].Value);
            enemy.IsWeakToBows = ConvertStringToAttributeBool(physicalResistanceCells[4].Value);
            enemy.IsWeakToStaves = ConvertStringToAttributeBool(physicalResistanceCells[5].Value);

            var diseaseResistanceCells = (List<UEDataTableCell>)cells[30].Value;
            enemy.AttributeResistances = GetDiseaseResistances(diseaseResistanceCells.ToArray());

            enemy.IsGuardedFromStealing = cells[31].Value;
            enemy.ItemID = cells[32].Value;
            enemy.ItemDropPercentage = cells[33].Value;
            enemy.AIPath = cells[34].Value;
            
            var abilityCells = (List<UEDataTableCell>)cells[35].Value;
            enemy.AbilityList = ConvertCellListToStringArray(abilityCells);

            var eventCells = (List<UEDataTableCell>)cells[36].Value;
            enemy.BattleEvents = ConvertCellListToStringArray(eventCells);

            enemy.DiseaseOffset = ConvertObjectToVector3(cells[37].Value);
            enemy.EnemyEffectOffset = ConvertObjectToVector3(cells[38].Value);
            enemy.StatusUIOffset = ConvertObjectToVector3(cells[39].Value);
            enemy.DamageUIOffset = ConvertObjectToVector3(cells[40].Value);
            enemy.IconL = ConvertObjectToVector2(cells[41].Value);
            enemy.PixelScaleL = ConvertObjectToVector2(cells[42].Value);
            enemy.IconS = ConvertObjectToVector2(cells[43].Value);
            enemy.PixelScaleS = ConvertObjectToVector2(cells[44].Value);

            return enemy;
        }

        public override UEDataTableObject ConvertModelToTableRow(Enemy enemy)
        {
            var originalRow = enemy.OriginalRow;
            var cells = originalRow.Cells;

            cells[0].Value = enemy.EnemyID;
            cells[1].Value = enemy.DisplayNameID;
            cells[2].Value = enemy.FlipbookPath;
            cells[3].Value = enemy.TexturePath;
            cells[4].Value = enemy.DisplayRank;
            cells[5].Value = enemy.EnemyLevel;
            cells[6].Value = enemy.DamageRatio;

            cells[7].Value = CommonOctomodUtilities.ConvertRaceTypeToString(enemy.RaceType);
            cells[8].Value = CommonOctomodUtilities.ConvertSizeTypeToString(enemy.Size);
            cells[9].Value = enemy.IsNPC;
            cells[10].Value = enemy.PlaysSlowAnimationOnDeath;
            cells[11].Value = enemy.IsMainEnemy;
            cells[12].Value = enemy.IsExemptFromBattle;
            cells[13].Value = enemy.UsesCatDamageType;
            cells[14].Value = enemy.HasNoKnockbackAnimation;

            var paramsCells = ((UEDataTableObject)cells[15].Value).Cells;
            paramsCells[0].Value = enemy.HP;
            paramsCells[1].Value = enemy.MP;
            paramsCells[2].Value = enemy.BP;
            paramsCells[3].Value = enemy.Shields;
            paramsCells[4].Value = enemy.PhysicalAttack;
            paramsCells[5].Value = enemy.PhysicalDefense;
            paramsCells[6].Value = enemy.ElementalAttack;
            paramsCells[7].Value = enemy.ElementalDefense;
            paramsCells[8].Value = enemy.Accuracy;
            paramsCells[9].Value = enemy.Evasion;
            paramsCells[10].Value = enemy.Critical;
            paramsCells[11].Value = enemy.Speed;
            ((UEDataTableObject)cells[15].Value).Cells = paramsCells;

            cells[16].Value = enemy.ExperiencePoints;
            cells[17].Value = enemy.JobPoints;
            cells[18].Value = enemy.Money;
            cells[19].Value = enemy.MoneyFromCollecting;
            cells[20].Value = enemy.CanBeCaptured;
            cells[21].Value = enemy.DefaultTameRate;
            cells[22].Value = enemy.CapturedEnemyID;
            cells[23].Value = enemy.FirstBP;
            cells[24].Value = enemy.BreakType;
            cells[25].Value = enemy.InvocationValue;
            cells[26].Value = enemy.InvocationTurn;
            cells[27].Value = CommonOctomodUtilities.ConvertDeadTypeToString(enemy.DeadType);

            var elementalResistanceCells = (List<UEDataTableCell>)cells[28].Value;
            elementalResistanceCells[1].Value = ConvertAttributeBoolToString(enemy.IsWeakToFire);
            elementalResistanceCells[2].Value = ConvertAttributeBoolToString(enemy.IsWeakToIce);
            elementalResistanceCells[3].Value = ConvertAttributeBoolToString(enemy.IsWeakToLightning);
            elementalResistanceCells[4].Value = ConvertAttributeBoolToString(enemy.IsWeakToWind);
            elementalResistanceCells[5].Value = ConvertAttributeBoolToString(enemy.IsWeakToLight);
            elementalResistanceCells[6].Value = ConvertAttributeBoolToString(enemy.IsWeakToDark);
            cells[28].Value = elementalResistanceCells;

            var physicalResistanceCells = (List<UEDataTableCell>)cells[29].Value;
            physicalResistanceCells[0].Value = ConvertAttributeBoolToString(enemy.IsWeakToSwords);
            physicalResistanceCells[1].Value = ConvertAttributeBoolToString(enemy.IsWeakToSpears);
            physicalResistanceCells[2].Value = ConvertAttributeBoolToString(enemy.IsWeakToDaggers);
            physicalResistanceCells[3].Value = ConvertAttributeBoolToString(enemy.IsWeakToAxes);
            physicalResistanceCells[4].Value = ConvertAttributeBoolToString(enemy.IsWeakToBows);
            physicalResistanceCells[5].Value = ConvertAttributeBoolToString(enemy.IsWeakToStaves);
            cells[29].Value = physicalResistanceCells;

            var diseaseResistanceCells = (List<UEDataTableCell>)cells[30].Value;
            diseaseResistanceCells = ConvertAttributeResistancesToCellList(diseaseResistanceCells, enemy.AttributeResistances);
            cells[30].Value = diseaseResistanceCells;

            cells[31].Value = enemy.IsGuardedFromStealing;
            cells[32].Value = enemy.ItemID;
            cells[33].Value = enemy.ItemDropPercentage;
            cells[34].Value = enemy.AIPath;

            var abilityCells = (List<UEDataTableCell>)cells[35].Value;
            abilityCells = ConvertStringArrayToCellList(abilityCells, enemy.AbilityList);
            cells[35].Value = abilityCells;

            var eventCells = (List<UEDataTableCell>)cells[36].Value;
            eventCells = ConvertStringArrayToCellList(eventCells, enemy.BattleEvents);
            cells[36].Value = eventCells;

            cells[37].Value = ConvertVector3ToObject(cells[37].Value, enemy.DiseaseOffset);
            cells[38].Value = ConvertVector3ToObject(cells[38].Value, enemy.EnemyEffectOffset);
            cells[39].Value = ConvertVector3ToObject(cells[39].Value, enemy.StatusUIOffset);
            cells[40].Value = ConvertVector3ToObject(cells[40].Value, enemy.DamageUIOffset);
            cells[41].Value = ConvertVector2ToObject(cells[41].Value, enemy.IconL);
            cells[42].Value = ConvertVector2ToObject(cells[42].Value, enemy.PixelScaleL);
            cells[43].Value = ConvertVector2ToObject(cells[43].Value, enemy.IconS);
            cells[44].Value = ConvertVector2ToObject(cells[44].Value, enemy.PixelScaleS);

            originalRow.Cells = cells;

            return originalRow;
        }

        public bool ConvertStringToAttributeBool(string attributeString)
        {
            return attributeString == "EATTRIBUTE_RESIST::NewEnumerator1";
        }

        public string ConvertAttributeBoolToString(bool attributeBool)
        {
            return attributeBool ? "EATTRIBUTE_RESIST::NewEnumerator1" : "EATTRIBUTE_RESIST::NewEnumerator0";
        }

        public AttributeResistance[] GetDiseaseResistances(UEDataTableCell[] attributeCells)
        {
            AttributeResistance[] resistances = new AttributeResistance[12];
            for(int i = 0; i < 12; i++)
            {
                resistances[i] = CommonOctomodUtilities.ConvertStringToAttributeResistance(attributeCells[i].Value);
            }
            return resistances;
        }

        public List<UEDataTableCell> ConvertAttributeResistancesToCellList(List<UEDataTableCell> attributeCells, AttributeResistance[] resistances)
        {
            for (int i = 0; i < 12; i++)
            {
                attributeCells[i].Value = CommonOctomodUtilities.ConvertAttributeResistanceToString(resistances[i]);
            }
            return attributeCells;
        }
    }
}
