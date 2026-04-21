namespace ContentApi.DTO;

// sealing because none of the DTOs are intended to be inherited
public sealed record CreateUserRequest(string Username, string Password);



