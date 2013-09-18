using System.Collections.Generic;

namespace HardwareControl.Elements.Logic
{
    interface ILogic
    {
        ElementsValues DoLogic(List<Wire> inputs);
    }
}
