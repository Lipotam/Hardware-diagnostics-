using HardwareControl.Elements.Logic;

namespace HardwareControl.Elements
{
    class LogicElement : Element
    {
        private readonly ILogic logic;

        public LogicElement(ElementsType type, ILogic logic)
            : base(type)
        {
            this.logic = logic;
        }

        public override ElementsValues GetSelfValue()
        {
            return this.logic.DoLogic(_inputs);
        }
    }
}
