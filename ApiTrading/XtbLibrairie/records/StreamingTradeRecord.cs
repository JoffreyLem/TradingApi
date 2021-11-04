﻿using Newtonsoft.Json.Linq;
using XtbLibrairie.codes;

namespace XtbLibrairie.records
{
    using JSONObject = JObject;

    public class StreamingTradeRecord : BaseResponseRecord
    {
        public double? Close_price { get; set; }

        public long? Close_time { get; set; }

        public bool? Closed { get; set; }

        public long? Cmd { get; set; }

        public string Comment { get; set; }

        public double? Commision { get; set; }

        public string CustomComment { get; set; }

        public long? Expiration { get; set; }

        public double? Margin_rate { get; set; }

        public double? Open_price { get; set; }

        public long? Open_time { get; set; }

        public long? Order { get; set; }

        public long? Order2 { get; set; }

        public long? Position { get; set; }

        public double? Profit { get; set; }

        public double? Sl { get; set; }

        public string State { get; set; }

        public double? Storage { get; set; }

        public string Symbol { get; set; }

        public double? Tp { get; set; }

        public STREAMING_TRADE_TYPE Type { get; set; }

        public double? Volume { get; set; }

        public int? Digits { get; set; }

        public void FieldsFromJSONObject(JSONObject value)
        {
            Close_price = (double?) value["close_price"];
            Close_time = (long?) value["close_time"];
            Closed = (bool?) value["closed"];
            Cmd = (long) value["cmd"];
            Comment = (string) value["comment"];
            Commision = (double?) value["commision"];
            CustomComment = (string) value["customComment"];
            Expiration = (long?) value["expiration"];
            Margin_rate = (double?) value["margin_rate"];
            Open_price = (double?) value["open_price"];
            Open_time = (long?) value["open_time"];
            Order = (long?) value["order"];
            Order2 = (long?) value["order2"];
            Position = (long?) value["position"];
            Profit = (double?) value["profit"];
            Type = new STREAMING_TRADE_TYPE((long) value["type"]);
            Sl = (double?) value["sl"];
            State = (string) value["state"];
            Storage = (double?) value["storage"];
            Symbol = (string) value["symbol"];
            Tp = (double?) value["tp"];
            Volume = (double?) value["volume"];
            Digits = (int?) value["digits"];
        }

        public override string ToString()
        {
            return "StreamingTradeRecord{" +
                   "symbol=" + Symbol +
                   ", close_time=" + Close_time +
                   ", closed=" + Closed +
                   ", cmd=" + Cmd +
                   ", comment=" + Comment +
                   ", commision=" + Commision +
                   ", customComment=" + CustomComment +
                   ", expiration=" + Expiration +
                   ", margin_rate=" + Margin_rate +
                   ", open_price=" + Open_price +
                   ", open_time=" + Open_time +
                   ", order=" + Order +
                   ", order2=" + Order2 +
                   ", position=" + Position +
                   ", profit=" + Profit +
                   ", sl=" + Sl +
                   ", state=" + State +
                   ", storage=" + Storage +
                   ", symbol=" + Symbol +
                   ", tp=" + Tp +
                   ", type=" + Type.Code +
                   ", volume=" + Volume +
                   ", digits=" + Digits +
                   '}';
        }

        public async void Update(StreamingTradeRecord traderecord)
        {
            Close_price = traderecord.Close_price;
            Close_time = traderecord.Close_time;
            Closed = traderecord.Closed;
            Cmd = traderecord.Cmd;
            Comment = traderecord.Comment;
            Commision = traderecord.Commision;
            //   this.commission_agent = traderecord.Commission_agent;
            CustomComment = traderecord.CustomComment;
            Digits = traderecord.Digits;
            Expiration = traderecord.Expiration;
            //this.expirationString = traderecord.Close_price;
            Margin_rate = traderecord.Margin_rate;
            Open_price = traderecord.Open_price;
            Open_time = traderecord.Open_time;
            // this.order = traderecord.Close_price;
            //this.order2 =traderecord.Close_price;
            //this.position = traderecord.Close_price;
            Profit = traderecord.Profit;
            Sl = traderecord.Sl;
            Storage = traderecord.Storage;
            Symbol = traderecord.Symbol;
            // this.timestamp = traderecord.Time;
            Tp = traderecord.Tp;
            //  this.value_date =traderecord.Va;
            Volume = traderecord.Volume;
        }
    }
}