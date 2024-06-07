The entry point of the program shall look like:


```csharp

    public class Program
    {
        public static void Main(string[] args)
        {
            List<ICabs> cabs = new List<Cabs>
            {
                new Cabs("Evan's Cab"),
            };
    
            Console.WriteLine("Emma's cab company will pickup and deliver a customer.");
            EmmaCabCompany.CallCab(cabs, new Customer("Darrell", "1 Fulton Drive", "1 University Avenue"));
            
            Console.WriteLine("");
            
            Console.WriteLine("Emma's cab company will pickup and deliver a customer.");
            EmmaCabCompany.CallCab(cabs, new Customer("Diane", "1 Fulton Drive", "1 University Avenue"));
    
            
            // Emma's cab company will pickup and deliver a customer.
            // Evan's Cab picked up Darrell at 1 Fulton Drive
            // Evan's Cab dropped off Darrell at 1 University Avenue
            
            // Emma's cab company will pickup and deliver a customer.
            // Evan's Cab picked up Diane at 1 Fulton Drive
            // Evan's Cab dropped off Diane at 1 University Avenue
        }
    }
```
