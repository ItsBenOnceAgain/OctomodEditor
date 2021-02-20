using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class Ailment
    {
        public string AilmentName { get; set; }
        public int InvocationValue { get; set; }
        public int InvocationTurn { get; set; }
        public int DiseaseRate { get; set; }
    }
}
