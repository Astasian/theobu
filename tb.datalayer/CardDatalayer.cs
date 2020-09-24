using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tb.datalayer.Models;

namespace tb.datalayer
{
    public class CardDatalayer : ICardDatalayer
    {
        private readonly DatabaseContext _dbContext;
        private readonly ILogger _logger;

        public CardDatalayer(DatabaseContext databaseContext, ILogger<CardDatalayer> logger)
        {
            _dbContext = databaseContext;
            _logger = logger;
        }

        public async Task Delete(int id)
        {
            _dbContext.Entry(new Card { Id = id }).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Card> GetCard(int id)
        {
            return await _dbContext.Cards.FindAsync(id);
        }

        public async Task<List<Card>> GetCards(int language, int count, int page)
        {
            var skippable = (page - 1) * count;
            return await _dbContext.Cards.AsQueryable().Skip(skippable).Take(count).ToListAsync();
        }

        public async Task<int> Insert(Card card)
        {
            // to be save
            card.Id = 0;
            _dbContext.Cards.Add(card);
            await _dbContext.SaveChangesAsync();
            return card.Id;
        }

        public async Task<Card> Update(Card card)
        {
            var dbCard = await _dbContext.Cards.FindAsync(card.Id);
            dbCard.Description = card.Description;
            dbCard.LanguageId = card.LanguageId;
            dbCard.Tabus = card.Tabus;
            _dbContext.Update(dbCard);
            await _dbContext.SaveChangesAsync();
            return dbCard;
        }
    }



    public interface ICardDatalayer
    {
        Task<List<Card>> GetCards(int language, int count, int page);
        Task<Card> GetCard(int id);
        Task Delete(int id);
        Task<int> Insert(Card card);
        Task<Card> Update(Card card);
    }
}
