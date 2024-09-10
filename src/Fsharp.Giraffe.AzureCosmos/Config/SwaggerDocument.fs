module Fsharp.Giraffe.AzureCosmos.Config.SwaggerDocument

open Swashbuckle.AspNetCore.SwaggerGen
open Microsoft.OpenApi.Models
open System.Collections.Generic

type GiraffeEndpointDocumentFilter() =
    interface IDocumentFilter with
        member this.Apply(swaggerDoc, context) =
            // Define the operation for the /api/hello endpoint
            let helloOperation = OpenApiOperation()
            helloOperation.Summary <- "Get Hello API"
            helloOperation.Description <- "Returns a greeting message."
            helloOperation.Responses.Add("200", OpenApiResponse(Description = "Returns Hello message"))

            // Define the path item for the endpoint
            let helloPathItem = OpenApiPathItem()
            helloPathItem.Operations.Add(OperationType.Get, helloOperation)

            // Add the custom path to the Swagger document
            swaggerDoc.Paths.Add("/api/hello", helloPathItem)
            
            // Define the operation for the /user/login endpoint
            let loginOperation = OpenApiOperation()
            loginOperation.Summary <- "User Login"
            loginOperation.Description <- "Authenticates a user with a username and password and returns a JWT token if successful."
            
            // Define the request body for /user/login
            let requestBody = OpenApiRequestBody()
            requestBody.Description <- "User login credentials"
            requestBody.Content.Add("application/x-www-form-urlencoded", OpenApiMediaType(
                Schema = OpenApiSchema(
                    Type = "object",
                    Properties = dict [
                        ("username", OpenApiSchema(Type = "string"))
                        ("password", OpenApiSchema(Type = "string"))
                    ],
                    Required = HashSet<string>(["username"; "password"])
                )
            ))
            loginOperation.RequestBody <- requestBody

            // Define possible responses for the /user/login endpoint
            loginOperation.Responses.Add("200", OpenApiResponse(Description = "Returns a JWT token"))
            loginOperation.Responses.Add("401", OpenApiResponse(Description = "Unauthorized: Incorrect account or password."))

            // Define the path item for the /user/login endpoint
            let loginPathItem = OpenApiPathItem()
            loginPathItem.Operations.Add(OperationType.Post, loginOperation)

            // Add the /user/login path to the Swagger document
            swaggerDoc.Paths.Add("/user/login", loginPathItem)