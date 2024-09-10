module Fsharp.Giraffe.AzureCosmos.Models.MessageModels

[<CLIMutable>]
type MessageModel =
    {
        Text : string
    }
    
[<CLIMutable>]
type ErrorMessageModel =
    {
        Error: string
    }