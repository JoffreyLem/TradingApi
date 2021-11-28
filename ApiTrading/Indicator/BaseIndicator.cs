namespace Indicator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exception;
    using Modele;

    public abstract class BaseIndicator<T> : List<T>, IIndicator
    {
        protected BaseIndicator(int loopBackPeriodRequested = 0)
        {
            IndicatorLevel = IndicatorLevel.L1;
            if (loopBackPeriodRequested == 0)
                LookbackPeriod = loopBackPeriodRequested + 2;
            else
                LookbackPeriod = loopBackPeriodRequested + 2;
        }


        public int LookbackPeriod { get; set; }
        public IndicatorLevel IndicatorLevel { get; set; }

        public abstract bool Buy(int i);

        public abstract bool Sell(int i);

        public abstract void Update(List<Candle> history);


        public Signal GetSignal(int i)
        {
            if (Count > LookbackPeriod)
            {
                dynamic test = this.Last();
                var type = GetType().UnderlyingSystemType;
                var currentName = type.Name;
                if (Buy(i))

                    return Signal.Buy;

                if (Sell(i))

                    return Signal.Sell;


                return Signal.None;
            }


            return Signal.None;
        }


        Signal IIndicator.GetState(int i, decimal? close)
        {
            throw new NotImplementedException();
        }


        private decimal? GetDynamicDecimalValue(dynamic test)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(test))
            {
                var propertieinfos = prop.ComponentType.GetProperties();

                foreach (var propertyInfo in propertieinfos)
                    if (propertyInfo.PropertyType == typeof(decimal?))
                        return (decimal?)propertyInfo.GetValue(test, null);
            }

            return null;
        }

        public Signal GetState(int i, decimal? close = null)
        {
            if (Count > LookbackPeriod)
            {
                if (!close.HasValue) throw new CloseValueNotGivenException("Close value necessaire pour GetState");
                dynamic test = this.Last();
                var type = GetType().UnderlyingSystemType;
                var currentName = type.Name;
                var data = GetDataSet(i);
                dynamic item = data.First();

                var value = GetDynamicDecimalValue(item);

                if (value > close)

                    return Signal.Sell;


                return Signal.Buy;
            }

            return Signal.None;
        }

        public List<T> GetDataSet(int i)
        {
            if (i < 2) i = 2;

            return this.Select(x => x).Reverse().Take(i).ToList();
        }


        protected void Update(IEnumerable<T> data)
        {
            if (data.Count() > LookbackPeriod)
            {
                dynamic last = data.Last();
                dynamic actualLast = this.LastOrDefault();

                decimal? lastValue = GetDynamicDecimalValue(last);
                decimal? actualLastValue = GetDynamicDecimalValue(actualLast);


                if (actualLast is null)
                {
                    AddRange(data);
                }
                else
                {
                    if (last.Date > actualLast.Date)
                    {
                        Add(last);
                    }
                    else if (last.Date == actualLast.Date && lastValue != actualLastValue)
                    {
                        Remove(actualLast);
                        Add(last);
                    }
                }
            }
        }
    }
}