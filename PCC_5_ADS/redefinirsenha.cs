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
    public partial class redefinirsenha : Form
    {
        public redefinirsenha()
        {
            InitializeComponent();
        }
        private void limpar()
        {
            txtmenu.Text = null;
            txt_novasenha.Text = null;
            txt_novamente.Text = null;
        }
        private string connectionString = ConfigurationManager.AppSettings["SqlConnection"];

        private void btn_alterar_Click(object sender, EventArgs e)
        {
            if (txtmenu.Text == "7")
            {
                if (txt_novasenha.Text == txt_novamente.Text)
                {
                    using (SqlConnection conexao = new SqlConnection(connectionString))
                    {

                        SqlCommand sql = new SqlCommand("UPDATE ORDEMSS set SENHA=@SENHA where ID=@ID", conexao);
                        sql.Parameters.Add("@ID", SqlDbType.Int).Value = 1;
                        sql.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = txt_novasenha.Text;

                        try
                        {
                            conexao.Open();
                            sql.ExecuteNonQuery();
                            MessageBox.Show("Alterado com sucesso !");
                            this.Close();
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
                else
                {
                    MessageBox.Show("Senhas diferentes");
                    limpar();
                }
            }
            else
            {
                MessageBox.Show("Resposta incorreta");
                limpar();
            }
        }
    }
}
