using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCompute
{
    class Program
    {
        static void Main(string[] args)
        {

            string inputFile = Path.Combine(Directory.GetCurrentDirectory(), "Test1.txt");

            // Read input from a file
            List<string> inputLines = new List<string>();
            using (StreamReader file = new StreamReader(inputFile))
            {
                string strLine;
                while ((strLine = file.ReadLine()) != null)
                {
                    inputLines.Add(strLine);
                }
                file.Close();
            }

            // Display input to console
            foreach (var str in inputLines)
            {
                Console.WriteLine(str);
            }

            Console.WriteLine();

            // Process input line by line 
            List<GoodsItem> myGoods = new List<GoodsItem>();

            for (int nIndex = 0; nIndex < inputLines.Count; nIndex++)
            {
                //Replace "Input" with "Output" and display 
                if (inputLines[nIndex].Contains("Input"))
                {
                    string tempString = inputLines[nIndex].Replace("Input", "Output");
                    inputLines[nIndex] = tempString;

                    Console.WriteLine(inputLines[nIndex]);
                }
                else    // Process input line having goods item
                {
                    // Process valid item 
                    if (inputLines[nIndex].Count() > 0)
                    {
                        GoodsItem item = new GoodsItem();
                        item.strInputDescription = inputLines[nIndex];
                        item.processInpuAndComputeTax();

                        myGoods.Add(item);
                    }
                    else // we reached end of Items group and display computed output 
                    {

                        DispayOutput(myGoods);

                        myGoods.Clear();
                    }

                }

            }

            // We reached end of file and display computed output for last items group
            DispayOutput(myGoods);

            myGoods.Clear();

            Console.ReadKey();

        }

        static void DispayOutput(List<GoodsItem> myItems)
        {

            double salesTax = 0;
            double totalPrice = 0;

            foreach (var goodsItem in myItems)
            {
                Console.WriteLine(goodsItem.strInputDescription);

                salesTax = salesTax + goodsItem.taxAmount;
                totalPrice = totalPrice + goodsItem.priceAfterTax;
            }

            Console.WriteLine("Sales Taxes: " + String.Format("{0:f2}", salesTax));
            Console.WriteLine("Total: " + String.Format("{0:f2}", totalPrice));

            Console.WriteLine();


        }

    }

    class GoodsItem
    {
        int numberOfItems;
        double priceBeforeTax;
        int taxRate;
        public double priceAfterTax;
        public double taxAmount;

        public string strInputDescription;

        public void processInpuAndComputeTax()
        {
            string[] arrTaxFreeItems = { "chocolate","chocolates","book","pills"};

            string[] inputString = strInputDescription.Split(' ');
            int nCount = inputString.Count();
            this.numberOfItems = Convert.ToInt32(inputString[0]);
            this.priceBeforeTax = Convert.ToDouble(inputString[nCount-1]);
            this.taxRate = 0;

            List<string> myList = inputString.ToList();
            List<string> myTaxFreeList = arrTaxFreeItems.ToList();

            // check if item is not book, food, medicines and apply 10% tax
            if ( ! myList.Any(x => myTaxFreeList.Any(y => y == x)))
            {
                this.taxRate += 10;
            }

            // check if item is imported and add additional 5 % tax
            if( myList.Contains("imported"))
            {
                this.taxRate += 5;
            }

            this.taxAmount = (priceBeforeTax * taxRate) / 100;

            // round taxAmount to nearest 0.05 (ie. 1/0.05 = 20)
            if (this.taxAmount != 0)
                this.taxAmount = Math.Ceiling( this.taxAmount * 20 ) / 20;

            this.priceAfterTax = this.priceBeforeTax + this.taxAmount;

            // formatting value to have two digits after decimal
            inputString[nCount - 1] = String.Format("{0:f2}", priceAfterTax);

            // update items string description with computed value
            this.strInputDescription = String.Join(" ", inputString);

        }
    }

}
