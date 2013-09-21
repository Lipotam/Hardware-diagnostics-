using System;
using System.Collections.Generic;
using System.Linq;

using HardwareControl.Elements;

namespace HardwareControl.Lab3
{
    class DefectSet
    {
        private Dictionary<String, Dictionary<bool, bool>> defects;
        private ModelingSet modelingSet;
        private ModelingSet modelingWires;

        public ModelingSet ModelingSet
        {
            get
            {
                return this.modelingSet;
            }
        }
        public ModelingSet ModelingWires
        {
            get
            {
                return this.modelingWires;
            }
        }
        public String ConstFalseDefects
        {
            get
            {
                return this.defects.Keys.ToList().Where(wire => this.defects[wire][false]).Aggregate("", (current, wire) => current + (wire + "; "));
            }
        }
        public String ConstTrueDefects
        {
            get
            {
                return this.defects.Keys.ToList().Where(wire => this.defects[wire][true]).Aggregate("", (current, wire) => current + (wire + "; "));
            }
        }

        public int DefectsCount
        {
            get
            {
                int count = 0;
                foreach (Dictionary<bool, bool> values in this.defects.Values.ToList())
                {
                    if (values[true])
                    {
                        count++;
                    }
                    if (values[false])
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public int MaxDefectCount
        {
            get
            {
                return this.defects.Count * 2;
            }
        }

        private DefectSet(List<String> wires, ModelingSet set, ModelingSet modelingWires)
        {
            this.defects = new Dictionary<string, Dictionary<bool, bool>>();
            this.modelingSet = set;
            this.modelingWires = modelingWires;
            foreach (String wire in wires)
            {
                Dictionary<bool, bool> values = new Dictionary<bool, bool>();
                values.Add(true, false);
                values.Add(false, false);
                this.defects.Add(wire, values);
            }
        }

        public DefectSet EmptyDefectSet()
        {
            return new DefectSet(this.defects.Keys.ToList(), null, null);
        }

        public static DefectSet FindDefects(ShemaMap map, ModelingSet set)
        {
            DefectSet defects = new DefectSet(map.Wires.ConvertAll(wire => wire.Name), set, map.IOController.ProcessModelingWires(set, map.Wires));
            foreach (Element startElement in map.IOController.Outputs)
            {
                defects = ReverseWayActivation(defects, startElement);
            }
            return defects;
        }

        private static DefectSet ReverseWayActivation(DefectSet set, Element element)
        {
            switch (element.Type)
            {
                case ElementsType.Not:
                case ElementsType.Output:
                    {
                        Wire input = element.Inputs[0];
                        bool value = set.modelingWires.GetValue(input.Name) == ElementsValues.True ? false : true;
                        set.defects[input.Name][value] = true;
                        return ReverseWayActivation(set, input.Setter);
                    }
                case ElementsType.And:
                case ElementsType.NotAnd:
                    {
                        int kolNull = 0;
                        Wire wireWithNull = null;
                        foreach (Wire input in element.Inputs)
                        {
                            if (set.modelingWires.GetValue(input.Name) == ElementsValues.False)
                            {
                                kolNull++;
                                wireWithNull = input;
                            }
                        }
                        if (kolNull == 1)
                        {
                            set.defects[wireWithNull.Name][true] = true;
                            return ReverseWayActivation(set, wireWithNull.Setter);
                        }
                        if (kolNull == 0)
                        {
                            foreach (Wire input in element.Inputs)
                            {
                                set.defects[input.Name][false] = true;
                                set = ReverseWayActivation(set, input.Setter);
                            }
                        }
                        return set;
                    }
                case ElementsType.Or:
                case ElementsType.NotOr:
                    {
                        int kolOnes = 0;
                        Wire wireWithOne = null;
                        foreach (Wire input in element.Inputs)
                        {
                            if (set.modelingWires.GetValue(input.Name) == ElementsValues.True)
                            {
                                kolOnes++;
                                wireWithOne = input;
                            }
                        }
                        if (kolOnes == 1)
                        {
                            set.defects[wireWithOne.Name][false] = true;
                            return ReverseWayActivation(set, wireWithOne.Setter);
                        }
                        if (kolOnes == 0)
                        {
                            foreach (Wire input in element.Inputs)
                            {
                                set.defects[input.Name][true] = true;
                                set = ReverseWayActivation(set, input.Setter);
                            }
                        }
                        return set;
                    }
                default: return set;
            }
        }

        public static DefectSet DefectSetUnion(DefectSet set1, DefectSet set2)
        {
            DefectSet result = set1.EmptyDefectSet();
            foreach (string wire in result.defects.Keys.ToList())
            {
                Dictionary<bool, bool> values = new Dictionary<bool, bool>();
                values.Add(true, set1.defects[wire][true] | set2.defects[wire][true]);
                values.Add(false, set1.defects[wire][false] | set2.defects[wire][false]);
                result.defects[wire] = values;
            }
            return result;
        }

        public bool IsDefectDetected(String wire, bool type)
        {
            return this.defects[wire][type];
        }
    }
}
