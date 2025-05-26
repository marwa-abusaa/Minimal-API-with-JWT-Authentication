using JwtMinimalApi.Models;

namespace JwtMinimalApi.Services;

public class UserService
{
    private readonly List<User> users = new List<User>
    {
        new User { Id = 1,FirstName = "Marwa", LastName = "AbuSaa", Password = "1230", UserName = "marwaSaa"},
        new User { Id = 2,FirstName = "Aya", LastName = "Ba'ara", Password = "9870", UserName = "ayaBaara"}
    };
}
