namespace Users

open Database
open Microsoft.Data.Sqlite
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

module Database =
  let getAll connectionString : Task<Result<User seq, exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! query connection "SELECT id, username, password, role FROM Users" None
    }

  let getById connectionString id : Task<Result<User option, exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! querySingle connection "SELECT id, username, password, role FROM Users WHERE id=@id" (Some <| dict ["id" => id])
    }

  let getByName connectionString name : Task<Result<User option, exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! querySingle connection "SELECT id, username, password, role FROM Users WHERE name=@name" name
    }

  let update connectionString v : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "UPDATE Users SET id = @id, username = @username, password = @password, role = @role WHERE id=@id" v
    }

  let insert connectionString v : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "INSERT INTO Users(id, username, password, role) VALUES (@id, @username, @password, @role)" v
    }

  let delete connectionString id : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "DELETE FROM Users WHERE id=@id" (dict ["id" => id])
    }

