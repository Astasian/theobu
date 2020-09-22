using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tb.actor.grainsInterfaces;

namespace tb.actor.grains
{
    public class GameGrain : Grain, IGameGrain
    {
    
        public Task AddPlayer(Guid playerId)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> GetId()
        {
            return Task.FromResult(this.GetPrimaryKey());
        }

        public Task<IEnumerable<Guid>> GetPlayers()
        {
            throw new NotImplementedException();
        }
    }
}
