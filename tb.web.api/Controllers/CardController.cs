using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tb.datalayer;
using tb.datalayer.Models;

namespace tb.web.api.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class CardController: ControllerBase
    {
        public readonly ICardDatalayer _cardDatalayer;
        public CardController(ICardDatalayer cardDatalayer)
        {
            _cardDatalayer = cardDatalayer;
        }

        [HttpGet]
        public Task<List<Card>> GetCards(int language, int count, int page) => _cardDatalayer.GetCards(language, count, page);

        [HttpGet]
        public Task<Card> GetCard(int id) => _cardDatalayer.GetCard(id);

        [HttpDelete]
        public Task Delete(int id) => _cardDatalayer.Delete(id);

        [HttpPost]
        public Task<int> Insert(Card card) => _cardDatalayer.Insert(card);

        [HttpPut]
        public Task<Card> Update(Card card) => _cardDatalayer.Update(card);
    }
}
