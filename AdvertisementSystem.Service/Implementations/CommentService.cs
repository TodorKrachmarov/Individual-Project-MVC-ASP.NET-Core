namespace AdvertisementSystem.Services.Implementations
{
    using Contracts;
    using Data;
    using Data.Models;
    using System;
    using System.Linq;
    using Models.Comment;
    using AutoMapper.QueryableExtensions;
    using System.Collections.Generic;

    using static Data.DataConstants;
    
    public class CommentService : ICommentService
    {
        private readonly AdvertisementDbContext db;

        public CommentService(AdvertisementDbContext db)
        {
            this.db = db;
        }

        public void Create(int adId, string content, string authorId)
        {
            var comment = new Comment
            {
                Content = content,
                AdId = adId,
                AuthorId = authorId
            };

            this.db.Comments.Add(comment);
            this.db.SaveChanges();
        }

        public bool AdExist(int id)
            => this.db.Ads.Any(a => a.Id == id);

        public bool CommentExist(int id)
            => this.db.Comments.Any(c => c.Id == id);

        public bool Exist(int commentId, string authorId)
            => this.db.Comments.Any(c => c.Id == commentId && c.AuthorId == authorId);

        public EditCommentServiceModel FindToEdit(int id)
            => this.db
                    .Comments
                    .Where(c => c.Id == id)
                    .ProjectTo<EditCommentServiceModel>()
                    .FirstOrDefault();

        public int Edit(int id, string content)
        {
            var comment = this.db.Comments.FirstOrDefault(c => c.Id == id);

            comment.Content = content;

            this.db.SaveChanges();

            return comment.AdId;
        }

        public DeleteCommentServiceModel FindToDelete(int id)
            => this.db
                    .Comments
                    .Where(c => c.Id == id)
                    .ProjectTo<DeleteCommentServiceModel>()
                    .FirstOrDefault();

        public int Delete(int id)
        {
            var comment = this.db.Comments.FirstOrDefault(c => c.Id == id);

            var adId = comment.AdId;

            this.db.Comments.Remove(comment);
            this.db.SaveChanges();

            return adId;
        }

        public IEnumerable<ListCommentServiceModel> AdComments(int adId, int page) 
            => this.db
                    .Comments
                    .OrderByDescending(a => a.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .Where(a => a.AdId == adId)
                    .ProjectTo<ListCommentServiceModel>()
                    .ToList();

        public int TotalAdComentsCount(int adId)
        {
            var count = this.db
                            .Comments
                            .Where(a => a.AdId == adId)
                            .Count();

            var num = Math.Ceiling(count / (double)PageSize);

            return (int)num;
        }
    }
}
