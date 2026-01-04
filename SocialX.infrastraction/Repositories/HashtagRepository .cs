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
    public class HashtagRepository:GenericRepository<Hashtag>,IHashtagRepository
    {
        private readonly AppDbContext _db;
        public HashtagRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
