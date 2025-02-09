using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.UnitTests.ServiceTests
{
    public abstract class ServiceTestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }

                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                _dbContext = new ApplicationDbContext(options);
                return _dbContext;
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
