using ContentApi.DTO;
using ContentApi.Services;

public interface IUserService : ICrudService<CreateUserRequest, UpdateUserRequest>
{
}
