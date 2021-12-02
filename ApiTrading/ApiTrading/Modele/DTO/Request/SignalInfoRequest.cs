using System;
using System.ComponentModel.DataAnnotations;
using Modele;

namespace ApiTrading.Modele.DTO.Request
{
    public class SignalInfoRequest
    {
        [Required] public string Timeframe { get; set; }

        [Required] public string Symbol { get; set; }

        [Required] public Signal Signal { get; set; }

        [Required] public DateTime DateTime { get; set; }

        [Required] public decimal EntryLevel { get; set; }

        public decimal? StopLoss { get; set; }

        public decimal? TakeProfit { get; set; }
    }
}