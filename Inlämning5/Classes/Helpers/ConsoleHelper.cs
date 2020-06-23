using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Inlämning5.Classes
{
    public class ConsoleHelper
    {
    
    
    
    }
    public static class MongoDbExt
    {
        public static bool Ping(this IMongoDatabase db, int secondToWait = 5)
        {
            if (secondToWait <= 0)
                throw new ArgumentOutOfRangeException("secondToWait", secondToWait, "Must be at least 1 second");

            return db.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}").Wait(secondToWait * 1000);
        }
    }


}
