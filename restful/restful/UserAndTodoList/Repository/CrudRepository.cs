using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using restful.Auth.Config;
using restful.UserAndTodoList.Controller;
using restful.UserAndTodoList.Model;

namespace restful.UserAndTodoList.Repository
{
    public class UserRepository : ICrudRepository
    {
        private readonly IMongoCollection<UserModel> _mongoCollection;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CrudController> _logger;
        private readonly IGridFSBucket gridFS;

        public UserRepository(IOptions<MongoDBSettings> mongoDBSettings, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManage, ILogger<CrudController> logger)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _mongoCollection = database.GetCollection<UserModel>("main_user");
            gridFS = new GridFSBucket(database);
            _userManager = userManage;
            _logger = logger;
        }

        public async Task<UserModel> GetById(ObjectId id)
        {
            return await _mongoCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<UserModel>> GetListUserByUsername(string username)
        {
            return await _mongoCollection.Find(c => c.UserName == username).ToListAsync();
        }

        public async Task Create(UserModel user)
        {
            try
            {
                var us = await _userManager.FindByEmailAsync(user.Email);
                if (us != null) { _logger.LogError("This email already used"); }
                else
                {
                    var u = new CreatedRequest
                    {
                        Email = user.Email,
                        Password = user.Password,
                    };

                    us = new ApplicationUser
                    {
                        Email = u.Email,
                        UserName = u.Email,
                        FullName = u.Email,
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    };
                    var createUsers = await _userManager.CreateAsync(us, u.Password);

                    _logger.LogInformation("User created for login");
                    try
                    {
                        var addedToRole = await _userManager.AddToRoleAsync(us, user.Roles);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"{e.Message}");
                    }

                    var salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
                    user.Password = hashedPassword;
                    user.CreatedUserTime = DateTime.UtcNow;

                    await _mongoCollection.InsertOneAsync(user);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Exeption. User doesn't created for login", ex.ToString());
            }

        }

        public async Task<bool> Update(ObjectId id, UserModel user)
        {
            var exist = await GetById(id);
            if(user.Email != null)
            {
                var us = await _userManager.FindByEmailAsync(exist.Email);
                if (user.Password != null)
                {
                    var rp = await _userManager.RemovePasswordAsync(us);
                    var ap = await _userManager.AddPasswordAsync(us, user.Password);
                }
                if(us != null)
                {
                    us.UserName = user.Email ?? us.Email;
                    us.Email = user.Email ?? us.Email;
                    us.FullName = user.Email ?? us.Email;
                }

                var result = await _userManager.UpdateAsync(us);
            }
            if(user.Password != null && user.Email == null)
            {
                var us = await _userManager.FindByEmailAsync(exist.Email);
                if(us != null)
                {
                    var rp = await _userManager.RemovePasswordAsync(us);
                    var ap = await _userManager.AddPasswordAsync(us, user.Password);
                }
            }

            var filter = Builders<UserModel>.Filter.Eq(s => s.Id, id);
            if (user != null)
                {

                var update = Builders<UserModel>.Update.Combine()
                    .Set(u => u.UserName, user.UserName ?? exist.UserName)
                    .Set(u => u.Email, user.Email ?? exist.Email)
                    .Set(u => u.DateOfBirth, user.DateOfBirth.Equals(null) ? exist.DateOfBirth : user.DateOfBirth)
                    .Set(u => u.LinkToProfileIcon, user.LinkToProfileIcon ?? exist.LinkToProfileIcon);

               var result = await _mongoCollection.UpdateOneAsync(filter, update);
               return result.ModifiedCount > 0;
            }
            
            return false;
        }

        public async Task<bool> Delete(ObjectId id)
        {
            var result = await _mongoCollection.DeleteOneAsync(c => c.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteIcon(ObjectId id)
        {
            var filter = Builders<UserModel>.Filter.Eq(s => s.Id, id);
            if (filter != null)
            {
                var exist = await GetById(id);
                var update = Builders<UserModel>.Update.Combine()
                    .Set(u => u.LinkToProfileIcon, null);

                var result = await _mongoCollection.UpdateOneAsync(filter, update);
                await gridFS.DeleteAsync((ObjectId) exist.LinkToProfileIcon);
                return result.ModifiedCount > 0;

            }
            return false;
        }

        public async Task<bool> Upload(IFormFile file, ObjectId userId)
        {
            using MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            var id = await gridFS.UploadFromStreamAsync(file.FileName, ms);
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, userId);
            var update = Builders<UserModel>.Update.Set(u => u.LinkToProfileIcon, id);

            var result = await _mongoCollection.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<byte[]> FindFileByIdAsync(ObjectId objectId)
        {
            return await gridFS.DownloadAsBytesAsync(objectId);
        }     
    }
}