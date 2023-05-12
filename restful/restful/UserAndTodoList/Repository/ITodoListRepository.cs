using MongoDB.Bson;
using restful.UserAndTodoList.Model;

namespace restful.Auth.Repository
{
    public interface ITodoListRepository
    {
        public Task<IEnumerable<TodoListModel>> GetAllAsync(string id);
        public Task<TodoListModel> GetById(ObjectId id);
        public Task<bool> DeleteAsync(ObjectId id);
        public Task CreateAsync(ObjectId id, TodoListModel model);
        public Task<bool> UpdateAsync(ObjectId id, TodoListModel model);
    }
}
