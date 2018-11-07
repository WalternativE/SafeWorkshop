namespace Migrations
open SimpleMigrations

[<Migration(201811071430L, "Create Articles")>]
type CreateArticles() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Articles(
      id TEXT NOT NULL,
      author_id TEXT NOT NULL,
      title TEXT NOT NULL,
      date TEXT NOT NULL,
      content TEXT NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Articles")
