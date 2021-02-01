using NetStatDashboard.Helpers;
using NetStatDashboard.Interfaces;
using NetStatDashboard.Models;
using NetStatDashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetStatDashboard.ViewModels
{
    public sealed class HostViewModel : NotifiyPropertyChanged, ICloseable
    {
        public event EventHandler<EventArgs> RequestClose;
        Host _host;
        public RelayCommand SaveCommand { get; set; }
        public readonly HostRepository repository;

        public HostViewModel(Host model = null)
        {
            repository = new HostRepository();
            Model = model;
            SaveCommand = new RelayCommand(Save);
        }


        public Host Model
        {
            get { return _host; }
            set { SetProperty(ref _host, value); }
        }


        public void Save()
        {
            dynamic res;

            if (Model.Id > 0)
                res = repository.Edit(Model);
            else
                res = repository.AddNew(Model);

            if (res.Item1)
            {
                if (MessageBox.Show("Successful, do you want to add an other host?", "Info"
                     , MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Model = new Host();
                }
                else
                {
                    RequestClose(this, EventArgs.Empty);
                }
            }
            else
            {
                if (MessageBox.Show(res.Item2+", do you want to try again?", "Error"
                        , MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {

                }
                else
                {
                    RequestClose(this, EventArgs.Empty);
                }
            }

        }
    }
}
