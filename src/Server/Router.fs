module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters

let browser = pipeline {
    plug acceptHtml
    plug putSecureBrowserHeaders
    plug fetchSession
    set_header "x-pipeline-type" "Browser"
}

let defaultView = router {
    get "/" (fun nxt ctx -> htmlView (Index.layout ctx) nxt ctx)
    get "/index.html" (redirectTo false "/")
    get "/default.html" (redirectTo false "/")
}

let clientApp = router {
    get "" (fun nxt ctx -> htmlView (ClientApp.layout ctx) nxt ctx)
    get "/" (fun nxt ctx -> htmlView (ClientApp.layout ctx) nxt ctx)
}

let browserRouter = router {
    not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
    pipe_through browser //Use the default browser pipeline

    forward "" defaultView //Use the default view
    forward "/client" clientApp //Use the client application
    forward "/users" Users.Controller.router
    forward "/articles" Articles.Controller.resource
}

//Other scopes may use different pipelines and error handlers

let appRouter = router {
    forward "" browserRouter
}