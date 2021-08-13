using DataEditorUE4.Models;
using DataEditorUE4.Utilities;
using OctomodEditor.Models;
using OctomodEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Parsers
{
    public abstract class OctopathModelParser<T> where T : OctopathModel
    {
        public abstract string Directory { get; }
        public abstract string FilePrefix { get; }

        public abstract T ParseSingle(KeyValuePair<string, UEDataTableObject> row);

        public abstract UEDataTable ConvertListToTable(Dictionary<string, T> objectList);

        public UEDataTable GetTableFromFile(bool useBaseGame = false)
        {
            (string uassetPath, string uexpPath) = CommonOctomodUtilities.GetProperUassetAndUexpReadPaths(Directory, FilePrefix, useBaseGame);

            var enemyTable = DataTableParser.CreateDataTable(uassetPath, uexpPath);
            return enemyTable;
        }

        public Dictionary<string, T> ParseTable(UEDataTable table)
        {
            Dictionary<string, T> octopathObjectList = new Dictionary<string, T>();
            foreach (var row in table.Rows)
            {
                var octopathObject = ParseSingle(row);
                octopathObjectList.Add(octopathObject.Key, octopathObject);
            }

            return octopathObjectList;
        }

        public void SaveTable(Dictionary<string, T> objectList)
        {
            var table = ConvertListToTable(objectList);
            (string uassetPath, string uexpPath) =
                CommonOctomodUtilities.GetProperUassetAndUexpModWritePathsAndCreateDirectories(Directory, FilePrefix);

            DataTableFileWriter.WriteTableToFile(table, uassetPath, uexpPath);
        }

        public string[] ConvertCellArrayToStringArray(UEDataTableCell[] cells)
        {
            List<string> strings = new List<string>();
            foreach (UEDataTableCell cell in cells)
            {
                strings.Add(cell.Value);
            }
            return strings.ToArray();
        }

        public Vector2 ConvertObjectToVector2(UEDataTableObject vectorObject)
        {
            var dataCells = vectorObject.Cells;
            return new Vector2(dataCells[0].Value, dataCells[1].Value);
        }

        public Vector3 ConvertObjectToVector3(UEDataTableObject vectorObject)
        {
            var dataCells = vectorObject.Cells;
            return new Vector3(dataCells[0].Value, dataCells[1].Value, dataCells[2].Value);
        }

        public Vector4 ConvertObjectToVector4(UEDataTableObject vectorObject)
        {
            var dataCells = vectorObject.Cells;
            return new Vector4(dataCells[0].Value, dataCells[1].Value, dataCells[2].Value, dataCells[3].Value);
        }
    }
}
