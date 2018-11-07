module ArticleList

open System
open Elmish
open Fable.PowerPack.PromiseImpl
open Fable.PowerPack
open Thoth.Json
open Fable.PowerPack.Fetch
open Fable.Core.JsInterop
open Fulma
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Article
open Shared

[<Literal>]
let LoremText = "Donec fermentum interdum elit, in congue justo maximus congue. Mauris tincidunt ultricies lacus, vel pulvinar diam luctus et. In vel tellus vitae dolor efficitur pulvinar eu non tortor. Nunc eget augue id nisl bibendum congue vitae vitae purus. Phasellus pharetra nunc at justo dictum rutrum. Nullam diam diam, tincidunt id interdum a, rutrum ac lorem."

type Model = {
    articles : Article list
    errorMessage : string option
}

type Msg =
    | ViewArticle of Article
    | AddNewArticle
    | ArticlesLoaded of Article []
    | ArticlesLoadedFailed of string
    | EditArticle of Article
    | DeleteArticle of Article

let getArticles () =
    promise {
        let url = "/api/articles"
        let props = []

        let! res = Fetch.fetch url props
        let! txt = res.text()

        return Decode.Auto.unsafeFromString<Article[]> txt
    }

let init () =
    let initialModel = {
        articles = []
        errorMessage = None
    }

    let cmd =
        Cmd.ofPromise
            getArticles
            ()
            ArticlesLoaded
            (fun exn -> ArticlesLoadedFailed exn.Message)

    initialModel, cmd

let update (msg: Msg) (currentModel: Model) : Model * Cmd<Msg> =
    let ignoreForNow = currentModel, Cmd.none
    match msg with
    | ViewArticle a -> ignoreForNow
    | AddNewArticle -> ignoreForNow
    | ArticlesLoaded articles -> { currentModel with articles = List.ofArray articles}, Cmd.none
    | ArticlesLoadedFailed err -> { currentModel with articles = []; errorMessage = Some err }, Cmd.none
    | EditArticle a -> ignoreForNow
    | DeleteArticle a -> ignoreForNow

let button txt isActive onClick =
    Button.button
        [ Button.Color IsPrimary
          Button.IsActive isActive
          Button.OnClick onClick ]
        [ str txt ]

let view (model: Model) (dispatch: Msg -> unit) =
    div [] [
        yield!
            model.articles
            |> List.map (fun a ->
                Message.message [] [
                    Message.header [] [
                        str a.title
                    ]
                    Message.body [] [
                        p [] [ str (sprintf "Author: %s" a.author_id) ]
                        p [] [ str a.content ]  ]
                ])
        yield
            Button.button [ Button.Color IsPrimary; Button.OnClick (fun _ -> dispatch AddNewArticle) ] [ str "Add New" ]
    ]
