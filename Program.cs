using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.AudioBypassService;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using Newtonsoft.Json;

namespace ConsoleApp45
{
    class Program
    {
        private static VkApi apiVK;
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAudioBypass();
            apiVK = new VkApi(serviceCollection);
            var login = /*Console.ReadLine() */"380662601201";
            var password = /*Console.ReadLine()"*/"StirlitzPartizan";
            apiVK.SetLanguage(VkNet.Enums.Language.Ru);
            apiVK.Authorize(new ApiAuthParams { Login = login, Password = password, });
            //var mess = apiVK.Messages.Search(new MessagesSearchParams { Query = "Ира", Date = DateTime.Now, Count = 1});
            //string res = GET($"https://api.vk.com/method/messages.search?q=Ира&access_token={apiVK.Token}&v=5.101");
            var aud = apiVK.Audio.Get(new AudioGetParams { OwnerId = apiVK.UserId, Count = 25, Offset = 5});
            //string toke = apiVK.Token;
            //Console.WriteLine(toke);
            //string aud = GET($"https://api.vk.com/method/audio.get?owner_id=249669816&count=10&access_token={apiVK.Token}&v=5.101");
            //
            //RootObject response = JsonConvert.DeserializeObject<RootObject>(res);


            //

            WebClient wc = new WebClient();
            foreach (var track in aud)
            {
                if (track.Url.ToString().Contains("psv4"))
                    wc.DownloadFile(ConvertURL(track.Url), AppDomain.CurrentDomain.BaseDirectory + "890.mp3");
            }
            
            
        }
        private static string GET(string Url)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }
        private static Uri ConvertURL(Uri wrongURL)
        {
            if (wrongURL != null)
            {
                List<string> mass = wrongURL.AbsolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (!wrongURL.OriginalString.Contains("psv4.vkuseraudio.net"))
                    return new Uri(wrongURL.OriginalString.Replace(wrongURL.Query, null).Replace(wrongURL.AbsolutePath, null) + $"/{mass[0]}/{mass[2]}.mp3");
                else
                    return new Uri(wrongURL.OriginalString.Replace(wrongURL.Query, null).Replace(wrongURL.AbsolutePath, null) + $"/{mass[0]}/{mass[1]}/{mass[3]}/{mass[4]}.mp3");
            }
            else return null;
        }
    }
    public class Item
    {
        public int date { get; set; }
        public int from_id { get; set; }
        public int id { get; set; }
        public int @out { get; set; }
        public int peer_id { get; set; }
        public string text { get; set; }
        public int conversation_message_id { get; set; }
        public List<object> fwd_messages { get; set; }
        public bool important { get; set; }
        public int random_id { get; set; }
        public List<object> attachments { get; set; }
        public bool is_hidden { get; set; }
        public int? update_time { get; set; }
    }

    public class Response
    {
        public int count { get; set; }
        public List<Item> items { get; set; }
    }

    public class RootObject
    {
        public Response response { get; set; }
    }
}
