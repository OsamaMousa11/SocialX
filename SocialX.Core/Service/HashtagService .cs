using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.HashtagDto;
using SocialX.Core.Exceptions;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;

namespace SocialX.Core.Service
{
    public class HashtagService : IHashtagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HashtagService> _logger;

        public HashtagService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<HashtagService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<HashtagResponse> CreateHashtagAsync(
            string tagName,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                throw new BadRequestException("Hashtag name is required");

            var normalizedName = Normalize(tagName);

            var existing = await _unitOfWork.Repository<Hashtag>()
                .FindAsync(h => h.Name == normalizedName, cancellationToken: cancellationToken);

            if (existing != null)
                return MapToResponse(existing);

            var hashtag = new Hashtag
            {
                Id = Guid.NewGuid(),
                Name = normalizedName
            };

            await _unitOfWork.Repository<Hashtag>()
                .AddAsync(hashtag, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Hashtag {Name} created", normalizedName);

            return MapToResponse(hashtag);
        }
        public async Task<HashtagResponse> GetHashtagByIdAsync(
            Guid hashtagId,
            CancellationToken cancellationToken = default)
        {
            if (hashtagId == Guid.Empty)
                throw new BadRequestException("Invalid hashtag id");

            var hashtag = await _unitOfWork.Repository<Hashtag>()
                .FindAsync(
                    h => h.Id == hashtagId,
                    includeProperties: "TweetHashtags",
                    cancellationToken: cancellationToken);

            if (hashtag == null)
                throw new NotFoundException("Hashtag not found");

            return MapToResponse(hashtag);
        }


        public async Task<HashtagResponse> GetHashtagByNameAsync(
            string tagName,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                throw new BadRequestException("Hashtag name is required");

            var normalizedName = Normalize(tagName);

            var hashtag = await _unitOfWork.Repository<Hashtag>()
                .FindAsync(
                    h => h.Name == normalizedName,
                    includeProperties: "TweetHashtags",
                    cancellationToken: cancellationToken);

            if (hashtag == null)
                throw new NotFoundException("Hashtag not found");

            return MapToResponse(hashtag);
        }

 
        public async Task<PaginatedResult<HashtagResponse>> SearchHashtagsAsync(
            string searchTerm,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new BadRequestException("Search term is required");

            if (pageNumber < 1 || pageSize < 1)
                throw new BadRequestException("Invalid pagination");

            var term = searchTerm.Trim().ToLower();

            var totalCount = await _unitOfWork.Repository<Hashtag>()
                .CountAsync(h => h.Name.ToLower().Contains(term), cancellationToken);

            var hashtags = await _unitOfWork.Repository<Hashtag>()
                .GetPagedAsync(
                    pageNumber,
                    pageSize,
                    predicate: h => h.Name.ToLower().Contains(term),
                    orderBy: q => q.OrderByDescending(h => h.TweetHashtags.Count),
                    includeProperties: "TweetHashtags",
                    cancellationToken: cancellationToken);

            var responses = hashtags
                .Select(MapToResponse)
                .ToList();

            return new PaginatedResult<HashtagResponse>(
                responses,
                totalCount,
                pageNumber,
                pageSize);
        }

     
        public async Task<bool> DeleteHashtagAsync(
            Guid hashtagId,
            CancellationToken cancellationToken = default)
        {
            if (hashtagId == Guid.Empty)
                throw new BadRequestException("Invalid hashtag id");

            var hashtag = await _unitOfWork.Repository<Hashtag>()
                .FindAsync(h => h.Id == hashtagId, cancellationToken: cancellationToken);

            if (hashtag == null)
                throw new NotFoundException("Hashtag not found");

            _unitOfWork.Repository<Hashtag>().Delete(hashtag);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Hashtag {Id} deleted", hashtagId);

            return true;
        }

        // =========================
        // HELPERS
        // =========================
        private static string Normalize(string tag)
        {
            tag = tag.Trim();
            return tag.StartsWith("#") ? tag.Substring(1) : tag;
        }

        private static HashtagResponse MapToResponse(Hashtag hashtag)
        {
            return new HashtagResponse
            {
                Id = hashtag.Id,
                Name = hashtag.Name,
                Count = hashtag.TweetHashtags?.Count ?? 0
            };
        }
    }
}
