using MongoDB.Bson;
using MongoDB.Driver;
using restful.Auth;
using restful.Auth.UserAndTodoList.Services;
using restful.UserAndTodoList.Model;
using restful.UserAndTodoList.Repository;
using System;

namespace restful.UserAndTodoList.Services
{
    public class CrudService : ICrudService
    {
        private readonly ICrudRepository _repository;
        private readonly ILogger<UserModel> _logger;

        public CrudService(ICrudRepository repository, ILogger<UserModel> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<UserModel> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Inputing data is not string");
            }
            _logger.LogInformation("Attempt get user bi id");
            return await _repository.GetById(objectId);
        }

        public async Task<List<UserModel>> GetListUserByUserName(string username)
        {
            return await _repository.GetListUserByUsername(username);
        }

        public async Task Create(string userId, UserModel user)
        {
            UserModel newModel = new UserModel();
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Email is null, but it shouldn't be");
            }
            await _repository.Create(parseStringToObjectId(id), user);
        }

        public async Task<bool> Upload(IFormFile file, string id)
        {
            return await _repository.Upload(file, parseStringToObjectId(id));
        }

        public async Task<byte[]> FindFileByIdAsync(string id)
        {
            return await _repository.FindFileByIdAsync(parseStringToObjectId(id));
        }

        public async Task<bool> Update(string id, UserModel user)
        {
            return await _repository.Update(parseStringToObjectId(id), user);
        }

        public async Task<bool> Delete(string id)
        {
            return await _repository.Delete(parseStringToObjectId(id));
        }

        public async Task<bool> DeleteIcon(string id)
        {
            return await _repository.DeleteIcon(parseStringToObjectId(id));
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
