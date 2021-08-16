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

        public abstract UEDataTableObject ConvertModelToTableRow(T model);

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

        public UEDataTable ConvertListToTable(UEDataTable originalTable, Dictionary<string, T> objectListToSave)
        {
            foreach (var pair in objectListToSave)
            {
                if (originalTable.Rows.ContainsKey(pair.Key))
                {
                    originalTable.Rows[pair.Key] = ConvertModelToTableRow(pair.Value);
                }
                else
                {
                    originalTable.Rows.Add(pair.Key, ConvertModelToTableRow(pair.Value));
                }
            }
            return originalTable;
        }

        public void SaveTable(UEDataTable originalTable, List<T> objectsToSave)
        {
            var modelDictionary = objectsToSave.ToDictionary(x => x.Key, x => x);
            var table = ConvertListToTable(originalTable, modelDictionary);
            (string uassetPath, string uexpPath) =
                CommonOctomodUtilities.GetProperUassetAndUexpModWritePathsAndCreateDirectories(Directory, FilePrefix);

            DataTableFileWriter.WriteTableToFile(table, uassetPath, uexpPath);
        }

        public string[] ConvertCellListToStringArray(List<UEDataTableCell> cells)
        {
            List<string> strings = new List<string>();
            foreach (UEDataTableCell cell in cells)
            {
                strings.Add(cell.Value);
            }
            return strings.ToArray();
        }

        public List<UEDataTableCell> ConvertStringArrayToCellList(List<UEDataTableCell> cells, string[] strings)
        {
            for(int i = 0; i < cells.Count; i++)
            {
                cells[i].Value = strings[i];
            }
            return cells;
        }

        public Vector2 ConvertObjectToVector2(UEDataTableObject vectorObject)
        {
            var dataCells = vectorObject.Cells;
            return new Vector2(dataCells[0].Value, dataCells[1].Value);
        }

        public UEDataTableObject ConvertVector2ToObject(UEDataTableObject vectorObject, Vector2 vector)
        {
            vectorObject.Cells[0].Value = vector.X;
            vectorObject.Cells[1].Value = vector.Y;
            return vectorObject;
        }

        public Vector3 ConvertObjectToVector3(UEDataTableObject vectorObject)
        {
            var dataCells = vectorObject.Cells;
            return new Vector3(dataCells[0].Value, dataCells[1].Value, dataCells[2].Value);
        }

        public UEDataTableObject ConvertVector3ToObject(UEDataTableObject vectorObject, Vector3 vector)
        {
            vectorObject.Cells[0].Value = vector.X;
            vectorObject.Cells[1].Value = vector.Y;
            vectorObject.Cells[2].Value = vector.Z;
            return vectorObject;
        }

        public Vector4 ConvertObjectToVector4(UEDataTableObject vectorObject)
        {
            var dataCells = vectorObject.Cells;
            return new Vector4(dataCells[0].Value, dataCells[1].Value, dataCells[2].Value, dataCells[3].Value);
        }

        public UEDataTableObject ConvertVector4ToObject(UEDataTableObject vectorObject, Vector4 vector)
        {
            vectorObject.Cells[0].Value = vector.X;
            vectorObject.Cells[1].Value = vector.Y;
            vectorObject.Cells[2].Value = vector.Z;
            vectorObject.Cells[3].Value = vector.W;
            return vectorObject;
        }
    }
}
