module Fsharp.Giraffe.AzureCosmos.App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.OpenApi.Models
open Giraffe

open Handlers.HelloHandlers
open Handlers.ManagerAdmin
open Config.SwaggerDocument

// ---------------------------------
// Web app
// ---------------------------------

let webApp =
    choose
        [ subRoute "/user" (choose [ POST >=> route "/login" >=> handleLogin ])
          subRoute "/api" (choose [ GET >=> route "/hello" >=> verifyToken >=> handleGetHello ])
          setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder: CorsPolicyBuilder) =
    builder
        .WithOrigins("http://localhost:5000", "https://localhost:5001")
        .AllowAnyMethod()
        .AllowAnyHeader()
    |> ignore

let configureApp (app: IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()

    (match env.IsDevelopment() with
     | true -> app.UseDeveloperExceptionPage()
     | false -> app.UseGiraffeErrorHandler(errorHandler).UseHttpsRedirection())
        .UseCors(configureCors)
        .UseSwagger()
        .UseSwaggerUI(fun c ->
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1")
            c.RoutePrefix <- ""
        )
        .UseGiraffe(webApp)

let configureServices (services: IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore
    
    services.AddMvcCore() |> ignore
    services.AddEndpointsApiExplorer() |> ignore
    
    services.AddSwaggerGen(fun c ->
    // Định nghĩa tài liệu Swagger
    c.SwaggerDoc("v1", OpenApiInfo(Title = "My API", Version = "v1"))
    
    // Cấu hình bảo mật cho Swagger
    c.AddSecurityDefinition("Bearer", 
        OpenApiSecurityScheme(
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
        )
    )

    // Cấu hình yêu cầu bảo mật
    let securitySchemeReference = OpenApiReference(
        Id = "Bearer",
        Type = ReferenceType.SecurityScheme
    )

    let securityRequirement = OpenApiSecurityRequirement()
    let requiredScopes = System.Collections.Generic.List<string>() :> System.Collections.Generic.IList<string>
    securityRequirement.Add(
        OpenApiSecurityScheme(
            Reference = securitySchemeReference
        ), 
        requiredScopes
    )

    c.AddSecurityRequirement(securityRequirement)

    // Đăng ký bộ lọc tài liệu tùy chỉnh để thêm các điểm cuối Giraffe
    c.DocumentFilter<GiraffeEndpointDocumentFilter>()
    ) |> ignore


let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main args =
    Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(Action<IApplicationBuilder> configureApp)
                .ConfigureServices(configureServices)
                .ConfigureLogging(configureLogging)
            |> ignore)
        .Build()
        .Run()

    0
