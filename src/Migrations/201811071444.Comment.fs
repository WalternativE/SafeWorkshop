namespace Migrations
open SimpleMigrations

[<Migration(201811071444L, "Create Comments")>]
type CreateComments() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Comments(
      id TEXT NOT NULL,
      author_id TEXT NOT NULL,
      article_id TEXT NOT NULL,
      date TEXT NOT NULL,
      content TEXT NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Comments")
