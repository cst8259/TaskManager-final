using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.DataAccess;

public partial class TaskManagerContext : DbContext
{
    public TaskManagerContext()
    {
    }

    public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ToDoList> ToDoLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasOne(d => d.ToDoList).WithMany(p => p.Items)
                .HasForeignKey(d => d.ToDoListId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
