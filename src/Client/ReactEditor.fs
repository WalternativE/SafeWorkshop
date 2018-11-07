module ReactEditor

open Fable.Core.JsInterop
open Fable.Import
open Fable.Core
open Fable.Helpers.React

module Editor =

    type Props =
        | NoOp

    // let inline editor (props: Props list) : React.ReactElement =
    //     ofImport "default" "./component/component.js" (keyValueList CaseRules.LowerFirst props) []