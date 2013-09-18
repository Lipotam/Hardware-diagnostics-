using System;

namespace HardwareControl.Elements
{
    class Wire
    {
        private Element _setter;
        private Element _getter;
        private bool _isDefected;
        private bool _defectType;

        public Element Setter
        {
            get
            {
                return _setter;
            }
        }
        public Element Getter
        {
            get
            {
                return _getter;
            }
        }
        public String Name
        {
            get
            {
                return _setter.GetName() + "=>" + _getter.GetName();
            }
        }

        public Wire(Element setter, Element getter)
        {
            _setter = setter;
            _setter.AddOutput(this);
            _getter = getter;
            _getter.AddInput(this);
            _isDefected = false;
        }

        public ElementsValues GetValue()
        {
            if (_isDefected)
            {
                return _defectType ? ElementsValues.True : ElementsValues.False;
            }
            else
            {
                return _setter != null ? _setter.GetSelfValue() : ElementsValues.Unset;
            }
        }

        public void Remove()
        {
            _setter.RemoveOutput(this);
            _setter = null;
            _getter.RemoveInput(this);
            _getter = null;
        }

        public void SetDefect(bool type)
        {
            _isDefected = true;
            _defectType = type;
        }

        public void RemoveDefect()
        {
            _isDefected = false;
        }
    }
}
