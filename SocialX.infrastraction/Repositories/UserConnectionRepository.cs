using SocialX.Core.Domain.Entites;
using SocialX.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.infrastraction.Repositories
{
    public class UserConnectionRepository:GenericRepository<UserConnection>, IUserConnectionRepository
    {
        private readonly AppDbContext _db;
        public UserConnectionRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }   
    }
}
