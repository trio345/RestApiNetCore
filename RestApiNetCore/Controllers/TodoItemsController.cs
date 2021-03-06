﻿using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiNetCore.Filters;
using RestApiNetCore.Models;
using RestApiNetCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace RestApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILoggerManager _logger;
        /*private ILoggerManager _logger;*/
        public TodoItemsController(ITodoRepository todoRepository, ILoggerManager logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult List()
        {
            _logger.LogInfo("Trying to fetch all data Todo Items");
            
            return Ok(_todoRepository.All);
        }

        [HttpGet("GetDetails/{id}")]
        public IActionResult ListDetail(string id)
        {
            var item = _todoRepository.Find(id);
            if (item == null)
            {
                var message = string.Format("Todo item with id = {0} not found", id);
                return NotFound(message);
                /*var message = string.Format("Product with id = {0} not found", id);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);*/
            }

            return Ok(item);
        }

        [NotImplExceptionFilter]
        public IActionResult GetTodo(string id)
        {
            throw new NotImplementedException("This method is not implemented");
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoModels item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    _logger.LogError("Todo Item Name And Notes Required");
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequires.ToString());
                }
                bool itemExists = _todoRepository.DoesItemExist(item.ID);
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.TodoItemInUse.ToString());
                }

                _logger.LogInfo("Todo Item inserted");
                _todoRepository.Insert(item);
            } catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
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
