using System;
using System.Collections.Generic;

namespace TaskManager.DataAccess;

public partial class ToDoList
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
