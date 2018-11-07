module ClientApp

open System
open Giraffe.GiraffeViewEngine

let clientApp =
    [
        div [_id "elmish-app"] []
        script [_src "/js/vendors.js"] []
        script [_src "/js/app.js"] []
    ]

let layout ctx = App.layout ctx clientApp