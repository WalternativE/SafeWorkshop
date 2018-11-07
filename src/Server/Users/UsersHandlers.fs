namespace Users

open Saturn
open Config
open Giraffe.Core
open Giraffe.ResponseWriters
open FSharp.Control.Tasks.ContextInsensitive
open Microsoft.AspNetCore.Authentication
open System.Security.Claims
open System
open Microsoft.AspNetCore.Authentication.Cookies

module Handlers =

    [<CLIMutable>]
    type LoginView = {username: string; password: string}

    let generateClaim id username role =
        let claims = [|
            Claim(ClaimTypes.NameIdentifier, id)
            Claim(ClaimTypes.Name,username)
            Claim(ClaimTypes.Role, role)|]
        let ci = ClaimsIdentity(claims)
        ClaimsPrincipal(ci)

    let router = router {
        get "/login" (fun nxt ctx -> htmlView (View.login ctx false) nxt ctx)
        post "/login" (fun nxt ctx ->
            task {
                let config = Controller.getConfig ctx
                let! cnt = Controller.getModel<LoginView> ctx
                let! user = Database.getByLoginInfo config.connectionString cnt.username cnt.password
                return!
                    match user with
                    | Ok (Some u) ->
                        task {
                            let claims = generateClaim u.id u.username u.role
                            do! ctx.SignInAsync(claims)
                            return! redirectTo false "/" nxt ctx
                        }
                    | Ok (None) ->
                        htmlView (View.login ctx true) nxt ctx
                    | Error(errorValue) ->
                        raise errorValue
            })

        get "/signup" (fun nxt ctx -> htmlView (View.signup ctx Map.empty) nxt ctx)
        post "/signup" (fun nxt ctx ->
            task {
                let config = Controller.getConfig ctx
                let! cnt = Controller.getModel<LoginView> ctx
                let u = {
                    id = Guid.NewGuid().ToString()
                    username = cnt.username
                    password = cnt.password
                    role = "user"
                }
                let! result = Database.insert config.connectionString u
                return!
                    match result with
                    | Ok _ ->
                        task {
                            let claims = generateClaim u.id u.username u.role
                            do! ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims)
                            return! redirectTo false "/" nxt ctx
                        }
                    | Error ev ->
                        raise ev
            })
        get "/logout" (fun nxt ctx ->
            task {
                do! ctx.SignOutAsync()
                return! redirectTo false "/" nxt ctx
            })
    }