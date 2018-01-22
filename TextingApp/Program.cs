using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TextingApp
{
    public class Message
    {
        public string To { get; set; }
        public string From { get; set; }
		public string Body { get; set; }
		public string Status { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.twilio.com/2010-04-01");
			//1
			var request = new RestRequest("Accounts/ACa2cf9565f661f8e1ba285ad71bf95617/Messages.json", Method.GET);
			client.Authenticator = new HttpBasicAuthenticator("ACa2cf9565f661f8e1ba285ad71bf95617", "552015e2fc09d2bfcb86ec93e12b463b");
			//2
			var response = new RestResponse();
			//3a
			Task.Run(async () =>
			{
				response = await GetResponseContentAsync(client, request) as RestResponse;
			}).Wait();
			//4
			JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<Message> messagelist = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            foreach (Message message in messageList)
            {
				Console.WriteLine("To: {0}, message.To");
				Console.WriteLine("From: {0}, message.From");
				Console.WriteLine("Body: {0}, message.Body");
				Console.WriteLine("Status: {0}, message.Status");
            }
			Console.ReadLine();
		}

		//3b
		public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
		{
			var tcs = new TaskCompletionSource<IRestResponse>();
			theClient.ExecuteAsync(theRequest, response => {
				tcs.SetResult(response);
			});
			return tcs.Task;
        }
    }
}
