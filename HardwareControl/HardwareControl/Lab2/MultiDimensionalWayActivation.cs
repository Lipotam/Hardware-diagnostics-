using System;
using System.Collections.Generic;
using System.Linq;
using HardwareControl.Elements;
using HardwareControl.Lab1;

namespace HardwareControl.Lab2
{
    class MultiDimensionalWayActivation
    {
        private static List<String> _wires;
        private static List<Element> _allElements;
        private static List<Element> _onWayOut;

        public static List<ModelingSet> FindTests(Wire wire, bool type, List<String> wires, ShemaMap map)
        {
            _wires = new List<string>(wires);
            _allElements = new List<Element>(map.IOController.Elements);
            _onWayOut = new List<Element>();
            _onWayOut.Add(wire.Setter);
            List<DCube> cubes = new List<DCube>();
            switch (wire.Setter.Type)
            {
                case ElementsType.Input:
                    {
                        DCube cube = new DCube(_wires);
                        if (type)
                        {
                            cube.SetValue(wire.Name, D_cubeValues.NotD);
                        }
                        else
                        {
                            cube.SetValue(wire.Name, D_cubeValues.D);
                        }
                        cubes.Add(cube);
                        break;
                    }
                case ElementsType.Not:
                    {
                        if (type)
                        {
                            DCube cube = new DCube(_wires);
                            cube.SetValue(wire.Setter.Inputs[0].Name, D_cubeValues.One);
                            cube.SetValue(wire.Name, D_cubeValues.NotD);
                            cubes.Add(cube);
                        }
                        else
                        {
                            DCube cube = new DCube(_wires);
                            cube.SetValue(wire.Setter.Inputs[0].Name, D_cubeValues.Null);
                            cube.SetValue(wire.Name, D_cubeValues.D);
                            cubes.Add(cube);
                        }
                        break;
                    }
                case ElementsType.And:
                    {
                        if (type)
                        {
                            foreach (Wire w in wire.Setter.Inputs)
                            {
                                DCube c = new DCube(_wires);
                                c.SetValue(w.Name, D_cubeValues.Null);
                                c.SetValue(wire.Name, D_cubeValues.NotD);
                                cubes.Add(c);
                            }
                        }
                        else
                        {
                            DCube c = new DCube(_wires);
                            foreach (Wire w in wire.Setter.Inputs)
                            {
                                c.SetValue(w.Name, D_cubeValues.One);
                            }
                            c.SetValue(wire.Name, D_cubeValues.D);
                            cubes.Add(c);
                        }
                        break;
                    }
                case ElementsType.NotAnd:
                    {
                        if (type)
                        {
                            DCube c = new DCube(_wires);
                            foreach (Wire w in wire.Setter.Inputs)
                            {
                                c.SetValue(w.Name, D_cubeValues.One);
                            }
                            c.SetValue(wire.Name, D_cubeValues.NotD);
                            cubes.Add(c);
                        }
                        else
                        {
                            foreach (Wire w in wire.Setter.Inputs)
                            {
                                DCube c = new DCube(_wires);
                                c.SetValue(w.Name, D_cubeValues.Null);
                                c.SetValue(wire.Name, D_cubeValues.D);
                                cubes.Add(c);
                            }
                        }
                        break;
                    }
                case ElementsType.Or:
                    {
                        if (type)
                        {
                            DCube c = new DCube(_wires);
                            foreach (Wire w in wire.Setter.Inputs)
                            {
                                c.SetValue(w.Name, D_cubeValues.Null);
                            }
                            c.SetValue(wire.Name, D_cubeValues.NotD);
                            cubes.Add(c);
                        }
                        else
                        {
                            foreach (Wire w in wire.Setter.Inputs)
                            {
                                DCube c = new DCube(_wires);
                                c.SetValue(w.Name, D_cubeValues.One);
                                c.SetValue(wire.Name, D_cubeValues.D);
                                cubes.Add(c);
                            }
                        }
                        break;
                    }
                case ElementsType.NotOr:
                    {
                        if (type)
                        {
                            foreach (Wire w in wire.Setter.Inputs)
                            {
                                DCube c = new DCube(_wires);
                                c.SetValue(w.Name, D_cubeValues.One);
                                c.SetValue(wire.Name, D_cubeValues.NotD);
                                cubes.Add(c);
                            }
                        }
                        else
                        {
                            DCube c = new DCube(_wires);
                            foreach (Wire w in wire.Setter.Inputs)
                            {
                                c.SetValue(w.Name, D_cubeValues.Null);
                            }
                            c.SetValue(wire.Name, D_cubeValues.D);
                            cubes.Add(c);
                        }
                        break;
                    }
                default: return null;
            }
            cubes = IntersectCubeSets(cubes, ActivateWay(wire));
            List<Element> anotherElements = _allElements.Except(_onWayOut).ToList();
            foreach (Element e in anotherElements)
            {
                cubes = IntersectCubeSets(cubes, GenerateSingularCubes(e));
            }
            return InterceptModelingSets(ConvertD_cubesToModelingSets(cubes, map));
        }

