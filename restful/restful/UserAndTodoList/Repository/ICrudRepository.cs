using MongoDB.Bson;
using restful.Auth;
using restful.UserAndTodoList.Model;

namespace restful.UserAndTodoList.Repository
{
    public interface ICrudRepository
    {
        public Task<UserModel> GetById(ObjectId id);
        public Task<List<UserModel>> GetListUserByUsername(string username);
        public Task Create(UserModel user);
        public Task<bool> Update(ObjectId id, UserModel user);
        public Task<bool> Delete(ObjectId id);
        public Task<bool> Upload(IFormFile file, ObjectId id);
        public Task<byte[]> FindFileByIdAsync(ObjectId objectId);
        public Task<bool> DeleteIcon(ObjectId id);
    }
}
