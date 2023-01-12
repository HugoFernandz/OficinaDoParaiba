using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business;
using System.Configuration;

namespace PCC_5_ADS
{
    public partial class comissaos : Form
    {
        public comissaos()
        {
            InitializeComponent();
            carregadatagrid();
        }

        private string connectionString = ConfigurationManager.AppSettings["SqlConnection"];

        private void carregadatagrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new comissao().Todos();
        }

        private void btnvoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                var retorno = MessageBox.Show("Deseja mesmo resetar Comissão?", "REDEFINIR", MessageBoxButtons.YesNo);
                if (retorno.Equals(DialogResult.Yes))
                {
                    SqlCommand sql = new SqlCommand("update COMISSAOS set valor=0", conexao);
                    try
                    {

                        conexao.Open();
                        sql.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        MessageBox.Show("Alterado com sucesso");
                        conexao.Close();
                    }
                    dataGridView1.ClearSelection();
                    carregadatagrid();
                }
            }
        }
    }
}
