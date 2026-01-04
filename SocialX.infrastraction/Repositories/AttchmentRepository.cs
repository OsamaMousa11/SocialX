using SocialX.Core.Domain.Entites;
using SocialX.Core.Domain.IRepositoryContract;
using SocialX.Core.Repositories;
using SocialX.infrastraction.IRepositoryContract;
using SocialX.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Attachment = SocialX.Core.Domain.Entites.Attachment;

namespace SocialX.infrastraction.Repositories
{
    public class AttchmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        private readonly AppDbContext _db;
        
        public AttchmentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

      
    }
}
