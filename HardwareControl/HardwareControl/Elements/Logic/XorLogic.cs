using System.Collections.Generic;
using System.Linq;

namespace HardwareControl.Elements.Logic
{
    class XorLogic : ILogic
    {

        public ElementsValues DoLogic(List<Wire> inputs)
        {
            if (inputs.All(wire => ((wire.GetValue() == ElementsValues.Unset) || (wire.GetValue() == ElementsValues.Undefined))) || inputs.Count != 2)
            {
                return ElementsValues.Undefined;
            }
            else
            {
                return inputs.First().GetValue() == inputs.Last().GetValue() ? ElementsValues.False : ElementsValues.True;
            }
        }
    }
}
