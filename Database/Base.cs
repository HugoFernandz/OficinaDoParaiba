using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;

namespace Database
{
    public class Base : Ibase
    {
        private string connectionString = ConfigurationManager.AppSettings["SqlConnection"];
        public int Key
        {
            get
            {
                foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    Opcoesbase opcoes = (Opcoesbase)pi.GetCustomAttribute(typeof(Opcoesbase));
                    if (opcoes != null && opcoes.ChavePrimaria)
                    {
                        return Convert.ToInt32(pi.GetValue(this));
                    }
                }
                return 0;
            }
        }

        public List<Ibase> Busca()
        {
            var list = new List<Ibase>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<string> where = new List<string>();
                string chavePrimaria = string.Empty;
                foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    Opcoesbase opcoesBase = (Opcoesbase)pi.GetCustomAttribute(typeof(Opcoesbase));
                    if (opcoesBase != null)
                    {
                        if (opcoesBase.ChavePrimaria)
                        {
                            chavePrimaria = pi.Name + "=" + pi.GetValue(this);
                        }
                        if (opcoesBase.UsaBD && !opcoesBase.ChavePrimaria)
                        {
                            var valor = pi.GetValue(this);
                            if (valor != null)
                            {
                                if (tipoPropriedade(pi) == "varchar(255)" || tipoPropriedade(pi) == "datetime")
                                {
                                    where.Add(pi.Name + "='" + valor + "'");
                                }
                                else
                                {
                                    where.Add(pi.Name + "=" + valor);
                                }
                            }
                        }
                    }

                }
                string sql;
                if (chavePrimaria.Equals("Id="))
                {
                    sql = "select * from " + this.GetType().Name + "s where ";
                }
                else
                {
                    sql = "select * from " + this.GetType().Name + "s where " + chavePrimaria;
                }

                if (where.Count > 0)
                {
                    sql += string.Join(" and ", where.ToArray());
                }
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                sqlCommand.Connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var obj = (Ibase)Activator.CreateInstance(this.GetType());
                    setProperty(ref obj, reader);
                    list.Add(obj);
                }
            }
            return list;
        }

        public virtual void Salvar()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<string> campos = new List<string>();
                List<string> valores = new List<string>();
                foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    Opcoesbase opcoes = (Opcoesbase)pi.GetCustomAttribute(typeof(Opcoesbase));
                    if (opcoes != null && opcoes.UsaBD && !opcoes.ChavePrimaria)
                    {
                        if (this.Key == 0)
                        {
                            campos.Add(pi.Name);
                            valores.Add("'" + pi.GetValue(this) + "'");
                        }
                        else
                        {
                            if (!opcoes.ChavePrimaria)
                            {
                                valores.Add(" " + pi.Name + " = '" + pi.GetValue(this) + "'");
                            }
                        }

                    }
                }
                string sql = null;
                if (this.Key == 0)
                {
                    sql = "insert into " + this.GetType().Name + "s (" + string.Join(", ", campos.ToArray()) + ") " +
                    "values(" + string.Join(", ", valores.ToArray()) + ");";
                }
                else
                {
                    sql = "update " + this.GetType().Name + "s set " + string.Join(", ", valores.ToArray()) + " where id=" + this.Key + ";";
                }

                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Ibase> Excluir()
        {
            var list = new List<Ibase>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "delete from " + this.GetType().Name + "s where id=" + this.Key + ";";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            return list;
        }
       public List<Ibase> Todos()
        {
            var list = new List<Ibase>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "select * from " + this.GetType().Name + "s";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var obj = (Ibase)Activator.CreateInstance(this.GetType());
                    setProperty(ref obj, reader);
                    list.Add(obj);
                }
            }
            return list;
        }
        private void setProperty(ref Ibase obj, SqlDataReader reader)
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Opcoesbase opcoesBase = (Opcoesbase)pi.GetCustomAttribute(typeof(Opcoesbase));
                if (opcoesBase != null && opcoesBase.UsaBD)
                {
                    pi.SetValue(obj, reader[pi.Name]);
                }
            }
        }

        private string tipoPropriedade(PropertyInfo pi)
        {
            switch (pi.PropertyType.Name)
            {
                case "Int32":
                    return "int";
                case "Int64":
                    return "bigint";
                case "double":
                    return "decimal(9,2)";
                case "Single":
                    return "float";
                case "Datetime":
                    return "datetime";
                default:
                    return "varchar(255)";
            }
        }

        public virtual void CriarTabela()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<string> campos = new List<string>();
                string chaveprimaria = "";
                foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    Opcoesbase pOpcoesBase = (Opcoesbase)pi.GetCustomAttribute(typeof(Opcoesbase));
                    if (pOpcoesBase != null && pOpcoesBase.UsaBusca)
                    {
                        if (pOpcoesBase.ChavePrimaria)
                        {
                            chaveprimaria = pi.Name + " int identity, ";
                        }
                        else
                        {
                            campos.Add(pi.Name + " " + tipoPropriedade(pi) + " ");
                        }
                    }
                }

                string tabela = "IF EXISTS(SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo]." + this.GetType().Name + "s') AND type in (N'U'))DROP TABLE[dbo].[" + this.GetType().Name + "s]";
                SqlCommand command = new SqlCommand(tabela, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

                string queryString = "CREATE TABLE " + this.GetType().Name + "s (";
                queryString += chaveprimaria;
                queryString += string.Join(",", campos.ToArray());
                queryString += ");";

                command = new SqlCommand(queryString, connection);
                command.ExecuteNonQuery();

            }

        }
    }   
}

