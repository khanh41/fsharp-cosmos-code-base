module Fsharp.Giraffe.AzureCosmos.Handlers.HelloHandlers

open Microsoft.AspNetCore.Http
open Giraffe
open Fsharp.Giraffe.AzureCosmos.Models.MessageModels

let handleGetHello =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let response = { Text = "Hello world, from Giraffe!" }
            return! json response next ctx
        }
