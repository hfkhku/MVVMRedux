using MStopwatch.Models;
using MStopwatch.States;
using Redux;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;

namespace MStopwatch
{
    static class StateStore
    {
        public static IStore<ApplicationState> Store { get; private set; }

        private static IScheduler scheduler = Scheduler.Default;

        static StateStore()
        {
            var initialState = new ApplicationState(
                timerScheduler: scheduler,
                displayFormat: Constants.TimeSpanFormatNoMillsecond,
                nowSpan: TimeSpan.Zero,
                mode: StopwatchMode.Init,
                buttonLabel: Constants.StartLabel,
                startTime: new DateTime(),
                now: new DateTime(),
                lapTimeList: new ObservableCollection<LapTime>(),
                maxLapTime: TimeSpan.Zero,
                minLapTime: TimeSpan.Zero
                );

            Store = new Store<ApplicationState>(Reducers.ReduceApplication, initialState);
        }
    }
}
