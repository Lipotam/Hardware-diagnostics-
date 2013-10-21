using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HardwareControl.Elements;

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
						sb.Append(String.Format("[a{0}] + [a{1}] = [a{2}]" + Environment.NewLine, period - 1, i, pos));
					}
				}
				if (CheckIfCharacteristic(sets, i, period))
				{
					characteristic = i;
				}
			}
			return String.Format("Period = {0}" + Environment.NewLine + "Characteristic: [a{1}]" + Environment.NewLine
				+ "Shift Property:" + Environment.NewLine + sb, period, characteristic);
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

		public static Dictionary<String, List<int>> ChechAllCAPolynoms(int power, ShemaMap map, List<int> polynomLFSR)
		{
			Dictionary<String, List<int>> result = new Dictionary<String, List<int>>();
			List<List<int>> polynomCA = new List<List<int>>();

			for (int i = 1; i < 512; i++)
			{
				string s = Convert.ToString(i, 2); //Convert to binary in a string

				int[] bits = s.PadLeft(9, '0') // Add 0's from left
							 .Select(c => int.Parse(c.ToString())) // convert each char to int
							 .ToArray();

				List<int> polynom = new List<int>();
				if (bits[0] == 1) polynom.Add(9);
				if (bits[1] == 1) polynom.Add(8);
				if (bits[2] == 1) polynom.Add(7);
				if (bits[3] == 1) polynom.Add(6);
				if (bits[4] == 1) polynom.Add(5);
				if (bits[5] == 1) polynom.Add(4);
				if (bits[6] == 1) polynom.Add(3);
				if (bits[7] == 1) polynom.Add(2);
				if (bits[8] == 1) polynom.Add(1);

				polynomCA.Add(polynom);
			}

			foreach (List<int> set in polynomCA)
			{
				result.Add(PolynomToString(set), new List<int> { CheckPolynomCA(power, set, map, polynomLFSR), set.Count + 1 });
			}
			return result;
		}

		private static int CheckPolynomCA(int power, List<int> polynom, ShemaMap map, List<int> polynomLFSR)
		{
			List<ModelingSet> allPolynomsLFSR = new List<ModelingSet>();
			List<bool> LFSRval = ToBinary(0, 7);
			ModelingSet LFSRms = BinaryToModelingSet(LFSRval, map.IOController);
			List<List<bool>> allPolynomsCA = new List<List<bool>>();
			List<bool> CAval = ToBinary(0, power);
			do
			{
				allPolynomsLFSR.Add(LFSRms);
				if (!CheckPolynomRepeat(CAval, allPolynomsCA))
				{
					allPolynomsCA.Add(new List<bool>(CAval));
				}
				bool outVal = (map.IOController.ProcessModeling(LFSRms).GetValue(map.IOController.OutputNames[0]) == ElementsValues.True);
				NextPolynomSet(LFSRval, polynomLFSR, true);
				NextPolynomSet(CAval, polynom, outVal);
				LFSRms = BinaryToModelingSet(LFSRval, map.IOController);
			}
			while (!allPolynomsLFSR.Contains(LFSRms, new ModelingSetsComparer()));
			return allPolynomsCA.Count;
		}

		private static List<bool> NextPolynomSet(List<bool> set, List<int> polynom, bool inVal)
		{
			bool newVal = inVal;
			foreach (int i in polynom)
			{
				newVal ^= set[i - 1];
			}
			set.RemoveAt(set.Count - 1);
			set.Insert(0, newVal);
			return set;
		}

		private static bool CheckPolynomRepeat(List<bool> value, List<List<bool>> sets)
		{
			foreach (List<bool> set in sets)
			{
				bool isEqual = true;
				for (int i = 0; i < set.Count; i++)
				{
					if (set[i] != value[i])
					{
						isEqual = false;
						break;
					}
				}
				if (isEqual)
				{
					return true;
				}
			}
			return false;
		}

		private static ModelingSet BinaryToModelingSet(List<bool> setValues, IOController controller)
		{
			ModelingSet set = controller.CreateInputSet();
			for (int i = 0; i < setValues.Count; i++)
			{
				set.SetValue(controller.InputNames[i], setValues[i] ? ElementsValues.True : ElementsValues.False);
			}
			return set;
		}

		private static String PolynomToString(List<int> polynom)
		{
			StringBuilder str = new StringBuilder();
			foreach (int power in polynom)
			{
				str.Append("2^" + power.ToString() + " + ");
			}
			str.Append("1");
			return str.ToString();
		}
	}
}
