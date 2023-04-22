using MeetingManagement.Application.Exceptions;

namespace MeetingsManagement.UnitTests
{
    public class UserServiceTests
    {
        private IUserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;
        private string _userId;
        [SetUp]
        public void Setup()
        {
            _userId = Guid.NewGuid().ToString();
            _userRepositoryMock = new Mock<IUserRepository>();
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
        public void GetUserEntity_UserNotFound_ThrowsError()
        {
            _userRepositoryMock.Setup(x => x.GetAsync(_userId)).ReturnsAsync((UserEntity?)null);

            Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.GetUserEntity(_userId) );
        }

        [Test]
        public async Task GetUserEntity_UserFound_ReturnUserEntity()
        {
            _userRepositoryMock.Setup(x => x.GetAsync(_userId)).ReturnsAsync(new UserEntity { Id = new Guid(_userId) });
            var user = await _userService.GetUserEntity(_userId);

            Assert.That(user, Is.TypeOf<UserEntity>());
            Assert.That(user.Id.ToString(), Is.EqualTo(_userId));
        }
    }
}