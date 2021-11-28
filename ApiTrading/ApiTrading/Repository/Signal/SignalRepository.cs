namespace ApiTrading.Repository.Signal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DbContext;
    using global::Modele;
    using Microsoft.AspNetCore.Identity;
    using Modele;
    using Modele.DTO.Response;

    public class SignalRepository : GenericRepository<SignalInfoStrategy>, ISignalRepository
    {
        public SignalRepository(ApiTradingDatabaseContext context) : base(context)
        {
        }

        public async Task SaveSignal(SignalInfoStrategy signal)
        {
            await Context.SignalInfoStrategies.AddAsync(signal);
        }

        public async Task<List<string>> GetAllGiverSignal()
        {
            return Context.SignalInfoStrategies.Where(x => x.User.Id != 1).GroupBy(x => x.User.UserName)
                .Select(x => x.First()).Select(x => x.User.UserName).ToList();
        }

        public async Task SubscribeToSignal(IdentityUser<int> user, string symbol)
        {
            var subscription = new Subscription();
            subscription.Symbol = symbol;
            subscription.User = user;
            var selected = Context.Subscriptions.FirstOrDefault(x => x.User == user && x.Symbol == symbol);
            
            if (selected == null)
            {
                Context.Subscriptions.Remove(selected);
            }
            await Context.Subscriptions.AddAsync(subscription);

        }

        public async Task UnsubscribeToSignal(IdentityUser<int> user, string symbol)
        {
            var selected = Context.Subscriptions.FirstOrDefault(x => x.User == user && x.Symbol == symbol);

           

        }

        public async Task<List<Subscription>> GetCurrentSignalSubscription(IdentityUser<int> user)
        {
            return Context.Subscriptions.Where(x => x.User == user).ToList();
        }

        public async Task SaveSignals(List<SignalInfoStrategy> signals)
        {
            await Context.SignalInfoStrategies.AddRangeAsync(signals);
        }

        public async Task<List<SignalInfoStrategy>> GetSignalsOfUser(string symbol, string timeframe, string userName)
        {
            return Context.SignalInfoStrategies.Where(x => x.User.UserName == userName).Where(x => x.Symbol == symbol)
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