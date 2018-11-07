namespace Articles

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn
open Giraffe.Auth
open Microsoft.AspNetCore.Authentication.Cookies
open Giraffe.Core

module Controller =

  let indexAction (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getAll cnf.connectionString
      match result with
      | Ok result ->
        return! Controller.renderHtml ctx (Views.index ctx (List.ofSeq result))
      | Error ex ->
        return raise ex
    }

  let showAction (ctx: HttpContext) (id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getById cnf.connectionString id
      match result with
      | Ok (Some result) ->
        return! Controller.renderHtml ctx (Views.show ctx result)
      | Ok None ->
        return! Controller.renderHtml ctx NotFound.layout
      | Error ex ->
        return raise ex
    }

  let addAction (ctx: HttpContext) =
    task {
      return! Controller.renderHtml ctx (Views.add ctx None Map.empty)
    }

  let editAction (ctx: HttpContext) (id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getById cnf.connectionString id
      match result with
      | Ok (Some result) ->
        return! Controller.renderHtml ctx (Views.edit ctx result Map.empty)
      | Ok None ->
        return! Controller.renderHtml ctx  NotFound.layout
      | Error ex ->
        return raise ex
    }

  let createAction (ctx: HttpContext) =
    task {
      let! input = Controller.getModel<Article> ctx
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then

        let cnf = Controller.getConfig ctx
        let! result = Database.insert cnf.connectionString input
        match result with
        | Ok _ ->
          return! Controller.redirect ctx (Links.index ctx)
        | Error ex ->
          return raise ex
      else
        return! Controller.renderHtml ctx (Views.add ctx (Some input) validateResult)
    }

  let updateAction (ctx: HttpContext) (id : string) =
    task {
      let! input = Controller.getModel<Article> ctx
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then
        let cnf = Controller.getConfig ctx
        let! result = Database.update cnf.connectionString input
        match result with
        | Ok _ ->
          return! Controller.redirect ctx (Links.index ctx)
        | Error ex ->
          return raise ex
      else
        return! Controller.renderHtml ctx (Views.edit ctx input validateResult)
    }

  let deleteAction (ctx: HttpContext) (id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.delete cnf.connectionString id
      match result with
      | Ok _ ->
        return! Controller.redirect ctx (Links.index ctx)
      | Error ex ->
        return raise ex
    }

  let resource = controller {
    plug (except [Index; Show]) (fun nxt ctx ->
      if AuthData.isAuth ctx then
        nxt ctx
      else
       redirectTo false "/login" nxt ctx )

    subController "/comments" Comments.Controller.resource

    index indexAction
    show showAction
    add addAction
    edit editAction
    create createAction
    update updateAction
    delete deleteAction
  }

module Api =
  let indexAction (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getAll cnf.connectionString
      match result with
      | Ok result ->
        return! Controller.json ctx (Array.ofSeq result)
      | Error ex ->
        return raise ex
    }

  let showAction (ctx: HttpContext) (id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getById cnf.connectionString id
      match result with
      | Ok (Some result) ->
        return! Controller.json ctx (Some result)
      | Ok None ->
        return! Controller.json ctx None
      | Error ex ->
        return raise ex
    }

  let createAction (ctx: HttpContext) =
    task {
      let! input = Controller.getModel<Article> ctx
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then

        let cnf = Controller.getConfig ctx
        let! result = Database.insert cnf.connectionString input
        match result with
        | Ok _ ->
          return!  Response.created ctx "Article created"
        | Error ex ->
          return raise ex
      else
        return! Response.badRequest ctx validateResult
    }

  let updateAction (ctx: HttpContext) (id : string) =
    task {
      let! input = Controller.getModel<Article> ctx
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then
        let cnf = Controller.getConfig ctx
        let! result = Database.update cnf.connectionString input
        match result with
        | Ok _ ->
          return! Response.created ctx "Article created"
        | Error ex ->
          return raise ex
      else
        return! Response.badRequest ctx validateResult
    }

  let deleteAction (ctx: HttpContext) (id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.delete cnf.connectionString id
      match result with
      | Ok _ ->
        return! Response.created ctx "Article created"
      | Error ex ->
        return raise ex
    }

  let resource = controller {
    plug [All] (fun nxt ctx ->
      if AuthData.isAuth ctx then
        nxt ctx
      else
        Response.unauthorized ctx "Cookies" "" "Request unauthorized" )

    subController "/comments" Comments.Api.resource

    index indexAction
    show showAction
    create createAction
    update updateAction
    delete deleteAction
  }
