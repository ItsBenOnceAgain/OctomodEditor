using DataEditorUE4.Models;
using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctomodEditor.Parsers
{
    public class ShopListParser : OctopathModelParser<ShopList>
    {
        public override string Directory => @"Shop/Database/";

        public override string FilePrefix => "ShopList";

        public override UEDataTableObject ConvertModelToTableRow(ShopList model)
        {
            var originalRow = model.OriginalRow;
            var cells = originalRow.Cells;

            var shopCellList = (List<UEDataTableCell>)cells[0].Value;
            for (int i = 0; i < shopCellList.Count; i++)
            {
                shopCellList[i].Value = model.PurchaseItemIDs[i];
            }
            cells[0].Value = shopCellList;

            originalRow.Cells = cells;
            return originalRow;
        }

        public override ShopList ParseSingle(KeyValuePair<string, UEDataTableObject> row)
        {
            var cells = row.Value.Cells;

            ShopList shopList = new ShopList();
            shopList.Key = row.Key;
            shopList.OriginalRow = row.Value;

            var shopCellList = (List<UEDataTableCell>)cells[0].Value;
            List<string> shopListStrings = new List<string>();

            for(int i = 0; i < shopCellList.Count; i++)
            {
                shopListStrings.Add(shopCellList[i].Value);
            }
            shopList.PurchaseItemIDs = shopListStrings.ToArray();
            return shopList;
        }
    }
}
