using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HardwareControl.Elements;
using HardwareControl.Lab1;
using HardwareControl.Lab2;
using HardwareControl.Lab3;
using HardwareControl.Lab4;

namespace HardwareControl
{
    public partial class Form1 : Form
    {
        private ShemaMap map;
        private List<ModelingSet> modelingSets;
        private bool selectedType;

        //lab4
        private readonly List<int> polynom7 = new List<int> { 7, 5, 3, 1 };
        private LFSRInfo info7;
        private List<string> minimalDefectList;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.map = new MyShema();

            if (this.map.Shema != null)
            {
                shemaPictureBox.Image = this.map.Shema;
            }
            this.modelingSets = new List<ModelingSet>();
            int columnWidth = (testsListView.Width - 22) / (this.map.IOController.InputNames.Count + this.map.IOController.OutputNames.Count);
            foreach (String name in this.map.IOController.InputNames)
            {
                testsListView.Columns.Add(name, columnWidth);
                listView1.Columns.Add(name, columnWidth);
            }
            foreach (String name in this.map.IOController.OutputNames)
            {
                testsListView.Columns.Add(name, columnWidth);
                listView1.Columns.Add(name, columnWidth);
            }

            //lab3 tab
            minDefectsListView.Columns.Add("Modeling Set", 75);
            int column = (minDefectsListView.Width - 140) / (map.Wires.Count * 2);
            foreach (Wire wire in map.Wires)
            {
                minDefectsListView.Columns.Add(wire.Name + "=0", column);
            }
            foreach (Wire wire in map.Wires)
            {
                minDefectsListView.Columns.Add(wire.Name + "=1", column);
            }
            minDefectsListView.Columns.Add("Switches", 60);

            // lab4 tab
            column = (listViewPolynom1.Width - 55) / 3;
            listViewPolynom1.Columns.Add("N", column);
            listViewPolynom1.Columns.Add("x", column);
            listViewPolynom1.Columns.Add("Match", column);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateTestsDialog dialog = new GenerateTestsDialog
                {
                    WireNames = this.map.GetWiresNames()
                };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<ModelingSet> tests = OneDimensionalWayActivation.FindTests(this.map.Wires[dialog.SelectedIndex], dialog.SelectedType, this.map.IOController);

                testsListView.Items.Clear();
                this.modelingSets.Clear();
                foreach (ModelingSet test in tests)
                {
                    testsListView.Items.Add(new ListViewItem(test.ToList().ToArray()));
                }
                this.modelingSets.AddRange(tests);
            }
            SimulateAllSets(testsListView);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateTestsDialog dialog = new GenerateTestsDialog
                {
                    WireNames = this.map.GetWiresNames()
                };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                selectedType = dialog.SelectedType;
                List<ModelingSet> tests = MultiDimensionalWayActivation.FindTests(this.map.Wires[dialog.SelectedIndex], selectedType, this.map.GetWiresNames(), this.map);

                listView1.Items.Clear();
                this.modelingSets.Clear();
                List<ModelingSet> results = map.IOController.ProcessModeling(tests);
                List<int> setsToRemove = GetSetsToRemove(results);

