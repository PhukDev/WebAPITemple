﻿using WebAPITemple.Data;
using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;

namespace WebAPITemple.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PublisherDTO> GetAllPublishers()
        {
            var allPublishersDomain = _dbContext.Publishers.ToList();
            var allPublisherDTO = new List<PublisherDTO>();
            foreach (var publisherDomain in allPublishersDomain)
            {
                allPublisherDTO.Add(new PublisherDTO()
                {
                    Id = publisherDomain.Id,
                    Name = publisherDomain.Name
                });
            }
            return allPublisherDTO;
        }

        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisherWithIdDomain = _dbContext.Publishers.FirstOrDefault(x => x.Id == id);
            if (publisherWithIdDomain == null)
            {
                return null;
            }
            var publisherNoIdDTO = new PublisherNoIdDTO
            {
                Name = publisherWithIdDomain.Name,
            };
            return publisherNoIdDTO;
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var publisherDomainModel = new Publishers
            {
                Name = addPublisherRequestDTO.Name,
            };
            _dbContext.Publishers.Add(publisherDomainModel);
            _dbContext.SaveChanges();
            return addPublisherRequestDTO;
        }

        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                publisherDomain.Name = publisherNoIdDTO.Name;
                _dbContext.SaveChanges();
            }
            return publisherNoIdDTO;  
        }

        public Publishers DeletePublisherById(int id)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                _dbContext.Publishers.Remove(publisherDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }
    }
}
