using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
namespace App_Redis
{
    class Program
    {
        private static IDatabase redisCache { get; set; }
        static void Main(string[] args)
        {
            redisCache = ConnectToRedis();


            RedisKey surnameKey = new RedisKey("Surname");

            redisCache.StringSet(new RedisKey(surnameKey), new RedisValue("Smith"));
            // Overwriting 
            redisCache.StringSet(new RedisKey(surnameKey), new RedisValue("Johnson"));
            redisCache.StringSet(new RedisKey(surnameKey), new RedisValue("Williams"));
            redisCache.StringSet(new RedisKey(surnameKey), new RedisValue("Garcia"));
            redisCache.StringSet(new RedisKey("name"), new RedisValue("Bob"));
            redisCache.StringSet(new RedisKey("age"), new RedisValue("20"));
            redisCache.StringSet(new RedisKey("email"), new RedisValue("bobik@redis.com"));


            RedisValue value = redisCache.StringGet(surnameKey);

            //Incerement 
            RedisKey salaryKey = new RedisKey("Salary");
            redisCache.StringSet(salaryKey, new RedisValue("5000"));

            redisCache.StringIncrement(salaryKey); //5001
            RedisValue salaryvalue=redisCache.StringGet(salaryKey);
            Console.WriteLine(salaryvalue);

            salaryvalue=redisCache.StringIncrement(salaryKey, 50); //5051
            Console.WriteLine(salaryvalue);

            RedisValue oldValue= redisCache.StringGetSet(surnameKey, new RedisValue("Trump"));
            Console.WriteLine(oldValue); // "Garcia"


            //redis -  GETRANGE
            RedisValue name= redisCache.StringGetRange(new RedisKey("email"), 0, 4);
            Console.WriteLine(name);
            RedisValue emailFull=redisCache.StringGetRange(new RedisKey("email"), 0, -1);
            Console.WriteLine(emailFull);
            RedisValue dotCom = redisCache.StringGetRange(new RedisKey("email"), -3, -1);
            Console.WriteLine(dotCom);
            RedisValue redis = redisCache.StringGetRange(new RedisKey("email"), -9, -5);
            Console.WriteLine(redis);


            redisCache.ListRightPush(new RedisKey("dbs"),
                                     new RedisValue[] {
                                         new RedisValue("SQL"),
                                         new RedisValue("Mongo"),
                                         new RedisValue("Redis"),
                                         new RedisValue("Cosmos"),
                                     }
                                     );

            redisCache.KeyExpire(new RedisKey("dbs"), DateTime.Now.AddSeconds(10));

            IEnumerable<RedisValue> values= redisCache.ListRange(new RedisKey("dbs"));
            foreach(var item in values)
                Console.WriteLine(item);

            Thread.Sleep(15000);

            values = redisCache.ListRange(new RedisKey("dbs"));
            foreach (var item in values)
                Console.WriteLine(item);

            Console.ReadLine();
        }

        private static IDatabase ConnectToRedis()
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { "127.0.0.1:6379" },
                Password = "ghlijyan",
                Ssl = false
            };
            ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(options);
            IDatabase conn = muxer.GetDatabase();
            return conn;
        }
    }

    

}
