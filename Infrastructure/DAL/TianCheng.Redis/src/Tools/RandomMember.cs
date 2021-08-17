using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Common;
using TianCheng.DAL;

namespace TianCheng.Redis.Tools
{
    public class RandomMember
    {
        private readonly DBConnectionOptions Connection;

        public RandomMember()
        {
            ConnectionProvider connectionProvider = ServiceLoader.GetService<ConnectionProvider>();
            Connection = connectionProvider.RedisDefault;
        }

        public bool Init(string setId, int[] arr)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            IRedisTypedClient<int> tab = client.As<int>();
            var st = tab.Sets[setId];

            foreach (int i in arr)
            {
                st.Add(i);
            }

            //foreach (byte val in arr)
            //{
            //    client.SAdd(setId, new byte[] { val });
            //}
            return true;
        }

        public List<int> Get(string setId, int length)
        {
            using RedisClient client = new RedisClient(Connection.ServerAddress);
            //IRedisTypedClient<int> tab = client.As<int>();
            //var st = tab.Sets[setId];
            //st.GetRandomItem();
            var ret = client.Custom(Commands.SRandMember, setId, length);

            List<int> result = new List<int>();
            foreach (var item in ret.Children)
            {
                result.Add(item.GetResult<int>());
            }
            return result;

            //var info = client.SRandMember(setId, length);
            //return info[0];
        }
    }
}
