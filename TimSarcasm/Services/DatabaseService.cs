using Discord;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TimSarcasm.Models;

namespace TimSarcasm.Services
{
    public enum DatabaseType
    {
        SQLite,
        MySQL,
        InMemory
    }
    public class DatabaseService : DbContext
    {
        private DatabaseType DatabaseType { get; set; }
        private string DatabaseConfigString { get; set; }
        private LogService Logger { get; }

        public DatabaseService(LogService logger)
        {
            this.Logger = logger;
        }
        // Used for EF cli tools
        public DatabaseService()
        {
            var config = new ConfigurationService();

            Configure(config.Config.DatabaseType, config.Config.DatabaseConnectionString);
        }
        public void Configure(string databaseType, string configurationString)
        {
            if (Logger != null)
                Logger.Log(new LogMessage(LogSeverity.Info, "DatabaseService",
                    "Configuring " + databaseType + " database with \"" + configurationString + "\""));
            switch (databaseType.ToLower())
            {
                case "sqlite":
                    DatabaseType = DatabaseType.SQLite;
                    break;
                case "mysql":
                    DatabaseType = DatabaseType.MySQL;
                    break;
                case "inmemory":
                    DatabaseType = DatabaseType.InMemory;
                    break;
                default:
                    throw new Exception("Unknown database type set.");
            }
            DatabaseConfigString = configurationString;
        }
        public DbSet<ServerProperties> ServerProperties { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PermissionEntry> PermissionEntries { get; set; }
        public DbSet<PermissionsGroup> PermissionGroups { get; set; }
        public DbSet<PermissionsGroupUser> PermissionsGroupUsers { get; set; }
        public DbSet<LoggedMessage> LoggedMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (Logger != null)
                Logger.Log(new LogMessage(LogSeverity.Info, "DatabaseService", "Connecting to database"));
            switch (DatabaseType)
            {
                case DatabaseType.SQLite:
                    options.UseSqlite(DatabaseConfigString);
                    break;
                case DatabaseType.MySQL:
                    options.UseInMemoryDatabase(DatabaseConfigString);
                    break;
                case DatabaseType.InMemory:
                    options.UseMySql(DatabaseConfigString);
                    break;
            }
        }
    }
}
