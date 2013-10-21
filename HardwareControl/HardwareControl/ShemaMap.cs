using System;
using System.Collections.Generic;
using System.Drawing;
using HardwareControl.Elements;

namespace HardwareControl
{
	public class ShemaMap
	{
		protected Image shemaImage;
		protected IOController _IOController;
		protected List<Wire> _wires;

		public List<Wire> Wires
		{
			get
			{
				return _wires;
			}
		}
		public IOController IOController
		{
			get
			{
				return _IOController;
			}
		}
		public Image Shema
		{
			get
			{
				return this.shemaImage;
			}
		}

		public ShemaMap()
		{
			this.shemaImage = null;
			_IOController = new IOController();
			_wires = new List<Wire>();
		}

		public ShemaMap(Image img)
		{
			this.shemaImage = img;
			_IOController = new IOController();
			_wires = new List<Wire>();
		}

		protected virtual void CreateShema() { }

		public Dictionary<String, Wire> GenerateWiresDictionary()
		{
			Dictionary<String, Wire> wires = new Dictionary<string, Wire>();
			foreach (Wire wire in _wires)
			{
				wires.Add(wire.Name, wire);
			}
			return wires;
		}

		public List<String> GetWiresNames()
		{
			List<String> names = new List<string>();
			foreach (Wire wire in _wires)
			{
				names.Add(wire.Name);
			}
			return names;
		}
	}
}
