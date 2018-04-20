using ENode.Commanding;
using System;

namespace Shop.Commands.Goodses
{
    public class AcceptNewCommentCommand:Command<Guid>
    {
        public Guid CommentId { get; private set; }

        private AcceptNewCommentCommand() { }
        public AcceptNewCommentCommand(Guid id, Guid commentId) : base(id)
        {
            CommentId = commentId;
        }
    }
}
