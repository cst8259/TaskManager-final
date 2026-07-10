using System;
using System.Collections.Generic;

namespace TaskManager.DataAccess;

public partial class Item
{
    public int Id { get; set; }

    public string Task { get; set; } = null!;

    public int Completed { get; set; }

    public int ToDoListId { get; set; }

    public virtual ToDoList ToDoList { get; set; } = null!;
}
