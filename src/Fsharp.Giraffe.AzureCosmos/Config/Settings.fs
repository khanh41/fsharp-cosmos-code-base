module Fsharp.Giraffe.AzureCosmos.Config.Settings

type CosmosDbSettings =
    { Endpoint: string
      Key: string
      DatabaseName: string
      ContainerName: string }

type JwtSettings =
    { SecretKey: string
      Issuer: string
      Audience: string
      ExpiryMinutes: int }
