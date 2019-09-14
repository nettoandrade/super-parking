using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Parking.Server.Models;

namespace Parking.Server.Controllers.DAO
{
    public class ParkingDao
    {
        
    static string serverName = "ec2-174-129-227-80.compute-1.amazonaws.com";                                          
    static string port = "5432";
    static string userName = "bvarzzeczllmfw";                                               
    static string password = "84f3684a8f799eef9c1a2260ebdee071e67753af0e259d892dd361a40b005cfc";                                             
    static string databaseName = "dcap88ssfll8df";                                       
    NpgsqlConnection pgsqlConnection = null;
    string connString = null;

    public ParkingDao()
    {
        connString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};SSL Mode=Require;Trust Server Certificate=true;",
                                                    serverName, port, userName, password, databaseName);
    }

           //Inserir registros
        public void InserirRegistros(string dsTabela,string vlMinimo, int vlAdicional)
        {
           
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {                 
                    pgsqlConnection.Open();

                    string cmdInserir = String.Format("Insert Into tabela_de_preco(DS_TABELA,VALOR_MINIMO,VALOR_ADICIONAL) values('{0}','{1}',{2})",dsTabela,vlMinimo,vlAdicional);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                    }                    
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
        }

    public List<TabelaDePreco> GetTabelaPreco(){
        List<TabelaDePreco> tabela = new List<TabelaDePreco>();        
        DataTable dt = new DataTable();
        try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select * from TABELA_DE_PRECO order by 1";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }
        
            foreach (DataRow row in dt.Rows)
            {
                TabelaDePreco dados = new TabelaDePreco(){
                    DescTabela = row["DS_TABELA"].ToString(),
                    vlMinimo = Double.Parse(row["VALOR_MINIMO"].ToString()),
                    vlAdicional = Double.Parse(row["VALOR_ADICIONAL"].ToString())
                };
                tabela.Add(dados);
            }            

            return tabela;
    }    

    public DataTable GetTodosRegistros()
        { 

            DataTable dt = new DataTable();

            try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select * from funcionarios order by id";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }

            return dt;
        }

        //Deleta registros
        public void DeletarRegistro(string nome)
    {
        try
        {
            using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
            {
                //abre a conexao                
                pgsqlConnection.Open();

                string cmdDeletar = String.Format("Delete From funcionarios Where nome = '{0}'",nome);

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdDeletar, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }
            }
        }
        catch (NpgsqlException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            pgsqlConnection.Close();
        }
    }
    
        internal List<Vaga> GetVagas()
        {
            List<Vaga> vagas = new List<Vaga>();
            DataTable dt = new DataTable();

            try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select * from public.vagas order by 1";

                    using (NpgsqlDataAdapter Adpt = new NpgsqlDataAdapter(cmdSeleciona, pgsqlConnection))
                    {
                        Adpt.Fill(dt);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }

            foreach (DataRow row in dt.Rows)
            {
                Vaga dados = new Vaga(row["DS_SETOR"].ToString(),
                                      int.Parse(row["NR_VAGA_TOTAL"].ToString()),
                                      int.Parse(row["NR_VAGA_TOTAL"].ToString()));
                vagas.Add(dados);
            } 

            return vagas;
        }
    }
    
}