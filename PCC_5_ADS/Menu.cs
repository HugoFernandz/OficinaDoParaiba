using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCC_5_ADS
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            //log.Close();
        }

        private void produtosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void btn_produtos_Click(object sender, EventArgs e)
        {
            frm_produtos frm = new frm_produtos();
            frm.Show();
        }

        private void cadastrosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btn_clientes_Click(object sender, EventArgs e)
        {
            frm_clientes frm = new frm_clientes();
            frm.Show();
        }

		private void button1_Click(object sender, EventArgs e)
		{
            frm_mecanico frm = new frm_mecanico();
            frm.Show();
		}

		private void btn_veiculos_Click(object sender, EventArgs e)
		{
            frm_veiculos frm = new frm_veiculos();
            frm.Show();
		}

		private void btn_sair_Click(object sender, EventArgs e)
		{
            Application.Exit();
		}

		private void btn_caixa_Click(object sender, EventArgs e)
		{
            frm_caixa frm = new frm_caixa();
            frm.Show();
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{

		}

        private void button1_Click_1(object sender, EventArgs e)
        {
            comissaos frm = new comissaos();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Login();
            f.Closed += (s, args) => this.Close();
            f.Show();
        }
    }
}
