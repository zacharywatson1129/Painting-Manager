using Autofac.Extras.Moq;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaintingLibrary.Tests
{
    public class SqliteDataAcessTests
    {
        [Fact]
        public void GetAllPaintings_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDataAccess>()
                    .Setup(x => x.loadAllPaintings())
                    .Returns(LoadSamplePaintings());

                var cls = mock.Create<SqliteDataAccess>();
                var expected = LoadSamplePaintings();
                var actual = cls.loadAllPaintings();

                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);
            }
        }

        private List<Category> LoadCategories()
        {
            List<Category> output = new List<Category>()
            {
                new Category() {ID = 1, Name="Moutains"},
                new Category() {ID = 2, Name="Streams"},
                new Category() {ID = 3, Name="Winter Scences"},
                new Category() {ID = 4, Name="Country Scences"}
            };
            return output;
        }

        private List<Painting> LoadSamplePaintings()
        {
            var categories = LoadCategories();
            List<Painting> output = new List<Painting>()
            {
                new Painting
                {
                    Id = 1,
                    DatePainted = new DateTime(1999, 8, 6),
                    FileName = "img1234.jpeg",
                    Categories = new List<Category> { categories[0] },
                    Width = 20,
                    Height = 20,
                    Name = "Mountain by the Sea",
                    PaintingSurface = SurfaceType.Canvas
                },
                new Painting
                {
                    Id = 2,
                    DatePainted = new DateTime(2020, 8, 6),
                    FileName = "img1235.jpeg",
                    Categories = new List<Category> { categories[2], categories[3] },
                    Width = 50,
                    Height = 30,
                    Name = "Creek in the Country",
                    PaintingSurface = SurfaceType.Panel
                },
                new Painting
                {
                    Id = 3,
                    DatePainted = new DateTime(2023, 8, 6),
                    FileName = "img1236.jpeg",
                    Categories = new List<Category> { categories[2] },
                    Width = 16,
                    Height = 20,
                    Name = "Snowy Hill",
                    PaintingSurface = SurfaceType.Canvas
                }
            };
            return output;
        }
    }
}
