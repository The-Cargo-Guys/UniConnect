using MyAspNetVueApp.Models;
using System.Threading.Tasks;
using UniHack.Repositories;
using UniHackPrototype.Models;

namespace UniHack.Services.Services
{
	public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public void CreateCommentAndAddToDb(string content, User author)
        {
            Comment newComment = new Comment
            {
                Content = content,
                Author = author
            };
            _commentRepository.AddAsync(newComment, newComment.Id);
        }
    }
}
