using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using restful.Auth.Config;
using restful.Auth.UserAndTodoList.Services;
using restful.UserAndTodoList.Model;

namespace restful.UserAndTodoList.Controller
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    public class CrudController : ControllerBase
    {
        private readonly ICrudService _service;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public CrudController(ICrudService service)
        {
            _service = service;
        }

        [HttpGet("/user/get/{id}")]
        [Authorize]
        public async Task<UserModel> GetById([FromRoute] string id)
        {
            return await _service.GetById(id);
        }

        [HttpGet("/user/get/list/{username}")]
        public async Task<List<UserModel>> GetListUserByUserName(string username)
        {
            return await _service.GetListUserByUserName(username);
        }

        [HttpPut("/file/upload/{id}")]
        [Authorize]
        public async Task<bool> Upload([FromRoute] string id, IFormFile file)
        {
            return await _service.Upload(file, id);
        }

        [HttpGet("/file/download/{id}")]
        [Authorize]
        public async Task<byte[]> FindFileByIdAsync([FromRoute] string id)
        {
            return await _service.FindFileByIdAsync(id);
        }

        [HttpPost("/user/create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UserModel userModel)
        {                  
            await _service.Create(userModel);
            return Ok("File uploaded successfully.");
        }

        [HttpPut("/user/update/{id}")]
        [Authorize]
        public async Task<bool> Update([FromRoute] string id, UserModel userModel)
        {
            return await _service.Update(id, userModel);
        }

        [HttpDelete("/user/delete/{id}")]
        [Authorize]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _service.Delete(id);
        }

        [HttpDelete("/user/delete/icon/{id}")]
        [Authorize]
        public async Task<bool> DeleteIcon([FromRoute] string id)
        {
            return await _service.DeleteIcon(id);
        }
    }
}