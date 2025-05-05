using System;
using MySql.Data.MySqlClient;
using DotNetEnv;

namespace dbConfig
{
    public static class DbConfig
    {
        static DbConfig()
        {
            // Load .env file (mặc định là ".env" ở root project)
            Env.Load();
        }

        public static MySqlConnection GetConnection()
        {
            string host = Env.GetString("DB_SERVER");
            string port = Env.GetString("DB_PORT");
            string name = Env.GetString("DB_NAME");
            string user = Env.GetString("DB_USER");
            string pass = Env.GetString("DB_PASS");

            string connectionString = $"Server={host};Port={port};Database={name};Uid={user};Pwd={pass};";
            // Console.WriteLine($"Connection String: {connectionString}");
            Console.WriteLine($"Connecting to database successfully!");
            return new MySqlConnection(connectionString);
        }
    }
}