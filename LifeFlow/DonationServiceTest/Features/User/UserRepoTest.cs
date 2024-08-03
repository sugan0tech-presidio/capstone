using DonationService.Commons;
using DonationService.Features.User;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;

namespace DonationServiceTest.Features.User;

[TestFixture]
public class UserRepoTest
{
    private DbContextOptions<DonationServiceContext> _dbContextOptions;
    private DonationServiceContext _context;
    private UserRepo _userRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DonationServiceContext>()
            .UseInMemoryDatabase("MatrimonyTestDb")
            .Options;

        _context = new DonationServiceContext(_dbContextOptions);
        _userRepo = new UserRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetById_ShouldReturnEntity_WhenEntityExists()
    {
        // Arrange
        var user = new DonationService.Entities.User
        {
            Email = "user@example.com",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray(),
            Name = "John"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepo.GetById(user.Id);

        // Assert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual("user@example.com", result.Email);
        ClassicAssert.AreEqual("John", result.Name);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _userRepo.GetById(99));
        ClassicAssert.AreEqual("User with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        await _context.Users.AddRangeAsync(
            new DonationService.Entities.User
            {
                Email = "user1@example.com",
                PhoneNumber = "1234567890",
                IsVerified = true,
                Password = "password"u8.ToArray(),
                HashKey = "key"u8.ToArray(),
                Name = "test"
            },
            new DonationService.Entities.User
            {
                Email = "user2@example.com",
                PhoneNumber = "9876543210",
                IsVerified = true,
                Password = "password"u8.ToArray(),
                HashKey = "key"u8.ToArray(),
                Name = ""
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepo.GetAll();

        // Assert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var user = new DonationService.Entities.User
        {
            Email = "user@example.com",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray(),
            Name = ""
        };

        // Act
        var result = await _userRepo.Add(user);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual("user@example.com", result.Email);
        ClassicAssert.AreEqual(1, await _context.Users.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRepo.Add(null));
        ClassicAssert.AreEqual("User cannot be null. (Parameter 'entity')", ex.Message);
    }
}