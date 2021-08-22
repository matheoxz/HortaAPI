using Npgsql;
using System.Collections.Generic;
using System;
using HortaIoT.Models;

namespace HortaIoT.Repository{

    public class DbContext : IDbContext
    {
        NpgsqlConnection connection;

        public DbContext(string connectionString){
            connection = new NpgsqlConnection(connectionString);
        }

        public NpgsqlConnection GetConnection()
        {
            return connection;
        }
    }
}