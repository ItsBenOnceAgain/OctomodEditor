using DataEditorUE4.Models;
using OctomodEditor.Models;
using OctomodEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctomodEditor.Parsers
{
    public class ShopInfoParser : OctopathModelParser<ShopInfo>
    {
        public override string Directory => @"Shop/Database/";

        public override string FilePrefix => "ShopInfo";

        public override UEDataTableObject ConvertModelToTableRow(ShopInfo model)
        {
            var originalRow = model.OriginalRow;
            var cells = originalRow.Cells;

            cells[0].Value = model.ShopName;
            cells[1].Value = CommonOctomodUtilities.ConvertShopTypeToString(model.ShopType);
            cells[2].Value = model.ShopBGM;
            cells[3].Value = model.InnBasePrice;
            cells[4].Value = model.InnDiscountItem;
            cells[5].Value = model.InnDiscountBasePrice;

            originalRow.Cells = cells;
            return originalRow;
        }

        public override ShopInfo ParseSingle(KeyValuePair<string, UEDataTableObject> row)
        {
            var cells = row.Value.Cells;

            ShopInfo info = new ShopInfo();
            info.Key = row.Key;
            info.OriginalRow = row.Value;

            info.ShopName = cells[0].Value;
            info.ShopType = CommonOctomodUtilities.ConvertStringToShopType(cells[1].Value);
            info.ShopBGM = cells[2].Value;
            info.InnBasePrice = cells[3].Value;
            info.InnDiscountItem = cells[4].Value;
            info.InnDiscountBasePrice = cells[5].Value;

            return info;
        }
    }
}
