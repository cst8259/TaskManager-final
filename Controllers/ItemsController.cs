using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess;
using TaskManager.DTOs;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly TaskManagerContext _context;

    public ItemsController(TaskManagerContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        var response = new ItemDto
        {
            Id = item.Id,
            Task = item.Task,
            Completed = item.Completed,
            ToDoListId = item.ToDoListId
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(CreateItemDto request)
    {
        var listExists = await _context.ToDoLists
            .AnyAsync(l => l.Id == request.ToDoListId);

        if (!listExists)
        {
            return BadRequest(new { error = "Invalid ToDoListId" });
        }

        var item = new Item
        {
            Task = request.Task,
            Completed = 0, // new items are always incomplete 
            ToDoListId = request.ToDoListId
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        var response = new ItemDto
        {
            Id = item.Id,
            Task = item.Task,
            Completed = item.Completed,
            ToDoListId = item.ToDoListId
        };

        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateItem(int id, UpdateItemDto updatedItem)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        item.Task = updatedItem.Task;
        item.Completed = updatedItem.Completed;
        await _context.SaveChangesAsync();

        var response = new ItemDto
        {
            Id = item.Id,
            Task = item.Task,
            Completed = item.Completed,
            ToDoListId = item.ToDoListId
        };

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
