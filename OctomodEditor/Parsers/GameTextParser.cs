using DataEditorUE4.Models;
using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctomodEditor.Parsers
{
    public class GameTextParser : OctopathModelParser<GameText>
    {
        public GameTextParser(string language)
        {
            Language = language;
            FilePrefix = $"GameText{language}";
        }

        public override string Directory => "GameText/Database/";

        public override string FilePrefix { get; }

        public string Language { get; set; }

        public override UEDataTableObject ConvertModelToTableRow(GameText model)
        {
            var originalRow = model.OriginalRow;
            var cells = originalRow.Cells;

            cells[0].Value = model.Text;
            originalRow.Cells = cells;

            return originalRow;
        }

        public override GameText ParseSingle(KeyValuePair<string, UEDataTableObject> row)
        {
            var cells = row.Value.Cells;

            GameText text = new GameText();
            text.Key = row.Key;
            text.OriginalRow = row.Value;

            text.Text = cells[0].Value;

            return text;
        }
    }
}
