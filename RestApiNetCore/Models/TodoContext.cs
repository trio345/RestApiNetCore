using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApiNetCore.Models;

namespace RestApiNetCore.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) :base(options)
        {

        }

        public DbSet<TodoItems> TodoItems { get; set; }

        public DbSet<RestApiNetCore.Models.Book> Book { get; set; }

    }
}
