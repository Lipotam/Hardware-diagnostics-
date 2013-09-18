using System.Collections.Generic;
using System.Linq;

namespace HardwareControl.Elements.Logic
{
    class NotLogic : ILogic
    {
        public ElementsValues DoLogic(List<Wire> inputs)
        {
            bool returnValue = inputs[0].GetValue() != ElementsValues.True;
            if (inputs.All(wire => ((wire.GetValue() == ElementsValues.Unset) || (wire.GetValue() == ElementsValues.Undefined))))
            {
                return ElementsValues.Undefined;
            }
            else
            {
                return returnValue ? ElementsValues.True : ElementsValues.False;
            }
        }
    }
}
