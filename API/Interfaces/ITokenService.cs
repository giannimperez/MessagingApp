using API.Models;

namespace API.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT containing the specified username.
        /// </summary>
        /// <param name="user">Username to be in token.</param>
        /// <returns>JWT containing username.</returns>
        string CreateToken(User user);

        /// <summary>
        /// Returns the username contained within the provided JWT.
        /// </summary>
        /// <param name="jwt">JWT containing username.</param>
        /// <returns>Username inside JWT.</returns>
        string GetUsernameFromAuthHeader(string jwt);
    }
}
