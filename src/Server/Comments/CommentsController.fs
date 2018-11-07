namespace Comments

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn
open Saturn.ControllerHelpers

module Controller =

  let indexAction article_id (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getAllForArticle cnf.connectionString article_id
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
        return! Controller.renderHtml ctx (NotFound.layout)
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
        return! Controller.renderHtml ctx (NotFound.layout)
      | Error ex ->
        return raise ex
    }

  let createAction article_id (ctx: HttpContext) =
    task {
      let! input = Controller.getModel<Comment> ctx
      let userId = AuthData.getUserId ctx
      let input = {input with article_id = article_id; author_id = userId}
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

  let updateAction article_id (ctx: HttpContext) (id : string) =
    task {
      let! input = Controller.getModel<Comment> ctx
      let userId = AuthData.getUserId ctx
      let input = {input with article_id = article_id; author_id = userId}
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

  let resource article_id = controller {
    index (indexAction article_id)
    show showAction
    add addAction
    edit editAction
    create (createAction article_id)
    update (updateAction article_id)
    delete deleteAction
  }


module Api =

  let indexAction article_id (ctx : HttpContext) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getAllForArticle cnf.connectionString article_id
      match result with
      | Ok result ->
        return! Controller.json ctx (List.ofSeq result)
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

  let createAction article_id (ctx: HttpContext) =
    task {
      let! input = Controller.getModel<Comment> ctx
      let userId = AuthData.getUserId ctx
      let input = {input with article_id = article_id; author_id = userId; id = System.Guid.NewGuid().ToString()}
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then

        let cnf = Controller.getConfig ctx
        let! result = Database.insert cnf.connectionString input
        match result with
        | Ok _ ->
          return! Response.created ctx "Comment created"
        | Error ex ->
          return raise ex
      else
        return! Response.badRequest ctx validateResult
    }

  let updateAction article_id (ctx: HttpContext) (id : string) =
    task {
      let! input = Controller.getModel<Comment> ctx
      let userId = AuthData.getUserId ctx
      let input = {input with article_id = article_id; author_id = userId}
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then
        let cnf = Controller.getConfig ctx
        let! result = Database.update cnf.connectionString input
        match result with
        | Ok _ ->
          return! Response.accepted ctx input.id
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
        return! Response.ok ctx id
      | Error ex ->
        return raise ex
    }

  let resource article_id = controller {
    index (indexAction article_id)
    show showAction
    create (createAction article_id)
    update (updateAction article_id)
    delete deleteAction
  }

