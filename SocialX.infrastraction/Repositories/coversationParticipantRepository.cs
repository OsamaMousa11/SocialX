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
    public class coversationParticipantRepository:GenericRepository<ConversationParticipant>, IConversationParticipantRepository
    {
        private readonly AppDbContext _db;
        public coversationParticipantRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
