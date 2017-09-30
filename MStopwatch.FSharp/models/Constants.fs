namespace MStopwatch.Models

[<SealedAttribute>]
type Constants = 
    static member TimeSpanFormat = @"hh\:mm\:ss\""fff"
    static member TimeSpanFormatNoMillsecond = @"hh\:mm\:ss\"""
    static member DateTimeFormat = "yyyy/MM/dd HH:mm:ss"
    static member StartLabel = "Start"
    static member StopLabel = "Stop"
    static member ResetLabel = "Reset"
    private new() = Constants()
