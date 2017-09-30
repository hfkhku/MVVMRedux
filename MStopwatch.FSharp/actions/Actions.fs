namespace MStopwatch.Actions

open Redux
open System
open System.Collections.Generic
open System.Linq
open System.Text
open System.Threading.Tasks

type TimeFormatAction() = 
    interface IAction
    member val Format = "" with get, set

type ChangeModeAction() = 
    interface IAction

type TimerAction() = 
    interface IAction
    member val Now = DateTime() with get, set

type LapAction() = 
    interface IAction
