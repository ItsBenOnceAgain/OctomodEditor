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

        public override bool Equals(object obj)
        {
            return obj is Ailment ailment &&
                   AilmentName == ailment.AilmentName &&
                   InvocationValue == ailment.InvocationValue &&
                   InvocationTurn == ailment.InvocationTurn &&
                   DiseaseRate == ailment.DiseaseRate;
        }

        public override int GetHashCode()
        {
            int hashCode = -1888645463;
            hashCode = hashCode * -1521134295 + AilmentName.GetHashCode();
            hashCode = hashCode * -1521134295 + InvocationValue.GetHashCode();
            hashCode = hashCode * -1521134295 + InvocationTurn.GetHashCode();
            hashCode = hashCode * -1521134295 + DiseaseRate.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Ailment a, Ailment b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Ailment a, Ailment b)
        {
            return !(a == b);
        }
    }
}
