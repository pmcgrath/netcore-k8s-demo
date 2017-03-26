using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace webapi
{
    public interface IContactRepository
    {
        bool Delete(
            Guid id);

        Models.Contact Get(
            Guid id);

        IEnumerable<Models.Contact> GetAll();

        void Save(
            Models.Contact value);
    }


    public class InMemoryContactRepository : IContactRepository
    {
        private readonly ConcurrentDictionary<Guid, Models.Contact> _store = new ConcurrentDictionary<Guid, Models.Contact>();


        public bool Delete(
            Guid id)
        {
            Models.Contact _;
            return this._store.TryRemove(id, out _);
        }


        public Models.Contact Get(
            Guid id) => this._store[id];


        public IEnumerable<Models.Contact> GetAll() => this._store.Values;


        public void Save(
            Models.Contact value) => this._store[value.Id] = value;
    }


    public class RedisContactRepository : IContactRepository
    {
        private readonly string _keyPrefix;
        private readonly ConnectionMultiplexer _connection;


        private IDatabase GetDB(
            int index = 0) => this._connection.GetDatabase(index);


        public RedisContactRepository(
            string configuraton = "127.0.0.1")      // See https://github.com/StackExchange/StackExchange.Redis/issues/410
        {
            this._connection = ConnectionMultiplexer.Connect(configuraton);

            this._keyPrefix = $"{this.GetType().Name}:";
        }


        public bool Delete(
            Guid id)
        {
            return this.GetDB().KeyDelete(this.GenerateRedisKey(id));
        }


        public Models.Contact Get(
            Guid id)
        {
            var hash = this.GetDB().HashGetAll(this.GenerateRedisKey(id));
            if (hash == null) { return null; }

            return new Models.Contact
                {
                    Id = id,
                    Name = hash.First(item => item.Name == nameof(Models.Contact.Name)).Value,
                    MobileNumber = hash.First(item => item.Name == nameof(Models.Contact.Name)).Value,
                };
        }


        public IEnumerable<Models.Contact> GetAll()
        {
            return this._connection
                .GetEndPoints()
                .Select(endpoint => this._connection.GetServer(endpoint))
                .SelectMany(server => server.Keys(pattern: $"{this._keyPrefix}*"))
                .Select(key => this.Get(this.ExtractIdFromRedisKey(key)));
        }


        public void Save(
            Models.Contact value)
        {
            this.GetDB().HashSet(
                this.GenerateRedisKey(value.Id),
                new HashEntry[]
                {
                    new HashEntry(nameof(Models.Contact.Name), value.Name),
                    new HashEntry(nameof(Models.Contact.Name), value.MobileNumber),
                });
        }


        private string GenerateRedisKey(
            Guid id) => $"{this._keyPrefix}{id}";


        private Guid ExtractIdFromRedisKey(
            string key) => Guid.Parse(key.Substring(this._keyPrefix.Length));
    }
}
