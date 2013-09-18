using HardwareControl.Elements.Logic;

namespace HardwareControl.Elements
{
    class OrElement : LogicElement
    {
        public OrElement() : base(ElementsType.Or, new OrLogic()) { }
    }
}
