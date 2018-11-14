using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Miner
{
    public partial class Tutorial : Form
    {
        public Tutorial()
        {
            InitializeComponent();
        }

        private void Tutorial_Load(object sender, EventArgs e)
        {

        }

        private void Tutorial_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Miner.Properties.Resources.Tutorial,0,0,Width,Height);
        }
    }
}
