using Task.Models;

namespace Task;

public interface IRentRepositories
{
    Task<string> AddNewHallAsync(Hall hall);
    Task<string> ChangeHallAsync(Hall hall);
    Task<string> DeleteHallAsync(Guid hallId);
    Task<List<string>> SearchHallAsync(DateTime startTime, DateTime endTime, DateTime date, int capacity);
    Task<string> BookHallAsync(RentTime rentTime);
    Task<string> AddNewAddon(HallAddon addon);
}
