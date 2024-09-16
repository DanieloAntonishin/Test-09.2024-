using System.ComponentModel.DataAnnotations.Schema;

namespace Task.Models;

public class Hall : Entity
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public double BasePrice { get; set; }
    public bool IsRented { get; set; } = false;
    [ForeignKey("HallAddon")]
    public List<HallAddon> Addon { get; set; }
}
