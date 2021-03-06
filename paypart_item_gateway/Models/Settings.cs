﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_item_gateway.Models
{
    public enum Status
    {
        Pending,
        Active,
        InActive
    }
    public class Settings
    {
        public string mongoUrl;
        public string connectionString;
        public string database;
        public string brokerList;
        public string addBillerTopic;
        public int redisCancellationToken;
    }
}
