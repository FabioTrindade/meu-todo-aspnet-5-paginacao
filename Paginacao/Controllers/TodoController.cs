using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paginacao.Data;
using Paginacao.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Paginacao.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet(template: "load")]
        public async Task<IActionResult> LoadAsync([FromServices] AppDbContext context)
        {
            for (int i = 0; i < 1348; i++)
            {
                var todo = new Todo()
                {
                    Id = i + 1,
                    Done = false,
                    CreatedAt = DateTime.Now,
                    Title = string.Format("Tarefa {0}", i)
                };

                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet(template:"bybalta/skip/{skip:int}/take/{take:int}")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context,
            [FromRoute] int skip = 0,
            [FromRoute] int take = 25)
        {           
            var total = await context.Todos.CountAsync();
            var todos = await context
                .Todos
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(new { 
                total,
                skip,
                take,
                data = todos 
            });
        }

        [HttpGet("byfabio/skip/{skip:int}/take/{take:int}")]
        public async Task<IActionResult> GetAsyncCustom([FromServices] AppDbContext context,
        [FromRoute] int skip = 0,
        [FromRoute] int take = 25)
        {
            var listTodos = await context.TodoViewModel
                .FromSqlRaw("SELECT Id, Title, Done, CreatedAt, COUNT() OVER() AS [Total] FROM TODOS")
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var todos = listTodos
                .ConvertAll(c => new Todo { Id = c.Id, Title = c.Title, Done = c.Done, CreatedAt = c.CreatedAt });
            
            return Ok(new
            {
                total = listTodos.FirstOrDefault()?.Total,
                skip,
                take,
                data = todos
            });
        }

    }
}


