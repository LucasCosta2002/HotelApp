namespace AppMaquina.Client.Servicios
{
    public interface IHTTPServicio
    {
        Task<HTTPRespuesta<T>> Get<T>(string url);
        Task<HTTPRespuesta<object>> Post<T>(string url, T enviar);
    }
}