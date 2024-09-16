using Microsoft.AspNetCore.Mvc;
using Task.Models;

namespace Task.Controllers;

[ApiController]
[Route("/rent")]
public class RentController : Controller
{
    IRentRepositories _rentRepositories { get; set; } 
    private bool firstInit = false;                         // First init flag for first time add info
    public RentController(IRentRepositories repositories)
    {
        _rentRepositories = repositories;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddNewHall(Hall hall)
    {

        try
        {
            if (hall == null)
                return BadRequest();

            if (firstInit == true)          // First init condition
            {
                await _rentRepositories.AddNewHallAsync(new Hall() { Id = Guid.NewGuid(), Name = "Hall A", Capacity = 50, BasePrice = 2000 });
                await _rentRepositories.AddNewHallAsync(new Hall() { Id = Guid.NewGuid(), Name = "Hall B", Capacity = 100, BasePrice = 3500 });
                await _rentRepositories.AddNewHallAsync(new Hall() { Id = Guid.NewGuid(), Name = "Hall C", Capacity = 30, BasePrice = 1500 });

                await _rentRepositories.AddNewAddon(new HallAddon() { Id = Guid.NewGuid(), Name = "Projector", Price = 500 });
                await _rentRepositories.AddNewAddon(new HallAddon() { Id = Guid.NewGuid(), Name = "WI-FI", Price = 300 });
                await _rentRepositories.AddNewAddon(new HallAddon() { Id = Guid.NewGuid(), Name = "Sound", Price = 800 });

                return Json(new { key = "ID: ", value = hall.Id });
            }
            else
            {
                var createNewHall = await _rentRepositories.AddNewHallAsync(hall);      // Add new Hall 
                return Json(new { key = "ID: ", value = hall.Id });
            }
        }
        catch (Exception ex)
        {

            return Json(new { key = "Exception: ", value = ex.Message });
        }
    }


    [HttpPut]
    public async Task<IActionResult> ChangeHall(Hall hall)
    {

        try
        {
            if (hall == null)
                return BadRequest();

            await _rentRepositories.ChangeHallAsync(hall);              // Change info about hall

            return Json(new { key = "Result: ", value = "Confirm" });
        }
        catch (Exception ex)
        {
            return Json(new { key = "Exception: ", value = ex.Message });
        }
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> DeleteHall(Guid hallId)
    {

        try
        {
            if (hallId == Guid.Empty)
                return BadRequest();

            await _rentRepositories.DeleteHallAsync(hallId);            // Delete hall from table

            return Json(new { key = "Result: ", value = "Confirm" });
        }
        catch (Exception ex)
        {
            return Json(new { key = "Exception: ", value = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> SearchHall(DateTime startTime, DateTime endTime, DateTime date, int capacity)
    {

        try
        {
            var hallList = await _rentRepositories.SearchHallAsync(startTime,endTime,date,capacity);        // Search hall by data, time and capacity

            return Json(new { key = "Result: ", value = hallList });
        }
        catch (Exception ex)
        {
            return Json(new { key = "Exception: ", value = ex.Message });
        }
    }

    [HttpPost("Book")]
    public async Task<IActionResult> BookHall(RentTime rentTime)
    {

        try
        {
            if (rentTime == null)
                return BadRequest();

            var hall = await _rentRepositories.BookHallAsync(rentTime);         // Booking hall 

            return Json(new { key = "Result: ", value = hall });
        }
        catch (Exception ex)
        {
            return Json(new { key = "Exception: ", value = ex.Message });
        }
    }
}
