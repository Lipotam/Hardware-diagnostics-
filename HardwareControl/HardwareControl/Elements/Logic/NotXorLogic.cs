using System.Collections.Generic;
using System.Linq;

namespace HardwareControl.Elements.Logic
{
    class NotXorLogic : ILogic
    {

        public ElementsValues DoLogic(List<Wire> inputs)
        {
            if (inputs.All(wire => ((wire.GetValue() == ElementsValues.Unset) || (wire.GetValue() == ElementsValues.Undefined))) || inputs.Count != 2)
            {
                return ElementsValues.Undefined;
            }
            else
            {
                return inputs.First().GetValue() == inputs.Last().GetValue() ? ElementsValues.True : ElementsValues.False;
            }
        }
    }
}
