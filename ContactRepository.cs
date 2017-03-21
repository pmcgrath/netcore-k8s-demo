using System;
using System.Collections.Generic;


namespace webapi
{
    public interface IContactRepository
    {
        bool Delete(
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
}