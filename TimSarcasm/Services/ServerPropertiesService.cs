using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimSarcasm.Models;

namespace TimSarcasm.Services
{
    public class ServerPropertiesService
    {
        private DatabaseService DbService { get; set; }
        public ServerPropertiesService(DatabaseService dbService)
        {
            DbService = dbService;
        }

        public ServerProperties GetProperties(ulong serverId)
        {
            ServerProperties entity;
            var results = DbService.ServerProperties.Where(sp => sp.ServerId == serverId);
            if (results.Count() == 0)
            {
                entity = new ServerProperties()
                {
                    ServerId = serverId
                };
                entity = DbService.ServerProperties.Add(entity).Entity;
                DbService.SaveChanges();
                return entity;
            }
            return results.First();
        }
        public ServerProperties UpdateProperties(ServerProperties entity)
        {
            entity = DbService.ServerProperties.Update(entity).Entity;
            DbService.SaveChanges();
            return entity;
        }
    }
}
