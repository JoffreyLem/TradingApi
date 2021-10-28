using System.Collections.Generic;

namespace ApiTrading.Domain
{
    public class AutResult
    {
        public string Token {get;set;}
        public string RefreshToken { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
    }
}