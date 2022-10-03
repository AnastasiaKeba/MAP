using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1(Form2 F)
        {
            InitializeComponent();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(textBox1.Text);
            newForm.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try { if (Convert.ToInt32(textBox1.Text) < 2 || textBox1.Text == "") textBox1.Clear(); }
            catch (System.FormatException) { textBox1.Clear(); }
        }
    }
}
