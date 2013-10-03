using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardwareControl.Lab4
{
    public class LFSR
    {
        public static LFSRInfo GenerateAllSets(List<int> polynom, int power)
        {
            List<bool> a0 = ToBinary(0, power);
            List<List<bool>> allSets = new List<List<bool>>();
            while (!allSets.Contains(a0, new SetComparer()))
            {
                allSets.Add(new List<bool>(a0));
                NextPolynomSet(a0, polynom);
            }
            allSets.Add(a0);
            return new LFSRInfo(allSets, GetInfo(allSets));
        }

        private static String GetInfo(List<List<bool>> sets)
        {
            int period = sets.Count - 1;
            int characteristic = -1;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < period; i++)
            {
                if (i != period - 1)
                {
                    List<bool> sum = AddMod2(sets[period - 1], sets[i]);
                    int pos = FindSet(sets, sum);
                    if (pos != -1)
                    {
                        sb.Append(String.Format("[a{0}] + [a{1}] = [a{2}]" + System.Environment.NewLine, period - 1, i, pos));
                    }
                }
                if (CheckIfCharacteristic(sets, i, period))
                {
                    characteristic = i;
                }
            }
            return String.Format("Period = {0}" + System.Environment.NewLine + "Characteristic: [a{1}]" + System.Environment.NewLine
                + "Shift Property:" + System.Environment.NewLine + sb.ToString(), period, characteristic);
        }

        private static bool CheckIfCharacteristic(List<List<bool>> sets, int pos, int period)
        {
            for (int i = 0; i < sets[pos].Count; i++)
            {
                int j = 2 * i;
                int j1 = j / sets[0].Count;
                int j2 = j % sets[0].Count;
                if (sets[pos][i] != sets[(pos + j1) % period][j2])
                {
                    return false;
                }
            }
            return true;
        }

        private static int FindSet(List<List<bool>> sets, List<bool> set)
        {
            SetComparer comparer = new SetComparer();
            for (int i = 0; i < sets.Count; i++)
            {
                if (comparer.Equals(set, sets[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        private static List<bool> AddMod2(List<bool> set1, List<bool> set2)
        {
            List<bool> result = new List<bool>();
            for (int i = 0; i < set1.Count; i++)
            {
                result.Add(set1[i] ^ set2[i]);
            }
            return result;
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

        private static List<bool> NextPolynomSet(List<bool> set, List<int> polynom)
        {
            bool newVal = true;
            foreach (int i in polynom)
            {
                newVal ^= set[i - 1];
            }
            set.RemoveAt(set.Count - 1);
            set.Insert(0, newVal);
            return set;
        }
    }
}
