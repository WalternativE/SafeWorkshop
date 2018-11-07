namespace Users

module View =

    open Giraffe.GiraffeViewEngine

    let userForm (method : string) (action : string) (submitText : string) =
        form [_method method; _action action] [
            div [_class "field"] [
                label [_for "username"] [ rawText "Username:" ]
                div [_class "control" ] [
                    input [_class "input"; _id "username"; _name "username"; _type "text"]
                ]
            ]
            div [_class "field"] [
                label [_for "password"] [ rawText "Password:" ]
                div [_class "control";] [
                    input [_class "input"; _id "password"; _name "password"; _type "password"]
                ]
            ]
            div [_class "field is-grouped"] [
                div [_class "control"] [
                    button [_type "submit"; _class "button is-link"] [rawText submitText]
                ]
                div [_class "control"] [
                    button [_type "reset"; _class "button is-text"] [rawText "Clear"]
                ]
            ]
        ]

    let loginView =
        [
            div [_class "container" ] [
                h1 [_class "is-size-2"] [ rawText "Login" ]
                userForm "POST" "/login" "Submit"
            ]
        ]

    let createUserView isError =
        [
            div [_class "container" ] [
                yield h1 [_class "is-size-2"] [ rawText "Signup" ]
                if isError then
                    yield! [ p [] [rawText "THERE IS AN ERROR"] ]
                else
                    yield! [ ]
                yield userForm "POST" "/users/signup" "Submit"
            ]
        ]

    let loginLayout ctx =
        App.layout ctx loginView

    let createUserLayout isError ctx =
        App.layout ctx (createUserView isError)