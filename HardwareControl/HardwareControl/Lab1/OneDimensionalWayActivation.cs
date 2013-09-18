using System;
using System.Collections.Generic;
using System.Linq;
using HardwareControl.Elements;

namespace HardwareControl.Lab1
{
    class OneDimensionalWayActivation
    {
        private static Dictionary<Element, Dictionary<bool, List<ModelingSet>>> _elementSets = new Dictionary<Element, Dictionary<bool, List<ModelingSet>>>();

        public static List<ModelingSet> FindTests(Wire wire, bool type, IOController controller)
        {
            return UnionModelingSets(GetSetsByOutputValue(wire.Setter, !type, controller), ActivateWay(wire, controller));
        }

        private static List<ModelingSet> GetSetsByOutputValue(Element element, bool value, IOController controller)
        {
            if (!_elementSets.ContainsKey(element))
            {
                switch (element.Type)
                {
                    case ElementsType.Input:
                        {
                            Dictionary<bool, List<ModelingSet>> _values = new Dictionary<bool, List<ModelingSet>>();
                            ModelingSet setTrue = controller.CreateInputSet();
                            setTrue.SetValue(element.GetName(), ElementsValues.True);
                            ModelingSet setFalse = controller.CreateInputSet();
                            setFalse.SetValue(element.GetName(), ElementsValues.False);
                            _values.Add(true, new List<ModelingSet>() { setTrue });
                            _values.Add(false, new List<ModelingSet>() { setFalse });
                            _elementSets.Add(element, _values);
                            break;
                        }
                    case ElementsType.Not:
                        {
                            Dictionary<bool, List<ModelingSet>> _values = new Dictionary<bool, List<ModelingSet>>();
                            _values.Add(true, GetSetsByOutputValue(element.Inputs[0].Setter, false, controller));
                            _values.Add(false, GetSetsByOutputValue(element.Inputs[0].Setter, true, controller));
                            _elementSets.Add(element, _values);
                            break;
                        }
                    case ElementsType.And:
                        {
                            Dictionary<bool, List<ModelingSet>> _values = new Dictionary<bool, List<ModelingSet>>();
                            List<ModelingSet> setsFalse = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.ExeptAllOnes))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsFalse = InterceptModelingSets(setsFalse, sets);
                            }
                            List<ModelingSet> setsTrue = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.AllOnes))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsTrue = InterceptModelingSets(setsTrue, sets);
                            }
                            _values.Add(false, setsFalse);
                            _values.Add(true, setsTrue);
                            _elementSets.Add(element, _values);
                            break;
                        }
                    case ElementsType.Or:
                        {
                            Dictionary<bool, List<ModelingSet>> _values = new Dictionary<bool, List<ModelingSet>>();
                            List<ModelingSet> setsFalse = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.AllNulls))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsFalse = InterceptModelingSets(setsFalse, sets);
                            }
                            List<ModelingSet> setsTrue = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.ExeptAllNulls))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsTrue = InterceptModelingSets(setsTrue, sets);
                            }
                            _values.Add(false, setsFalse);
                            _values.Add(true, setsTrue);
                            _elementSets.Add(element, _values);
                            break;
                        }
                    case ElementsType.NotAnd:
                        {
                            Dictionary<bool, List<ModelingSet>> _values = new Dictionary<bool, List<ModelingSet>>();
                            List<ModelingSet> setsTrue = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.ExeptAllOnes))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsTrue = InterceptModelingSets(setsTrue, sets);
                            }
                            List<ModelingSet> setsFalse = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.AllOnes))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsFalse = InterceptModelingSets(setsFalse, sets);
                            }
                            _values.Add(false, setsFalse);
                            _values.Add(true, setsTrue);
                            _elementSets.Add(element, _values);
                            break;
                        }
                    case ElementsType.NotOr:
                        {
                            Dictionary<bool, List<ModelingSet>> _values = new Dictionary<bool, List<ModelingSet>>();
                            List<ModelingSet> setsTrue = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.AllNulls))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsTrue = InterceptModelingSets(setsTrue, sets);
                            }
                            List<ModelingSet> setsFalse = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.ExeptAllNulls))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsFalse = InterceptModelingSets(setsFalse, sets);
                            }
                            _values.Add(false, setsFalse);
                            _values.Add(true, setsTrue);
                            _elementSets.Add(element, _values);
                            break;
                        }
                    case ElementsType.NotXor:
                        {
                            Dictionary<bool, List<ModelingSet>> _values = new Dictionary<bool, List<ModelingSet>>();
                            List<ModelingSet> setsFalse = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.AllOnes))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsFalse = InterceptModelingSets(setsFalse, sets);
                            }
                            List<ModelingSet> setsTrue = new List<ModelingSet>();
                            foreach (List<bool> set in GenerateSets(element.Inputs.Count, GenerationType.AllOnes))
                            {
                                List<ModelingSet> sets = new List<ModelingSet>();
                                for (int i = 0; i < element.Inputs.Count; i++)
                                {
                                    sets = UnionModelingSets(sets, GetSetsByOutputValue(element.Inputs[i].Setter, set[i], controller));
                                }
                                setsTrue = InterceptModelingSets(setsTrue, sets);
                            }
                            _values.Add(false, setsFalse);
                            _values.Add(true, setsTrue);
                            _elementSets.Add(element, _values);
                            break;
                        }
                    default: return null;
                }
            }
            return _elementSets[element][value];
        }

        private static List<ModelingSet> ActivateWay(Wire wire, IOController controller)
        {
            List<ModelingSet> sets = new List<ModelingSet>();
            while (wire.Getter.Type != ElementsType.Output)
            {
                Element element = wire.Getter;
                List<Wire> anotherInputs = new List<Wire>(element.Inputs);
                anotherInputs.Remove(wire);
                if ((element.Type == ElementsType.And) || (element.Type == ElementsType.NotAnd))
                {
                    List<ModelingSet> anotherInputsSets = new List<ModelingSet>();
                    foreach (List<bool> list in GenerateSets(anotherInputs.Count, GenerationType.AllOnes))
                    {
                        for (int i = 0; i < anotherInputs.Count; i++)
                        {
                            anotherInputsSets = UnionModelingSets(anotherInputsSets, GetSetsByOutputValue(anotherInputs[i].Setter, list[i], controller));
                        }
                    }
                    sets = UnionModelingSets(sets, anotherInputsSets);
                }
                if ((element.Type == ElementsType.Or) || (element.Type == ElementsType.NotOr))
                {
                    List<ModelingSet> anotherInputsSets = new List<ModelingSet>();
                    foreach (List<bool> list in GenerateSets(anotherInputs.Count, GenerationType.AllNulls))
                    {
                        for (int i = 0; i < anotherInputs.Count; i++)
                        {
                            anotherInputsSets = UnionModelingSets(anotherInputsSets, GetSetsByOutputValue(anotherInputs[i].Setter, list[i], controller));
                        }
                    }
                    sets = UnionModelingSets(sets, anotherInputsSets);
                }
                wire = element.Outputs[0];
            }
            return sets;
        }

        private static List<List<bool>> GenerateSets(int number, GenerationType type)
        {
            List<List<bool>> sets = new List<List<bool>>();
            int maxValue = Convert.ToInt32(Math.Pow(2, number));
            switch (type)
            {
                case GenerationType.AllSets:
                    {
                        for (int i = 0; i < maxValue; i++)
                        {
                            sets.Add(ToBinary(i, number));
                        }
                        break;
                    }
                case GenerationType.AllOnes:
                    {
                        sets.Add(ToBinary(maxValue - 1, number));
                        break;
                    }
                case GenerationType.AllNulls:
                    {
                        sets.Add(ToBinary(0, number));
                        break;
                    }
                case GenerationType.ExeptAllOnes:
                    {
                        for (int i = 0; i < maxValue - 1; i++)
                        {
                            sets.Add(ToBinary(i, number));
                        }
                        break;
                    }
                case GenerationType.ExeptAllNulls:
                    {
                        for (int i = 1; i < maxValue; i++)
                        {
                            sets.Add(ToBinary(i, number));
                        }
                        break;
                    }
                default: return null;
            }
            return sets;
        }

        private static List<bool> ToBinary(int number, int length)
        {
            List<bool> bin = new List<bool>();
            int n = number;
            for (int i = 0; i < length; i++)
            {
                bin.Add((n % 2) == 1);
                n /= 2;
            }
            return bin;
        }

        private static List<ModelingSet> InterceptModelingSets(List<ModelingSet> set1, List<ModelingSet> set2)
        {
            return set1.Union(set2, new ModelingSetsComparer()).ToList();
        }

        private static List<ModelingSet> UnionModelingSets(List<ModelingSet> set1, List<ModelingSet> set2)
        {
            if (set1.Count == 0)
            {
                return new List<ModelingSet>(set2);
            }
            else if (set2.Count == 0)
            {
                return new List<ModelingSet>(set1);
            }
            else
            {
                List<ModelingSet> union = new List<ModelingSet>();
                foreach (ModelingSet i in set1)
                {
                    foreach (ModelingSet j in set2)
                    {
                        ModelingSet s = i.UnionSet(j);
                        if (s != null)
                        {
                            union.Add(s);
                        }
                    }
                }
                return union;
            }
        }
    }
}
