module Fsharp.Giraffe.AzureCosmos.Repositories.CosmosDbRepository

open System
open Microsoft.Azure.Cosmos
open Fsharp.Giraffe.AzureCosmos.Models.CosmosDbModels

let getDocumentById (client: CosmosClient) (databaseName: string) (containerName: string) (id: string) =
    // Implementation to retrieve a document by ID from Cosmos DB
    // For now, we will just return a dummy document
    let rng = System.Random()

    { Date = DateTime.Now.AddDays(rng.Next(-14, 14))
      TemperatureC = rng.Next(-20, 55)
      Summary = "Freezing" }
