﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickleAndHope.Models
{
    public class Order
    {
        public List<LineItem> Items { get; set; }
        public string Recipient { get; set; }
        public decimal TotalPrice { get; set; }
        public int UserId { get; set; }
    }

    public class LineItem
    {
        public string PickleType { get; set; }
        public int Quantityt { get; set; }
        public string Message { get; set; }
    }
}
