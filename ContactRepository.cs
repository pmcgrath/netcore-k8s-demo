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

        void Upsert(
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


        public void Upsert(
            Models.Contact value) => this._store[value.Id] = value;
    }


    public class RedisContactRepository : IContactRepository
    {
        private readonly ConnectionMultiplexer _connection;


        private IDatabase GetDB(
            int index = 0) => this._connection.GetDatabase(index);


        public RedisContactRepository()
        {
            this._connection = ConnectionMultiplexer.Connect("localhost");
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
            this._connection.GetEndPoints()[0].

            // PENDING - Ask Hamish
            var id = Guid.NewGuid();
            var hash = this.GetDB().HashGetAll(this.GenerateRedisKey(id));

            return null;
        }


        public void Upsert(
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
            Guid id) => $"{this.GetType().Name}:{id}";
    }
}
