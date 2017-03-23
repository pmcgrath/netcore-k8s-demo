using StackExchange.Redis;
using System;
using System.Collections.Generic;


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
        private readonly Dictionary<Guid, Models.Contact> _store = new Dictionary<Guid, Models.Contact>();


        public bool Delete(
            Guid id)
        {
            lock(this)
            {
                if (! this._store.ContainsKey(id)) { return false; }

                this._store.Remove(id);
                return true;
            }
        }


        public Models.Contact Get(
            Guid id)
        {
            return this._store[id];
        }


        public IEnumerable<Models.Contact> GetAll()
        {
            lock(this) { return this._store.Values; }
        }


        public void Upsert(
            Models.Contact value)
        {
            lock(this) { this._store[value.Id] = value; }
        }
    }


    public class RedisContactRepository : IContactRepository
    {
        private readonly ConnectionMultiplexer _connection;


        public RedisContactRepository()
        {
            this._connection = ConnectionMultiplexer.Connect("localhost");
        }


        public bool Delete(
            Guid id)
        {
            return this._connection.GetDatabase().KeyDelete(this.GenerateRedisKey(id));
        }


        public Models.Contact Get(
            Guid id)
        {
            var hash = this._connection.GetDatabase().HashGetAll(this.GenerateRedisKey(id));

            return null;
        }


        public IEnumerable<Models.Contact> GetAll()
        {
            // PENDING - Ask Hamish
            var id = Guid.NewGuid();
            var hash = this._connection.GetDatabase().HashGetAll(this.GenerateRedisKey(id));

            return null;
        }


        public void Upsert(
            Models.Contact value)
        {
            this._connection.GetDatabase().HashSet(
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
