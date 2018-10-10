using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XOProject.Tests
{
   internal static class DbSetExtension
    {
        public static void MockDbSet<T>(this Mock<DbSet<T>> mockSet, IEnumerable<T> source) where T:class
        {
            var list = source.AsQueryable();
            mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetEnumerator())
            .Returns(new TestUseAsyncEnumerator<T>(list.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestUseAsyncQueryProvider<T>(list.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(list.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(list.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
        }
    }


}
