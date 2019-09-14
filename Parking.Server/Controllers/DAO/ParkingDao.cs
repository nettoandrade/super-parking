using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Parking.Server.Models;

namespace Parking.Server.Controllers.DAO{
    public class ParkingDao{

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

        internal Ticket GetTicket(string ticket)
        {
        Ticket dados = null;
        DataTable dt = new DataTable();
        try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select * from ticket where \"DS_TICKET\" = '" + ticket + "'";

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

            foreach (DataRow row in dt.Rows){
                dados = new Ticket(){
                    ticket = row["DS_TICKET"].ToString(),
                    dataPagamento = String.IsNullOrEmpty(row["DT_PAGAMENTO"].ToString()) ? (DateTime?)null : DateTime.Parse(row["DT_PAGAMENTO"].ToString()),
                    valor = double.Parse(row["NR_VALOR"].ToString()),
                    pago = row["SN_PAGO"].ToString().Equals("S") ? true : false,
                    tabela = row["DS_TABELA"].ToString()
                };
            }

            return dados;
        }

        internal object GetValorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
        DataTable dt = new DataTable();
        try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = String.Format("SELECT SUM(\"NR_VALOR\") VALOR FROM TICKET WHERE \"DT_PAGAMENTO\" BETWEEN TO_DATE('{0}','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}','DD/MM/YYYY HH24:MI:SS')",dataInicio,dataFim);

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

            foreach (DataRow row in dt.Rows){
                return row["VALOR"].ToString();
            }

            return 0;
        }

        internal Carro GetRegistroCarro(string ticket)
        {
        Carro dados = null;
        DataTable dt = new DataTable();
        try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select * from estacionamento where \"DS_TICKET\" = '" + ticket + "'";

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

            foreach (DataRow row in dt.Rows){
                dados = new Carro(){
                    dsTicket = row["DS_TICKET"].ToString(),
                    dtInicio = DateTime.Parse(row["DT_INICIO"].ToString()),
                    dtFim = String.IsNullOrEmpty(row["DT_FIM"].ToString()) ? (DateTime?)null : DateTime.Parse(row["DT_FIM"].ToString()),
                    setor = row["DS_SETOR"].ToString(),
                    placa = row["DS_PLACA"].ToString()
                };
            }

            return dados;
        }

        internal bool SetPagarTicket(string ticket, double valor){
            bool retorno = false;
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    pgsqlConnection.Open();

                    string cmdInserir = String.Format("UPDATE PUBLIC.TICKET SET \"SN_PAGO\" = 'S', \"DT_PAGAMENTO\" = TO_DATE('{0}','DD/MM/YYYY HH24:MI:SS'), \"NR_VALOR\" = '{1}' WHERE \"DS_TICKET\" = '{2}'",DateTime.Now,valor,ticket);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                    {
                        retorno = pgsqlcommand.ExecuteNonQuery() > 0 ? true : false;
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
            return retorno;
        }

        internal bool SetTicket(string ticket, double valor, string tabela)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    pgsqlConnection.Open();

                    string cmdInserir = String.Format("INSERT INTO public.ticket(\"DS_TICKET\", \"NR_VALOR\", \"DS_TABELA\") VALUES ('{0}','{1}','{2}')",ticket,valor,tabela);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                    {
                        return pgsqlcommand.ExecuteNonQuery() > 0 ? true : false;
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

        internal object SetEstacionar(string placa, string setor, string ticket)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    pgsqlConnection.Open();

                    string cmdInserir = String.Format("INSERT INTO public.estacionamento(\"DS_PLACA\", \"DT_INICIO\", \"DS_SETOR\", \"DS_TICKET\") VALUES ('{0}',TO_DATE('{1}','DD/MM/YYYY HH24:MI:SS'),'{2}','{3}')",placa,DateTime.Now,setor,ticket);
                    List<Vaga> vaga = GetVagas(setor);
                    if(vaga.Count > 0 && vaga[0].vagaDisponivel > 0){
                     using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                        {
                            return pgsqlcommand.ExecuteNonQuery() > 0 ? true : false;
                        }
                    } else {
                        return "Vaga Não Disponivel!!";
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

        public TabelaDePreco GetTabelaPreco(string tabela){
        TabelaDePreco dados = null;
        DataTable dt = new DataTable();
        try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string cmdSeleciona = "Select * from TABELA_DE_PRECO where \"DS_TABELA\" = '" + tabela + "'";

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

            foreach (DataRow row in dt.Rows){
                dados = new TabelaDePreco(){
                    DescTabela = row["DS_TABELA"].ToString(),
                    vlMinimo = Double.Parse(row["VALOR_MINIMO"].ToString()),
                    vlAdicional = Double.Parse(row["VALOR_ADICIONAL"].ToString())
                };
            }

            return dados;
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

        internal List<Vaga> GetVagas(string sigla)
        {
            List<Vaga> vagas = new List<Vaga>();
            DataTable dt = new DataTable();

            try
            {
                using (pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // abre a conexão com o PgSQL e define a instrução SQL
                    pgsqlConnection.Open();
                    string select = "SELECT * FROM public.vagas ";
                    string where = "where " + (string.IsNullOrEmpty(sigla) ? " 1=1 " : "\"DS_SETOR\" = '" + sigla + "'");
                    string order = " order by 1";
                    string cmdSeleciona = select + where + order;

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
