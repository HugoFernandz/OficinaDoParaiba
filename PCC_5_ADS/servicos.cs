using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using Database;

namespace PCC_5_ADS
{
    public partial class frm_caixa : Form
    {

        public frm_caixa()
        {
            InitializeComponent();
            inicio();
        }

        private string connectionString = ConfigurationManager.AppSettings["SqlConnection"];

        private void inicio()
        {
            txtid.Enabled = false;
            txtmecanico.Enabled = false;
            txtdescricao.Enabled = false;
            txttotal.Enabled = false;
            txtmaodeobra.Enabled = false;
            txtstatus.Enabled = false;
            txtplaca.Enabled = false;
            txtpeca.Enabled = false;
            txtqtdd.Enabled = false;
            txtdata.Enabled = false;
            btnbuscarservico.Enabled = true;
            btnnovoservico.Enabled = true;
            txtidbusca.Enabled = true;
            btnpdf.Enabled = false;
            btnsalvar.Enabled = false;
            button1.Enabled = true;
            radiopecaloja.Enabled = false;
            radiopecacliente.Enabled = false;
            btnpeca.Enabled = false;
            txtordem.Enabled = false;
        }
        private void limpar()
        {
            txtordem.Text = null;
            txtid.Text = null;
            txtmecanico.Text = null;
            txtdescricao.Text = null;
            radiopecacliente.Checked = false;
            radiopecaloja.Checked = false;
            txttotal.Text = null;
            txtmaodeobra.Text = null;
            txtstatus.Text = null;
            txtplaca.Text = null;
            txtpeca.Text = null;
            txtqtdd.Text = null;
            txtdata.Text = null; ;
            txtidbusca.Text = null;
            txtbplaca.Text = null;
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
        }

        private void dados()
        {
            txtmecanico.Enabled = true;
            txtdescricao.Enabled = true;
            radiopecacliente.Enabled = false;
            radiopecaloja.Enabled = false;
            txttotal.Enabled = false;
            txtmaodeobra.Enabled = true;
            txtstatus.Enabled = true;
            txtplaca.Enabled = true;
            txtpeca.Enabled = false;
            txtqtdd.Enabled = false;
            btnpeca.Enabled = false;
            txtdata.Enabled = true;
            btnpdf.Enabled = true;
            btnsalvar.Enabled = true;
        }

