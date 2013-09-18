using HardwareControl.Elements.Logic;

namespace HardwareControl.Elements
{
    class NotXorElement : LogicElement
    {
        public NotXorElement() : base(ElementsType.NotXor, new NotXorLogic()) { }
    }
}
