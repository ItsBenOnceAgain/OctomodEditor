using DataEditorUE4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public abstract class OctopathModel
    {
        public int Offset { get; set; }
        public string Key { get; set; }
        public UEDataTableObject OriginalRow { get; set; }
    }
}
