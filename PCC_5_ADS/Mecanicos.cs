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
    public partial class frm_mecanico : Form
    {
        public frm_mecanico()
        {
            InitializeComponent();
            campos();
        }

        private string connectionString = ConfigurationManager.AppSettings["SqlConnection"];

        private void carregadatagrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new mecanico().Todos();
        }

        private void btnnovocadastro_Click(object sender, EventArgs e)
        {
            txtid.Enabled = false;
            btnnovocadastro.Enabled = false;
            txtnome.Enabled = true;
            txtendereco.Enabled = true;
            txtcpf.Enabled = true;
            txtrg.Enabled = true;
            txtcidade.Enabled = true;
            txtcelular.Enabled = true;
            txtbairro.Enabled = true;
            btnbuscar.Enabled = false;
            btnbnome.Enabled = false;
            txtbid.Enabled = false;
            txtbnome.Enabled = false;
            btnsalvar.Enabled = true;
            limpar();
        }

        public void limpar()
        {
            txtid.Text = null;
            txtnome.Text = null;
            txtcpf.Text = null;
            txtrg.Text = null;
            txtcidade.Text = null;
            txtcelular.Text = null;
            txtendereco.Text = null;
            txtbairro.Text = null;
            txtbnome.Text = null;
            txtbid.Text = null;
        }

        public void campos()
        {
            txtid.Enabled = false;
            txtnome.Enabled = false;
            txtendereco.Enabled = false;
            txtcpf.Enabled = false;
            txtrg.Enabled = false;
            txtcidade.Enabled = false;
            txtcelular.Enabled = false;
            txtbairro.Enabled = false;
            txtbid.Enabled = true;
            btnbuscar.Enabled = true;
            btnnovocadastro.Enabled = true;
            txtbnome.Enabled = true;
            btnbnome.Enabled = true;
            btnsalvar.Enabled = false;
        }

        public void dados()
        {
            txtid.Enabled = false;
            txtnome.Enabled = true;
            txtcpf.Enabled = true;
            txtrg.Enabled = true;
            txtcelular.Enabled = true;
            txtbairro.Enabled = true;
            txtcidade.Enabled = true;
            txtendereco.Enabled = true;
            btnsalvar.Enabled = true;
        }

        private void btnsalvar_Click(object sender, EventArgs e)
        {
            if (btnnovocadastro.Enabled == false)
            {
                if (txtnome.Text == string.Empty)
                {
                    MessageBox.Show("Campo NOME obrigatório");
                    label3.ForeColor = Color.Red;
                }
                else if (txtcpf.MaskCompleted == false)
                {
                    MessageBox.Show("Campo CPF obrigatório");
                }
                else if (txtrg.MaskCompleted == false)
                {
                    MessageBox.Show("Campo RG obrigatório");
                }
                else if (txtcelular.MaskCompleted == false)
                {
                    MessageBox.Show("Campo CELULAR obrigatório");
                }
                else if (txtbairro.Text == string.Empty)
                {
                    MessageBox.Show("Campo BAIRRO obrigatório");

                }
                else if (txtcidade.Text == string.Empty)
                {
                    MessageBox.Show("Campo CIDADE obrigatório");
                }
                else if (txtendereco.Text == string.Empty)
                {
                    MessageBox.Show("Campo ENDEREÇO obrigatório");
                }
                else
                {
                    mecanico mec = new mecanico();
                    mec.nome = txtnome.Text;
                    mec.cpf = txtcpf.Text;
                    mec.rg = txtrg.Text;
                    mec.cidade = txtcidade.Text;
                    mec.endereco = txtendereco.Text;
                    mec.bairro = txtbairro.Text;
                    mec.telcelular = txtcelular.Text;
                    mec.Salvar();
                    MessageBox.Show("Cadastrado com sucesso");
                    comissao comi = new comissao();
                    comi.mecanico = txtnome.Text;
                    comi.valor = "0";
                    comi.Salvar();
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
                    SqlCommand sql = new SqlCommand("UPDATE MECANICOS set NOME=@NOME,CPF=@CPF,RG=@RG,TELCELULAR=@TELCELULAR,BAIRRO=@BAIRRO,CIDADE=@CIDADE, ENDERECO=@ENDERECO where ID=@ID", conexao);
                    sql.Parameters.Add("@ID", SqlDbType.Int).Value = txtid.Text;
                    sql.Parameters.Add("@NOME", SqlDbType.VarChar).Value = txtnome.Text;
                    sql.Parameters.Add("@CPF", SqlDbType.VarChar).Value = txtcpf.Text;
                    sql.Parameters.Add("@RG", SqlDbType.VarChar).Value = txtrg.Text;
                    sql.Parameters.Add("@TELCELULAR", SqlDbType.VarChar).Value = txtcelular.Text;
                    sql.Parameters.Add("@BAIRRO", SqlDbType.VarChar).Value = txtbairro.Text;
                    sql.Parameters.Add("@CIDADE", SqlDbType.VarChar).Value = txtcidade.Text;
                    sql.Parameters.Add("@ENDERECO", SqlDbType.VarChar).Value = txtendereco.Text;

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

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from MECANICOS where ID=@ID", conexao);
                sql.Parameters.Add("@ID", SqlDbType.Int).Value = txtbid.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        txtid.Text = dt["ID"].ToString();
                        txtnome.Text = dt["NOME"].ToString();
                        txtrg.Text = dt["RG"].ToString();
                        txtcpf.Text = dt["CPF"].ToString();
                        txtcelular.Text = dt["TELCELULAR"].ToString();
                        txtbairro.Text = dt["BAIRRO"].ToString();
                        txtcidade.Text = dt["CIDADE"].ToString();
                        txtendereco.Text = dt["ENDERECO"].ToString();
                        dataGridView1.ClearSelection();
                        carregadatagrid();
                    }
                    else
                    {
                        MessageBox.Show("Mecânico não cadastrado");
                        limpar();
                        campos();
                    }

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dados();
                    txtbnome.Enabled = true;
                    btnsalvar.Enabled = true;
                    txtbnome.Enabled = true;
                    txtbid.Text = null;
                    txtbnome.Text = null;
                    conexao.Close();
                }
            }
        }

        private void btnbnome_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from MECANICOS where NOME=@NOME", conexao);
                sql.Parameters.Add("@NOME", SqlDbType.VarChar).Value = txtbnome.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        txtid.Text = dt["ID"].ToString();
                        txtnome.Text = dt["NOME"].ToString();
                        txtrg.Text = dt["RG"].ToString();
                        txtcpf.Text = dt["CPF"].ToString();
                        txtcelular.Text = dt["TELCELULAR"].ToString();
                        txtbairro.Text = dt["BAIRRO"].ToString();
                        txtcidade.Text = dt["CIDADE"].ToString();
                        txtendereco.Text = dt["ENDERECO"].ToString();
                        dataGridView1.ClearSelection();
                        carregadatagrid();
                    }
                    else
                    {
                        MessageBox.Show("Mecânico não encontrado");
                        limpar();
                        campos();
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dados();
                    txtbnome.Enabled = true;
                    btnsalvar.Enabled = true;
                    txtbnome.Enabled = true;
                    txtbid.Text = null;
                    txtbnome.Text = null;
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
                txtnome.Text = row.Cells["nome"].Value.ToString();
                txtcpf.Text = row.Cells["cpf"].Value.ToString();
                txtrg.Text = row.Cells["rg"].Value.ToString();
                txtcelular.Text = row.Cells["telcelular"].Value.ToString();
                txtbairro.Text = row.Cells["bairro"].Value.ToString();
                txtcidade.Text = row.Cells["cidade"].Value.ToString();
                txtendereco.Text = row.Cells["endereco"].Value.ToString();
                dados();
            }
        }
    }
}

