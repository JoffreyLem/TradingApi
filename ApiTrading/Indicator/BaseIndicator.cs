﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Indicator.Exception;
using Modele;
using Utility;

namespace Indicator
{
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

        public Log Log { get; set; }

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
                        return (decimal?) propertyInfo.GetValue(test, null);
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


     
    }
}