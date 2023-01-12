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
    public partial class frm_produtos : Form
    {
        public frm_produtos()
        {
            InitializeComponent();
            campos();
        }

        private string connectionString = ConfigurationManager.AppSettings["SqlConnection"];

        private void carregadatagrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new produto().Todos();
        }

        private void limpar()
        {
            txtid.Text = null;
            txtdescricao.Text = null;
            mskpreco.Text = null;
            txtquantidade.Text = null;
            txtbuscades.Text = null;
            txtbuscaid.Text = null;
            
        }
        private void campos()
        {
            txtid.Enabled = false;
            txtdescricao.Enabled = false;
            mskpreco.Enabled = false;
            txtquantidade.Enabled = false;
            btnbuscaproduto.Enabled = true;
            txtbuscaid.Enabled = true;
            txtbuscades.Enabled = true;
            btnnovoproduto.Enabled = true;
            button2.Enabled = true;
            btnsalvarprod.Enabled = false;
            
        }
        public void dados()
        {
            txtid.Enabled = false;
            txtdescricao.Enabled = true;
            mskpreco.Enabled = true;
            txtquantidade.Enabled = true;
            btnsalvarprod.Enabled = true;
        }

        private void btnnovoproduto_Click(object sender, EventArgs e)
        {
            txtid.Enabled = false;
            txtdescricao.Enabled = true;
            mskpreco.Enabled = true;
            txtquantidade.Enabled = true;
            btnbuscaproduto.Enabled = false;
            btnnovoproduto.Enabled = false;
            button2.Enabled = false;
            btnsalvarprod.Enabled = true;
            txtbuscades.Enabled = false;
            txtbuscaid.Enabled = false;
            limpar();

        }

        private void btnsalvarprod_Click(object sender, EventArgs e)
        {
            if (btnnovoproduto.Enabled == false)
            {
                if (txtdescricao.Text == string.Empty)
                {
                    MessageBox.Show("Campo DESCRIÇÃO obrigatório");
                }
                if (mskpreco.Text == string.Empty)
                {
                    MessageBox.Show("Campo PREÇO obrigatório");
                }
                if (txtquantidade.Text == string.Empty)
                {
                    MessageBox.Show("Campo QUANTIDADE obrigatório");
                }
                else
                {
                    produto cli = new produto();
                    cli.descricao = txtdescricao.Text;
                    cli.preco = mskpreco.Text;
                    cli.qtdd = txtquantidade.Text;
                    cli.Salvar();
                    MessageBox.Show("Cadastrado com sucesso");
                    limpar();
                    campos();
                    dataGridView1.ClearSelection();
                    carregadatagrid();
                }
            }
            else
            {
                using (SqlConnection conexao = new SqlConnection(connectionString))
                {
                    SqlCommand sql = new SqlCommand("UPDATE PRODUTOS set DESCRICAO=@DESCRICAO,PRECO=@PRECO,QTDD=@QTDD where ID=@ID", conexao);
                    sql.Parameters.Add("@ID", SqlDbType.Int).Value = txtid.Text;
                    sql.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = txtdescricao.Text;
                    sql.Parameters.Add("@PRECO", SqlDbType.VarChar).Value = mskpreco.Text;
                    sql.Parameters.Add("@QTDD", SqlDbType.VarChar).Value = txtquantidade.Text;

                    try
                    {
                        conexao.Open();
                        sql.ExecuteNonQuery();
                        MessageBox.Show("Alterado com sucesso !");
                        limpar();
                        dataGridView1.ClearSelection();
                        carregadatagrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conexao.Close();
                    }
                }
            }
        }
        private void btnvoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from PRODUTOS where DESCRICAO=@DESCRICAO", conexao);
                sql.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = txtbuscades.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        txtid.Text = dt["ID"].ToString();
                        txtdescricao.Text = dt["DESCRICAO"].ToString();
                        mskpreco.Text = dt["PRECO"].ToString();
                        txtquantidade.Text = dt["QTDD"].ToString();
                        dataGridView1.ClearSelection();
                        carregadatagrid();
                    }
                    else
                    {
                        MessageBox.Show("Produto não encontrado");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dados();
                    txtbuscaid.Enabled = true;
                    btnsalvarprod.Enabled = true;
                    btnbuscaproduto.Enabled = true;
                    txtbuscades.Text = null;
                    txtbuscaid.Text = null;
                    conexao.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                txtid.Text = row.Cells["id"].Value.ToString();
                txtdescricao.Text = row.Cells["descricao"].Value.ToString();
                mskpreco.Text = row.Cells["preco"].Value.ToString();
                txtquantidade.Text = row.Cells["qtdd"].Value.ToString();
                dados();

            }
        }

        private void btnbuscaproduto_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from PRODUTOS where ID=@ID", conexao);
                sql.Parameters.Add("@ID", SqlDbType.Int).Value = txtbuscaid.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        txtid.Text = dt["ID"].ToString();
                        txtdescricao.Text = dt["DESCRICAO"].ToString();
                        mskpreco.Text = dt["PRECO"].ToString();
                        txtquantidade.Text = dt["QTDD"].ToString();
                        dataGridView1.ClearSelection();
                        carregadatagrid();
                    }
                    else
                    {
                        MessageBox.Show("Produto não encontrado");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dados();
                    txtbuscaid.Enabled = true;
                    btnsalvarprod.Enabled = true;
                    btnbuscaproduto.Enabled = true;
                    txtbuscades.Text = null;
                    txtbuscaid.Text = null;
                    conexao.Close();
                }
            }
        }
    }
}
