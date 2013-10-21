using System;
using System.Collections.Generic;
using System.Linq;
using HardwareControl.Elements;

namespace HardwareControl
{
	public class IOController
	{
		private Dictionary<String, Element> _inputs;
		private Dictionary<String, Element> _outputs;
		private List<Element> _otherElements;

		public List<String> InputNames
		{
			get
			{
				return _inputs.Keys.ToList();
			}
		}
		public List<String> OutputNames
		{
			get
			{
				return _outputs.Keys.ToList();
			}
		}
		public List<Element> Elements
		{
			get
			{
				return _otherElements;
			}
		}
		public List<Element> Outputs
		{
			get
			{
				return _outputs.Values.ToList();
			}
		}

		public IOController()
		{
			_inputs = new Dictionary<string, Element>();
			_outputs = new Dictionary<string, Element>();
			_otherElements = new List<Element>();
		}

		public void AddIOElement(Element element)
		{
			switch (element.Type)
			{
				case ElementsType.Input:
					{
						_inputs.Add(element.GetName(), element);
						break;
					}
				case ElementsType.Output:
					{
						_outputs.Add(element.GetName(), element);
						break;
					}
				default: return;
			}
		}

		public void AddElement(Element element)
		{
			_otherElements.Add(element);
		}

		public void RemoveIOElement(Element element)
		{
			switch (element.Type)
			{
				case ElementsType.Input:
					{
						_inputs.Remove(element.GetName());
						break;
					}
				case ElementsType.Output:
					{
						_outputs.Remove(element.GetName());
						break;
					}
				default: return;
			}
		}

		public void RemoveElement(Element element)
		{
			_otherElements.Remove(element);
		}

		public void RenameIOElement(String oldName, Element element)
		{
			switch (element.Type)
			{
				case ElementsType.Input:
					{
						_inputs.Remove(oldName);
						_inputs.Add(element.GetName(), element);
						break;
					}
				case ElementsType.Output:
					{
						_outputs.Remove(oldName);
						_outputs.Add(element.GetName(), element);
						break;
					}
				default: return;
			}
		}

		public ModelingSet CreateInputSet()
		{
			return new ModelingSet(_inputs.Keys.ToList());
		}

		public ModelingSet ProcessModeling(ModelingSet inputs)
		{
			foreach (String name in _inputs.Keys.ToList())
			{
				((InputElement)(_inputs[name])).Value = inputs.GetValue(name);
			}
			List<String> outputNames = _outputs.Keys.ToList();
			ModelingSet outputValues = new ModelingSet(outputNames);
			foreach (String name in outputNames)
			{
				outputValues.SetValue(name, _outputs[name].GetSelfValue());
			}
			return outputValues;
		}

		public List<ModelingSet> ProcessModeling(List<ModelingSet> inputs)
		{
			List<ModelingSet> outputs = new List<ModelingSet>();
			foreach (ModelingSet inputSet in inputs)
			{
				outputs.Add(ProcessModeling(inputSet));
			}
			return outputs;
		}

		public ModelingSet ProcessModelingWires(ModelingSet inputs, List<Wire> wires)
		{
			foreach (String name in _inputs.Keys.ToList())
			{
				((InputElement)(_inputs[name])).Value = inputs.GetValue(name);
			}
			ModelingSet modelingWires = new ModelingSet(wires.ConvertAll(wire => wire.Name));
			foreach (Wire w in wires)
			{
				modelingWires.SetValue(w.Name, w.GetValue());
			}
			return modelingWires;
		}
	}
}
