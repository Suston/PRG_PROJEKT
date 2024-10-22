using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace konecdubnaseblizi
{
    public partial class konec : Form
    {
        public konec()
        {
            InitializeComponent();
        }

        private void konec_Load(object sender, EventArgs e)
        {
            rizz.Text = "Počet rizznutých bitches: " + Globals.Score.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
    }
}
