using restful.UserAndTodoList.Model;

namespace restful.UserAndTodoList.Services
{
    public interface ITodoListService
    {
        public Task<IEnumerable<TodoListModel>> GetAllAsync(string id);
        public Task<TodoListModel> GetById(string id);
        public Task<bool> DeleteAsync(string id);
        public Task CreateAsync(string id, TodoListModel model);
        public Task<bool> UpdateAsync(string id, TodoListModel model);
    }
}
