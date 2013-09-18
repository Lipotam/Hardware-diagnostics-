using HardwareControl.Elements.Logic;

namespace HardwareControl.Elements
{
    class NotElement : LogicElement
    {
        public NotElement() : base(ElementsType.Not, new NotLogic()) { }
    }
}
