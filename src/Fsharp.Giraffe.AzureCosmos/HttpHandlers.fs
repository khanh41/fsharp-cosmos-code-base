namespace Fsharp.Giraffe.AzureCosmos

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open Giraffe
    open Fsharp.Giraffe.AzureCosmos.Models

    let handleGetHello =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let response = {
                    Text = "Hello world, from Giraffe!"
                }
                return! json response next ctx
            }