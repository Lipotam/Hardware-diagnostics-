using HardwareControl.Elements.Logic;

namespace HardwareControl.Elements
{
    class NotOrElement : LogicElement
    {
        public NotOrElement() : base(ElementsType.NotOr, new NotOrLogic()) { }
    }
}
