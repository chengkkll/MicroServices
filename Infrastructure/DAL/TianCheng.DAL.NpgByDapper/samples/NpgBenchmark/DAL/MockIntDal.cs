﻿using NpgBenchmark.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.DAL.NpgByDapper;

namespace NpgBenchmark.DAL
{
    public class MockIntDal : IntOperation<MockIntDB>
    {
        protected override string TableName { get; set; } = "mock_serial";
        protected override string ConnectionString => "User ID=cheng;Password=123qwe;Host=192.168.0.16;Port=5432;Database=provider_manager;Pooling=true;";

    }
}
