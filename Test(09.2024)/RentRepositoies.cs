using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using Task.Models;

namespace Task;

public class RentRepositoies : IRentRepositories
{
    private MsSqlDbContext _context {  get; set; }

    public RentRepositoies(MsSqlDbContext context)              // DBcontext init
    {
        _context = context;
    }

    public async Task<string> AddNewHallAsync(Hall hall)        // Add method new Hall to DB
    {

        if (hall == null)
        {
            throw new ArgumentNullException(nameof(hall));
        }

        try
        {

            await _context.HallEntities.AddAsync(hall);        // Add new entity to Set
            _context.SaveChanges();
            return hall.Id.ToString();                       
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<string> AddNewAddon(HallAddon addon)       // Add method new Addons for Hall to DB
    {

        if (addon == null)
        {
            throw new ArgumentNullException(nameof(addon));
        }

        try
        {

            await _context.AddonsEntities.AddAsync(addon);
            _context.SaveChanges();
            return addon.Id.ToString();
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    async Task<string> IRentRepositories.BookHallAsync(RentTime rentTime)         // Book method to DB
    {
        if (rentTime.HallId == Guid.Empty)          // Hall isn`t exist 
        {
            throw new ArgumentNullException();
        }
        var hall = _context.HallEntities.Where(i=>i.Id == rentTime.HallId).FirstOrDefault();                    // Get hall by foreight key
        var book = _context.RentTimeEntities.Where(i => i.HallId == hall.Id).FirstOrDefault();                  // Get by book entity by hall id 
        if (book.Date.Date == rentTime.Date.Date && book.EndTime.TimeOfDay <= rentTime.StartTime.TimeOfDay)     // Check if hall can be ready to book by time
        {
            return "Hall allready booked";
        }
        else
        {
            var sum = hall.BasePrice;       //Hall base price 

            if (hall.Addon != null)
            {
                foreach (var add in hall.Addon)     // Looking for addons and sum their price too
                {
                    sum += _context.AddonsEntities.Where(i => i.Id == add.Id).First().Price;
                }
            }

            if (rentTime.StartTime.Hour >= 18 && rentTime.EndTime.Hour <= 23)           // Condition for discount or markup by time period
            {
                sum -= (sum * 20) / 100;
            }
            else if (rentTime.StartTime.Hour >= 6 && rentTime.EndTime.Hour <= 9)
            {
                sum -= (sum * 10) / 100;
            }
            else if (rentTime.StartTime.Hour >= 12 && rentTime.EndTime.Hour <= 14)
            {
                sum += (sum * 15) / 100;
            }

            hall.IsRented = true;                                           // Turn flag to rent for hall
            await _context.RentTimeEntities.AddAsync(rentTime);             
            _context.SaveChanges();
            return "Rent complete. Total amount = " + sum;
            throw new NotImplementedException();
        }
    }

    async Task<string> IRentRepositories.ChangeHallAsync(Hall hall)                    // Update method for hall in DB
    {
        if (hall.Id == Guid.Empty)
        {
            throw new ArgumentNullException();
        }
        var h = _context.HallEntities.Where(u => u.Id == hall.Id).FirstOrDefault();    // Get and change info for hall
        h.Name = hall.Name;
        h.Capacity = hall.Capacity;
        h.BasePrice = hall.BasePrice;
        h.IsRented = hall.IsRented;
        h.Addon = hall.Addon;
        await _context.SaveChangesAsync();
        return "Updated";
    }

    async Task<string> IRentRepositories.DeleteHallAsync(Guid hallId)                   // Delete hall method from DB
    {
        if (hallId == Guid.Empty)
        {
            throw new ArgumentNullException();
        }
        var hall = _context.HallEntities.Where(u => u.Id == hallId).FirstOrDefault();   // Find and remove information
        _context.HallEntities.Remove(hall);
        _context.SaveChanges();
        return "Deleted succesfully";
    }

    async Task<List<string>> IRentRepositories.SearchHallAsync(DateTime startTime, DateTime endTime, DateTime date, int capacity)       // Search hall by info 
    {
        var r = _context.RentTimeEntities.Where(e => e.StartTime.Hour >= startTime.Hour && e.EndTime.Hour <= endTime.Hour && 
        DateTime.Compare(e.Date.Date,date.Date)==0).ToList();                                                                           // Get List of Hall by search criteria

        List<string> hallNames = new List<string>();
        foreach(var l in  r)
        {
            hallNames.Add(_context.HallEntities.Where(i=>i.Capacity == capacity).FirstOrDefault().Name);            // Print search result
        }
        return hallNames;
    }
}
