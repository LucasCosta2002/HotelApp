using System.Text;
using System.Text.Json;

namespace AppMaquina.Client.Servicios
{
    public class HTTPServicio : IHTTPServicio
    {
        private readonly HttpClient http;

        public HTTPServicio(HttpClient http)
        {
            this.http = http;
        }

        public async Task<HTTPRespuesta<T>> Get<T>(string url)
        {
            var response = await http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var respuesta = await Deserealizar<T>(response);
                return new HTTPRespuesta<T>(respuesta, false, response);
            }
            else
            {
                return new HTTPRespuesta<T>(default, true, response);
            }
        }

        public async Task<HTTPRespuesta <object>> Post<T>(string url, T enviar)
        {
            var enviarJson = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJson, Encoding.UTF8, "application/json");
            var respuesta = await http.PostAsync(url, enviarContent);

            return new HTTPRespuesta<object>(null, !respuesta.IsSuccessStatusCode, respuesta);
        }

        private async Task<T?> Deserealizar<T>(HttpResponseMessage response)
        {
            var respuestaString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(respuestaString, new JsonSerializerOptions() { PropertyNameCaseInsensitive= true });
        }
    }
}
