using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Service;

namespace ViewModel
{
    public class Page1: INotifyPropertyChanged
    {
        public static Page1? Instance { get; private set; }

        IRhinoService _rhinoService;
        public Page1(IRhinoService rhinoService)
        {
            Instance = this;
            _rhinoService = rhinoService;
            CreateCommand = new RelayCommand(() => _rhinoService.Create());
            HideCommand = new RelayCommand(()=> _rhinoService.Hide());
            SetViewCommand = new RelayCommand(() => _rhinoService.SetView());
        }

        public ICommand CreateCommand { get; }
        public ICommand HideCommand { get; }
        public ICommand SetViewCommand { get; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
