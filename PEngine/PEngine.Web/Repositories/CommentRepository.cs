using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PEngine.Web.Helper;
using PEngine.Web.Models;
using PEngine.Web.Models.DataModels;
using ZstdSharp.Unsafe;

namespace PEngine.Web.Repositories;

public class CommentRepository : RepositoryBase
{
    public CommentRepository(BlogContext context) : base(context)
    {
        
    }

    private IQueryable<CommentMeta> QueryCommentTree(
        Expression<Func<CommentMeta, bool>> predicate)
    {
        return _context.CommentMetas.Where(predicate);
    }

    public List<CommentQueryResult> GetTopNodes(long postId, int page, int count, bool includeDeleted)
    {
        return QueryCommentTree(rel => rel.Path == null && rel.PostId == postId)
            .Skip((page - 1) * count)
            .Take(count)
            .ToQueryResult(_context.Comments)
            .ToList();
    }

    public List<CommentQueryResult>? GetSubNodes(Guid id, bool includeDeleted)
    {
        var parent = _context.CommentMetas.FirstOrDefault(rel => rel.CommentId == id);

        if (parent is null)
        {
            return null;
        }

        return QueryCommentTree(rel => rel.Path.EndsWith($"{id}"))
            .ToQueryResult(_context.Comments)
            .ToList();
    }

    public bool InsertComment(Guid? parentId, CommentUpdateInput input)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            string? parentPath = null;

            if (parentId is not null)
            {
                var parent = _context.CommentMetas.FirstOrDefault(rel => rel.CommentId == parentId);

                if (parent is null)
                {
                    throw new InvalidOperationException();
                }

                parentPath = $"{parent.Path}/{parent.CommentId}";
            }

            var commentEntry = _context.Comments.Add(new ()
            {

            });
            
            var newCommentId = commentEntry.Entity.Id;

            _context.CommentMetas.Add(new()
            {
                CommentId = newCommentId,
                Path = parentPath
            });
            
            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            return false;
        }

        return true;
    }

    public bool UpdateComment(Guid commentId, string content, string password)
    {
        return false;
    }

    public bool DeleteComment(Guid commentId, string password)
    {
        var salt = commentId.ToByteArray();
        var hash = password.Password(salt.ToBase64()).ToBase64();
        
        _context.Database.ExecuteSqlRaw(
            $"UPDATE {nameof(BlogContext.CommentMetas)} " +
            $"SET Deleted = {{0}} " +
            $"WHERE Id={{1}} AND Password={{2}} AND Deleted = {{3}}", 
            true, commentId, hash, false);

        return _context.SaveChanges() > 0;
    }

    public bool PurgeComment(Guid id)
    {
        var transaction = _context.Database.BeginTransaction();

        try
        {
            var rel = _context.CommentMetas.FirstOrDefault(c => c.CommentId == id);

            if (rel is null)
            {
                return false;
            }

            var path = $"{rel.Path}/{rel.CommentId}";

            _context.Database.ExecuteSqlRaw(
                $"DELETE FROM {nameof(BlogContext.Comments)} " +
                $"WHERE Id = {{0}}", rel.CommentId);

            _context.Database.ExecuteSqlRaw(
                $"DELETE FROM {nameof(BlogContext.CommentMetas)} " +
                $"WHERE CommentId = {{0}} OR Path LIKE CONCAT({{1}}, '%')",
                rel.CommentId, path
            );

            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            return false;
        }

        return true;
    }
}

public static class CommentRepositoryExtensions 
{
    public static IQueryable<CommentQueryResult> ToQueryResult(this IQueryable<CommentMeta> query, IQueryable<Comment> inner)
    {
        return query.Join(inner,
            rel => rel.CommentId,
            c => c.Id,
            (r, c) => new CommentQueryResult()
            {
                Comment = c,
                Deleted = r.Deleted,
                Screened = r.Screened,
                Encrypted = r.Encrypted,
                Replies = r.ChildrenCount
            });
    }
}