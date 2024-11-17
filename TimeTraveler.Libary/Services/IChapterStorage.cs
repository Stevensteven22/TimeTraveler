using System.Linq.Expressions;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.Services;

public interface IChapterStorage
{
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task<Chapter> GetChapterAsync(int id);

    Task<IList<Chapter>> GetChaptersAsync(
        Expression<Func<Chapter, bool>> where,
        int skip,
        int take
    );
}
