module Fsharp.Giraffe.AzureCosmos.Models.AuthModels

open System

type User = { Username: string; Password: string }

type Token =
    { Value: string
      Expiry: DateTimeOffset }

type AuthResponse = { Token: Token; User: User }

type AuthError = { Message: string }

type AuthResult =
    | Success of AuthResponse
    | Error of AuthError

type AuthRequest =
    {

      Username: string
      Password: string }