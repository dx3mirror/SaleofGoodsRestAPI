using Microsoft.EntityFrameworkCore;
using Moq;
using ProductSalesEntity.Entity;
using ProductSalesRepository.Repository;
using Xunit;

namespace XUnitTestRepository
{
    public class TestSaleRepository
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSalesWithProductsAndTrackingNumbers()
        {
            // Arrange
            var sales = new List<Sale>
        {
            new Sale { SaleId = 1, Product = new Product { ProductId = 1, }, TrackingNumbers = new List<TrackingNumber> { new TrackingNumber { } } },
            new Sale { SaleId = 2, Product = new Product { ProductId = 2, }, TrackingNumbers = new List<TrackingNumber> { new TrackingNumber { } } },
            new Sale { SaleId = 3, Product = new Product { ProductId = 3, }, TrackingNumbers = new List<TrackingNumber> { new TrackingNumber { } } },
            // add more test data
        };

            var mockContext = new Mock<ProductSalesContext>();
            var mockDbSet = new Mock<DbSet<Sale>>();

            mockDbSet.As<IQueryable<Sale>>().Setup(m => m.Provider).Returns(sales.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Sale>>().Setup(m => m.Expression).Returns(sales.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Sale>>().Setup(m => m.ElementType).Returns(sales.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Sale>>().Setup(m => m.GetEnumerator()).Returns(sales.AsQueryable().GetEnumerator());

            mockContext.Setup(c => c.Sales).Returns(mockDbSet.Object);

            var repository = new SaleRepository(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(sales.Count, result.Count());

            // Add more assertions to ensure that products and tracking numbers are loaded for each sale
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSaleByIdWithProductAndTrackingNumbers()
        {
            // Arrange
            var saleId = 1;
            var sale = new Sale { SaleId = saleId, Product = new Product { ProductId = 1, /* other properties */ }, TrackingNumbers = new List<TrackingNumber> { new TrackingNumber { /* tracking number properties */ } } };

            var mockContext = new Mock<ProductSalesContext>();
            var mockDbSet = new Mock<DbSet<Sale>>();

            mockDbSet.Setup(m => m.FindAsync(saleId)).ReturnsAsync(sale);

            mockContext.Setup(c => c.Sales).Returns(mockDbSet.Object);

            var repository = new SaleRepository(mockContext.Object);

            // Act
            var result = await repository.GetByIdAsync(saleId);

            // Assert
            Assert.NotNull(result);
          
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistingSale()
        {
            // Arrange
            var saleId = 99;

            var mockContext = new Mock<ProductSalesContext>();
            var mockDbSet = new Mock<DbSet<Sale>>();

            mockDbSet.Setup(m => m.FindAsync(saleId)).ReturnsAsync((Sale)null);

            mockContext.Setup(c => c.Sales).Returns(mockDbSet.Object);

            var repository = new SaleRepository(mockContext.Object);

            // Act
            var result = await repository.GetByIdAsync(saleId);

            // Assert
            Assert.Null(result);
         
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNegativeSaleId()
        {
            // Arrange
            var saleId = -1; // Negative sale ID

            var mockContext = new Mock<ProductSalesContext>();
            var repository = new SaleRepository(mockContext.Object);

            // Act
            var result = await repository.GetByIdAsync(saleId);

            // Assert
            Assert.Null(result);
            // Add more assertions if needed
        }

        [Fact]
        public async Task AddAsync_ShouldAddSaleToDatabase()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Sale>>();
            var mockContext = new Mock<ProductSalesContext>();
            mockContext.Setup(c => c.Sales).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var repository = new SaleRepository(mockContext.Object);
            var sale = new Sale { SaleId = 1, Product = new Product { ProductId = 1, /* other properties */ }, TrackingNumbers = new List<TrackingNumber> { new TrackingNumber { /* tracking number properties */ } } };

            // Act
            await repository.UpdateAsync(sale);

            // Assert
            mockSet.Verify(m => m.Update(sale), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateSaleInDatabase()
        {
            // Arrange
            var mockContext = new Mock<ProductSalesContext>();
            var mockSet = new Mock<DbSet<Sale>>();
            var repository = new SaleRepository(mockContext.Object);
            var sale = new Sale { SaleId = 1, Product = new Product { ProductId = 1, /* other properties */ }, TrackingNumbers = new List<TrackingNumber> { new TrackingNumber { /* tracking number properties */ } } };

            mockContext.Setup(c => c.Sales).Returns(mockSet.Object);
            mockSet.Setup(m => m.Update(It.IsAny<Sale>()));
            mockContext.Setup(c => c.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            // Act
            await repository.UpdateAsync(sale);

            // Assert
            mockSet.Verify(m => m.Update(sale), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }





        [Fact]
        public async Task DeleteAsync_ShouldDeleteSaleFromDatabase()
        {
            // Arrange
            var mockContext = new Mock<ProductSalesContext>();
            var mockSet = new Mock<DbSet<Sale>>();
            var repository = new SaleRepository(mockContext.Object);
            var saleId = 1;

            mockContext.Setup(c => c.Sales).Returns(mockSet.Object);
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync(new Sale())
                .Callback<object[]>(ids => mockSet.Setup(s => s.FindAsync(ids)).ReturnsAsync((Sale)null));

            // Act
            await repository.DeleteAsync(saleId);

            // Assert
            mockSet.Verify(m => m.Remove(It.IsAny<Sale>()), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }







    }
}
