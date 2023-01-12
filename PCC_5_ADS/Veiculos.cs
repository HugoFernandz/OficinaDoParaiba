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
	public partial class frm_veiculos : Form
	{
		public frm_veiculos()
		{
			InitializeComponent();
            campos();
        }

        private string connectionString = ConfigurationManager.AppSettings["SqlConnection"];

        private void carregadatagrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new veiculo().Todos();
        }
        public void limpar()
        {
            txtid.Text = null;
            txtplaca.Text = null;
            txtmarca.Text = null;
            txtquilometragem.Text = null;
            txtano.Text = null;
            txtmodelo.Text = null;
            txtcor.Text = null;
            cbbcambio.Text = null;
            txtmotor.Text = null;
            txtbplaca.Text = null;
            cbbdono.Text = null;
        }

        public void campos()
        {
            txtid.Enabled = false;
            txtplaca.Enabled = false;
            txtmarca.Enabled = false;
            txtquilometragem.Enabled = false;
            txtano.Enabled = false;
            txtmodelo.Enabled = false;
            txtcor.Enabled = false;
            cbbcambio.Enabled = false;
            cbbdono.Enabled = false;
            txtmotor.Enabled = false;
            btnsalvar.Enabled = false;
            btnbnome.Enabled = true;
            txtbplaca.Enabled = true;
            btnnovoveiculo.Enabled = true;
        }

        public void dados()
        {
            txtid.Enabled = false;
            txtplaca.Enabled = true;
            txtmarca.Enabled = true;
            txtquilometragem.Enabled = true;
            txtano.Enabled = true;
            txtmodelo.Enabled = true;
            txtcor.Enabled = true;
            cbbcambio.Enabled = true;
            txtmotor.Enabled = true;
            cbbdono.Enabled = true;
            btnsalvar.Enabled = true;
        }

        private void btnnovocadastro_Click(object sender, EventArgs e)
		{
            btnnovoveiculo.Enabled = false;
            txtid.Enabled = false;
            txtplaca.Enabled = true;
            txtmarca.Enabled = true;
            txtquilometragem.Enabled = true;
            txtano.Enabled = true;
            txtmodelo.Enabled = true;
            txtcor.Enabled = true;
            cbbcambio.Enabled = true;
            txtmotor.Enabled = true;
            btnsalvar.Enabled = true;
            cbbdono.Enabled = true;
            txtbplaca.Enabled = false;
            btnbnome.Enabled = false;
            limpar();
            preencherCBdono();
        }

        private void btnbnome_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from VEICULOS where PLACA=@PLACA", conexao);
                sql.Parameters.Add("@PLACA", SqlDbType.VarChar).Value = txtbplaca.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        txtid.Text = dt["ID"].ToString();
                        txtplaca.Text = dt["PLACA"].ToString();
                        txtmodelo.Text = dt["MODELO"].ToString();
                        cbbcambio.Text = dt["CAMBIO"].ToString();
                        txtcor.Text = dt["COR"].ToString();
                        txtano.Text = dt["ANO"].ToString();
                        txtmarca.Text = dt["MARCA"].ToString();
                        txtmotor.Text = dt["MOTOR"].ToString();
                        txtquilometragem.Text = dt["QUILOMETRAGEM"].ToString();
                        cbbdono.Text = dt["DONO"].ToString();
                        carregadatagrid();
                        atualizaCBdono();
                    }
                    else
                    {
                        MessageBox.Show("Veiculo não encontrado");
                        limpar();
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dados();
                    txtbplaca.Enabled = true;
                    btnsalvar.Enabled = true;
                    txtbplaca.Text = null;
                    conexao.Close();
                }
            }
        }

        private void btnsalvar_Click(object sender, EventArgs e)
        {
            if (btnnovoveiculo.Enabled == false)
            {
                if (txtplaca.Text == string.Empty )
                {
                    MessageBox.Show("Campo PLACA obrigatório");
                }
                else if (txtmodelo.Text == string.Empty)
                {
                    MessageBox.Show("Campo MODELO obrigatório");
                }
                else if (txtano.Text == string.Empty)
                {
                    MessageBox.Show("Campo ANO obrigatório");
                }
                else if (cbbcambio.Text == string.Empty)
                {
                    MessageBox.Show("Campo CAMBIO obrigatório");
                }
                else if (txtcor.Text == string.Empty)
                {
                    MessageBox.Show("Campo COR obrigatório");
                }
                else if (txtmarca.Text == string.Empty)
                {
                    MessageBox.Show("Campo MARCA obrigatório");
                }
                else if (txtmotor.Text == string.Empty)
                {
                    MessageBox.Show("Campo MOTOR obrigatório");
                }
                else if (txtquilometragem.Text == string.Empty)
                {
                    MessageBox.Show("Campo KM obrigatório");
                }
                else if (cbbdono.Text == string.Empty)
                {
                    MessageBox.Show("Campo DONO obrigatório");
                }
                else
                {
                    veiculo vei = new veiculo();
                    vei.placa = txtplaca.Text;
                    vei.marca = txtmarca.Text;
                    vei.quilometragem = txtquilometragem.Text;
                    vei.ano = txtano.Text;
                    vei.modelo = txtmodelo.Text;
                    vei.cor = txtcor.Text;
                    vei.cambio = cbbcambio.Text;
                    vei.motor = txtmotor.Text;
                    vei.dono = cbbdono.Text;
                    vei.Salvar();
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
                    SqlCommand sql = new SqlCommand("UPDATE VEICULOS set PLACA=@PLACA,MARCA=@MARCA,QUILOMETRAGEM=@QUILOMETRAGEM,ANO=@ANO,MODELO=@MODELO,COR=@COR, CAMBIO=@CAMBIO, MOTOR=@MOTOR, DONO=@DONO where ID=@ID", conexao);
                    sql.Parameters.Add("@ID", SqlDbType.Int).Value = txtid.Text;
                    sql.Parameters.Add("@PLACA", SqlDbType.VarChar).Value = txtplaca.Text;
                    sql.Parameters.Add("@MARCA", SqlDbType.VarChar).Value = txtmarca.Text;
                    sql.Parameters.Add("@QUILOMETRAGEM", SqlDbType.VarChar).Value = txtquilometragem.Text;
                    sql.Parameters.Add("@ANO", SqlDbType.VarChar).Value = txtano.Text;
                    sql.Parameters.Add("@MODELO", SqlDbType.VarChar).Value = txtmodelo.Text;
                    sql.Parameters.Add("@COR", SqlDbType.VarChar).Value = txtcor.Text;
                    sql.Parameters.Add("@CAMBIO", SqlDbType.VarChar).Value = cbbcambio.Text;
                    sql.Parameters.Add("@MOTOR", SqlDbType.VarChar).Value = txtmotor.Text;
                    sql.Parameters.Add("@DONO", SqlDbType.VarChar).Value = cbbdono.Text;

                    try
                    {
                        conexao.Open();
                        sql.ExecuteNonQuery();
                        MessageBox.Show("Alterado com sucesso !");
                        limpar();
                        campos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conexao.Close();
                        dataGridView1.ClearSelection();
                        carregadatagrid();
                    }
                }
            }
        }

        private void btnvoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void preencherCBdono()
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("Falha ao efetuar a conexão. Erro: " + sqle);
                }
                String scom = "select * from clientes";
                SqlDataAdapter da = new SqlDataAdapter(scom, conexao);
                DataTable dtResultado = new DataTable();
                dtResultado.Clear();
                cbbdono.DataSource = null;
                da.Fill(dtResultado);
                cbbdono.DataSource = dtResultado;
                cbbdono.ValueMember = "id";
                cbbdono.DisplayMember = "nome";
                cbbdono.SelectedItem = "";
                cbbdono.Refresh();
                cbbdono.Text = null;
                conexao.Close();
            }
        }
        private void atualizaCBdono()
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("Falha ao efetuar a conexão. Erro: " + sqle);
                }
                String scom = "select * from clientes";
                SqlDataAdapter da = new SqlDataAdapter(scom, conexao);
                DataTable dtResultado = new DataTable();
                dtResultado.Clear();
                cbbdono.DataSource = null;
                da.Fill(dtResultado);
                cbbdono.DataSource = dtResultado;
                cbbdono.ValueMember = "id";
                cbbdono.DisplayMember = "nome";
                cbbdono.SelectedItem = "";
                cbbdono.Refresh();
                conexao.Close();
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                txtid.Text = row.Cells["id"].Value.ToString();
                txtplaca.Text = row.Cells["placa"].Value.ToString();
                txtmodelo.Text = row.Cells["modelo"].Value.ToString();
                txtano.Text = row.Cells["ano"].Value.ToString();
                cbbcambio.Text = row.Cells["cambio"].Value.ToString();
                txtcor.Text = row.Cells["cor"].Value.ToString();
                txtmarca.Text = row.Cells["marca"].Value.ToString();
                txtmotor.Text = row.Cells["motor"].Value.ToString();
                txtquilometragem.Text = row.Cells["km"].Value.ToString();
                cbbdono.Text = row.Cells["dono"].Value.ToString();
                dados();
                atualizaCBdono();
            }
        }
    }
}
