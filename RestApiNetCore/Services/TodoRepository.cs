using RestApiNetCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestApiNetCore.Services
{
    public class TodoRepository : ITodoRepository
    {
        private List<TodoModels> _todoList;

        public TodoRepository()
        {
            InitializeData();
        }

        public IEnumerable<TodoModels> All
        {
            get { return _todoList; }
        }

        public void Delete(string id)
        {
            _todoList.Remove(this.Find(id));
        }

        public bool DoesItemExist(string id)
        {
            return _todoList.Any(item => item.ID == id);
        }

        public TodoModels Find(string id)
        {
            var item = _todoList.FirstOrDefault(item => item.ID == id);
            return item;
        }

        public void Insert(TodoModels item)
        {
            _todoList.Add(item);
        }

        public void Update(TodoModels item)
        {
            var todoItem = this.Find(item.ID);
            var index = _todoList.IndexOf(todoItem);
            _todoList.RemoveAt(index);
            _todoList.Insert(index, item);
        }

        private void InitializeData()
        {
            _todoList = new List<TodoModels>();

            var todoItem1 = new TodoModels
            {
                ID = "6bb8a868-dba1-4f1a-93b7-24ebce87e243",
                Name = "Learn app development",
                Notes = "Attend Xamarin University",
                Done = true
            };

            var todoItem2 = new TodoModels
            {
                ID = "b94afb54-a1cb-4313-8af3-b7511551b33b",
                Name = "Develop apps",
                Notes = "Use Xamarin Studio/Visual Studio",
                Done = false
            };

            var todoItem3 = new TodoModels
            {
                ID = "ecfa6f80-3671-4911-aabe-63cc442c1ecf",
                Name = "Publish apps",
                Notes = "All app stores",
                Done = false,
            };

            _todoList.Add(todoItem1);
            _todoList.Add(todoItem2);
            _todoList.Add(todoItem3);
        }
    }
}
