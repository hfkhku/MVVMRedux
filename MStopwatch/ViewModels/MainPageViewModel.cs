using MStopwatch.Actions;
using MStopwatch.Models;
using MStopwatch.States;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace MStopwatch.ViewModels
{
    class MainPageViewModel:BindableBase, INavigationAware
    {
        private string startButtonLabel;
        public string StartButtonLabel
        {
            get { return startButtonLabel; }
            set { SetProperty(ref startButtonLabel, value); }
        }

        private string nowSpan;
        public string NowSpan
        {
            get { return nowSpan; }
            set { SetProperty(ref nowSpan, value); }
        }

        private bool isShowed;
        public bool IsShowed
        {
            get { return isShowed; }
            set {
                if(SetProperty(ref isShowed, value))
                {
                    if (value)
                    {
                        StateStore.Store.Dispatch(new TimeFormatAction() { Format = Constants.TimeSpanFormat });
                    }else
                    {
                        StateStore.Store.Dispatch(new TimeFormatAction() { Format = Constants.TimeSpanFormatNoMillsecond });
                    }
                }
            }
        }

        private DelegateCommand lapCommand;
        public DelegateCommand LapCommand =>
            lapCommand ?? (lapCommand = new DelegateCommand(Lap, CanLap));

        private bool CanLap()
        {
            return StateStore.Store.GetState().Mode == StopwatchMode.Start;
        }

        private void Lap()
        {
            StateStore.Store.Dispatch(new LapAction());
        }

        public ObservableCollection<LapTime> Items { get; } = new ObservableCollection<LapTime>();

        private IDisposable TimerSubscription { get; set; }

        private DelegateCommand startCommand;
        public DelegateCommand StartCommand =>
            startCommand ?? (startCommand = new DelegateCommand(Start));

        private void Start()
        {
            var mode = StateStore.Store.GetState().Mode;
            var scheduler = StateStore.Store.GetState().TimerScheduler;
            if (mode == StopwatchMode.Init)
            {
                TimerSubscription = Observable.Interval(TimeSpan.FromMilliseconds(10), scheduler)
                .Subscribe(_ =>
                {
                    StateStore.Store.Dispatch(new TimerAction() { Now = scheduler.Now.DateTime.ToLocalTime() });
                });
            }
            else if (mode == StopwatchMode.Start)
            {
                TimerSubscription.Dispose();
                TimerSubscription = null;
            }

            StateStore.Store.Dispatch(new ChangeModeAction());
        }

        IRegionManager RegionManager;

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }

        public MainPageViewModel(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;

            ConfirmationRequest = new InteractionRequest<IConfirmation>();

            StateStore.Store.ObserveOnDispatcher().Subscribe(state =>
            {
                StartButtonLabel = state.ButtonLabel;
                NowSpan = state.NowSpan.ToString(state.DisplayFormat);
                LapCommand.RaiseCanExecuteChanged();
                Items.Clear();
                Items.AddRange(state.LapTimeList);
                if (state.Mode == StopwatchMode.Stop)
                {
                    var nowSpan = state.NowSpan;
                    var maxLapTime = state.MaxLapTime;
                    var minLapTime = state.MinLapTime;

                    ConfirmationRequest.Raise(new Confirmation
                    {
                        Title = "Confirmation",
                        Content = $"All time: {nowSpan.ToString(state.DisplayFormat)}\r\nMax laptime: {maxLapTime.TotalMilliseconds} ms\nMin laptime: { minLapTime.TotalMilliseconds}ms\n\nShow all lap result?"
                    }, r => {
                        if (r.Confirmed)
                        {
                            RegionManager.RequestNavigate("ContentRegion", "ResultPageView");
                        }
                    });
                }
            });
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        void RaiseConfirmation(string content)
        {
            
        }
    }
}
