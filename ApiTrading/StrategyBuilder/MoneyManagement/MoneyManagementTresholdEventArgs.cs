using System;
using System.ComponentModel;

namespace Strategy.MoneyManagement
{
    public class MoneyManagementTresholdEventArgs : EventArgs
    {
        public enum MoneyManagementTresholdEventType
        {
            [Description("LooseStreak")] LooseStreak,
            [Description("Drowdown")] Drowdown,
            [Description("Profitfactor")] Profitfactor
        }

        public MoneyManagementTresholdEventArgs(MoneyManagementTresholdEventType type)
        {
            EventType = type;
        }

        public MoneyManagementTresholdEventType EventType { get; set; }
    }
}