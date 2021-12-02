using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTrading.Modele;
using Microsoft.AspNetCore.Identity;
using Modele;

namespace ApiTrading.Repository.Signal
{
    public interface ISignalRepository
    {
        public Task SaveSignal(SignalInfoStrategy signal);
        public Task SaveSignals(List<SignalInfoStrategy> signals);
        public Task<List<SignalInfoStrategy>> GetSignalsOfUser(string symbol, string timeframe, IdentityUser<int> user);
        public Task<List<SignalInfoStrategy>> GetSignalsOfSystem(string strategyName, string symbol, string timeframe);
        public Task<List<string>> GetAllGiverSignal();

        public Task SubscribeToSignal(IdentityUser<int> user, string symbol);

        public Task UnsubscribeToSignal(IdentityUser<int> user, string symbol);

        public Task<List<Subscription>> GetCurrentSignalSubscription(IdentityUser<int> user);

        public Task<List<Subscription>> GetSubscriptionsOfSymbol(string symbol);
    }
}