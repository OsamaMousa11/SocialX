using SocialX.Core.Domain.Entites;
using SocialX.Core.Repositories;
using SocialX.infrastraction.IRepositoryContract;
using SocialX.infrastraction.Repositories;
using SocialX.Infrastructure.Data;
using System.Linq.Expressions;

namespace SocialX.Core.IGenericRepositories
{
    public class TweetRepository :GenericRepository<Tweet>, ITweetRepository
    {
        private readonly AppDbContext _db;
        public TweetRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}