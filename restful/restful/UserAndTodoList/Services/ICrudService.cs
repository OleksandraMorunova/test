using MongoDB.Bson;
using restful.UserAndTodoList.Model;

namespace restful.Auth.UserAndTodoList.Services
{
    public interface ICrudService
    {
        public Task<UserModel> GetById(string id);
        public Task<List<UserModel>> GetListUserByUserName(string username);
        public Task Create(UserModel user);
        public Task<bool> Update(string id, UserModel user);
        public Task<bool> Delete(string id);
        public Task<bool> Upload(IFormFile file, string id);
        public Task<byte[]> FindFileByIdAsync(string objectId);
        public Task<bool> DeleteIcon(string id);
    }
}