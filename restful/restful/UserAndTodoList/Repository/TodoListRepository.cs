using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using restful.Auth.Config;
using restful.UserAndTodoList.Model;

namespace restful.Auth.Repository
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly IMongoCollection<TodoListModel> _mongoCollection;

        public TodoListRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _mongoCollection = database.GetCollection<TodoListModel>("todo_list");
        }


        public async Task CreateAsync(ObjectId id, TodoListModel model)
        {
            model.UserId = id;
            await _mongoCollection.InsertOneAsync(model);
        }

        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var result =  await _mongoCollection.DeleteOneAsync(d => d.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<TodoListModel>> GetAllAsync(string id)
        {
            return await _mongoCollection.Find(f => f.UserId.ToString() == id).ToListAsync();
        }

        public async Task<TodoListModel> GetById(ObjectId id)
        {
            return await _mongoCollection.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(ObjectId id, TodoListModel model)
        {   
            var exist = await GetById(id);
            var filter = Builders<TodoListModel>.Filter.Eq(s => s.Id, id);
            var update = Builders<TodoListModel>.Update.Combine()
            .Set(m => m.Title, model.Title ?? exist.Title)
            .Set(m => m.Description,model.Description ?? exist.Description);
              
            var result = await _mongoCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;                 
        }
    }
}
