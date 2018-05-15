﻿using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace EarTrumpet.ViewModels
{
    public class FullWindowViewModel : BindableBase
    {
        public event EventHandler<AppExpandedEventArgs> AppExpanded = delegate { };
        public event EventHandler<object> AppCollapsed = delegate { };

        public ObservableCollection<DeviceViewModel> AllDevices => _mainViewModel.AllDevices;
        public bool IsShowingModalDialog { get; private set; }

        MainViewModel _mainViewModel;

        public FullWindowViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _mainViewModel.OnFullWindowOpened();
        }

        public void Close()
        {
            OnAppCollapsed();
            _mainViewModel.OnFullWindowClosed();
        }

        public void OnAppExpanded(AppItemViewModel vm, UIElement container)
        {
            if (IsShowingModalDialog)
            {
                OnAppCollapsed();
            }

            AppExpanded?.Invoke(this, new AppExpandedEventArgs { Container = container, ViewModel = vm });

            IsShowingModalDialog = true;
            RaisePropertyChanged(nameof(IsShowingModalDialog));
        }

        public void OnAppCollapsed()
        {
            if (IsShowingModalDialog)
            {
                AppCollapsed?.Invoke(this, null);
                IsShowingModalDialog = false;
                RaisePropertyChanged(nameof(IsShowingModalDialog));
            }
        }
    }
}