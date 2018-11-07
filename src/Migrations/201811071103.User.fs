namespace Migrations
open SimpleMigrations

[<Migration(201811071103L, "Create Users")>]
type CreateUsers() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Users(
      id TEXT NOT NULL,
      username TEXT NOT NULL,
      password TEXT NOT NULL,
      role TEXT NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Users")
