using HardwareControl.Elements.Logic;

namespace HardwareControl.Elements
{
    class NotAndElement : LogicElement
    {
        public NotAndElement() : base(ElementsType.NotAnd, new NotAndLogic()) { }
    }
}
