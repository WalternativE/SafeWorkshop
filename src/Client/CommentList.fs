module CommentList

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
open Comment
open Comment

type Model = {
    article_id: string
}

type Msg =
    | NoOp

let init article_id =
    let initialModel = {
        article_id = article_id
    }

    initialModel, Cmd.none

let update (msg: Msg) (currentModel: Model) : Model * Cmd<Msg> =
    currentModel, Cmd.none

let button txt isActive onClick =
    Button.button
        [ Button.Color IsPrimary
          Button.IsActive isActive
          Button.OnClick onClick ]
        [ str txt ]

let view (model: Model) (dispatch: Msg -> unit) =
    div [] []
