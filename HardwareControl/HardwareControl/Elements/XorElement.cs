using HardwareControl.Elements.Logic;

namespace HardwareControl.Elements
{
    class XorElement : LogicElement
    {
        public XorElement() : base(ElementsType.Xor, new NotOrLogic()) { }
    }
}
