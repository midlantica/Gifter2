using System.Linq;
using System.Collections.Generic;
using Gifter.Models;
using Microsoft.Extensions.Configuration;
using Gifter.Utils;

namespace Gifter.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration configuration) : base(configuration) { }

        public List<Comment> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT Id, UserProfileId, PostId, Message
                            FROM Comment";

                    var reader = cmd.ExecuteReader();

                    var comments = new List<Comment>();
                    while (reader.Read())
                    {
                        comments.Add(new Comment()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            PostId = DbUtils.GetInt(reader, "PostId"),
                            Message = DbUtils.GetString(reader, "Message")
                        });
                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        public Comment GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT UserProfileId, PostId, Message
                            FROM Comment
                           WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    Comment comment = null;
                    if (reader.Read())
                    {
                        comment = new Comment()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            PostId = DbUtils.GetInt(reader, "PostId"),
                            Message = DbUtils.GetString(reader, "Message")
                        };
                    }

                    reader.Close();

                    return comment;
                }
            }
        }
        public Comment GetByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT UserProfileId, PostId, Message, Name
                            FROM Comment c
                            LEFT JOIN UserProfile up ON UserProfileId = up.Id
                           WHERE PostId = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    Comment comment = null;
                    if (reader.Read())
                    {
                        comment = new Comment()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            PostId = DbUtils.GetInt(reader, "PostId"),
                            Message = DbUtils.GetString(reader, "Message"),
                            UserProfile = new UserProfile()
                            {
                                Name = DbUtils.GetString(reader, "Name")
                            }
                        };
                    }

                    reader.Close();

                    return comment;
                }
            }
        }

        public void Add(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Post (UserProfileId, PostId, MEssage)
                        OUTPUT INSERTED.ID
                        VALUES (@UserProfileId, @PostId, @Message)";

                    DbUtils.AddParameter(cmd, "@UserProfileId", comment.UserProfileId);
                    DbUtils.AddParameter(cmd, "@PostId", comment.PostId);
                    DbUtils.AddParameter(cmd, "@Message", comment.Message);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Post
                           SET UserProfileId = @UserProfileId,
                               PostId = @PostId,
                               Message = @Message
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@UserProfileId", comment.UserProfileId);
                    DbUtils.AddParameter(cmd, "@PostId", comment.PostId);
                    DbUtils.AddParameter(cmd, "@Message", comment.Message);
                    DbUtils.AddParameter(cmd, "@Id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Comment WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}