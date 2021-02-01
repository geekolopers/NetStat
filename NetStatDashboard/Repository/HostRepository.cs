using NetStatDashboard.Models;
using NetStatDashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetStatDashboard.Repository
{
    public class HostRepository
    {
   
        public (bool,string) AddNew(Host host)
        {
            try
            {
                var lsData = RepoBase.GetHosts();

                if (lsData.Any(d => d.Name.ToLower() == host.Name.ToLower()
                             || d.HostName.ToLower() == host.HostName.ToLower()))
                    return (false,"Host already exists");

                if (lsData.Count > 0)
                    host.Id = lsData.Max(s => s.Id) + 1;
                else
                    host.Id = 1;

                lsData.Add(host);
                RepoBase.SaveHosts(lsData);

                return (true, "Successful");

            }
            catch(Exception ex)
            {
                return (false,ex.Message);
            }
        }

        public (bool, string) Edit(Host host)
        {
            try
            {
                var lsData = RepoBase.GetHosts();

                if (!lsData.Any(d => d.Id==host.Id))
                    return (false, "Host does not exist");

                lsData.Remove(lsData.FirstOrDefault(d => d.Id == host.Id));
                lsData.Add(host);
                RepoBase.SaveHosts(lsData);

                return (true, "Successful");

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string) Remove(Host host)
        {
            try
            {
                var lsData = RepoBase.GetHosts();

                if (!lsData.Any(d => d.Id == host.Id))
                    return (false, "Host does not exist");

                lsData.Remove(lsData.FirstOrDefault(d => d.Id == host.Id));
                RepoBase.SaveHosts(lsData);

                return (true, "Successful");

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public IList<Host> GetAll()
        {
            return RepoBase.GetHosts();
        }

    }
}
