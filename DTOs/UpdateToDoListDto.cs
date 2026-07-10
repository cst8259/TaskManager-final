using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs;

public class UpdateToDoListDto
{
    [Required]
    [MinLength(3)]
    public string Title { get; init; } = "";
}