                for (int i = 0; i < tests.Count; i++)
                {
                    if (setsToRemove.All(s => s != i))
                    {
                        listView1.Items.Add(new ListViewItem(tests[i].ToList().ToArray()));
                        modelingSets.Add(tests[i]);
                    }
                }
            }
            SimulateAllSets(listView1);

        }

        private List<int> GetSetsToRemove(List<ModelingSet> modelingSets)
        {
            List<int> setsToRemove = new List<int>();
            for (int i = 0; i < modelingSets.Count; i++)
            {
                string key = modelingSets[i].ElementNames[modelingSets[i].ElementNames.Count - 1];
                ElementsValues value = modelingSets[i].GetValue(key);
                if ((value == ElementsValues.False && selectedType) || (value == ElementsValues.True && !selectedType))
                {
                    setsToRemove.Add(i);
                }
            }

            return setsToRemove;
        }

        private void SimulateAllSets(ListView listView)
        {
            List<ModelingSet> results = map.IOController.ProcessModeling(modelingSets);

            for (int i = 0; i < results.Count; i++)
            {
                List<String> subItems = new List<string>(modelingSets[i].ToList());
                subItems.AddRange(results[i].ToList());
                listView.Items[i] = new ListViewItem(subItems.ToArray());
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            defectsListView.Items.Clear();
            minDefectsListView.Items.Clear();
            List<DefectSet> allDefectSets = PowerConsumptionMinimization.FindAllDefectSets(map);
            foreach (DefectSet defectSet in allDefectSets)
            {
                List<String> items = new List<String>
                                         {
                                             defectSet.ModelingSet.ToString(),
                                             defectSet.ConstFalseDefects,
                                             defectSet.ConstTrueDefects,
                                             defectSet.DefectsCount.ToString()
                                         };
                defectsListView.Items.Add(new ListViewItem(items.ToArray()));
            }

            int switchesNum = 0;
            List<DefectSet> minimalDefectSets = PowerConsumptionMinimization.MinimalDefectSets(allDefectSets);
            for (int i = 0; i < minimalDefectSets.Count; i++)
            {
                switchesNum += minimalDefectSets[i].ModelingWires.SwitchingNumbers(minimalDefectSets[(i + 1) % minimalDefectSets.Count].ModelingWires);
            }
            switchesLabel.Text = "Switches before minimisation = " + switchesNum.ToString();
            minimalDefectSets = PowerConsumptionMinimization.MinimalPowerSequence(minimalDefectSets);
            switchesNum = 0;
            for (int i = 0; i < minimalDefectSets.Count; i++)
            {
                List<String> items = new List<string> { minimalDefectSets[i].ModelingSet.ToString() };
                foreach (Wire wire in map.Wires)
                {
                    items.Add(minimalDefectSets[i].IsDefectDetected(wire.Name, false) ? "+" : " ");
                }
                foreach (Wire wire in map.Wires)
                {
                    items.Add(minimalDefectSets[i].IsDefectDetected(wire.Name, true) ? "+" : " ");
                }
                int switches = minimalDefectSets[i].ModelingWires.SwitchingNumbers(minimalDefectSets[(i + 1) % minimalDefectSets.Count].ModelingWires);
                switchesNum += switches;
                items.Add(switches.ToString());
                minDefectsListView.Items.Add(new ListViewItem(items.ToArray()));
            }
            switchesLabel.Text += "; Switches after minimisation = " + switchesNum.ToString();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // lab3 part
            List<DefectSet> allDefectSets = PowerConsumptionMinimization.FindAllDefectSets(map);
            List<DefectSet> minimalDefectSets = PowerConsumptionMinimization.MinimalDefectSets(allDefectSets);
            minimalDefectList = new List<string>();
            foreach (var minimalDefectSet in minimalDefectSets)
            {
                minimalDefectList.Add(minimalDefectSet.ModelingSet.ToString());
            }
            // lab3 part

            this.info7 = LFSR.GenerateAllSets(this.polynom7, 7);
            for (int i = 0; i < this.info7.Sets.Count; i++)
            {
                string fullSetString = string.Empty;
                for (int j = 0; j < this.info7.Sets[i].Count; j++)
                {
                    fullSetString += this.info7.Sets[i][j] ? "1" : "0";
                }

                var a = minimalDefectList.FirstOrDefault(x => x == fullSetString);
                string match = a != null ? this.minimalDefectList.IndexOf(a).ToString() : string.Empty;

                List<String> items = new List<string>() { i.ToString(), fullSetString, match };
                listViewPolynom1.Items.Add(new ListViewItem(items.ToArray()));
            }
        }
    }
}
