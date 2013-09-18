using System;
using System.Collections.Generic;
using System.Linq;

using HardwareControl.Elements;

namespace HardwareControl
{
    class ModelingSet
    {
        private Dictionary<String, ElementsValues> _values;

        public ModelingSet(List<String> set)
        {
            _values = new Dictionary<string, ElementsValues>();
            foreach (String name in set)
            {
                _values.Add(name, ElementsValues.Unset);
            }
        }

        public ModelingSet(ModelingSet set)
        {
            _values = new Dictionary<string, ElementsValues>(set._values);
        }

        public List<String> ElementNames
        {
            get
            {
                return _values.Keys.ToList();
            }
        }

        public bool SetValue(String name, ElementsValues value)
        {
            if (_values.ContainsKey(name))
            {
                _values[name] = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public ElementsValues GetValue(String name)
        {
            if (_values.ContainsKey(name))
            {
                return _values[name];
            }
            else
            {
                return ElementsValues.Unset;
            }
        }

        public ModelingSet UnionSet(ModelingSet set)
        {
            ModelingSet result = new ModelingSet(this._values.Keys.ToList());
            foreach (String name in _values.Keys.ToList())
            {
                if ((GetValue(name) == ElementsValues.Undefined) || (set.GetValue(name) == ElementsValues.Undefined))
                {
                    return null;
                }
                if ((_values[name] == ElementsValues.Unset) && (set._values[name] != ElementsValues.Unset))
                {
                    result.SetValue(name, set._values[name]);
                }
                else if ((_values[name] != ElementsValues.Unset) && (set._values[name] == ElementsValues.Unset))
                {
                    result.SetValue(name, _values[name]);
                }
                else if ((_values[name] != ElementsValues.Unset) && (set._values[name] != ElementsValues.Unset))
                {
                    if (_values[name] != set._values[name])
                    {
                        return null;
                    }
                    else
                    {
                        result.SetValue(name, _values[name]);
                    }
                }
            }
            return result;
        }

        public override string ToString()
        {
            String str = "";
            foreach (String name in _values.Keys.ToList())
            {
                String value = "";
                switch (_values[name])
                {
                    case ElementsValues.True:
                        {
                            value = "1";
                            break;
                        }
                    case ElementsValues.False:
                        {
                            value = "0";
                            break;
                        }
                    case ElementsValues.Undefined:
                        {
                            value = "X";
                            break;
                        }
                    case ElementsValues.Unset:
                        {
                            value = "U";
                            break;
                        }
                }
                str += name + "=" + value + "; ";
            }
            return str;
        }

        public List<String> ToList()
        {
            List<String> values = new List<string>();
            foreach (ElementsValues val in _values.Values.ToList())
            {
                switch (val)
                {
                    case ElementsValues.True:
                        {
                            values.Add("1");
                            break;
                        }
                    case ElementsValues.False:
                        {
                            values.Add("0");
                            break;
                        }
                    case ElementsValues.Undefined:
                        {
                            values.Add("U");
                            break;
                        }
                    case ElementsValues.Unset:
                        {
                            values.Add("X");
                            break;
                        }
                }
                //values.Add(val.ToString());
            }
            return values;
        }

        public int SwitchingNumbers(ModelingSet set)
        {
            return this._values.Keys.Count(value => _values[value] != set._values[value]);
        }
    }
}
