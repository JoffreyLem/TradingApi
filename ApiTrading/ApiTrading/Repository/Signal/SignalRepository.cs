using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTrading.DbContext;
using ApiTrading.Modele;
using Microsoft.AspNetCore.Identity;
using Modele;

namespace ApiTrading.Repository.Signal
{
    public class SignalRepository : GenericRepository<SignalInfoStrategy>, ISignalRepository
    {
        public SignalRepository(ApiTradingDatabaseContext context) : base(context)
        {
        }

        public async Task SaveSignal(SignalInfoStrategy signal)
        {
            await Context.SignalInfoStrategies.AddAsync(signal);
            await SaveChangeAsync();
        }

        public async Task<List<string>> GetAllGiverSignal()
        {
            return Context.SignalInfoStrategies.Where(x => x.User.Id != 1).Select(x => x.User).ToList()
                .GroupBy(x => x.UserName).Select(x => x.Key).ToList();
        }

        public async Task SubscribeToSignal(IdentityUser<int> user, string symbol)
        {
            var subscription = new Subscription();
            subscription.Symbol = symbol;
            subscription.User = user;
            var selected = Context.Subscriptions.FirstOrDefault(x => x.User == user && x.Symbol == symbol);

            if (selected == null) await Context.Subscriptions.AddAsync(subscription);

            await SaveChangeAsync();
        }

        public async Task UnsubscribeToSignal(IdentityUser<int> user, string symbol)
        {
            var selected = Context.Subscriptions.FirstOrDefault(x => x.User == user && x.Symbol == symbol);

            if (selected != null) Context.Subscriptions.Remove(selected);

            await SaveChangeAsync();
        }

        public async Task<List<Subscription>> GetCurrentSignalSubscription(IdentityUser<int> user)
        {
            return Context.Subscriptions.Where(x => x.User == user).ToList();
        }

        public async Task<List<Subscription>> GetSubscriptionsOfSymbol(string symbol)
        {
            return Context.Subscriptions.Where(x => x.Symbol == symbol).ToList();
        }

        public async Task SaveSignals(List<SignalInfoStrategy> signals)
        {
            if (signals.Count > 0)
            {
                await Context.SignalInfoStrategies.AddRangeAsync(signals);
                await SaveChangeAsync();
            }
        }

        public async Task<List<SignalInfoStrategy>> GetSignalsOfUser(string symbol, string timeframe,
            IdentityUser<int> user)
        {
            return Context.SignalInfoStrategies.Where(x => x.User == user).Where(x => x.Symbol == symbol)
                .Where(x => x.Timeframe == timeframe).ToList();
        }

        public async Task<List<SignalInfoStrategy>> GetSignalsOfSystem(string strategyName, string symbol,
            string timeframe)
        {
            return Context.SignalInfoStrategies.Where(x =>
                    x.Symbol == symbol && x.Timeframe == timeframe && x.Strategy == strategyName && x.User.Id == 1)
                .ToList();
        }
    }
}