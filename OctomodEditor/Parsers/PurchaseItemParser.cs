using DataEditorUE4.Models;
using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctomodEditor.Parsers
{
    public class PurchaseItemParser : OctopathModelParser<PurchaseItem>
    {
        public override string Directory => @"Shop/Database/";

        public override string FilePrefix => "PurchaseItemTable";

        public override UEDataTableObject ConvertModelToTableRow(PurchaseItem model)
        {
            var originalRow = model.OriginalRow;
            var cells = originalRow.Cells;

            model.ItemLabel = cells[0].Value;
            model.FCPrice = cells[1].Value;
            model.PossibleFlag = cells[2].Value;
            model.PossibleItemLabel = cells[3].Value;
            model.ArrivalStatus = cells[4].Value;
            model.ObtainFlag = cells[5].Value;
            model.ProperSteal = cells[6].Value;

            originalRow.Cells = cells;
            return originalRow;
        }

        public override PurchaseItem ParseSingle(KeyValuePair<string, UEDataTableObject> row)
        {
            var cells = row.Value.Cells;

            PurchaseItem pItem = new PurchaseItem();
            pItem.Key = row.Key;
            pItem.OriginalRow = row.Value;

            pItem.ItemLabel = cells[0].Value;
            pItem.FCPrice = cells[1].Value;
            pItem.PossibleFlag = cells[2].Value;
            pItem.PossibleItemLabel = cells[3].Value;
            pItem.ArrivalStatus = cells[4].Value;
            pItem.ObtainFlag = cells[5].Value;
            pItem.ProperSteal = cells[6].Value;

            return pItem;
        }
    }
}
