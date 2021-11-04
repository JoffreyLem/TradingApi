using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsoleTables;
using Modele;
using Utility;

namespace Strategy.MoneyManagement
{
    public class Result
    {
        private const int rec = 5;

        public Result()
        {
            Positions = new List<Position>();
        }

        public Result(double balance)
        {
            Balance = balance;
            Positions = new List<Position>();
        }

        public Result(ref List<Position> closedOrder)
        {
            Positions = closedOrder;
        }

        public Result(List<Position> closedOrder)
        {
            Positions = closedOrder;
        }

        public Log Log { get; set; }
        public double Balance { get; set; }

        public List<Position> Positions { get; set; }

        public double? GetProfit(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            return Math.Round(selectedPosition.Sum(x => x.Profit.GetValueOrDefault()), 0);
        }

        public double? GetProfitPositif(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = selectedPosition.Select(x => x.Profit.GetValueOrDefault()).Where(x => x > 0).DefaultIfEmpty()
                .Sum();
            return Math.Round(data, 0);
        }

        public double? GetProfitNegatif(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = selectedPosition.Select(x => x.Profit.GetValueOrDefault()).Where(x => x < 0).DefaultIfEmpty()
                .Sum();

            return Math.Round(data, 0);
        }

        public double? GetMoyenneProfit(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = selectedPosition.Select(x => x?.Profit ?? 0).ToList();
            if (data.Any()) return Math.Round(data.Average(x => x), 0);
            return 0;
        }

        public int GetTotalPosition(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            return selectedPosition.Count;
        }

        public double? GetPositionPositive(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = (double) selectedPosition.Count(position => position.Profit > 0);
            return Math.Round(data, 0);
        }

        public double? GetPositionNegative(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = (double) selectedPosition.Count(position => position.Profit < 0);
            return Math.Round(data, 0);
        }


        public double? GetMoyennePositive(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = selectedPosition.Where(x => x.Profit > 0).Select(x => x?.Profit ?? 0).ToList();
            if (data.Count() > 0) return Math.Round(data.Average(x => x), 0);
            return 0;
        }

        public double? GetMoyenneNegative(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = selectedPosition.Where(x => x.Profit < 0).Select(x => x?.Profit ?? 0).ToList();
            if (data.Count() > 0) return Math.Round(data.Average(x => x), 0);
            return 0;
        }

        public double? GetRatioMoyennePositifNegatif(List<Position> positions = null)
        {
            return GetMoyennePositive(positions) / GetMoyenneNegative(positions);
        }

        public double? GetGainMax(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = selectedPosition.Max(x => x.Profit);
            return Math.Round(data.GetValueOrDefault(), 2);
        }

        public double? GetPerteMax(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var data = selectedPosition.Min(x => x.Profit);
            return Math.Round(data.GetValueOrDefault(), 2);
        }


        public double? GetTauxReussite(List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            var positif = GetPositionPositive(selectedPosition);
            var negatif = GetPositionNegative(selectedPosition);
            var sum1 = positif + negatif;
            var tx1 = (double) positif / sum1 * 100;
            if (sum1 > 0) return Math.Round((double) tx1, 4);
            return 0;
        }

        public double? GetProfitFactor(List<Position> positions = null)
        {
            var profitPositif = positions is null ? GetProfitPositif() : GetProfitPositif(positions);
            var profitNegatif = positions is null ? GetProfitNegatif() : GetProfitNegatif(positions);
            return profitPositif / Math.Abs(profitNegatif ?? 1);
        }

        public double? GetDrawndownMax(double? balance, List<Position> positions = null)
        {
            var selectedPosition = positions ?? Positions;
            if (selectedPosition.Count > 1)
                try
                {
                    var max = selectedPosition.Max(x => x.Profit);
                    var posMax = selectedPosition.Where(x => x.Profit == max).First();
                    var dataMAx = selectedPosition.Where(x => x.DateClose <= posMax.DateClose).Sum(x => x.Profit) +
                                  balance;
                    var min = selectedPosition.Where(x => x.DateClose > posMax.DateClose).Min(x => x.Profit);
                    var posMin = selectedPosition.Where(x => x.Profit == min).First();
                    var dataMin = selectedPosition.Where(x => x.DateClose <= posMin.DateClose).Sum(x => x.Profit) +
                                  balance;
                    return (dataMAx - dataMin) / dataMAx * 100;
                }
                catch (InvalidOperationException)
                {
                    return 0;
                }

            return 0;
        }


