using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaintingLibrary.Tests
{
    public class PaintingModelTests
    {

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(20)]
        public void TestWidthProperty_Normal(int width)
        {
            Painting p = new Painting();
            p.Width = width;
            Assert.Equal(width, p.Width);
        }


        [Theory]
        [InlineData(-10)]
        [InlineData(-20)]
        [InlineData(-25)]
        public void TestWidthProperty_NegativeWidthSetPositive(int width)
        {
            Painting p = new Painting();
            p.Width = width;
            Assert.Equal(-width, p.Width);
        }

    }
}