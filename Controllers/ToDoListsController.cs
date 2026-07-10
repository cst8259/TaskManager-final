using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess;
using TaskManager.DTOs;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoListsController : ControllerBase
{
    private readonly TaskManagerContext _context;

    public ToDoListsController(TaskManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetLists()
    {
        var lists = await _context.ToDoLists.ToListAsync();
        var response = lists.Select(l => new ToDoListSummaryDto
            {
                Id = l.Id,
                Title = l.Title
            })
            .ToList();
        
        return Ok(response);
    }

     [HttpGet("{id:int}")]
    public async Task<IActionResult> GetList(int id)
    {
        var list = await _context.ToDoLists
            .FirstOrDefaultAsync(l => l.Id == id);

        if (list == null)
        {
            return NotFound();
        }

        var response = new ToDoListSummaryDto
        {
            Id = list.Id,
            Title = list.Title
        };

        return Ok(response);
    }

    [HttpGet("{id:int}/Items")]
    public async Task<IActionResult> GetListItems(int id)
    {
        var list = await _context.ToDoLists.AnyAsync(l => l.Id == id);

        if (!list)
        {
            return NotFound();
        }

        var items = await _context.Items
            .Where(i => i.ToDoListId == id)
            .ToListAsync();

        var response = items.Select(i => new ItemDto
            {
                Id = i.Id,
                Task = i.Task,
                Completed = i.Completed,
                ToDoListId = i.ToDoListId
            })
            .ToList();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateList(CreateToDoListDto request)
    {
        var list = new ToDoList
        {
            Title = request.Title
        };

        _context.ToDoLists.Add(list);
        await _context.SaveChangesAsync();

        var response = new ToDoListSummaryDto
        {
            Id = list.Id,
            Title = list.Title
        };

        return CreatedAtAction(nameof(GetList), new { id = list.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateList (int id, UpdateToDoListDto updatedList)
    {
        var list = await _context.ToDoLists.FindAsync(id);

        if (list == null)
        {
          return NotFound();
        }

        list.Title = updatedList.Title;
        await _context.SaveChangesAsync();

        var response = new ToDoListSummaryDto
        {
            Id = list.Id,
            Title = list.Title
        };

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteList (int id)
    {
        var list = await _context.ToDoLists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (list == null)
        {
          return NotFound();
        }

        if (list.Items != null && list.Items.Any())
        {
          return BadRequest(new { error = "List is not empty"});
        }

        _context.ToDoLists.Remove(list);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
