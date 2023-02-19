using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace task1
{//John, Doe, “Lviv, Kleparivska 35, 4”, 500.0, 2022-27-01, 1234567, Water
    //<first_name: string>, <last_name: string>, <address: string>,
    //<payment: decimal>, <date: date>, <account_number: long>, <service: string>
    class input
    {
        public string firstName;
        public string lastName;
        public string city;
        public decimal payment;
        public DateTime date;
        public long accNumber;
        public string service;
        public static input parseInput(string s)
        {
            input res = new input();
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
                return null;
            }


            return res ;
        }
    }
    class City
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
