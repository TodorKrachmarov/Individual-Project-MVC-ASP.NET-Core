namespace AdvertisementSystem.Services.Contracts
{
    using Models.Comment;
    using System.Collections.Generic;

    public interface ICommentService
    {
        void Create(int adId, string content, string authorId);

        bool AdExist(int id);

        bool CommentExist(int id);

        bool Exist(int commentId, string authorId);

        EditCommentServiceModel FindToEdit(int id);

        int Edit(int id, string content);

        DeleteCommentServiceModel FindToDelete(int id);

        int Delete(int id);

        IEnumerable<ListCommentServiceModel> AdComments(int adId, int page);

        int TotalAdComentsCount(int adId);
    }
}