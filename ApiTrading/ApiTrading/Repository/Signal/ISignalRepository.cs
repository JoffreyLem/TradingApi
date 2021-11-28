namespace ApiTrading.Repository.Signal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Modele;

    public interface ISignalRepository
    {
        public Task SaveSignal(SignalInfoStrategy signal);
        public Task SaveSignals(List<SignalInfoStrategy> signals);
        public Task<List<SignalInfoStrategy>> GetSignalsOfUser(string symbol, string timeframe,string userName);
        public Task<List<SignalInfoStrategy>> GetSignalsOfSystem(string strategyName, string symbol, string timeframe);

        public Task<List<string>> GetAllGiverSignal();
    }
}