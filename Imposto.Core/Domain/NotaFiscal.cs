using Imposto.Core.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Imposto.Core.Domain
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public int NumeroNotaFiscal { get; set; }
        public int Serie { get; set; }
        public string NomeCliente { get; set; }

        public string EstadoDestino { get; set; }
        public string EstadoOrigem { get; set; }

        public List<NotaFiscalItem> ItensDaNotaFiscal { get; set; }

        public NotaFiscal()
        {
            ItensDaNotaFiscal = new List<NotaFiscalItem>();
        }

        public void EmitirNotaFiscal(Pedido pedido)
        {
            NotaFiscal nf = new NotaFiscal();

            nf.NumeroNotaFiscal = 99999;
            nf.Serie = new Random().Next(int.MaxValue);
            nf.NomeCliente = pedido.NomeCliente;

            nf.EstadoDestino = pedido.EstadoOrigem;
            nf.EstadoOrigem = pedido.EstadoDestino;

            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                NotaFiscalItem notaFiscalItem = new NotaFiscalItem();

                if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "RJ"))
                {
                    notaFiscalItem.Cfop = "6.000";                    
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "PE"))
                {
                    notaFiscalItem.Cfop = "6.001";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "MG"))
                {
                    notaFiscalItem.Cfop = "6.002";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "PB"))
                {
                    notaFiscalItem.Cfop = "6.003";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "PR"))
                {
                    notaFiscalItem.Cfop = "6.004";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "PI"))
                {
                    notaFiscalItem.Cfop = "6.005";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "RO"))
                {
                    notaFiscalItem.Cfop = "6.006";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "SE"))
                {
                    notaFiscalItem.Cfop = "6.007";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "TO"))
                {
                    notaFiscalItem.Cfop = "6.008";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "SE"))
                {
                    notaFiscalItem.Cfop = "6.009";
                }
                else if ((pedido.EstadoOrigem == "SP") && (pedido.EstadoDestino == "PA"))
                {
                    notaFiscalItem.Cfop = "6.010";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "RJ"))
                {
                    notaFiscalItem.Cfop = "6.000";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "PE"))
                {
                    notaFiscalItem.Cfop = "6.001";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "MG"))
                {
                    notaFiscalItem.Cfop = "6.002";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "PB"))
                {
                    notaFiscalItem.Cfop = "6.003";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "PR"))
                {
                    notaFiscalItem.Cfop = "6.004";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "PI"))
                {
                    notaFiscalItem.Cfop = "6.005";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "RO"))
                {
                    notaFiscalItem.Cfop = "6.006";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "SE"))
                {
                    notaFiscalItem.Cfop = "6.007";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "TO"))
                {
                    notaFiscalItem.Cfop = "6.008";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "SE"))
                {
                    notaFiscalItem.Cfop = "6.009";
                }
                else if ((pedido.EstadoOrigem == "MG") && (pedido.EstadoDestino == "PA"))
                {
                    notaFiscalItem.Cfop = "6.010";
                }

                if (pedido.EstadoDestino == pedido.EstadoOrigem)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                }
                else
                {
                    notaFiscalItem.TipoIcms = "10";
                    notaFiscalItem.AliquotaIcms = 0.17;
                }
                
                if (notaFiscalItem.Cfop == "6.009")
                {
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido*0.90; //redução de base
                }
                else
                {
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido;
                }

                if (string.IsNullOrEmpty(notaFiscalItem.Cfop))
                    notaFiscalItem.Cfop = "0.000";

                notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms*notaFiscalItem.AliquotaIcms;

                notaFiscalItem.BaseIPI = itemPedido.ValorItemPedido;

                if (itemPedido.Brinde)
                {
                    notaFiscalItem.AliquotaIPI = 0;
                }
                else
                {
                    notaFiscalItem.AliquotaIPI = 10;
                }

                notaFiscalItem.ValorIPI = (itemPedido.ValorItemPedido * (notaFiscalItem.AliquotaIPI / 100));

                if (pedido.EstadoDestino.Equals("SP"))
                {
                    notaFiscalItem.Desconto = 10;
                    itemPedido.ValorItemPedido = (itemPedido.ValorItemPedido - (itemPedido.ValorItemPedido * (notaFiscalItem.Desconto / 100)));
                }

                if (itemPedido.Brinde)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                    notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;
                }
                
                notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
                notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;
                
                nf.ItensDaNotaFiscal.Add(notaFiscalItem);
            }

            GenerateXML(nf);
        }

        private void GenerateXML(NotaFiscal nf)
        {
            NotaFiscalRepository repository = new NotaFiscalRepository();

            string path = ConfigurationManager.AppSettings["PathXMLFile"];
            string guid = Guid.NewGuid().ToString();
            string fileName = path + guid + ".xml";

            XmlSerializer serializer = new XmlSerializer(typeof(NotaFiscal));
            serializer.Serialize(File.Create(fileName), nf);

            if (File.Exists(fileName))
            {
                int idNf = repository.AdicionarNotaFiscal(nf);

                if (idNf > 0)
                {
                    foreach (NotaFiscalItem item in nf.ItensDaNotaFiscal)
                    {
                        repository.AdicionarNotaFiscalItem(item, idNf);
                    }
                }
            }
        }
    }
}
