using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace PhenixGenerator
{
    internal class Program
    {
        private static Guid GenerateSeededGuid(Random rand)
        {
            byte[] numArray = new byte[16];
            rand.NextBytes(numArray);
            return new Guid(numArray);
        }

        private static void Main(string[] args)
        {
            var dateTime1 = new DateTime(2019, 1, 1);
            bool exit = false;
            Options option = null;
            ParserResult<Options> parserResult = Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                option = o;
            }).WithNotParsed(e =>
            {
                Console.WriteLine(e);
                exit = true;
            });

            string folderName = option.FolderName;
            int storeCount = option.StoreCount;
            int productCount = option.ProductCount;
            int txCount = option.TransactionCount;
            long dayCount = option.DayCount;
            int seed = option.Seed;
            bool calculateSize = option.CalculateSize;

            if (exit)
                return;
            decimal num1 = 71L * txCount * dayCount + 12L * productCount * storeCount * dayCount;
            Console.WriteLine($"Estimated size : {num1:##.##}{GetUnit(ref num1)}");
            if (calculateSize)
                return;
            Directory.CreateDirectory(folderName + "/reference/");
            Directory.CreateDirectory(folderName + "/transactions/");
            var rand = new Random();
            if (seed != 0)
                rand = new Random(seed);
            var referenceList1 = new List<Reference>();
            for (int index = 0; index < productCount; ++index)
                referenceList1.Add(new Reference()
                {
                    Price = rand.Next(100000),
                    ProductId = index
                });
            var guidList = new List<Guid>();
            for (int index = 0; index < storeCount; ++index)
                guidList.Add(GenerateSeededGuid(rand));
            var dateTime2 = dateTime1;
            for (int index1 = 0; index1 < dayCount; ++index1)
            {
                Console.WriteLine($"{DateTime.Now:s} - Starting day {index1} on {dayCount}.");
                int num2 = 0;
                foreach (Guid guid in guidList)
                {
                    Console.Write($"\r{DateTime.Now:s} - Operation 1/2 - {num2 * 100 / storeCount}%");
                    var referenceList2 = new List<Reference>();
                    foreach (var reference in referenceList1)
                    {
                        int num3 = (int)Math.Round(reference.Price * ((rand.Next(40) - 20) / 100M + 1M));
                        referenceList2.Add(new Reference()
                        {
                            Price = num3,
                            ProductId = reference.ProductId
                        });
                    }
                    var store = new Store()
                    {
                        DateTime = dateTime2,
                        References = referenceList2,
                        StoreId = guid
                    };
                    File.WriteAllLines(folderName + "/reference/" + store.GetFileName(), store.GetContent());
                }
                Console.WriteLine($"\r{DateTime.Now:s} - Operation 1/2 - 100%");
                TransactionList transactionList = new TransactionList()
                {
                    DateTime = dateTime2,
                    Txs = new List<Transaction>()
                };
                for (int index2 = 0; index2 < txCount; ++index2)
                {
                    if (index2 % 10 == 0)
                        Console.Write($"\r{DateTime.Now:s} - Operation 2/2 - {index2 * 100 / txCount}%");
                    int index3 = rand.Next(productCount);
                    int index4 = rand.Next(storeCount);
                    Guid guid = guidList[index4];
                    Reference reference = referenceList1[index3];
                    int num3 = reference.Price >= 1000 ? (reference.Price >= 10000 ? rand.Next(1, 2) : rand.Next(1, 5)) : rand.Next(1, 15);
                    transactionList.Txs.Add(new Transaction()
                    {
                        DateTime = dateTime2,
                        ProductId = index3,
                        StoreId = guid,
                        Quantity = num3,
                        TxId = index2
                    });
                }
                Console.WriteLine($"\r{DateTime.Now:s} - Operation 2/2 - 100%");
                File.WriteAllLines(folderName + "/transactions/" + transactionList.GetFileName(), transactionList.GetContent());
                dateTime2 = dateTime2.AddDays(1.0);
            }
            Console.WriteLine("END");
        }

        private static string GetUnit(ref decimal num1)
        {
            string unit = "octets";
            if (num1 > 1024M)
            {
                num1 /= 1024M;
                unit = "Ko";
                if (num1 > 1024M)
                {
                    num1 /= 1024M;
                    unit = "Mo";
                    if (num1 > 1024M)
                    {
                        num1 /= 1024M;
                        unit = "Go";
                        if (num1 > 1024M)
                        {
                            num1 /= 1024M;
                            unit = "To";
                        }
                    }
                }
            }

            return unit;
        }

        private class Options
        {
            [Option('o', "outputFolder", Default = "out", HelpText = "Output folder name.")]
            public string FolderName { get; set; }

            [Option('s', "storeCount", Default = 120, HelpText = "Number of store.")]
            public int StoreCount { get; set; }

            [Option('p', "productCount", Default = 30000, HelpText = "Number of product.")]
            public int ProductCount { get; set; }

            [Option('t', "transactionCount", Default = 150000, HelpText = "Number of transaction per day.")]
            public int TransactionCount { get; set; }

            [Option('d', "dayCount", Default = 30, HelpText = "Number of day to generate.")]
            public int DayCount { get; set; }

            [Option('r', "randomSeed", Default = 0, HelpText = "The seed for the random generator (0 for time-dependant default seed value).")]
            public int Seed { get; set; }

            [Option('c', "calculateSize", Default = false, HelpText = "Get the estimated size for given parameters.")]
            public bool CalculateSize { get; set; }
        }
    }
}
