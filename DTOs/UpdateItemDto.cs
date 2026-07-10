using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs;

public class UpdateItemDto
{
    [Required]
    [MinLength(1)]
    public string Task { get; init; } = "";

    [Range(0, 1)]
    public int Completed { get; init; }
}
