namespace Modele
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Identity;
   

    public class SignalInfoStrategy
    {

        public SignalInfoStrategy()
        {
            
        }
        public SignalInfoStrategy(string timeframe, string symbol)
        {
            this.Timeframe = timeframe;
            this.Symbol = symbol;
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var cls = mth.ReflectedType.Namespace;
            Strategy = cls;
        }
        
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        
        [JsonIgnore]
        public string Strategy { get; set; }
        [JsonIgnore]
        public string Timeframe { get; set; }
        [JsonIgnore]
        public string Symbol { get; set; }
        public Signal Signal { get; set; }
        
        public DateTime DateTime { get; set; }
        public decimal EntryLevel { get; set; }
        
        public decimal? StopLoss { get; set; }
        
        public decimal? TakeProfit { get; set; }
        [JsonIgnore]
        public IdentityUser<int> User { get; set; }
    }
}