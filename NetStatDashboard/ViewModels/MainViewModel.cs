using NetStatDashboard.Helpers;
using NetStatDashboard.Models;
using NetStatDashboard.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NetStatDashboard.ViewModels
{
    public sealed class MainViewModel : NotifiyPropertyChanged
    {


        public RelayCommand<Host> EditItemCommand { get; set; }
        public RelayCommand AddItemCommand { get; set; }
        public RelayCommand<Host> RemoveItemCommand { get; set; }
        public RelayCommand<Host> StopItemCommand { get; set; }

        public readonly HostRepository repository;
        static List<Host> _hosts;
        public MainViewModel()
        {
            repository = new HostRepository();
            Hosts = repository.GetAll().ToList();

            EditItemCommand = new RelayCommand<Host>(EditItem);
            AddItemCommand = new RelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand<Host>(RemoveItem);
            StopItemCommand = new RelayCommand<Host>(StopItem);

        }


        public void Start()
        {
            foreach (var item in Hosts.Where(d=> !d.IsStart))
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate (object state)
                    { StartPing(item); }), null);
        }

        void StartPing(Host host)
        {
            host.IsStop = false;
            while (!host.IsStop)
            {
                bool pingable = false;
                Ping pinger = null;
                host.IsStart = true;
                try
                {
                    pinger = new Ping();
                    PingReply reply = pinger.Send(host.HostName);
                    pingable = reply.Status == IPStatus.Success;




                }
                catch (PingException)
                {


                }
                finally
                {
                    if (host.LastStatusDate == null || host.Pingable != pingable)
                        host.LastStatusDate = DateTime.Now;

                    var lstup = (DateTime.Now - host.LastStatusDate.Value).TotalSeconds;
                    if (lstup<60)
                        host.LastUpdate = ((int)(DateTime.Now - host.LastStatusDate.Value).TotalSeconds).ToString() + " second(s) ago";
                    else if (lstup>60 && lstup<3600)
                        host.LastUpdate = ((int)(DateTime.Now - host.LastStatusDate.Value).TotalMinutes).ToString() + " minut(s) ago";
                    else if (lstup>3600 && lstup<86400)
                        host.LastUpdate = ((int)(DateTime.Now - host.LastStatusDate.Value).TotalHours).ToString() + " hour(s) ago";
                    else
                        host.LastUpdate = ((int)(DateTime.Now - host.LastStatusDate.Value).TotalDays).ToString() + " day(s) ago";


                    host.Pingable = pingable;

                    if (pinger != null)
                    {
                        pinger.Dispose();
                    }
                }

                Thread.Sleep(host.DelayPing * 1000);
            }

        }

        public List<Host> Hosts 
        {
            get { return _hosts; }
            set { SetProperty(ref _hosts, value); Start(); }
        }

        public void EditItem(Host model)
        {
            HostWindow hostWindow = new HostWindow();
            hostWindow.DataContext = new HostViewModel(model);
            hostWindow.Title = "Edit Host: "+model.Name;
            hostWindow.ResizeMode = ResizeMode.NoResize;
            hostWindow.ShowDialog();
        }

        public void AddItem()
        {
            HostWindow hostWindow = new HostWindow();
            hostWindow.DataContext = new HostViewModel(new Host());
            hostWindow.Title = "Add new Host";
            hostWindow.ResizeMode = ResizeMode.NoResize;
            hostWindow.ShowDialog();
        }

        public void RemoveItem(Host model)
        {
            if (MessageBox.Show("Are you sure to delete?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
                repository.Remove(model);
        }
        public void StopItem(Host model)
        {
            if (model.IsStart)
                model.IsStop = true;
            else
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate (object state)
                    { StartPing(model); }), null);

        }
    }
}
