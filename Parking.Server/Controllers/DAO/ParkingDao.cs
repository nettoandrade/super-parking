using System;
using System.Data;
using Npgsql;

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
        connString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                                    serverName, port, userName, password, databaseName);
    }

           //Inserir registros
        public void InserirRegistros(string nome,string email, int idade)
        {
           
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    //Abra a conexão com o PgSQL                  
                    pgsqlConnection.Open();

                    string cmdInserir = String.Format("Insert Into funcionarios(nome,email,idade) values('{0}','{1}',{2})",nome,email,idade);

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
        
    }
    
}