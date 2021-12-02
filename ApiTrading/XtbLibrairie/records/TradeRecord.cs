using System;
using Newtonsoft.Json.Linq;

namespace XtbLibrairie.records
{
    using JSONObject = JObject;

    public class TradeRecord : BaseResponseRecord
    {
        private double? close_price;
        private long? close_time;
        private bool? closed;
        private long? cmd;
        private string comment;
        private double? commission;
        private double? commission_agent;
        private string customComment;
        private long? digits;
        private long? expiration;
        private string expirationString;
        private double? margin_rate;
        private double? open_price;
        private long? open_time;
        private long? order;
        private long? order2;
        private long? position;
        private double? profit;
        private double? sl;
        private double? storage;
        private string symbol;
        private long? timestamp;
        private double? tp;
        private long? value_date;
        private double? volume;

        public TradeRecord(StreamingTradeRecord traderecord)
        {
            close_price = traderecord.Close_price;
            close_time = traderecord.Close_time;
            closed = traderecord.Closed;
            cmd = traderecord.Cmd;
            comment = traderecord.Comment;
            commission = traderecord.Commision;
            //   this.commission_agent = traderecord.Commission_agent;
            customComment = traderecord.CustomComment;
            digits = traderecord.Digits;
            expiration = traderecord.Expiration;
            //this.expirationString = traderecord.Close_price;
            margin_rate = traderecord.Margin_rate;
            open_price = traderecord.Open_price;
            open_time = traderecord.Open_time;
            order = traderecord.Order;
            order2 = traderecord.Order2;
            position = traderecord.Position;
            profit = traderecord.Profit;
            sl = traderecord.Sl;
            storage = traderecord.Storage;
            symbol = traderecord.Symbol;
            // this.timestamp = traderecord.Time;
            tp = traderecord.Tp;
            //  this.value_date =traderecord.Va;
            volume = traderecord.Volume;
        }

        public TradeRecord()
        {
        }

        public TradeRecord(StreamingTradeStatusRecord tradeStatusRecord)
        {
            Order = tradeStatusRecord.Order;
        }

        public virtual double? Close_price => close_price;

        public virtual long? Close_time => close_time;

        public virtual bool? Closed => closed;

        public virtual long? Cmd => cmd;

        public virtual string Comment => comment;

        public virtual double? Commission => commission;

        public virtual double? Commission_agent => commission_agent;

        public virtual string CustomComment => customComment;

        public virtual long? Digits => digits;

        public virtual long? Expiration => expiration;

        public virtual string ExpirationString => expirationString;

        [Obsolete] public virtual long? Login => null;

        public virtual double? Margin_rate => margin_rate;

        public virtual double? Open_price => open_price;

        public virtual long? Open_time => open_time;

        public virtual long? Order
        {
            get => order;
            set => order = value;
        }

        public virtual long? Order2
        {
            get => order2;
            set => order2 = value;
        }

        public virtual long? Position
        {
            get => position;
            set => position = value;
        }

        public virtual double? Profit => profit;

        public virtual double? Sl => sl;

        [Obsolete("Not used any more")] public virtual long? Spread => null;

        public virtual double? Storage => storage;

        public virtual string Symbol => symbol;

        [Obsolete("Not used any more")] public virtual double? Taxes => null;

        public virtual long? Timestamp => timestamp;

        public virtual double? Tp => tp;

        public virtual long? Value_date => value_date;

        public virtual double? Volume => volume;

        public void FieldsFromJSONObject(JSONObject value)
        {
            close_price = (double?) value["close_price"];
            close_time = (long?) value["close_time"];
            closed = (bool?) value["closed"];
            cmd = (long?) value["cmd"];
            comment = (string) value["comment"];
            commission = (double?) value["commission"];
            commission_agent = (double?) value["commission_agent"];
            customComment = (string) value["customComment"];
            digits = (long?) value["digits"];
            expiration = (long?) value["expiration"];
            expirationString = (string) value["expirationString"];
            margin_rate = (double?) value["margin_rate"];
            open_price = (double?) value["open_price"];
            open_time = (long?) value["open_time"];
            order = (long?) value["order"];
            order2 = (long?) value["order2"];
            position = (long?) value["position"];
            profit = (double?) value["profit"];
            sl = (double?) value["sl"];
            storage = (double?) value["storage"];
            symbol = (string) value["symbol"];
            timestamp = (long?) value["timestamp"];
            tp = (double?) value["tp"];
            value_date = (long?) value["value_date"];
            volume = (double?) value["volume"];
        }

        [Obsolete("Method outdated")]
        public bool FieldsFromJSONObject(JSONObject value, string str)
        {
            return false;
        }

        public override string ToString()
        {
            return "TradeRecord{" + "close_price=" + close_price + ", close_time=" + close_time + ", closed=" + closed +
                   ", cmd=" + cmd + ", comment=" + comment + ", commission=" + commission + ", commission_agent=" +
                   commission_agent + ", customComment=" + customComment + ", digits=" + digits + ", expiration=" +
                   expiration + ", expirationString=" + expirationString + ", margin_rate=" + margin_rate +
                   ", open_price=" + open_price + ", open_time=" + open_time + ", order=" + order + ", order2=" +
                   Order2 + ", position=" + Position + ", profit=" + profit + ", sl=" + sl + ", storage=" + storage +
                   ", symbol=" + symbol + ", timestamp=" + timestamp + ", tp=" + tp + ", value_date=" + value_date +
                   ", volume=" + volume + '}';
        }

        public async void Update(StreamingTradeRecord traderecord)
        {
            close_price = traderecord.Close_price;
            close_time = traderecord.Close_time;
            closed = traderecord.Closed;
            cmd = traderecord.Cmd;
            comment = traderecord.Comment;
            commission = traderecord.Commision;
            //   this.commission_agent = traderecord.Commission_agent;
            customComment = traderecord.CustomComment;
            digits = traderecord.Digits;
            expiration = traderecord.Expiration;
            //this.expirationString = traderecord.Close_price;
            margin_rate = traderecord.Margin_rate;
            open_price = traderecord.Open_price;
            open_time = traderecord.Open_time;
            // this.order = traderecord.Close_price;
            //this.order2 =traderecord.Close_price;
            //this.position = traderecord.Close_price;
            profit = traderecord.Profit;
            sl = traderecord.Sl;
            storage = traderecord.Storage;
            symbol = traderecord.Symbol;
            // this.timestamp = traderecord.Time;
            tp = traderecord.Tp;
            //  this.value_date =traderecord.Va;
            volume = traderecord.Volume;
        }
    }
}