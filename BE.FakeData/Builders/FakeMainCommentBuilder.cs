using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BE.Models;

namespace BE.FakeData.Builders
{
    public class FakeCommentBuilder
    {
        private int _id;
        private string _content;
        private User _user;
        private Post _post;
        private ICollection<CommentLike> _commentLikes;
        private DateTime _date;

        public FakeCommentBuilder(int id)
        {
            _id = id;
        }

        public FakeCommentBuilder WithContent(string content)
        {
            _content = content;
            return this;
        }

        public FakeCommentBuilder WithUser(User user)
        {
            _user = user;
            return this;
        }

        public FakeCommentBuilder WithPost(Post post)
        {
            _post = post;
            return this;
        }

        public FakeCommentBuilder WithCommentLikes(IEnumerable<CommentLike> commentLikes)
        {
            _commentLikes = commentLikes.ToList();
            return this;
        }

        public FakeCommentBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public Comment Build() => new Comment()
        {
            Id = _id,
            Content = _content,
            Date = DateTime.Now,
            Post = _post,
            User = _user,
            CommentLike = _commentLikes
        };
    }
}
