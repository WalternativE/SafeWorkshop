module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch

open Thoth.Json

open Shared

open Fulma

type Model =
    | ArticleList of ArticleList.Model
    | Article of Article.Model

type Msg =
    | ArticleListMsg of ArticleList.Msg
    | ArticleMsg of Article.Msg

let init () : Model * Cmd<Msg> =
    let model, cmd = ArticleList.init ()
    let initialModel = ArticleList(model)
    initialModel, Cmd.map ArticleListMsg cmd

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg, currentModel with
    | ArticleMsg cm, Article c ->
        let m, cmd = Article.update cm c
        Article m, Cmd.map ArticleMsg cmd
    | ArticleListMsg msg, ArticleList mdl ->
        let m, cmd = ArticleList.update msg mdl
        ArticleList m, Cmd.map ArticleListMsg cmd



let view (model : Model) (dispatch : Msg -> unit) =
    let el =
        match model with
        | Article m ->
            Article.view m (ArticleMsg >> dispatch)
        | ArticleList m ->
            ArticleList.view m (ArticleListMsg >> dispatch)


    div []
        [ Navbar.navbar [ Navbar.Color IsPrimary ]
            [ Navbar.Item.div [ ]
                [ Heading.h2 [ ]
                    [ str "SAFE Template" ] ] ]

          Container.container []
              [ Content.content []
                    [el]
              ]
        ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
