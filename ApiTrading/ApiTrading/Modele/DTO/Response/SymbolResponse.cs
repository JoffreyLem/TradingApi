namespace ApiTrading.Modele.DTO.Response
{
    public class SymbolResponse
    {
        public SymbolResponse()
        {
        }

        public SymbolResponse(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}