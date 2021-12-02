using System;

namespace StrategyManager
{
    public class StrategyAttributeType : Attribute
    {
        public StrategyAttributeType(Type type)
        {
            Type = type;
        }

        public StrategyAttributeType(Type type, string description, string name)
        {
            Type = type;
            Description = description;
            Name = name;
        }

        public StrategyAttributeType(Type type, string description)
        {
            Type = type;
            Description = description;
        }

        public Type Type { get; set; }
        public string Description { get; set; }

        public string Name { get; set; }
    }
}