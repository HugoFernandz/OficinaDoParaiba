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
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();

		}
        private void limpar()
        {
            txt_usuario.Text = null;
            txt_senha.Text = null;
        }
        private string connectionString = ConfigurationManager.AppSettings["SqlConnection"];

        private void btn_entrar_Click(object sender, EventArgs e)
        {
            if (txt_usuario.Text == "admin")
            {
                if (txt_senha.Text == "0")
                {
                    MessageBox.Show("Senha incorreta");
                    limpar();
                }
                else
                {
                    using (SqlConnection conexao = new SqlConnection(connectionString))
                    {
                        SqlCommand sql = new SqlCommand("SELECT ID=@ID FROM ORDEMSS  where SENHA=@SENHA", conexao);
                        sql.Parameters.Add("@ID", SqlDbType.Int).Value = 1;
                        sql.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = txt_senha.Text;
                        try
                        {
                            conexao.Open();
                            SqlDataReader dt = sql.ExecuteReader();
                            if (dt.HasRows)
                            {
                                dt.Read();
                                this.Hide();
                                Form f = new Menu();
                                f.Closed += (s, args) => this.Close();
                                f.Show();
                            }
                            else
                            {
                                MessageBox.Show("Senha incorreta");
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
            }
            else
            {
                MessageBox.Show("Login incorreto");
                limpar();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            redefinirsenha red = new redefinirsenha();
            red.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
