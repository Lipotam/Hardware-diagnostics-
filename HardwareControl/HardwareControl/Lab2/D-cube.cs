using System;
using System.Collections.Generic;

namespace HardwareControl.Lab2
{
    class DCube
    {
        private Dictionary<String, D_cubeValues> _cubeValues;
        private List<String> _wires;

        public List<String> Wires
        {
            get
            {
                return _wires;
            }
        }

        public DCube(List<String> wires)
        {
            _wires = new List<string>(wires);
            _cubeValues = new Dictionary<string, D_cubeValues>();
            foreach (String name in _wires)
            {
                _cubeValues.Add(name, D_cubeValues.X);
            }
        }

        public void SetValue(String name, D_cubeValues value)
        {
            if (_cubeValues.ContainsKey(name))
            {
                _cubeValues[name] = value;
            }
        }

        public D_cubeValues GetValue(String name)
        {
            if (_cubeValues.ContainsKey(name))
            {
                return _cubeValues[name];
            }
            else
            {
                return D_cubeValues.X;
            }
        }

        public DCube IntersectCubes(DCube cube)
        {
            DCube newCube = new DCube(_wires);
            foreach (String name in _wires)
            {
                D_cubeValues val1 = _cubeValues[name];
                D_cubeValues val2 = cube._cubeValues[name];
                if (val1 == val2)
                {
                    newCube.SetValue(name, val1);
                }
                else
                {
                    if ((val1 == D_cubeValues.X) || (val2 == D_cubeValues.X))
                    {
                        newCube.SetValue(name, (val1 == D_cubeValues.X) ? val2 : val1);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return newCube;
        }
    }
}
