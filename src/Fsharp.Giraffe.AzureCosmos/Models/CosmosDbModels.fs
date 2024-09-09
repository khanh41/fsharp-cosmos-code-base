module Fsharp.Giraffe.AzureCosmos.Models.CosmosDbModels

open System

type WeatherForecast =
    { Date: DateTime
      TemperatureC: int
      Summary: string }
