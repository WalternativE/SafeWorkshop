module Article

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
open Shared

type State =
    | Edit
    | View

type Model = {
    state: State
}

type Msg =
    | NoOp


let initNew () =
    let initialModel = {
        state = Edit
    }
    initialModel, Cmd.none

let initView article =
    let initialModel = {
        state = View
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

