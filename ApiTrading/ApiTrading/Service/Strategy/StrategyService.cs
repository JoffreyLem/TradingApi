using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Response;
using StrategyManager;

namespace ApiTrading.Service.Strategy
{
    public class StrategyService : IStrategyService
    {
        public List<StrategyResponse> StrategyResponses { get; set; }

        public StrategyService()
        {
            StrategyResponses = new List<StrategyResponse>();
        }
        
        public async Task<List<StrategyResponse>> GetAllStrategy()
        {
            foreach (Enum enumVal in Enum.GetValues(typeof(StrategyList)))
            {
                var strategyResponse = new StrategyResponse();
                
                var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
                var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                    .GetCustomAttribute<StrategyAttributeType>();
                strategyResponse.Name = attribute?.Name;
                strategyResponse.Description = attribute?.Description;
                StrategyResponses.Add(strategyResponse);
            }
            return StrategyResponses;
        }
    }
}