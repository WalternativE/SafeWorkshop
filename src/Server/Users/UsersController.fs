namespace Users

module Controller =

    open Saturn
    open Giraffe.Core
    open Giraffe.ResponseWriters
    open FSharp.Control.Tasks.ContextInsensitive

    [<CLIMutable>]
    type LoginDto =
        { username : string
          password : string }

    let indexHandler layout =
        fun nxt ctx -> htmlView (layout ctx) nxt ctx

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
    }