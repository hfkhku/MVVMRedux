namespace MStopwatch.States

open System
open System.Reactive.Concurrency
open MStopwatch.Models
open System.Collections.ObjectModel

type ApplicationState = 
    { TimerScheduler : IScheduler
      DisplayFormat : string
      NowSpan : TimeSpan
      Mode : StopwatchMode
      ButtonLabel : string
      StartTime : DateTime
      Now : DateTime
      LapTimeList : ObservableCollection<LapTime>
      MaxLapTime : TimeSpan
      MinLapTime : TimeSpan }
