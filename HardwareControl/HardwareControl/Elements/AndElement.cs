using HardwareControl.Elements.Logic;

namespace HardwareControl.Elements
{
    class AndElement : LogicElement
    {
        public AndElement() : base(ElementsType.And, new AndLogic()) { }
    }
}
