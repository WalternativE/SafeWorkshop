namespace Articles

open Database
open Microsoft.Data.Sqlite
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Shared

module Database =
  let getAll connectionString : Task<Result<Article seq, exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! query connection "SELECT id, author_id, title, date, content FROM Articles" None
    }

  let getById connectionString id : Task<Result<Article option, exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! querySingle connection "SELECT id, author_id, title, date, content FROM Articles WHERE id=@id" (Some <| dict ["id" => id])
    }

  let update connectionString v : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "UPDATE Articles SET id = @id, author_id = @author_id, title = @title, date = @date, content = @content WHERE id=@id" v
    }

  let insert connectionString v : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "INSERT INTO Articles(id, author_id, title, date, content) VALUES (@id, @author_id, @title, @date, @content)" v
    }

  let delete connectionString id : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "DELETE FROM Articles WHERE id=@id" (dict ["id" => id])
    }

