using System;
using System.Collections.Generic;

namespace HardwareControl.Elements
{
        abstract class Element
        {
            protected List<Wire> _inputs;
            protected List<Wire> _outputs;
            protected ElementsType _type;
            protected String _name;

            public List<Wire> Inputs
            {
                get
                {
                    return _inputs;
                }
            }
            public List<Wire> Outputs
            {
                get
                {
                    return _outputs;
                }
            }

            public ElementsType Type
            {
                get
                {
                    return _type;
                }
            }

            public Element(ElementsType type)
            {
                _inputs = new List<Wire>();
                _outputs = new List<Wire>();
                _type = type;
                _name = "";
            }

            public void AddInput(Wire wire)
            {
                _inputs.Add(wire);
            }

            public void RemoveInput(Wire wire)
            {
                _inputs.Remove(wire);
            }

            public void AddOutput(Wire wire)
            {
                _outputs.Add(wire);
            }

            public void RemoveOutput(Wire wire)
            {
                _outputs.Remove(wire);
            }

            public void SetName(String name)
            {
                _name = name;
            }

            public String GetName()
            {
                return _name;
            }

            public virtual void Rename(String newName)
            {
                _name = newName;
            }

            public virtual ElementsValues GetSelfValue()
            {
                return ElementsValues.Unset;
            }

            public void Remove()
            {
                while (_inputs.Count > 0)
                {
                    _inputs[0].Remove();
                }
                while (_outputs.Count > 0)
                {
                    _outputs[0].Remove();
                }
            }
        }
    
}
