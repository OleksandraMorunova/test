using MongoDB.Bson;
using restful.Auth.Repository;
using restful.UserAndTodoList.Model;

namespace restful.UserAndTodoList.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly ITodoListRepository _repository;

        public TodoListService(TodoListRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(string id, TodoListModel model)
        {
            await _repository.CreateAsync(parseStringToObjectId(id), model);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(parseStringToObjectId(id));
        }

        public async Task<IEnumerable<TodoListModel>> GetAllAsync(string id)
        {
            return await _repository.GetAllAsync(id);
        }

        public async Task<TodoListModel> GetById(string id)
        {
            return await _repository.GetById(parseStringToObjectId(id));
        }

        public async Task<bool> UpdateAsync(string id, TodoListModel model)
        {
            return await _repository.UpdateAsync(parseStringToObjectId(id), model);
        }

        private ObjectId parseStringToObjectId(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Inputing data is not string");
            }
            return objectId;
        }
    }
}
