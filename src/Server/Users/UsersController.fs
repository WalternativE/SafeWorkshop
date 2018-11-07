namespace Users

module Controller =

    open Saturn
    open Giraffe.Core
    open Giraffe.ResponseWriters
    open FSharp.Control.Tasks.ContextInsensitive
    open System.Security.Claims
    open Microsoft.AspNetCore.Authentication
    open Microsoft.AspNetCore.Authentication.Cookies

    [<CLIMutable>]
    type LoginDto =
        { username : string
          password : string }

    let indexHandler layout =
        fun nxt ctx -> htmlView (layout ctx) nxt ctx

    let generateClaim id username role =
        let claims =
            [| Claim(ClaimTypes.NameIdentifier, id)
               Claim(ClaimTypes.Name, username)
               Claim(ClaimTypes.Role, role) |]
        let ci = ClaimsIdentity(claims)
        ClaimsPrincipal(ci)

    let router = router {
        get "/login" (indexHandler Users.View.loginLayout)
        get "/signup" (indexHandler <| Users.View.createUserLayout false)
        post "/signup" (fun nxt ctx ->
            task {
                let! loginDto = Controller.getModel<LoginDto> ctx
                let config = Controller.getConfig<Config.Config> ctx

                let user =
                    { id = System.Guid.NewGuid().ToString()
                      username = loginDto.username
                      password = loginDto.password
                      role = "" }

                let! result = Database.insert config.connectionString user

                match result with
                | Ok _ ->
                    return! redirectTo false "/" nxt ctx
                | Error _ ->
                    return! htmlView (Users.View.createUserLayout true ctx) nxt ctx
            })
        post "/login" (fun nxt ctx ->
            task {
                let! loginDto = Controller.getModel<LoginDto> ctx
                let config = Controller.getConfig<Config.Config> ctx
                let! result = Database.getByNameAndPassword config.connectionString loginDto.username loginDto.password

                match result with
                | Ok (None) -> return! indexHandler Users.View.loginLayout nxt ctx
                | Error e -> return! indexHandler Users.View.loginLayout nxt ctx
                | Ok (Some user) ->
                    let claims = generateClaim user.id user.username user.role
                    do! ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims)
                    return! redirectTo false "/" nxt ctx
            })
        get "/logout" (fun nxt ctx ->
            task {
                do! ctx.SignOutAsync()
                return! redirectTo false "/" nxt ctx
            })
    }