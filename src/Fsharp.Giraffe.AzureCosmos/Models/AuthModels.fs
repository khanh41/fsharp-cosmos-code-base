module Fsharp.Giraffe.AzureCosmos.Models.AuthModels

open System
open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type RegisterModel =
    { [<Key>]
      Username: string
      Password: string
      Email: string }

[<CLIMutable>]
type LoginModel = { Username: string; Password: string }
