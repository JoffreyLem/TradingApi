namespace XtbLibrairie.codes
{
    using System;

    public class TRADE_OPERATION_CODE : BaseCode
    {
        public static readonly TRADE_OPERATION_CODE BUY = new(0L);
        public static readonly TRADE_OPERATION_CODE SELL = new(1L);
        public static readonly TRADE_OPERATION_CODE BUY_LIMIT = new(2L);
        public static readonly TRADE_OPERATION_CODE SELL_LIMIT = new(3L);
        public static readonly TRADE_OPERATION_CODE BUY_STOP = new(4L);
        public static readonly TRADE_OPERATION_CODE SELL_STOP = new(5L);
        public static readonly TRADE_OPERATION_CODE BALANCE = new(6L);

        public TRADE_OPERATION_CODE(long code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return Code.ToString();
        }

        public static TRADE_OPERATION_CODE GetOperationCode(long? l)
        {
            switch (l)
            {
                case 0:
                    return BUY;
                    break;
                case 1:
                    return SELL;
                    break;
                case 2:
                    return BUY_LIMIT;
                    break;
                case 3:
                    return SELL_LIMIT;
                    break;
                case 4:
                    return BUY_STOP;
                    break;
                case 5:
                    return SELL_STOP;
                    break;
                case 6:
                    return BALANCE;
                    break;
                default:
                    throw new Exception();
            }
        }

        public static TRADE_OPERATION_CODE GetOperationCode(int code)
        {
            switch (code)
            {
                case 0:
                    return BUY;
                    break;
                case 1:
                    return SELL;
                    break;
                case 2:
                    return BUY_LIMIT;
                    break;
                case 3:
                    return SELL_LIMIT;
                    break;
                case 4:
                    return BUY_STOP;
                    break;
                case 5:
                    return SELL_STOP;
                    break;
                case 6:
                    return BALANCE;
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}