        private void btnbuscarservico_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from SERVICOS where ORDEM=@ORDEM", conexao);
                sql.Parameters.Add("@ORDEM", SqlDbType.Int).Value = txtidbusca.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        txtid.Text = dt["ID"].ToString();
                        txtplaca.Text = dt["PLACA"].ToString();
                        txtmaodeobra.Text = dt["MAODEOBRA"].ToString();
                        txtmecanico.Text = dt["MECANICO"].ToString();
                        txtdescricao.Text = dt["DESCRICAO"].ToString();
                        txttotal.Text = dt["TOTAL"].ToString();
                        txtstatus.Text = dt["STATUS"].ToString();
                        txtdata.Text = dt["DATA"].ToString();
                        txtordem.Text = dt["ORDEM"].ToString();
                        carregadataplaca();
                        carregadataproduto();
                        radio();
                    }
                    else
                    {
                        MessageBox.Show("Ordem de Serviço não cadastrado");
                        limpar();
                        inicio();
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dados();
                    conexao.Close();
                }
            }
        }
        private void btnsalvar_Click(object sender, EventArgs e)
        {
            if (btnnovoservico.Enabled == false)
            {
                if (txtplaca.Text == string.Empty)
                {
                    MessageBox.Show("Campo PLACA obrigatório");
                }
                else if (txtmaodeobra.Text == string.Empty)
                {
                    MessageBox.Show("Campo MAO DE OBRA obrigatório");
                }
                else if (txtmecanico.Text == string.Empty)
                {
                    MessageBox.Show("Campo MECANICO obrigatório");
                }
                else if (txtdescricao.Text == string.Empty)
                {
                    MessageBox.Show("Campo DESCRIÇÃO obrigatório");
                }
                else if (txtstatus.Text == string.Empty)
                {
                    MessageBox.Show("Campo STATUS obrigatório");
                }
                else if (txtdata.Text == string.Empty)
                {
                    MessageBox.Show("Campo DATA obrigatório");
                }
                else
                {
                    servico ser = new servico();
                    ser.placa = txtplaca.Text;
                    ser.maodeobra = txtmaodeobra.Text;
                    ser.mecanico = txtmecanico.Text;
                    ser.descricao = txtdescricao.Text;
                    ser.total = txttotal.Text;
                    ser.status = txtstatus.Text;
                    ser.data = txtdata.Text;
                    ser.ordem = txtordem.Text;
                    ser.Salvar();
                    MessageBox.Show("Cadastrado com sucesso");
                    string comtotal = "0";
                    using (SqlConnection conexao = new SqlConnection(connectionString))
                    {
                        SqlCommand sql = new SqlCommand("select*from COMISSAOS where MECANICO=@MECANICO", conexao);
                        sql.Parameters.Add("@MECANICO", SqlDbType.VarChar).Value = txtmecanico.Text;
                        try
                        {

                            conexao.Open();
                            SqlDataReader dt = sql.ExecuteReader();
                            if (dt.HasRows)
                            {
                                dt.Read();
                                comtotal = dt["VALOR"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Mecânico não cadastrado");
                            }

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            conexao.Close();
                        }
                        string comissao = "0";
                        double percentual = 0.30;
                        comissao = (Convert.ToDouble(txtmaodeobra.Text) * percentual).ToString();
                        SqlCommand sqll = new SqlCommand("UPDATE COMISSAOS set VALOR=@VALOR where MECANICO=@MECANICO", conexao);
                        sqll.Parameters.Add("@MECANICO", SqlDbType.VarChar).Value = txtmecanico.Text;
                        sqll.Parameters.Add("@VALOR", SqlDbType.VarChar).Value = Convert.ToDouble(comtotal) + Convert.ToDouble(comissao);
                        try
                        {
                            conexao.Open();
                            sqll.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            conexao.Close();
                        }
                        SqlCommand sqlll = new SqlCommand("UPDATE ORDEMSS set ORDEM=@ORDEM where ID=1", conexao);
                        sqlll.Parameters.Add("@ORDEM", SqlDbType.Int).Value = txtordem.Text;
                        try
                        {
                            conexao.Open();
                            sqlll.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            conexao.Close();
                        }
                        limpar();
                        inicio();
                    }
                }
            }
            else
            {
                using (SqlConnection conexao = new SqlConnection(connectionString))
                {
                    SqlCommand sql = new SqlCommand("UPDATE SERVICOS set PLACA=@PLACA, MAODEOBRA=@MAODEOBRA, MECANICO=@MECANICO, COMISSAO=@COMISSAO, DESCRICAO=@DESCRICAO," +
                    "TOTAL=@TOTAL, STATUS=@STATUS, DATA=@DATA where ORDEM=@ORDEM", conexao);
                    sql.Parameters.Add("@ORDEM", SqlDbType.Int).Value = txtordem.Text;
                    sql.Parameters.Add("@PLACA", SqlDbType.VarChar).Value = txtplaca.Text;
                    sql.Parameters.Add("@MAODEOBRA", SqlDbType.VarChar).Value = txtmaodeobra.Text;
                    sql.Parameters.Add("@MECANICO", SqlDbType.VarChar).Value = txtmecanico.Text;
                    sql.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = txtdescricao.Text;
                    sql.Parameters.Add("@TOTAL", SqlDbType.VarChar).Value = txttotal.Text;
                    sql.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = txtstatus.Text;
                    sql.Parameters.Add("@DATA", SqlDbType.VarChar).Value = txtdata.Text;

                    try
                    {
                        conexao.Open();
                        sql.ExecuteNonQuery();
                        MessageBox.Show("Alterado com sucesso !");
                        limpar();
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

        private void btnnovoservico_Click(object sender, EventArgs e)
        {
            limpar();
            txtmaodeobra.Enabled = true;
            txtstatus.Enabled = true;
            txtplaca.Enabled = true;
            txtstatus.Enabled = true;
            txtpeca.Enabled = true;
            txtqtdd.Enabled = true;
            radiopecaloja.Enabled = true;
            radiopecacliente.Enabled = true;
            btnpeca.Enabled = true;
        }
        public void pedeef()
        {
            Document doc = new Document(PageSize.A4);
            doc.SetMargins(40, 40, 40, 80);
            doc.AddCreationDate();

            string caminho = @"C:\Users\victo\Desktop\" + "Relatorio.pdf";

            PdfWriter writer = PdfWriter.GetInstance(doc, new
            FileStream(caminho, FileMode.Create));
            doc.Open();
            Paragraph paragrafo = new Paragraph();
            string texto = "OFICINA DO PARAÍBA \n" + "\n" + "TELEFONE: 4418-1818               WPP: (11) 9 2427-1714 \n"
              + "ENDEREÇO : RUA DA CONSOLAÇÃO      N°100     BAIRRO DA COLINA - ATIBAIA\n" +
              "HORÁRIO DE FUNCIONAMENTO:        8:00 - 18:00 SEG A SEX\n EMAIL: contato@oficinadoparaiba.com\n" +
              "\n\nCLIENTE\nNome: " + clicliente + "      RG: " + clirg + "      CPF: " + clicpf + "      Bairro: " + clibairro +
              "\nCelular:" + clicelular + "      Endereço: " + cliendereco + "      Cidade: " + clicidade +
              "\n\n VEÍCULO\nPlaca: " + txtplaca.Text + "        Modelo: " + veimodelo + "       Cambio: " + veicambio +
              "\nMarca: " + veimarca + "        Km: " + veikm + "      Motor: " + veimotor +
              "\nCor: " + veicor + "       Ano: " + veiano + 
              "\n\nSERVICO PRESTADO\nDescrição: " + txtdescricao.Text + "\nMão de Obra: R$"+txtmaodeobra.Text+"\nTotal = R$ "+txttotal.Text+"       Situação: "+txtstatus.Text+  
              "\nMecânico: " + txtmecanico.Text + "      Data: " + txtdata.Text;
            paragrafo.Add(texto);
            doc.Add(paragrafo);
            doc.Close();
            MessageBox.Show("Criado com sucesso !");
            System.Diagnostics.Process.Start(caminho);
        }

        private void btnnovoservico_Click_1(object sender, EventArgs e)
        {
            limpar();
            txtmecanico.Enabled = true;
            txtdescricao.Enabled = true;
            radiopecacliente.Enabled = true;
            radiopecaloja.Enabled = true;
            txtmaodeobra.Enabled = true;
            txtstatus.Enabled = true;
            txtplaca.Enabled = true;
            txtpeca.Enabled = true;
            txtqtdd.Enabled = true;
            txtdata.Enabled = true;
            btnbuscarservico.Enabled = false;
            btnnovoservico.Enabled = false;
            txtidbusca.Enabled = false;
            btnpdf.Enabled = true;
            btnsalvar.Enabled = true;
            btnbplaca.Enabled = false;
            btnpeca.Enabled = true;
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from ORDEMSS where ID=1", conexao);

                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        txtordem.Text = (Convert.ToInt32(dt["ORDEM"])+1).ToString();
                    }
                    else
                    {
                        MessageBox.Show(" ");
                    }
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
            preencherCBmecanico();
            preencherCBpeca();
            preencherCBplaca();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        string clicliente = "0";
        string clirg = "0";
        string clicpf = "0";
        string clicelular = "0";
        string clibairro = "0";
        string clicidade = "0";
        string cliendereco = "0";

        string veimodelo = "0";
        string veicambio = "0";
        string veicor = "0";
        string veiano = "0";
        string veimarca = "0";
        string veimotor = "0";
        string veikm = "0";

        private void btnpdf_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from VEICULOS where PLACA=@PLACA", conexao);
                sql.Parameters.Add("@PLACA", SqlDbType.VarChar).Value = txtplaca.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        veimodelo = dt["MODELO"].ToString();
                        veicambio = dt["CAMBIO"].ToString();
                        veicor = dt["COR"].ToString();
                        veiano = dt["ANO"].ToString();
                        veimarca = dt["MARCA"].ToString();
                        veimotor = dt["MOTOR"].ToString();
                        veikm = dt["QUILOMETRAGEM"].ToString();
                        clicliente = dt["DONO"].ToString();
                    }
                    else
                    {
                        MessageBox.Show(" ");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conexao.Close();
                }


                SqlCommand sqll = new SqlCommand("select*from CLIENTES where NOME=@NOME", conexao);
                sqll.Parameters.Add("@NOME", SqlDbType.VarChar).Value = clicliente;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sqll.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        clirg = dt["RG"].ToString();
                        clicpf = dt["CPF"].ToString();
                        clicelular = dt["CELULAR"].ToString();
                        clibairro = dt["BAIRRO"].ToString();
                        clicidade = dt["CIDADE"].ToString();
                        cliendereco = dt["ENDERECO"].ToString();
                    }
                    else
                    {
                        MessageBox.Show(" ");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conexao.Close();
                }
                pedeef();
            }
        }

        private void radiopecacliente_CheckedChanged_1(object sender, EventArgs e)
        {
            txtpeca.Enabled = false;
            txtqtdd.Enabled = false;
            btnpeca.Enabled = false;
        }

        private void radiopecaloja_CheckedChanged_1(object sender, EventArgs e)
        {
            txtpeca.Enabled = true;
            txtqtdd.Enabled = true;
            btnpeca.Enabled = true;
        }

        private void btnpeca_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                ordems ser = new ordems();
                ser.ordem = txtordem.Text;
                ser.produto = txtpeca.Text;
                ser.quantidade = txtqtdd.Text;
                ser.login = "0";
                ser.senha = "0";
                ser.Salvar();
                string pecavalor = "0";
                string pecaestoque = "0";
                SqlCommand sql = new SqlCommand("select*from PRODUTOS where DESCRICAO=@DESCRICAO", conexao);
                sql.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = txtpeca.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        pecavalor = dt["PRECO"].ToString();
                        pecaestoque = dt["QTDD"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Peça não cadastrado");
                    }
                }


                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    double vezes = Convert.ToDouble(txtqtdd.Text);
                    string pecatotal = (Convert.ToDouble(pecavalor) * vezes).ToString();
                    txttotal.Text = (Convert.ToDouble(txttotal.Text) + Convert.ToDouble(pecatotal)).ToString();
                    conexao.Close();

                }
                SqlCommand sqll = new SqlCommand("UPDATE PRODUTOS set QTDD=@QTDD where DESCRICAO=@DESCRICAO", conexao);
                sqll.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = txtpeca.Text;
                sqll.Parameters.Add("@QTDD", SqlDbType.VarChar).Value = Convert.ToInt32(pecaestoque) - Convert.ToInt32(txtqtdd.Text);

                try
                {
                    conexao.Open();
                    sqll.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conexao.Close();
                    txtpeca.Text = null;
                    txtqtdd.Text = null;
                    txtmaodeobra.Enabled = false;
                    MessageBox.Show("Adicionado com Sucesso");
                    dataGridView2.ClearSelection();
                    carregadataproduto();
                }
            }
        }
        private void preencherCBmecanico()
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
                String scom = "select * from mecanicos";
                SqlDataAdapter da = new SqlDataAdapter(scom, conexao);
                DataTable dtResultado = new DataTable();
                dtResultado.Clear();
                txtmecanico.DataSource = null;
                da.Fill(dtResultado);
                txtmecanico.DataSource = dtResultado;
                txtmecanico.ValueMember = "id";
                txtmecanico.DisplayMember = "nome";
                txtmecanico.SelectedItem = "";
                txtmecanico.Refresh();
                txtmecanico.Text = null;
                conexao.Close();
            }
        }
        private void preencherCBplaca()
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
                String scom = "select * from veiculos";
                SqlDataAdapter da = new SqlDataAdapter(scom, conexao);
                DataTable dtResultado = new DataTable();
                dtResultado.Clear();
                txtplaca.DataSource = null;
                da.Fill(dtResultado);
                txtplaca.DataSource = dtResultado;
                txtplaca.ValueMember = "id";
                txtplaca.DisplayMember = "placa";
                txtplaca.SelectedItem = "";
                txtplaca.Refresh();
                txtplaca.Text = null;
                conexao.Close();
            }
        }
        private void preencherCBpeca()
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
                String scom = "select * from produtos";
                SqlDataAdapter da = new SqlDataAdapter(scom, conexao);
                DataTable dtResultado = new DataTable();
                dtResultado.Clear();
                txtpeca.DataSource = null;
                da.Fill(dtResultado);
                txtpeca.DataSource = dtResultado;
                txtpeca.ValueMember = "id";
                txtpeca.DisplayMember = "descricao";
                txtpeca.SelectedItem = "";
                txtpeca.Refresh();
                txtpeca.Text = null;
                conexao.Close();
            }
        }

        private void txtmaodeobra_TextChanged(object sender, EventArgs e)
        {
            txttotal.Text = txtmaodeobra.Text;
            if (txtmaodeobra.Text ==string.Empty)
            {
                txttotal.Text = null;
            }
        }

        private void btnbplaca_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from SERVICOS where PLACA=@PLACA", conexao);
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
                        txtmaodeobra.Text = dt["MAODEOBRA"].ToString();
                        txtmecanico.Text = dt["MECANICO"].ToString();
                        txtdescricao.Text = dt["DESCRICAO"].ToString();
                        txttotal.Text = dt["TOTAL"].ToString();
                        txtstatus.Text = dt["STATUS"].ToString();
                        txtdata.Text = dt["DATA"].ToString();
                        txtordem.Text = dt["ORDEM"].ToString();
                        carregadataplaca();
                        dados();
                        carregadataproduto();
                        radio();
                    }
                    else
                    {
                        MessageBox.Show("Placa não encontrado");
                        limpar();
                    }
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
        private void carregadataplaca()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                try
                {
                    string sql = "SELECT id, placa, maodeobra, mecanico, descricao, total, status, data, ordem FROM servicos";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conexao);
                    conexao.Open();
                    da.Fill(ds, "servicos");
                    conexao.Close();
                }

                catch (SqlException sqle)
                {
                    MessageBox.Show("Falha ao efetuar a conexão. Erro: " + sqle);
                }

                DataView dv = new DataView(ds.Tables["servicos"]);
                dv.AllowNew = false;
                dv.AllowDelete = true;
                dv.RowFilter = "placa Like '" + txtbplaca.Text + "*'";
                dataGridView1.DataSource = dv;
            }
        }

        private void carregadataproduto()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                try
                {
                    string sql = "SELECT produto, quantidade, ordem FROM ordemss";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conexao);
                    conexao.Open();
                    da.Fill(ds, "ordemss");
                    conexao.Close();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("Falha ao efetuar a conexão. Erro: " + sqle);
                }
                
                DataView dv = new DataView(ds.Tables["ordemss"]);
                dv.AllowNew = false;
                dv.AllowDelete = true;
                dv.RowFilter = "ordem Like '" + txtordem.Text + "*'";
                dataGridView2.DataSource = dv;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                txtid.Text = row.Cells["id"].Value.ToString();
                txtordem.Text = row.Cells["ordem"].Value.ToString();
                txtdata.Text = row.Cells["data"].Value.ToString();
                txtplaca.Text = row.Cells["placa"].Value.ToString();
                txtstatus.Text = row.Cells["status"].Value.ToString();
                txtdescricao.Text = row.Cells["descricao"].Value.ToString();
                txttotal.Text = row.Cells["total"].Value.ToString();
                txtmaodeobra.Text = row.Cells["maodeobra"].Value.ToString();
                dados();
                carregadataproduto();
                radio();

            }
        }
        private void radio()
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                SqlCommand sql = new SqlCommand("select*from ORDEMSS where ORDEM=@ORDEM", conexao);
                sql.Parameters.Add("@ORDEM", SqlDbType.VarChar).Value = txtordem.Text;
                try
                {
                    conexao.Open();
                    SqlDataReader dt = sql.ExecuteReader();
                    if (dt.HasRows)
                    {
                        dt.Read();
                        radiopecaloja.Enabled = true;
                        radiopecaloja.Checked = true; 
                    }
                    else
                    {
                        radiopecacliente.Enabled = true;
                        radiopecacliente.Checked = true;
                    }
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
}

