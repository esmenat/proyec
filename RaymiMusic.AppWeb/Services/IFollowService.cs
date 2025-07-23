using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Services
{
    public interface IFollowService
    {
        Task<Follow> GetFollowByUserAndArtistAsync(Guid userId, Guid artistId);
        Task DeleteFollowAsync( int followId);
        Task CreateFollowAsync(Follow follow);

    }
}
