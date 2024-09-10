module Fsharp.Giraffe.AzureCosmos.Models.AuthModels

open System
open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type RegisterModel =
    // Register User Model
    { [<Key>]
      Username: string
      Password: string
      Email: string
    }
    
    
[<CLIMutable>]
type LoginModel =
    // Login User Model
    {
        Username: string
        Password: string
    }

[<CLIMutable>]
type LoginSuccessfulModel =
    // Login Successful Model
    {
        Token: string
    }
