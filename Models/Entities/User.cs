namespace BikeServiceAPI.Models.Entities;

public class User : Person
{
	public bool? Premium { get; set; } = null!;
	public List<Bike> Bikes { get; set; } = new List<Bike>();
	public List<Tour>? Tours { get; set; } = null;
	//public List<ServiceEvent> ServiceEvents { get; set; } = null!;
	public List<Transaction> TransactionHistory { get; set; } = null!;
	public List<Bike> InsuredBikes { get; set; } = new List<Bike>();

    public User(string name, string email, string password, string phone, string? introduction = null) : base(name, email, password, phone, introduction)
    {
	    Tours = new List<Tour>();
	    TransactionHistory = new List<Transaction>();
	    
    }
}