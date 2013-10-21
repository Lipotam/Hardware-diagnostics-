using System;
using System.Collections.Generic;
using System.Linq;

using HardwareControl.Elements;

namespace HardwareControl
{
	public class ModelingSet
	{
		private Dictionary<String, ElementsValues> values;

		public ModelingSet(List<String> set)
		{
			this.values = new Dictionary<string, ElementsValues>();
			foreach (String name in set)
			{
				this.values.Add(name, ElementsValues.Unset);
			}
		}

		public ModelingSet(ModelingSet set)
		{
			this.values = new Dictionary<string, ElementsValues>(set.values);
		}

		public List<String> ElementNames
		{
			get
			{
				return this.values.Keys.ToList();
			}
		}

		public bool SetValue(String name, ElementsValues value)
		{
			if (this.values.ContainsKey(name))
			{
				this.values[name] = value;
				return true;
			}
			else
			{
				return false;
			}
		}

		public ElementsValues GetValue(String name)
		{
			if (this.values.ContainsKey(name))
			{
				return this.values[name];
			}
			else
			{
				return ElementsValues.Unset;
			}
		}

		public ModelingSet UnionSet(ModelingSet set)
		{
			ModelingSet result = new ModelingSet(this.values.Keys.ToList());
			foreach (String name in this.values.Keys.ToList())
			{
				if ((GetValue(name) == ElementsValues.Undefined) || (set.GetValue(name) == ElementsValues.Undefined))
				{
					return null;
				}
				if ((this.values[name] == ElementsValues.Unset) && (set.values[name] != ElementsValues.Unset))
				{
					result.SetValue(name, set.values[name]);
				}
				else if ((this.values[name] != ElementsValues.Unset) && (set.values[name] == ElementsValues.Unset))
				{
					result.SetValue(name, this.values[name]);
				}
				else if ((this.values[name] != ElementsValues.Unset) && (set.values[name] != ElementsValues.Unset))
				{
					if (this.values[name] != set.values[name])
					{
						return null;
					}
					else
					{
						result.SetValue(name, this.values[name]);
					}
				}
			}
			return result;
		}

		public override string ToString()
		{
			String str = "";
			foreach (String name in this.values.Keys.ToList())
			{
				String value = "";
				switch (this.values[name])
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
				str += value;
			}
			return str;
		}

		public List<String> ToList()
		{
			List<String> values = new List<string>();
			foreach (ElementsValues val in this.values.Values.ToList())
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
			return this.values.Keys.Count(value => this.values[value] != set.values[value]);
		}
	}
}
