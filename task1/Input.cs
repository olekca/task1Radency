using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace task1
{
    class Input
    {
        public string firstName;
        public string lastName;
        public string city;
        public decimal payment;
        public DateTime date;
        public long accNumber;
        public string service;
        public static Input ParseInput(string s, string filePath)//parses string to object
        { 
            Input res = new Input();
            ++Program.ParsedStrings;
            try
            {               
                string[] arr = Regex.Split(s, "”*, *\"*“*");
                res.firstName = arr[0];
                res.lastName = arr[1];
                res.city = arr[2];
                res.payment = decimal.Parse(arr[5].Replace(".", ","));
                res.date = DateTime.ParseExact(arr[6], "yyyy-dd-MM", null);
                res.accNumber = long.Parse(arr[7]);
                res.service = arr[8];
                
            }
            catch(Exception e)
            {
                ++Program.FoundErrors;
                if (!Program.InvalidFiles.Contains(filePath)) {
                    Program.InvalidFiles.Add(filePath);
                }
                return null;
            }


            return res ;
        }
    }
    class City//I made those classes for grouping, but not understood how to add it to linq.
    {
        public string cityName;
        public decimal total;
        public List<Service> services;
        


    }
    class Service
    {
        public string serviceName;
        public decimal total;
        public List<Payer> payers;
    }

    class Payer
    {
        public string firstName;
        public string lastName;
        public decimal payment;
        public DateTime date;
        public long accNumber;
    }
}
