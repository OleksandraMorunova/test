using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restful.UserAndTodoList.Model;
using restful.UserAndTodoList.Services;

namespace restful.UserAndTodoList.Controller

{
    [ApiController]
    [Route("v2")]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _service;

        public TodoListController(ITodoListService service)
        {
            _service = service;
        }

        [HttpPost("/todo/create")]
        [Authorize]
        public async Task CreateAsync(TodoListModel model)
        {
            await _service.CreateAsync(model);
        }

        [HttpDelete("/todo/delete/{id}")]
        [Authorize]
        public async Task<bool> DeleteAsync([FromRoute] string id)
        {
            return await _service.DeleteAsync(id);
        }

        [HttpGet("/todo/get/list/{id}")]
        public async Task<IEnumerable<TodoListModel>> GetAllAsync([FromRoute] string id)
        {
            return await _service.GetAllAsync(id);
        }

        [HttpGet("/todo/get/{id}")]
        [Authorize]
        public async Task<TodoListModel> GetById([FromRoute] string id)
        {
            return await _service.GetById(id);
        }

        [HttpPut("/todo/update/{id}")]
        [Authorize]
        public async Task<bool> UpdateAsync([FromRoute] string id, TodoListModel model)
        {
            return await _service.UpdateAsync(id, model);
        }
    }
}