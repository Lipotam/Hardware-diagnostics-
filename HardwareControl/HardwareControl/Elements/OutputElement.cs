namespace HardwareControl.Elements
{
    class OutputElement : Element
    {
        public OutputElement() : base(ElementsType.Output) { }

        public override ElementsValues GetSelfValue()
        {
            return _inputs.Count > 0 ? _inputs[0].GetValue() : ElementsValues.Unset;
        }
    }
}
