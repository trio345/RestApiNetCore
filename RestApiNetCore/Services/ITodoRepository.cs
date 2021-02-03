using RestApiNetCore.Models;
using System.Collections.Generic;

namespace RestApiNetCore.Services
{
    public interface ITodoRepository
    {
        bool DoesItemExist(string id);
        IEnumerable<TodoModels> All { get; }
        TodoModels Find(string id);
        void Insert(TodoModels item);
        void Update(TodoModels item);
        void Delete(string id);
    }
}
