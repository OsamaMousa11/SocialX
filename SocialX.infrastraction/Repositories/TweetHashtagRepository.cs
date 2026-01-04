using SocialX.Core.Domain.Entites;
using SocialX.Core.Domain.IRepositoryContract;
using SocialX.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.infrastraction.Repositories
{
    public class TweetHashtagRepository:GenericRepository<TweetHashtag>, ITweetHashtagRepository
    {
        private readonly AppDbContext _db;
        public TweetHashtagRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
