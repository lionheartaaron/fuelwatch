using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GraphParse
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = File.ReadAllText(@"C:\Users\Me\Desktop\FuelWatchRetail-04-2014.csv\FuelWatchRetail-04-2014.csv");
            var csv = source.Split(',');
            var dataSourceCount = csv.Length / 10;
            var fuelWatchData = new List<FuelWatchData>(dataSourceCount);
            for (var i = 1; i < dataSourceCount; i++)
            {
                var offset = i * 10;
                if (csv[offset + 3] == "ULP")
                {
                    fuelWatchData.Add(new FuelWatchData()
                    {
                        Date = DateTime.Parse(csv[offset]),
                        ULPPrice = decimal.Parse(csv[offset + 4])
                    });
                }
            }
            var weekdays = new Dictionary<DayOfWeek, Aggregate>();
            foreach (var data in fuelWatchData)
            {
                if (weekdays.TryGetValue(data.Date.DayOfWeek, out Aggregate val))
                {
                    val.TotalSum += data.ULPPrice;
                    val.Count++;
                }
                else
                {
                    var newAggregate = new Aggregate() { Count = 1, TotalSum = data.ULPPrice };
                    weekdays[data.Date.DayOfWeek] = newAggregate;
                }
            }
            foreach (KeyValuePair<DayOfWeek, Aggregate> day in weekdays)
            {
                Console.WriteLine($"{day.Key} - {day.Value.Value}");
            }
            Console.ReadKey();
        }

        public class Aggregate
        {
            public decimal TotalSum;
            public int Count;
            public decimal Value => TotalSum / Count;
        }
        public class FuelWatchData
        {
            public DateTime Date;
            public decimal ULPPrice;
        }

    }
}
