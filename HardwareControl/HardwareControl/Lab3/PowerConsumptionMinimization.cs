using System;
using System.Collections.Generic;
using HardwareControl.Elements;
using HardwareControl.Lab1;

namespace HardwareControl.Lab3
{
    class PowerConsumptionMinimization
    {
        public static List<DefectSet> FindAllDefectSets(ShemaMap map)
        {
            List<DefectSet> defects = new List<DefectSet>();
            foreach (List<bool> values in GenerateSets(map.IOController.InputNames.Count, GenerationType.AllSets))
            {
                ModelingSet set = map.IOController.CreateInputSet();
                for (int i = 0; i < map.IOController.InputNames.Count; i++)
                {
                    set.SetValue(map.IOController.InputNames[i], values[i] ? ElementsValues.True : ElementsValues.False);
                }
                defects.Add(DefectSet.FindDefects(map, set));
            }
            return defects;
        }

        public static List<DefectSet> MinimalDefectSets(List<DefectSet> defectSets)
        {
            List<DefectSet> minimalDefectSets = new List<DefectSet>();
            DefectSet overallDefects = defectSets[0].EmptyDefectSet();
            while (overallDefects.DefectsCount < overallDefects.MaxDefectCount)
            {
                int maxDefects = 0;
                DefectSet bestUnion = null;
                DefectSet bestUnionSet = null;
                foreach (DefectSet defectSet in defectSets)
                {
                    DefectSet union = DefectSet.DefectSetUnion(overallDefects, defectSet);
                    if (union.DefectsCount > maxDefects)
                    {
                        maxDefects = union.DefectsCount;
                        bestUnion = union;
                        bestUnionSet = defectSet;
                    }
                }
                minimalDefectSets.Add(bestUnionSet);
                overallDefects = bestUnion;
            }
            return minimalDefectSets;
        }

        public static List<DefectSet> MinimalPowerSequence(List<DefectSet> defectSets)
        {
            List<DefectSet> result = new List<DefectSet>();
            result.Add(defectSets[0]);
            defectSets.Remove(result[0]);
            while (defectSets.Count > 0)
            {
                DefectSet lastSet = result[result.Count - 1];
                DefectSet newSet = null;
                int minSwitches = lastSet.ModelingWires.ElementNames.Count;
                foreach (DefectSet defectSet in defectSets)
                {
                    int switches = lastSet.ModelingWires.SwitchingNumbers(defectSet.ModelingWires);
                    if (switches < minSwitches)
                    {
                        minSwitches = switches;
                        newSet = defectSet;
                    }
                }
                defectSets.Remove(newSet);
                result.Add(newSet);
            }
            return result;
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
    }
}
