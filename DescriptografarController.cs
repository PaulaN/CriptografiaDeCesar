
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Http;
using System.Net.Http;

namespace WebApplication1.Controllers
{
    public class DescriptografarController : ApiController
    {
        [HttpGet]
        [Route("Descriptografar/DescriptografaMensagem")]
        public string DescriptografaMensagem()
        {
        
           
            dynamic jsonCodigo = @"{ ""numero_casas"" : 12, ""token"" : ""c68477365fea27a9ce829abf96b232e86d49aced"", ""cifrado"": ""mxx oaybgfqde imuf mf ftq emyq ebqqp. gzwzaiz"" , ""decifrado"": """",""resumo_criptografico"": """"}";
            jsonCodigo = jsonCodigo.ToLower();
           
            dynamic json = JsonConvert.DeserializeObject<dynamic>(jsonCodigo);
            string cifrado = json.cifrado;

            cifrado = Convert.ToString(cifrado);
            cifrado = cifrado.Replace(".", "1");
            cifrado = cifrado.Replace(" ", "0");

            char[] myChar = cifrado.ToCharArray();

            Char[] Alfabeto = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            Char[] novoArray = new Char[myChar.Length];

            int numeroCasas = json.numero_casas;
            int tamanhoAlfabeto = Alfabeto.Length;

            //Decifra código
            for (int i = 0; i < myChar.Length; i++)
            {

                if (myChar[i] != '0' && myChar[i] != '1')
                {
                    int posicao = Array.BinarySearch(Alfabeto, myChar[i]);
                    int total = posicao - numeroCasas;

                    if (total >= 0 && posicao >= numeroCasas)
                    {
                        char novoChar = Alfabeto[posicao - numeroCasas];
                        novoArray[i] = novoChar;
                    }
                    else if (total < 0)
                    {
                        int result = numeroCasas - posicao;
                        int res = tamanhoAlfabeto - result;

                        char novaChar = Alfabeto[res];
                        novoArray[i] = novaChar;

                    }

                }
                else if (myChar[i] == '0')
                {
                    novoArray[i] = ' ';
                }
                else if (myChar[i] == '1')
                {
                    novoArray[i] = '.';
                }


            }

            string decifrado = new string(novoArray);
            json.decifrado = decifrado;

            //Criptografa com SHA1
            Encoding enc = Encoding.Unicode;
            byte[] buffer = enc.GetBytes(decifrado);
            System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            json.resumo_criptografico = hash;
            string jsonCompleto = JsonConvert.SerializeObject(json);


            //declarando a variavel do tipo StreamWriter para 
            //abrir ou criar um arquivo para escrita 
            StreamWriter x;

            //Colocando o caminho fisico e o nome do arquivo a ser criado
            //finalizando com .txt
            string CaminhoNome = "C:\\Users\\Public\\answer.json";

            //utilizando o método para criar um arquivo texto
            //e associando o caminho e nome ao metodo
            x = System.IO.File.CreateText(CaminhoNome);

            x.WriteLine(jsonCompleto);
            x.Close();


            return jsonCompleto;

        }


      


    }
}
