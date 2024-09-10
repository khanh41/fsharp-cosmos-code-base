module Fsharp.Giraffe.AzureCosmos.Service.AuthService

open System
open System.IdentityModel.Tokens.Jwt
open System.Security.Claims
open Microsoft.IdentityModel.Tokens
open System.Text

type AuthService() =
    let secret_key = "your_very_long_secret_key_that_is_128_bits_long"
    let audience = "yourAudience"
    let issuer = "yourIssuer"

    member this.generateToken (username: string) =
        let key = SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key))
        let creds = SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        let claims = [ Claim(JwtRegisteredClaimNames.Sub, username) ]
        let token = JwtSecurityToken(
            issuer = issuer,
            audience = audience,
            claims = claims,
            expires = DateTime.Now.AddMinutes(30.0),
            signingCredentials = creds
        )
        JwtSecurityTokenHandler().WriteToken(token)
        
    member this.verifyToken (token: string) : Option<ClaimsPrincipal> =
        try
            // Convert the key to a SymmetricSecurityKey
            let keyBytes = System.Text.Encoding.UTF8.GetBytes(secret_key)
            let securityKey = SymmetricSecurityKey(keyBytes)
            
            // Set up token validation parameters
            let tokenValidationParameters = TokenValidationParameters(
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = securityKey
            )

            // Create a JwtSecurityTokenHandler to validate the token
            let handler = JwtSecurityTokenHandler()
            let mutable validatedToken = Unchecked.defaultof<SecurityToken>

            // Validate the token and get the principal
            let principal = handler.ValidateToken(token, tokenValidationParameters, &validatedToken)

            Some(principal)
        with
        | :? SecurityTokenValidationException as ex ->
            // Log exception or handle it as needed
            printfn "Token validation failed: %s" (ex.Message)
            raise ex
        | ex ->
            // Handle other exceptions
            printfn "An error occurred: %s" (ex.Message)
            raise ex