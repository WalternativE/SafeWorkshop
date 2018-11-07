module App

open Giraffe.GiraffeViewEngine
open Microsoft.AspNetCore.Http

let layout (ctx: HttpContext) (content: XmlNode list)  =
    html [_class "has-navbar-fixed-top"] [
        head [] [
            meta [_charset "utf-8"]
            meta [_name "viewport"; _content "width=device-width, initial-scale=1" ]
            title [] [encodedText "SAFE Workshop"]
            link [_rel "stylesheet"; _href "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" ]
            link [_rel "stylesheet"; _href "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.6.1/css/bulma.min.css" ]
            link [_rel "stylesheet"; _href "https://fonts.googleapis.com/css?family=Open+Sans" ]
            link [_rel "shortcut icon"; _type "image/png"; _href "/Images/safe_favicon.png" ]
        ]
        body [] [
            yield nav [ _class "navbar is-fixed-top has-shadow" ] [
                div [_class "navbar-brand"] [
                    a [_class "navbar-item"; _href "/"] [
                        img [_src "https://avatars0.githubusercontent.com/u/35305523?s=200"; _width "28"; _height "28"]
                    ]
                ]
                div [_class "navbar-menu"; _id "navMenu"] [
                    div [_class "navbar-start"] [
                        a [_class "navbar-item"; _href "https://github.com/SaturnFramework/Saturn/blob/master/README.md"] [rawText "Getting started"]
                        a [_class "navbar-item"; _href "/users"] [rawText "Users"]
                        span [_class "navbar-item"] [rawText (AuthData.getUserName ctx)]
                    ]
                ]
            ]
            yield! content
        ]
    ]
