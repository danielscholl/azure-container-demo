using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("ACR WebHook Posted.");

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // REPLACE WITH YOUR HOOK
    string _slackhook = "<your_slack_hook";

    Uri _uri = new Uri(_slackhook);

    string _message = data?.ToString() ?? "test from azure function";
    string _channel = "#general";
    string _username = "webhookbot";

    HttpClient _httpClient = new HttpClient();
    var payload = new
    {
        text = _message,
        _channel,
        _username
    };
    var serializedPayload = JsonConvert.SerializeObject(payload);
    var response = _httpClient.PostAsync(_uri,
        new StringContent(serializedPayload, Encoding.UTF8, "application/json")).Result;


    return _message == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, response.ToString())
        : req.CreateResponse(HttpStatusCode.OK, String.Format("Message: {0}, Posted to: {1}", _message, _channel));
}
