using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiNetCore.Models;
using RestApiNetCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        /*private ILoggerManager _logger;*/
        public TodoItemsController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;

        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_todoRepository.All);
        }

        [HttpGet("GetDetails/{id}")]
        public IActionResult ListDetail(string id)
        {
            var item = _todoRepository.Find(id);
            if (item == null)
            {
                return BadRequest(ErrorCode.RecordNotFound.ToString());
            }

            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoModels item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequires.ToString());
                }
                bool itemExists = _todoRepository.DoesItemExist(item.ID);
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.TodoItemInUse.ToString());
                }

                _todoRepository.Insert(item);
            } catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }

            return Ok(item);
        }


        public enum ErrorCode
        {
            TodoItemNameAndNotesRequires,
            TodoItemInUse,
            CouldNotCreateItem,
            RecordNotFound,
            CouldNotUpdateItem,
            CouldNotDeleteItem
        }

        [HttpPut]
        public IActionResult Edit([FromBody] TodoModels item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
                }

                var existingItem = _todoRepository.Find(item.ID);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _todoRepository.Update(item);
            } catch(Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var item = _todoRepository.Find(id);
                if (item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _todoRepository.Delete(id);
            } catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }

            return NoContent();
        }

    }
}
