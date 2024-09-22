using Geo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Service.Access
{
    public abstract class OFSCAccess
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _instance;
        private readonly byte[] _authorization;

        public OFSCAccess(string instance, string clientId, string clientSecret)
        {
            this._instance = instance;
            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._authorization = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
        }

        public async Task UpdateActivity(int activityId, string content)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Patch, String.Format("https://{0}/rest/ofscCore/v1/activities/{1}", _instance, activityId));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(_authorization));
            request.Content = new StringContent(content, null, "application/json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