        private static List<DCube> ActivateWay(Wire wire)
        {
            List<DCube> cubes = new List<DCube>();
            while (wire.Getter.Type != ElementsType.Output)
            {
                _onWayOut.Add(wire.Getter);
                cubes = IntersectCubeSets(cubes, GenerateD_cubes(wire));
                wire = wire.Getter.Outputs[0];
            }
            return cubes;
        }

        private static List<DCube> IntersectCubeSets(List<DCube> set1, List<DCube> set2)
        {
            List<DCube> intersect = new List<DCube>();
            if (set1.Count == 0)
            {
                intersect.AddRange(set2);
            }
            else
            {
                if (set2.Count == 0)
                {
                    intersect.AddRange(set1);
                }
                else
                {
                    foreach (DCube cube1 in set1)
                    {
                        foreach (DCube cube2 in set2)
                        {
                            DCube newCube = cube1.IntersectCubes(cube2);
                            if (newCube != null)
                            {
                                intersect.Add(newCube);
                            }
                        }
                    }
                }
            }
            return intersect;
        }

        private static List<DCube> GenerateD_cubes(Wire wire)
        {
            List<DCube> cubes = new List<DCube>();
            switch (wire.Getter.Type)
            {
                case ElementsType.Not:
                    {
                        DCube c1 = new DCube(_wires);
                        c1.SetValue(wire.Name, D_cubeValues.D);
                        c1.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.NotD);
                        DCube c2 = new DCube(_wires);
                        c2.SetValue(wire.Name, D_cubeValues.NotD);
                        c2.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.D);
                        cubes.Add(c1);
                        cubes.Add(c2);
                        break;
                    }
                case ElementsType.And:
                    {
                        List<Wire> anotherWires = new List<Wire>(wire.Getter.Inputs);
                        anotherWires.Remove(wire);
                        DCube c1 = new DCube(_wires);
                        c1.SetValue(wire.Name, D_cubeValues.D);
                        c1.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.D);
                        foreach (Wire w in anotherWires)
                        {
                            c1.SetValue(w.Name, D_cubeValues.One);
                        }
                        DCube c2 = new DCube(_wires);
                        c2.SetValue(wire.Name, D_cubeValues.NotD);
                        c2.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.NotD);
                        foreach (Wire w in anotherWires)
                        {
                            c2.SetValue(w.Name, D_cubeValues.One);
                        }
                        cubes.Add(c1);
                        cubes.Add(c2);
                        break;
                    }
                case ElementsType.NotAnd:
                    {
                        List<Wire> anotherWires = new List<Wire>(wire.Getter.Inputs);
                        anotherWires.Remove(wire);
                        DCube c1 = new DCube(_wires);
                        c1.SetValue(wire.Name, D_cubeValues.D);
                        c1.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.NotD);
                        foreach (Wire w in anotherWires)
                        {
                            c1.SetValue(w.Name, D_cubeValues.One);
                        }
                        DCube c2 = new DCube(_wires);
                        c2.SetValue(wire.Name, D_cubeValues.NotD);
                        c2.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.D);
                        foreach (Wire w in anotherWires)
                        {
                            c2.SetValue(w.Name, D_cubeValues.One);
                        }
                        cubes.Add(c1);
                        cubes.Add(c2);
                        break;
                    }
                case ElementsType.Or:
                    {
                        List<Wire> anotherWires = new List<Wire>(wire.Getter.Inputs);
                        anotherWires.Remove(wire);
                        DCube c1 = new DCube(_wires);
                        c1.SetValue(wire.Name, D_cubeValues.D);
                        c1.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.D);
                        foreach (Wire w in anotherWires)
                        {
                            c1.SetValue(w.Name, D_cubeValues.Null);
                        }
                        DCube c2 = new DCube(_wires);
                        c2.SetValue(wire.Name, D_cubeValues.NotD);
                        c2.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.NotD);
                        foreach (Wire w in anotherWires)
                        {
                            c2.SetValue(w.Name, D_cubeValues.Null);
                        }
                        cubes.Add(c1);
                        cubes.Add(c2);
                        break;
                    }
                case ElementsType.NotOr:
                    {
                        List<Wire> anotherWires = new List<Wire>(wire.Getter.Inputs);
                        anotherWires.Remove(wire);
                        DCube c1 = new DCube(_wires);
                        c1.SetValue(wire.Name, D_cubeValues.D);
                        c1.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.NotD);
                        foreach (Wire w in anotherWires)
                        {
                            c1.SetValue(w.Name, D_cubeValues.Null);
                        }
                        DCube c2 = new DCube(_wires);
                        c2.SetValue(wire.Name, D_cubeValues.NotD);
                        c2.SetValue(wire.Getter.Outputs[0].Name, D_cubeValues.D);
                        foreach (Wire w in anotherWires)
                        {
                            c2.SetValue(w.Name, D_cubeValues.Null);
                        }
                        cubes.Add(c1);
                        cubes.Add(c2);
                        break;
                    }
            }
            return cubes;
        }

        private static List<DCube> GenerateSingularCubes(Element element)
        {
            List<DCube> cubes = new List<DCube>();
            switch (element.Type)
            {
                case ElementsType.Not:
                    {
                        DCube c1 = new DCube(_wires);
                        c1.SetValue(element.Inputs[0].Name, D_cubeValues.One);
                        c1.SetValue(element.Outputs[0].Name, D_cubeValues.Null);
                        DCube c2 = new DCube(_wires);
                        c2.SetValue(element.Inputs[0].Name, D_cubeValues.Null);
                        c2.SetValue(element.Outputs[0].Name, D_cubeValues.One);
                        cubes.Add(c1);
                        cubes.Add(c2);
                        break;
                    }
                case ElementsType.And:
                    {
                        DCube allOnes = new DCube(_wires);
                        foreach (Wire wire in element.Inputs)
                        {
                            allOnes.SetValue(wire.Name, D_cubeValues.One);
                            DCube c = new DCube(_wires);
                            c.SetValue(wire.Name, D_cubeValues.Null);
                            c.SetValue(element.Outputs[0].Name, D_cubeValues.Null);
                            cubes.Add(c);
                        }
                        allOnes.SetValue(element.Outputs[0].Name, D_cubeValues.One);
                        cubes.Add(allOnes);
                        break;
                    }
                case ElementsType.NotAnd:
                    {
                        DCube allOnes = new DCube(_wires);
                        foreach (Wire wire in element.Inputs)
                        {
                            allOnes.SetValue(wire.Name, D_cubeValues.One);
                            DCube c = new DCube(_wires);
                            c.SetValue(wire.Name, D_cubeValues.Null);
                            c.SetValue(element.Outputs[0].Name, D_cubeValues.One);
                            cubes.Add(c);
                        }
                        allOnes.SetValue(element.Outputs[0].Name, D_cubeValues.Null);
                        cubes.Add(allOnes);
                        break;
                    }
                case ElementsType.Or:
                    {
                        DCube allNulls = new DCube(_wires);
                        foreach (Wire wire in element.Inputs)
                        {
                            allNulls.SetValue(wire.Name, D_cubeValues.Null);
                            DCube c = new DCube(_wires);
                            c.SetValue(wire.Name, D_cubeValues.One);
                            c.SetValue(element.Outputs[0].Name, D_cubeValues.One);
                            cubes.Add(c);
                        }
                        allNulls.SetValue(element.Outputs[0].Name, D_cubeValues.Null);
                        cubes.Add(allNulls);
                        break;
                    }
                case ElementsType.NotOr:
                    {
                        DCube allNulls = new DCube(_wires);
                        foreach (Wire wire in element.Inputs)
                        {
                            allNulls.SetValue(wire.Name, D_cubeValues.Null);
                            DCube c = new DCube(_wires);
                            c.SetValue(wire.Name, D_cubeValues.One);
                            c.SetValue(element.Outputs[0].Name, D_cubeValues.Null);
                            cubes.Add(c);
                        }
                        allNulls.SetValue(element.Outputs[0].Name, D_cubeValues.One);
                        cubes.Add(allNulls);
                        break;
                    }
            }
            return cubes;
        }

        private static List<ModelingSet> ConvertD_cubesToModelingSets(List<DCube> cubes, ShemaMap map)
        {
            List<ModelingSet> sets = new List<ModelingSet>();
            foreach (DCube cube in cubes)
            {
                ModelingSet set = map.IOController.CreateInputSet();
                List<String> unset = new List<string>();
                Dictionary<String, Wire> wires = map.GenerateWiresDictionary();
                foreach (String wire in cube.Wires)
                {
                    if (wires[wire].Setter.Type == ElementsType.Input)
                    {
                        if ((cube.GetValue(wire) == D_cubeValues.One) || (cube.GetValue(wire) == D_cubeValues.D))
                        {
                            set.SetValue(wires[wire].Setter.GetName(), ElementsValues.True);
                        }
                        else if ((cube.GetValue(wire) == D_cubeValues.Null) || (cube.GetValue(wire) == D_cubeValues.NotD))
                        {
                            set.SetValue(wires[wire].Setter.GetName(), ElementsValues.False);
                        }
                        else
                        {
                            unset.Add(wires[wire].Setter.GetName());
                        }
                    }
                }
                if (unset.Count > 0)
                {
                    foreach (List<bool> list in GenerateSets(unset.Count, GenerationType.AllSets))
                    {
                        ModelingSet newSet = new ModelingSet(set);
                        for (int i = 0; i < list.Count; i++)
                        {
                            newSet.SetValue(unset[i], list[i] ? ElementsValues.True : ElementsValues.False);
                        }
                        sets.Add(newSet);
                    }
                }
                else
                {
                    sets.Add(set);
                }
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

        private static List<ModelingSet> InterceptModelingSets(List<ModelingSet> list)
        {
            List<ModelingSet> sets = new List<ModelingSet>();
            foreach (ModelingSet set in list)
            {
                List<ModelingSet> newSets = new List<ModelingSet>();
                newSets.Add(set);
                sets = sets.Union(newSets, new ModelingSetsComparer()).ToList();
            }
            return sets;
        }
    }
}
