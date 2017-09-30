namespace MStopwatch

open Redux
open MStopwatch.Actions
open MStopwatch.Models
open MStopwatch.States
open System
open System.Linq
open System.Collections.ObjectModel

[<SealedAttribute>]
type Reducers = 
    
    static member StopwatchReducer(previousState : ApplicationState, action : IAction) : ApplicationState = 
        match action with
        | :? TimeFormatAction as act -> Reducers.TimeFormatReducer(previousState, act)
        | :? ChangeModeAction as act -> Reducers.ChangeModeReducer(previousState, act)
        | :? TimerAction as act -> Reducers.TimerReducer(previousState, act)
        | :? LapAction as act -> Reducers.LapReducer(previousState, act)
        | _ -> previousState
    
    static member private LapReducer(previousState : ApplicationState, action : LapAction) : ApplicationState = 
        let timerScheduler = previousState.TimerScheduler
        let lapTimeList = previousState.LapTimeList
        
        let prevLap = 
            if lapTimeList.Any() then lapTimeList.Last().Time
            else previousState.StartTime
        { Time = previousState.Now
          Span = timerScheduler.Now.DateTime.ToLocalTime() - prevLap }
        |> lapTimeList.Add
        let max = lapTimeList.Max(fun s -> s.Span)
        let min = lapTimeList.Min(fun s -> s.Span)
        { previousState with LapTimeList = lapTimeList
                             MaxLapTime = max
                             MinLapTime = min }
    
    static member private TimerReducer(previousState : ApplicationState, action : TimerAction) : ApplicationState = 
        { previousState with NowSpan = action.Now - previousState.StartTime
                             Now = action.Now }
    
    static member private ChangeModeReducer(previousState : ApplicationState, action : ChangeModeAction) : ApplicationState = 
        let timerScheduler = previousState.TimerScheduler
        let lapTimeList = previousState.LapTimeList
        match previousState.Mode with
        | StopwatchMode.Init -> 
            { previousState with Mode = StopwatchMode.Start
                                 ButtonLabel = Constants.StopLabel
                                 StartTime = timerScheduler.Now.DateTime.ToLocalTime() }
        | StopwatchMode.Start -> 
            let prevLap = 
                if lapTimeList.Any() then lapTimeList.Last().Time
                else previousState.StartTime
            { Time = previousState.Now
              Span = timerScheduler.Now.DateTime.ToLocalTime() - prevLap }
            |> lapTimeList.Add
            { previousState with Mode = StopwatchMode.Stop
                                 ButtonLabel = Constants.ResetLabel
                                 MaxLapTime = lapTimeList.Max(fun s -> s.Span)
                                 MinLapTime = lapTimeList.Min(fun s -> s.Span)
                                 LapTimeList = lapTimeList }
        | StopwatchMode.Stop -> 
            lapTimeList.Clear()
            { previousState with Mode = StopwatchMode.Init
                                 ButtonLabel = Constants.StartLabel
                                 StartTime = timerScheduler.Now.DateTime.ToLocalTime()
                                 NowSpan = TimeSpan.Zero
                                 LapTimeList = lapTimeList
                                 MaxLapTime = TimeSpan.Zero
                                 MinLapTime = TimeSpan.Zero }
        | _ -> raise (InvalidOperationException())
    
    static member private TimeFormatReducer(previousState : ApplicationState, action : TimeFormatAction) : ApplicationState = 
        { previousState with DisplayFormat = action.Format }
    static member ReduceApplication(previousState : ApplicationState, action : IAction) : ApplicationState = 
        Reducers.StopwatchReducer(previousState, action)
    private new() = Reducers()
