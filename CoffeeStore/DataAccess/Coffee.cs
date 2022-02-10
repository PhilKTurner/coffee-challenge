using System.ComponentModel.DataAnnotations;

namespace CoffeeChallenge.CoffeeStore.DataAccess;

public class Coffee
{
    [Key]
    public int Id { get; set; }

    public int Inventory { get; set; }
}
