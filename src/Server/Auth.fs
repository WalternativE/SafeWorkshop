module AuthData

open Microsoft.AspNetCore.Http
open System.Security.Claims
open Microsoft.AspNetCore.Http

let getUserName (ctx: HttpContext) =
    if isNull ctx.User then
        "User"
    else
        ctx.User.Claims
        |> Seq.tryFind (fun n -> n.Type = ClaimTypes.Name)
        |> function
            | Some n -> n.Value
            | _ -> "User"

let isAuth (ctx: HttpContext) =
    not ((isNull ctx.User) || (ctx.User.Claims |> Seq.exists (fun n -> n.Type = ClaimTypes.Name) |> not))

let getUserId (ctx: HttpContext) =
    if isNull ctx.User then
        "-"
    else
        ctx.User.Claims
        |> Seq.tryFind (fun n -> n.Type = ClaimTypes.Name)
        |> function
            | Some n -> n.Value
            | _ -> "-"
