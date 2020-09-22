using Orleans;
using Orleans.Runtime;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace tb.actor.silo
{
    class ReadinessLifecycleParticipant : ILifecycleParticipant<ISiloLifecycle>
    {

        private Task OnActive(CancellationToken ct)
        {
            if (Utils.IsLinux)
            {
                File.Create("/tmp/ready");
            }

            return Task.CompletedTask;
        }

        private Task OnUnActive(CancellationToken ct)
        {
            if (File.Exists("/tmp/ready") && Utils.IsLinux)
            {
                File.Delete("/tmp/ready");
            }

            return Task.CompletedTask;
        }

        public void Participate(ISiloLifecycle lifecycle)
        {
            lifecycle.Subscribe<ReadinessLifecycleParticipant>(ServiceLifecycleStage.Active, OnActive, OnUnActive);
        }
    }
}
