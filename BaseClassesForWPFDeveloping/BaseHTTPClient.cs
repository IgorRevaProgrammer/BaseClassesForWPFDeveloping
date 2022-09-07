namespace BaseClassesForWPFDeveloping
{
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using System.Text;

    public abstract class ClientBase
    {
        private readonly string webAppAdress;
        private readonly string scheme;
        private readonly string token;
        protected StringBuilder actionAddressBuilder;

        public ClientBase(string WebAppAdress, string Scheme, string Token)
        {
            webAppAdress = WebAppAdress;
            scheme = Scheme;
            token = Token;
            actionAddressBuilder = new StringBuilder(webAppAdress);
        }

        protected HttpResponseMessage GetResponseFromHttp(HttpMethod method, object body)
        {
            HttpResponseMessage message;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(actionAddressBuilder.ToString())
                };
                actionAddressBuilder.Clear().Append(webAppAdress);
                if (body != null)
                {
                    string data = JsonConvert.SerializeObject(body);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                }
                message = client.SendAsync(request).Result;
            }
            return message;
        }
    }
}