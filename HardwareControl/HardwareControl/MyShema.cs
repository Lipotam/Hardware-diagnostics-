using HardwareControl.Elements;

namespace HardwareControl
{
    class MyShema : ShemaMap
    {
        public MyShema()
           
        {
            CreateShema();
        }

        protected override void CreateShema()
        {
            Element inp1 = ElementFactory.SharedFactory.CreateElement(ElementsType.Input, _IOController, "x1");
            Element inp2 = ElementFactory.SharedFactory.CreateElement(ElementsType.Input, _IOController, "x2");
            Element inp3 = ElementFactory.SharedFactory.CreateElement(ElementsType.Input, _IOController, "x3");
            Element inp4 = ElementFactory.SharedFactory.CreateElement(ElementsType.Input, _IOController, "x4");
            Element inp5 = ElementFactory.SharedFactory.CreateElement(ElementsType.Input, _IOController, "x5");
            Element inp6 = ElementFactory.SharedFactory.CreateElement(ElementsType.Input, _IOController, "x6");
            Element inp7 = ElementFactory.SharedFactory.CreateElement(ElementsType.Input, _IOController, "x7");
            Element or1 = ElementFactory.SharedFactory.CreateElement(ElementsType.NotAnd, _IOController, "F1");
            Element not = ElementFactory.SharedFactory.CreateElement(ElementsType.Not, _IOController, "F2");
            Element and1 = ElementFactory.SharedFactory.CreateElement(ElementsType.NotOr, _IOController, "F3");
            Element notAnd = ElementFactory.SharedFactory.CreateElement(ElementsType.NotOr, _IOController, "F4");
            Element and2 = ElementFactory.SharedFactory.CreateElement(ElementsType.NotXor, _IOController, "F5");
            Element or2 = ElementFactory.SharedFactory.CreateElement(ElementsType.NotAnd, _IOController, "F6");
            Element outp = ElementFactory.SharedFactory.CreateElement(ElementsType.Output, _IOController, "y");

            _wires.Add(new Wire(inp1, or1));
            _wires.Add(new Wire(inp2, or1));
            _wires.Add(new Wire(inp3, not));
            _wires.Add(new Wire(inp4, notAnd));
            _wires.Add(new Wire(inp5, and1));
            _wires.Add(new Wire(inp6, and1));
            _wires.Add(new Wire(inp7, notAnd));
            _wires.Add(new Wire(or1, or2));
            _wires.Add(new Wire(not, and2));
            _wires.Add(new Wire(and1, notAnd));
            _wires.Add(new Wire(notAnd, and2));
            _wires.Add(new Wire(and2, or2));
            _wires.Add(new Wire(or2, outp));
        }
    }
}
