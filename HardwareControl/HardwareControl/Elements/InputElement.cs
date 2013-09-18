namespace HardwareControl.Elements
{
    class InputElement : Element
    {
        private ElementsValues _value;
        public ElementsValues Value
        {
            set
            {
                _value = value;
            }
        }

        public InputElement()
            : base(ElementsType.Input)
        {
            _value = ElementsValues.Unset;
        }

        public override ElementsValues GetSelfValue()
        {
            return _value;
        }
    }
}
