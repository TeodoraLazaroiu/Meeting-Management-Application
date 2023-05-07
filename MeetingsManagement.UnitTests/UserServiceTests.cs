using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Application.Exceptions;

namespace MeetingsManagement.UnitTests
{
    public class UserServiceTests
    {
        private IUserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;

        private readonly string _userId = Guid.NewGuid().ToString();
        private readonly string _email = "address@email.com";

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            _userRepositoryMock.Setup(x => x.GetAsync(_userId)).ReturnsAsync(new UserEntity { Id = new Guid(_userId) });
            _userRepositoryMock.Setup(x => x.GetUserByEmail(_email)).ReturnsAsync(new UserEntity { Email = _email });

            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Test]
        public async Task GetUserList_NoUsersFound_ReturnsEmptyList()
        {
            _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<UserEntity>());
            var users = await _userService.GetUserList();

            Assert.That(users, Is.Empty);
        }

        [Test]
        public async Task GetUserList_UsersFound_ReturnsUsersList()
        {
            var user = new UserEntity();
            _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<UserEntity>() { user, user, user});
            var users = await _userService.GetUserList();

            Assert.That(users, Has.Count.EqualTo(3));
        }

        [Test]
        public void GetUserEntity_UserNotFound_ShouldThrow()
        {
            _userRepositoryMock.Setup(x => x.GetAsync(_userId)).ReturnsAsync((UserEntity?)null);

            Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.GetUserEntity(_userId) );
        }

        [Test]
        public async Task GetUserEntity_UserFound_ReturnUserEntity()
        {
            var user = await _userService.GetUserEntity(_userId);

            Assert.That(user, Is.TypeOf<UserEntity>());
            Assert.That(user.Id.ToString(), Is.EqualTo(_userId));
        }
        
        [Test]
        public async Task CreateUser_ValidRequest_ShouldCreateUser()
        {
            var userRegister = new RegisterUserDTO()
            {
                Password = "P@ssw0rd",
                Email = _email
            };
            _userRepositoryMock.Setup(x => x.GetUserByEmail(_email)).ReturnsAsync((UserEntity?)null);

            var id = await _userService.RegisterUser(userRegister);

            _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<UserEntity>()), Times.Once, "User was no created");
        }
        [Test]
        public void CreateUser_UserAlreadyExists_ShouldThrow()
        {
            var userRegister = new RegisterUserDTO()
            {
                Password = "P@ssw0rd",
                Email = _email
            };

            Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await _userService.RegisterUser(userRegister));
            _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<UserEntity>()), Times.Never, "User was created");
        }


    }
}