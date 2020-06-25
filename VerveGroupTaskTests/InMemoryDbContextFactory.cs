using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using StaffApp.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerveGroupTask.Tests
{
    public class InMemoryDbContextFactory
    {
        public TempDB GetTempDB()
        {
            var options = new DbContextOptionsBuilder<TempDB>()
            .UseInMemoryDatabase(databaseName: "VerveGroupTaskDb")
            .Options;
            var dbContext = new TempDB(options);

            return dbContext;
        }
    }
}
