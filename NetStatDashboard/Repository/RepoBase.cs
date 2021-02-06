using NetStatDashboard.Models;
using NetStatDashboard.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetStatDashboard.Repository
{
    public static class RepoBase
    {
        public static MainViewModel Main { get; set; }
        public static IList<Host> GetHosts()
        {
            try
            {
                using (var txtReader = new StreamReader("data.txt"))
                {
                    var txt = txtReader.ReadToEnd();

                    if (string.IsNullOrWhiteSpace(txt))
                        return new List<Host>();

                    var data = JsonConvert.DeserializeObject<List<Host>>(txt);

                    if (data == null)
                        return new List<Host>();


                    return data;

                }
            }
            catch
            {
                return new List<Host>();
            }
        }

        public static bool SaveHosts(IList<Host> data)
        {
            try
            {
                using (var txtWriter = new StreamWriter("data.txt"))
                {
                    string jsData = "";
                    if (data == null || data.Count <= 0)
                        jsData = "";
                    else
                        jsData = JsonConvert.SerializeObject(data.Select(d => new
                        {
                            d.ByPing,
                            d.DelayPing,
                            d.ErrorStatusCodes,
                            d.HostName,
                            d.Id,
                            d.Name,
                            d.NotifyTime
                        }));

                    txtWriter.Write(jsData);
                    txtWriter.Flush();
                    txtWriter.Close();
                }

                Main.Hosts = data.ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