        public string GetTableProfit(List<Position> positions = null, string columnName = null)
        {
            var tableProfit = new ConsoleTable("",
                "Profit Global",
                "Profit Positif",
                "Profit Négatif");
            tableProfit.AddRow(columnName ?? "Global",
                GetProfit(positions) + "e",
                GetProfitPositif(positions) + "e",
                GetProfitNegatif(positions) + "e");
            var test = tableProfit.ToString();

            var message = "";
            message += "\n";
            message += new string('-', rec) + "TABLE PROFIT" + new string('-', rec);
            message += "\n";
            message += tableProfit.ToString();
            message += new string('-', rec) + "TABLE PROFIT" + new string('-', rec);
            message += "\n";
            return message;
        }

        public string GetTablePositions(List<Position> positions = null, string columnName = null)
        {
            var tablePosition = new ConsoleTable("",
                "Total Position",
                "Position Positive",
                "Position Negative");

            tablePosition.AddRow(columnName ?? "Global",
                GetTotalPosition(positions),
                GetPositionPositive(positions),
                GetPositionNegative(positions));


            var message = "";
            message += "\n";
            message += new string('-', rec) + "TABLE POSITIONS" + new string('-', rec);
            message += "\n";
            message += tablePosition.ToString();
            message += new string('-', rec) + "TABLE POSITIONS" + new string('-', rec);
            message += "\n";
            return message;
        }

        public string GetTablePerformance(double? balance, List<Position> positions = null, string columnName = null)
        {
            var tablePerf = new ConsoleTable("",
                "Taux Reussite",
                "Profit Factor",
                "Drawndown Max",
                "Moyenne global",
                "Moyenne Positive",
                "Moyenne negative",
                "Ratio Gain Perte Moyen",
                "Gain Max",
                "Perte Max");

            tablePerf.AddRow(columnName ?? "Global",
                GetTauxReussite(positions),
                GetProfitFactor(positions),
                GetDrawndownMax(balance, positions),
                GetMoyenneProfit(positions) + "e",
                GetMoyennePositive(positions) + "e",
                GetMoyenneNegative(positions) + "e",
                GetRatioMoyennePositifNegatif(positions) + "e",
                GetGainMax(positions) + "e",
                GetPerteMax(positions) + "e");


            var message = "";
            message += "\n";
            message += new string('-', rec) + "TABLE PERFORMANCES" + new string('-', rec);
            message += "\n";
            message += tablePerf.ToString();
            message += new string('-', rec) + "TABLE PERFORMANCES" + new string('-', rec);
            message += "\n";
            return message;
        }

        public string GetData(double? balance, List<Position> positions = null, string columnName = null)
        {
            var message = "";
            message += "\n";
            var column = columnName ?? "Global";
            message += new string('#', rec) + column + new string('#', rec);
            message += "\n";
            message += GetTableProfit(positions);
            message += GetTablePositions(positions);
            message += GetTablePerformance(balance, positions);
            message += new string('#', rec) + column.Length + new string('#', rec);
            message += "\n";
            return message;
        }


        public void PrintResult(double? balance, List<Position> positions = null, string columnName = null)
        {
            var message = "";
            message += GetData(balance, positions, columnName);
            message += GetDetailledResult(balance);


            Log.WriteResultData(message);
        }

        public string GetDetailledResult(double? balance)
        {
            var split = Positions.GroupBy(x => new {x.DateOpen.Year, x.DateOpen.Month})
                .OrderBy(x => x.Key.Year)
                .ThenBy(x => x.Key.Month)
                .ToList();
            var message = "";
            message += "\n";
            for (var i1 = 0; i1 < split.Count; i1++)
            {
                var cnt = split.Count();
                var data = split[i1].Select(x => x).ToList();
                var month = split[i1].Key.Month;
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                var columnName = monthName + split[i1].Key.Year;
                message += GetData(balance, data, columnName);
            }

            return message;
        }
    }

    public enum TypeResult
    {
        simple,
        detailled
    }
}