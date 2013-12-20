using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using HardwareControl.Elements;
using HardwareControl.Lab1;
using HardwareControl.Lab2;
using HardwareControl.Lab3;
using HardwareControl.Lab4;
using HardwareControl.Lab6;
using HardwareControl.Lab7;

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
        private List<int> lsfrMatchList;

        //lab6
        private List<bool> signal;
        private UCA ucaBase;

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
            this.ucaBase = new UCA(new List<int> { 8, 5, 3, 1 }, 50);
            this.signal = this.ucaBase.GetSignal();
            this.labelPlynom.Text = "Polynom: " + this.ucaBase.GetPolynom();
            this.labelLength.Text = "Length: " + this.ucaBase.SignalLength;
            this.textBoxSignal.Text = UCA.SignalToString(this.signal);
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

        private List<int> GetSetsToRemove(List<ModelingSet> allModelingSets)
        {
            List<int> setsToRemove = new List<int>();
            for (int i = 0; i < allModelingSets.Count; i++)
            {
                string key = allModelingSets[i].ElementNames[allModelingSets[i].ElementNames.Count - 1];
                ElementsValues value = allModelingSets[i].GetValue(key);
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
            this.lsfrMatchList = new List<int>();
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

                this.lsfrMatchList.Add(matchedMinimalDefect != null ? this.minimalDefectList.IndexOf(matchedMinimalDefect) + 1 : 0);
                List<String> items = new List<string> { i.ToString(), fullSetString, match };
                this.listViewPolynom1.Items.Add(new ListViewItem(items.ToArray()));
            }

            int size = 0, index = 0;
            int[] valuesCount = new int[minimalDefectSets.Count + 1];
            foreach (var item in this.lsfrMatchList)
            {
                size++;
                valuesCount[item]++;
                if (valuesCount[item] > 1 && valuesCount[this.lsfrMatchList.ToArray()[index]] > 1)
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
                List<String> items = new List<string>
                    { key, (polynoms[key][0] * 100.0 / (Math.Pow(2, 7) - 1)).ToString(), (polynoms[key][1] - 1).ToString() };
                this.listViewLFSR.Items.Add(new ListViewItem(items.ToArray()));
            }
        }

        private void buttonStartLab6_Click(object sender, EventArgs e)
        {
            List<String> resUca1 = this.ucaBase.GetAllSingleErrors(this.signal, false);
            foreach (string str in resUca1)
            {
                this.textBoxUCA1.Text += str + Environment.NewLine;
            }
            efficiencyBoxUCA1.Text = (100 - this.ucaBase.Efficiency).ToString();

            List<String> resMca1 = this.ucaBase.GetAllSingleErrors(this.signal, true);
            foreach (string str in resMca1)
            {
                this.textBoxMCA1.Text += str + Environment.NewLine;
            }
            efficiencyBoxMCA1.Text = (100 - this.ucaBase.Efficiency).ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<String> resUca4 = this.ucaBase.GetAllPockerErrors(this.signal, false);
            foreach (string str in resUca4)
            {
                this.textBoxUCA4.Text += str + Environment.NewLine;
            }
            efficiencyBoxUCA4.Text = (100 - this.ucaBase.Efficiency).ToString();

            List<String> resMca4 = this.ucaBase.GetAllPockerErrors(this.signal, true);
            foreach (string str in resMca4)
            {
                this.textBoxMCA4.Text += str + Environment.NewLine;
            }
            efficiencyBoxMCA4.Text = (100 - this.ucaBase.Efficiency).ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<String> resUca3 = this.ucaBase.GetAllTripleErrors(this.signal, false);
            foreach (string str in resUca3)
            {
                this.textBoxUCA3.Text += str + Environment.NewLine;
            }
            efficiencyBoxUCA3.Text = (100 - this.ucaBase.Efficiency).ToString();

            List<String> resMca3 = this.ucaBase.GetAllTripleErrors(this.signal, true);
            foreach (string str in resMca3)
            {
                this.textBoxMCA3.Text += str + Environment.NewLine;
            }
            efficiencyBoxMCA3.Text = (100 - this.ucaBase.Efficiency).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<String> resUca2 = this.ucaBase.GetAllDoubleErrors(this.signal, false);
            foreach (string str in resUca2)
            {
                this.textBoxUCA2.Text += str + Environment.NewLine;
            }
            efficiencyBoxUCA2.Text = (100 - this.ucaBase.Efficiency).ToString();

            List<String> resMca2 = this.ucaBase.GetAllDoubleErrors(this.signal, true);
            foreach (string str in resMca2)
            {
                this.textBoxMCA2.Text += str + Environment.NewLine;
            }
            efficiencyBoxMCA2.Text = (100 - this.ucaBase.Efficiency).ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var cellsMatrix = new CellsMatrix((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            lab7ListView.Clear();
            lab7ListView.Columns.Add("CellNumber", "#", 40);
            lab7ListView.Columns.Add("CellState", "Cell state", 250);
            lab7ListView.Columns.Add("Walking", "Walking 0/1", 100);
            lab7ListView.Columns.Add("BAlgoritm", "B algoritm", 100);

            List<string> messages = cellsMatrix.GetResultMessages();

            cellsMatrix.Walking0To1Algoritm();
            var first = cellsMatrix.GetTestPassResultsMessages();

            cellsMatrix.AlgoritmB();
            var second = cellsMatrix.GetTestPassResultsMessages();

            for (int i = 0; i < first.Count; i++)
            {
                ListViewItem newItem = new ListViewItem(i.ToString());
                newItem.SubItems.AddRange(
               new[]
               {
                    messages[i],
                    first[i],
                    second[i]
                }
            );
                lab7ListView.Items.Add(newItem);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var cellsMatrix = new CellsMatrix((int)numericUpDown4.Value, (int)numericUpDown3.Value);

            cellsMatrix.AlgoritmB();
            var first = cellsMatrix.GetTestPassResultsMessages();

            cellsMatrix.MarshPSAlgoritm();
            var second = cellsMatrix.GetTestPassResultsMessages();

            var states = cellsMatrix.GetResultMessages();
            DataTable dt = new DataTable();
            dt.Columns.Add("Index");
            dt.Columns.Add("States");
            dt.Columns.Add("B Algoritm");
            dt.Columns.Add("Marsh PS Algoritm");

            for (int i = 0; i < first.Count; i++)
            {
                dt.Rows.Add(i, states[i], first[i], second[i]);
            }

            this.dataGridView1.DataSource = dt;
        }
    }
}
