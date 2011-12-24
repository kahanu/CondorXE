using System;
using System.Windows.Forms;

namespace CodeWrapper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            string textBox = txtCodeBox.Text;

            Wrapper wrapper = new Wrapper();
            string buffer = wrapper.Process(textBox);

            txtResults.Text = buffer;
            txtResults.Select();
        }

        private void btnClearBoxes_Click(object sender, EventArgs e)
        {
            txtCodeBox.Clear();
            txtResults.Clear();
        }
  

    }
}
