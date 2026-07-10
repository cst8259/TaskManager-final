namespace TaskManager.DTOs;

public class ItemDto
{
    public int Id { get; init; }
    public string Task { get; init; } = "";
    public int Completed { get; init; }
    public int ToDoListId { get; init; }
}
