namespace ApiTrading.Repository.Signal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Modele;
    using Microsoft.AspNetCore.Identity;
    using Modele;
    using Modele.DTO.Response;

    public interface ISignalRepository
    {
        public Task SaveSignal(SignalInfoStrategy signal);
        public Task SaveSignals(List<SignalInfoStrategy> signals);
        public Task<List<SignalInfoStrategy>> GetSignalsOfUser(string symbol, string timeframe, string userName);
        public Task<List<SignalInfoStrategy>> GetSignalsOfSystem(string strategyName, string symbol, string timeframe);
        public Task<List<string>> GetAllGiverSignal();

        public Task SubscribeToSignal(IdentityUser<int> user, string symbol);

        public Task UnsubscribeToSignal(IdentityUser<int> user, string symbol);

        public Task<List<Subscription>> GetCurrentSignalSubscription(IdentityUser<int> user);
    }
}