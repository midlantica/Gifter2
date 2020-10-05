using System.Collections.Generic;
using Gifter.Models;

namespace Gifter.Repositories
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        void Delete(int id);
        List<Comment> GetAll();
        Comment GetById(int id);
        Comment GetByPostId(int id);
        void Update(Comment comment);
    }
}