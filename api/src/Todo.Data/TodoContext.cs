﻿using Microsoft.EntityFrameworkCore;
using Todo.Data.Models;

namespace Todo.Data;

public class TodoContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }
}