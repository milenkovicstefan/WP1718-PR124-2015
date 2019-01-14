using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Parametri
    {
        private int filter;
        private int sort;
        private int allRides;

        public int Filter
        {
            get
            {
                return filter;
            }

            set
            {
                filter = value;
            }
        }

        public int Sort
        {
            get
            {
                return sort;
            }

            set
            {
                sort = value;
            }
        }

        public int AllRides
        {
            get
            {
                return allRides;
            }

            set
            {
                allRides = value;
            }
        }
    }
}