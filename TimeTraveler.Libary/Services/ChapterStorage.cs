using System.Linq.Expressions;
using SQLite;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.Services;

public class ChapterStorage : IChapterStorage
{
    public const int NumberChapter = 6;

    public const string DbName = $"{nameof(TimeTraveler)}db.sqlite3";

    public static readonly string TimeTravelerDbPath = PathHelper.GetLocalFilePath(DbName);

    private SQLiteAsyncConnection _connection;

    private SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(TimeTravelerDbPath);

    private readonly IPreferenceStorage _preferenceStorage;

    public ChapterStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }

    public bool IsInitialized =>
        _preferenceStorage.Get(ChapterStorageConstant.VersionKey, default(int))
        == ChapterStorageConstant.Version;

    public async Task InitializeAsync()
    {
        await using var dbFileStream = new FileStream(TimeTravelerDbPath, FileMode.OpenOrCreate);
        await using var dbAssetStream =
            typeof(ChapterStorage).Assembly.GetManifestResourceStream(DbName)
            ?? throw new Exception($"Manifest not found: {DbName}");
        await dbAssetStream.CopyToAsync(dbFileStream);

        _preferenceStorage.Set(ChapterStorageConstant.VersionKey, ChapterStorageConstant.Version);
    }

    public Task<Chapter> GetChapterAsync(int id) =>
        Connection.Table<Chapter>().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IList<Chapter>> GetChaptersAsync(
        Expression<Func<Chapter, bool>> where,
        int skip,
        int take
    ) => await Connection.Table<Chapter>().Where(where).Skip(skip).Take(take).ToListAsync();

    public async Task CloseAsync() => await Connection.CloseAsync();
}

public static class ChapterStorageConstant
{
    public const string VersionKey = nameof(ChapterStorageConstant) + "." + nameof(Version);

    public const int Version = 1;
}
