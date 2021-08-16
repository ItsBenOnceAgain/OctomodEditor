using System;
using System.Collections.Generic;
using System.Text;

namespace OctomodEditor.Models
{
    public class GameText : OctopathModel
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
