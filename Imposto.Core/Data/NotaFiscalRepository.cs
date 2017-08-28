using Imposto.Core.Domain;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Imposto.Core.Data
{
    public class NotaFiscalRepository
    {
        public int AdicionarNotaFiscal(NotaFiscal nf)
        {
            string strConn = ConfigurationManager.ConnectionStrings["NotaFiscalConnectionString"].ToString();
            int newIdNf = 0;

            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("P_NOTA_FISCAL", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pId", 0).Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.AddWithValue("@pNumeroNotaFiscal", nf.NumeroNotaFiscal);
                    cmd.Parameters.AddWithValue("@pSerie", nf.Serie);
                    cmd.Parameters.AddWithValue("@pNomeCliente", nf.NomeCliente);
                    cmd.Parameters.AddWithValue("@pEstadoDestino", nf.EstadoDestino.ToUpper());
                    cmd.Parameters.AddWithValue("@pEstadoOrigem", nf.EstadoOrigem.ToUpper());

                    con.Open();
                    cmd.ExecuteNonQuery();

                    if (cmd.Parameters["@pId"].Value != null)
                        newIdNf = Convert.ToInt32(cmd.Parameters["@pId"].Value);
                }
            }

            return newIdNf;
        }

        public void AdicionarNotaFiscalItem(NotaFiscalItem item, int idNf)
        {
            string strConn = ConfigurationManager.ConnectionStrings["NotaFiscalConnectionString"].ToString();

            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("P_NOTA_FISCAL_ITEM", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pId", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@pIdNotaFiscal", SqlDbType.Int).Value = idNf;
                    cmd.Parameters.Add("@pCfop", SqlDbType.VarChar).Value = item.Cfop;
                    cmd.Parameters.Add("@pTipoIcms", SqlDbType.VarChar).Value = item.TipoIcms;
                    cmd.Parameters.Add("@pBaseIcms", SqlDbType.Decimal, 18).Value = item.BaseIcms;
                    cmd.Parameters.Add("@pAliquotaIcms", SqlDbType.Decimal, 18).Value = item.AliquotaIcms;
                    cmd.Parameters.Add("@pValorIcms", SqlDbType.Decimal, 18).Value = item.ValorIcms;
                    cmd.Parameters.Add("@pNomeProduto", SqlDbType.VarChar).Value = item.NomeProduto;
                    cmd.Parameters.Add("@pCodigoProduto", SqlDbType.VarChar).Value = item.CodigoProduto;
                    cmd.Parameters.Add("@pBaseIPI", SqlDbType.Decimal, 18).Value = item.BaseIPI;
                    cmd.Parameters.Add("@pAliquotaIPI", SqlDbType.Decimal, 18).Value = item.AliquotaIPI;
                    cmd.Parameters.Add("@pValorIPI", SqlDbType.Decimal, 18).Value = item.ValorIPI;
                    cmd.Parameters.Add("@pDesconto", SqlDbType.Decimal, 18).Value = item.Desconto;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
