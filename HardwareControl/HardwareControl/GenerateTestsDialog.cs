using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HardwareControl
{
    public partial class GenerateTestsDialog : Form
    {
        private List<String> _wireNames;
        private int _selectedItem;
        private bool _selectedType;
        private readonly String[] _defectTypes = { "constant 0", "constant 1" };

        public List<String> WireNames
        {
            set
            {
                _wireNames = value;
            }
        }
        public int SelectedIndex
        {
            get
            {
                return _selectedItem;
            }
        }
        public bool SelectedType
        {
            get
            {
                return _selectedType;
            }
        }

        public GenerateTestsDialog()
        {
            InitializeComponent();
            _wireNames = null;
            _selectedItem = -1;
            _selectedType = false;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _selectedItem = wiresComboBox.SelectedIndex;
            _selectedType = ( this.typesComboBox.SelectedIndex != 0 );
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void GenerateTestsDialog_Load(object sender, EventArgs e)
        {
            wiresComboBox.Items.AddRange(_wireNames.ToArray());
            wiresComboBox.SelectedIndex = 0;
            typesComboBox.Items.AddRange(_defectTypes);
            typesComboBox.SelectedIndex = 0;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
