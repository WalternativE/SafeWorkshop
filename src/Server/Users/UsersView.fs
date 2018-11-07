namespace Users

open Giraffe.GiraffeViewEngine

module View =
  let login ctx failed =
    let validationMessage =
      div [_class "notification is-danger"] [
        a [_class "delete"; attr "aria-label" "delete"] []
        rawText "Wrong login or password"
      ]

    let field typ lbl key =
      div [_class "field"] [
        yield label [_class "label"] [rawText (string lbl)]
        yield div [_class "control has-icons-right"] [
          yield input [_class "input"; _name key ; _type typ]
        ]
      ]


    let buttons =
      div [_class "field is-grouped"] [
        div [_class "control"] [
          input [_type "submit"; _class "button is-link"; _value "Submit"]
        ]
        div [_class "control"] [
          a [_class "button is-text"; _href "/"] [rawText "Cancel"]
        ]
      ]

    let cnt = [
      div [_class "box"] [
        h2 [] [rawText "Login"]
        div [_class "container "] [
            form [ _action "/login"; _method "post"] [
            if failed then
                yield validationMessage
            yield field "text" "Username" "username"
            yield field "password" "Password" "password"
            yield buttons
            ]
          ]
        ]
      ]

    App.layout ctx ([section [_class "section"] cnt])

  let signup ctx (validationResult : Map<string, string>) =
    let validationMessage =
      div [_class "notification is-danger"] [
        a [_class "delete"; attr "aria-label" "delete"] []
        rawText "Oops, something went wrong! Please check the errors below."
      ]

    let field typ lbl key =
      div [_class "field"] [
        yield label [_class "label"] [rawText (string lbl)]
        yield div [_class "control has-icons-right"] [
          yield input [_class (if validationResult.ContainsKey key then "input is-danger" else "input"); _name key ; _type typ ]
          if validationResult.ContainsKey key then
            yield span [_class "icon is-small is-right"] [
              i [_class "fas fa-exclamation-triangle"] []
            ]
        ]
        if validationResult.ContainsKey key then
          yield p [_class "help is-danger"] [rawText validationResult.[key]]
      ]

    let buttons =
      div [_class "field is-grouped"] [
        div [_class "control"] [
          input [_type "submit"; _class "button is-link"; _value "Submit"]
        ]
        div [_class "control"] [
          a [_class "button is-text"; _href "/"] [rawText "Cancel"]
        ]
      ]

    let cnt = [
      div [_class "box"] [
        h2 [] [rawText "Sign Up"]
        div [_class "container "] [
            form [ _action "/signup"; _method "post"] [
            if not validationResult.IsEmpty then
                yield validationMessage
            yield field "text" "Username" "username"
            yield field "password" "Password" "password"
            yield buttons
            ]
        ]
        ]
    ]
    App.layout ctx ([section [_class "section"] cnt])