module Fsharp.Giraffe.AzureCosmos.Handlers.ManagerAdmin

open System
open Fsharp.Giraffe.AzureCosmos.Models
open AuthModels
open MessageModels
open Microsoft.AspNetCore.Http
open Giraffe
open Fsharp.Giraffe.AzureCosmos.Service.AuthService

let authService = AuthService()

let validateUser (info_user: LoginModel) =
    // Replace this with actual user validation logic
    if info_user.Username = "testuser" && info_user.Password = "testpassword" then
        Some(authService.generateToken info_user.Username)
    else
        None 

// Define the authentication handler
let handleLogin =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! form = ctx.Request.ReadFormAsync()

            let info_user = {
                Username = form.["username"].ToString()
                Password = form.["password"].ToString()
            }

            match validateUser(info_user) with
            | Some token ->
                return! json { Token = token } next ctx
            | None ->
                ctx.Response.StatusCode <- 401
                ctx.WriteJsonAsync { Error = "Incorrect account or password."} |> ignore
                return! next ctx
    }

let verifyToken =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            try
                // Read form data synchronously
                let authHeader = ctx.Request.Headers.["Authorization"].ToString()

                // Check if the header contains the Bearer token
                if String.IsNullOrEmpty(authHeader) || not (authHeader.StartsWith("Bearer ")) then
                    ctx.Response.StatusCode <- 400
                    ctx.WriteJsonAsync { Error = "Authorization header is missing or not valid" } |> ignore
                    return! next ctx
                else
                    // Extract the token from the 'Authorization' header
                    let token = authHeader.Substring("Bearer ".Length).Trim()
                    
                    match authService.verifyToken token with
                    | None ->
                        ctx.Response.StatusCode <- 401
                        ctx.WriteJsonAsync { Error = "Unauthorized access." } |> ignore
                        return! next ctx
                    | Some(principal) ->
                        ctx.User <- principal
                        return! next ctx
            with
            | ex ->
                // Return an error response if token validation fails
                ctx.Response.StatusCode <- 401
                ctx.WriteJsonAsync { Error = sprintf "Token validation failed: %s" ex.Message } |> ignore
                return! next ctx
    }