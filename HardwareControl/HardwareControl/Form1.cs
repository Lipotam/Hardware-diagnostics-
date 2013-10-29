using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HardwareControl.Elements;
using HardwareControl.Lab1;
using HardwareControl.Lab2;
using HardwareControl.Lab3;
using HardwareControl.Lab4;
using HardwareControl.Lab6;

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
        private List<int> LSFR_Match_List;

        //lab6
        private List<bool> _signal;
        private UCA UcaBase;

        public Form1()
        {
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.map = new MyShema();

            if (this.map.Shema != null)
            {
                this.shemaPictureBox.Image = this.map.Shema;
            }
            this.modelingSets = new List<ModelingSet>();
            int columnWidth = (this.testsListView.Width - 22) / (this.map.IOController.InputNames.Count + this.map.IOController.OutputNames.Count);
            foreach (String name in this.map.IOController.InputNames)
            {
                this.testsListView.Columns.Add(name, columnWidth);
                this.listView1.Columns.Add(name, columnWidth);
            }
            foreach (String name in this.map.IOController.OutputNames)
            {
                this.testsListView.Columns.Add(name, columnWidth);
                this.listView1.Columns.Add(name, columnWidth);
            }

            //lab3 tab
            this.minDefectsListView.Columns.Add("Modeling Set", 75);
            int column = (this.minDefectsListView.Width - 140) / (this.map.Wires.Count * 2);
            foreach (Wire wire in this.map.Wires)
            {
                this.minDefectsListView.Columns.Add(wire.Name + "=0", column);
            }
            foreach (Wire wire in this.map.Wires)
            {
                this.minDefectsListView.Columns.Add(wire.Name + "=1", column);
            }
            this.minDefectsListView.Columns.Add("Switches", 60);

            // lab4 tab
            column = (this.listViewPolynom1.Width - 55) / 3;
            this.listViewPolynom1.Columns.Add("N", column);
            this.listViewPolynom1.Columns.Add("x", column);
            this.listViewPolynom1.Columns.Add("Match", column);

            // lab6
            this.UcaBase = new UCA(new List<int> { 8, 5, 3, 1 }, 50);
            this._signal = UcaBase.GetSignal();
            this.labelPlynom.Text = "Polynom: " + UcaBase.GetPolynom();
            this.labelLength.Text = "Length: " + UcaBase.SignalLength;
            this.textBoxSignal.Text = UCA.SignalToString(_signal);
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

                this.testsListView.Items.Clear();
                this.modelingSets.Clear();
                foreach (ModelingSet test in tests)
                {
                    this.testsListView.Items.Add(new ListViewItem(test.ToList().ToArray()));
                }
                this.modelingSets.AddRange(tests);
            }
            this.SimulateAllSets(this.testsListView);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateTestsDialog dialog = new GenerateTestsDialog
                {
                    WireNames = this.map.GetWiresNames()
                };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.selectedType = dialog.SelectedType;
                List<ModelingSet> tests = MultiDimensionalWayActivation.FindTests(this.map.Wires[dialog.SelectedIndex], this.selectedType, this.map.GetWiresNames(), this.map);

                this.listView1.Items.Clear();
                this.modelingSets.Clear();
                List<ModelingSet> results = this.map.IOController.ProcessModeling(tests);
                List<int> setsToRemove = this.GetSetsToRemove(results);

                for (int i = 0; i < tests.Count; i++)
                {
                    if (setsToRemove.All(s => s != i))
                    {
                        this.listView1.Items.Add(new ListViewItem(tests[i].ToList().ToArray()));
                        this.modelingSets.Add(tests[i]);
                    }
                }
            }
            this.SimulateAllSets(this.listView1);

        }

        private List<int> GetSetsToRemove(List<ModelingSet> modelingSets)
        {
            List<int> setsToRemove = new List<int>();
            for (int i = 0; i < modelingSets.Count; i++)
            {
                string key = modelingSets[i].ElementNames[modelingSets[i].ElementNames.Count - 1];
                ElementsValues value = modelingSets[i].GetValue(key);
                if ((value == ElementsValues.False && this.selectedType) || (value == ElementsValues.True && !this.selectedType))
                {
                    setsToRemove.Add(i);
                }
            }

            return setsToRemove;
        }

        private void SimulateAllSets(ListView listView)
        {
            List<ModelingSet> results = this.map.IOController.ProcessModeling(this.modelingSets);

            for (int i = 0; i < results.Count; i++)
            {
                List<String> subItems = new List<string>(this.modelingSets[i].ToList());
                subItems.AddRange(results[i].ToList());
                listView.Items[i] = new ListViewItem(subItems.ToArray());
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.defectsListView.Items.Clear();
            this.minDefectsListView.Items.Clear();
            List<DefectSet> allDefectSets = PowerConsumptionMinimization.FindAllDefectSets(this.map);
            foreach (DefectSet defectSet in allDefectSets)
            {
                List<String> items = new List<String>
                                         {
                                             defectSet.ModelingSet.ToString(),
                                             defectSet.ConstFalseDefects,
                                             defectSet.ConstTrueDefects,
                                             defectSet.DefectsCount.ToString()
                                         };
                this.defectsListView.Items.Add(new ListViewItem(items.ToArray()));
            }

            int switchesNum = 0;
            List<DefectSet> minimalDefectSets = PowerConsumptionMinimization.MinimalDefectSets(allDefectSets);
            for (int i = 0; i < minimalDefectSets.Count; i++)
            {
                switchesNum += minimalDefectSets[i].ModelingWires.SwitchingNumbers(minimalDefectSets[(i + 1) % minimalDefectSets.Count].ModelingWires);
            }
            this.switchesLabel.Text = "Switches before minimisation = " + switchesNum.ToString();
            minimalDefectSets = PowerConsumptionMinimization.MinimalPowerSequence(minimalDefectSets);
            switchesNum = 0;
            for (int i = 0; i < minimalDefectSets.Count; i++)
            {
                List<String> items = new List<string> { minimalDefectSets[i].ModelingSet.ToString() };
                foreach (Wire wire in this.map.Wires)
                {
                    items.Add(minimalDefectSets[i].IsDefectDetected(wire.Name, false) ? "+" : " ");
                }
                foreach (Wire wire in this.map.Wires)
                {
                    items.Add(minimalDefectSets[i].IsDefectDetected(wire.Name, true) ? "+" : " ");
                }
                int switches = minimalDefectSets[i].ModelingWires.SwitchingNumbers(minimalDefectSets[(i + 1) % minimalDefectSets.Count].ModelingWires);
                switchesNum += switches;
                items.Add(switches.ToString());
                this.minDefectsListView.Items.Add(new ListViewItem(items.ToArray()));
            }
            this.switchesLabel.Text += "; Switches after minimisation = " + switchesNum.ToString();
        }

        private void Lab4_buttonStart_Click(object sender, EventArgs e)
        {
            // lab3 part
            List<DefectSet> allDefectSets = PowerConsumptionMinimization.FindAllDefectSets(this.map);
            List<DefectSet> minimalDefectSets = PowerConsumptionMinimization.MinimalDefectSets(allDefectSets);
            this.minimalDefectList = new List<string>();
            foreach (var minimalDefectSet in minimalDefectSets)
            {
                this.minimalDefectList.Add(minimalDefectSet.ModelingSet.ToString());
            }
            // lab3 part
            this.LSFR_Match_List = new List<int>();
            this.info7 = LFSR.GenerateAllSets(this.polynom7, 7);
            for (int i = 0; i < this.info7.Sets.Count; i++)
            {
                string fullSetString = string.Empty;
                for (int j = 0; j < this.info7.Sets[i].Count; j++)
                {
                    fullSetString += this.info7.Sets[i][j] ? "1" : "0";
                }

                var matchedMinimalDefect = this.minimalDefectList.FirstOrDefault(x => x == fullSetString);

                string match = matchedMinimalDefect != null ? this.minimalDefectList.IndexOf(matchedMinimalDefect).ToString() : string.Empty;

                this.LSFR_Match_List.Add(matchedMinimalDefect != null ? this.minimalDefectList.IndexOf(matchedMinimalDefect) + 1 : 0);
                List<String> items = new List<string> { i.ToString(), fullSetString, match };
                this.listViewPolynom1.Items.Add(new ListViewItem(items.ToArray()));
            }

            int size = 0, index = 0;
            int[] valuesCount = new int[minimalDefectSets.Count + 1];
            foreach (var item in this.LSFR_Match_List)
            {
                size++;
                valuesCount[item]++;
                if (valuesCount[item] > 1 && valuesCount[this.LSFR_Match_List.ToArray()[index]] > 1)
                {
                    valuesCount[item]--;
                    index++;
                    size--;
                }
                bool isFound = valuesCount.All(count => count >= 1);

                if (isFound)
                {
                    this.lab4Result.Text = "Size = " + size + ". Index is " + index;
                    return;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dictionary<String, List<int>> polynoms = LFSR.ChechAllCAPolynoms(9, this.map, this.polynom7);
            List<String> keys = polynoms.Keys.ToList();
            foreach (String key in keys)
            {
                List<String> items = new List<string>() { key, (polynoms[key][0] * 100.0 / (Math.Pow(2, 7) - 1)).ToString(), (polynoms[key][1] - 1).ToString() };
                this.listViewLFSR.Items.Add(new ListViewItem(items.ToArray()));
            }
        }

        private void buttonStartLab6_Click(object sender, EventArgs e)
        {
            List<String> resUCA1 = this.UcaBase.GetAllSingleErrors(this._signal, false);
            foreach (string str in resUCA1)
            {
                this.textBoxUCA1.Text += str + Environment.NewLine;
            }
            efficiencyBoxUCA1.Text = UcaBase.Efficiency.ToString();

            List<String> resMCA1 = this.UcaBase.GetAllSingleErrors(this._signal, true);
            foreach (string str in resMCA1)
            {
                this.textBoxMCA1.Text += str + Environment.NewLine;
            }
            efficiencyBoxMCA1.Text = UcaBase.Efficiency.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<String> resUCA4 = this.UcaBase.GetAllPockerErrors(this._signal, false);
            foreach (string str in resUCA4)
            {
                this.textBoxUCA4.Text += str + Environment.NewLine;
            }
            efficiencyBoxUCA4.Text = UcaBase.Efficiency.ToString();

            List<String> resMCA4 = this.UcaBase.GetAllPockerErrors(this._signal, true);
            foreach (string str in resMCA4)
            {
                this.textBoxMCA4.Text += str + Environment.NewLine;
            }
            efficiencyBoxMCA4.Text = UcaBase.Efficiency.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<String> resUCA3 = this.UcaBase.GetAllTripleErrors(this._signal, false);
            foreach (string str in resUCA3)
            {
                this.textBoxUCA3.Text += str + Environment.NewLine;
            }
            efficiencyBoxUCA3.Text = UcaBase.Efficiency.ToString();

            List<String> resMCA3 = this.UcaBase.GetAllTripleErrors(this._signal, true);
            foreach (string str in resMCA3)
            {
                this.textBoxMCA3.Text += str + Environment.NewLine;
            }
            efficiencyBoxMCA3.Text = UcaBase.Efficiency.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<String> resUCA2 = this.UcaBase.GetAllDoubleErrors(this._signal, false);
            foreach (string str in resUCA2)
            {
                this.textBoxUCA2.Text += str + Environment.NewLine;
            }
            efficiencyBoxUCA2.Text = UcaBase.Efficiency.ToString();

            List<String> resMCA2 = this.UcaBase.GetAllDoubleErrors(this._signal, true);
            foreach (string str in resMCA2)
            {
                this.textBoxMCA2.Text += str + Environment.NewLine;
            }
            efficiencyBoxMCA2.Text = UcaBase.Efficiency.ToString();
        }
    }
}
