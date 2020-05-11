using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UQ.Demo.Models;

namespace UQ.Demo.Services
{
    public interface ICosmosDbService<T> where T : IEntity
    {
        Task AddEntityAsync(T item);

        Task<IEnumerable<T>> GetEntitiesAsync(string whereClause);
    }
}
