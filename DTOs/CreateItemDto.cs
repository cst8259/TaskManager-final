using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs;

public class CreateItemDto
{
    [Required]
    [MinLength(1)]
    public string Task { get; init; } = "";

    [Required]
    public int ToDoListId { get; init; }
}
