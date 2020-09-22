using Orleans;
using Orleans.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace tb.actor.grainsInterfaces
{
    [Version(1)]
    public interface IGameGrain: IGrainWithGuidKey
    {
        Task<Guid> GetId();
        Task<IEnumerable<Guid>> GetPlayers();
        Task AddPlayer(Guid playerId);
    }
}
