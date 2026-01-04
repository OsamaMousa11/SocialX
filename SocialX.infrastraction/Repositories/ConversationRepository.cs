using SocialX.Core.Domain.Entites;
using SocialX.Core.Domain.IRepositoryContract;
using SocialX.Core.Repositories;
using SocialX.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.infrastraction.Repositories
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {
        private readonly AppDbContext _db;
        public ConversationRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
