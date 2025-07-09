using System.IO;
using System.Text.Json;

namespace sunmoon.utils
{
    /// <summary>
    /// Fornece uma coleção de funções para a manipulação de arquivos 
    /// json e dados relacionados
    /// </summary>
    public static class JsonUtils
    {
        /// <summary>
        /// Deserializa o arquivo json
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a ser retornado</typeparam>
        /// <param name="path">Caminho do arquivo json a ser carregado</param>
        /// <returns>Objeto do arquivo json selecionado</returns>
        public static T LoadJson<T>(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<T>(json);
            }
        }
    }
}