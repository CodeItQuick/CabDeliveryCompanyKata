using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Domain;

// Aggregate Root Id
[PrimaryKey("Id")]
[Table("Fleet")]
public class Fleet
{
    public int Id = 1;
    [ForeignKey("Cab")] public virtual List<Cab> FleetOfCabs { get; set; }

    public Fleet()
    {
        FleetOfCabs = new();
    }


    public void CreateFleet(string[] cabList)
    {
        var cabListStrings = cabList
            .Select(x => x)
            .ToList();
        FleetOfCabs = new List<Cab>();
        foreach (var cab in cabListStrings)
        {
            string?[] cabAttributes = cab.Split(",");
            if (cabAttributes.Length < 1 || string.IsNullOrWhiteSpace(cabAttributes[0]))
            {
            }
            else
            {
                var cabValue = new Cab(cabAttributes[0], 20, 46.2382, 63.1311);
                if (!string.IsNullOrWhiteSpace(cabAttributes[1]))
                {
                    var customer = new Customer(
                        cabAttributes[1], 
                        cabAttributes[2], 
                        cabAttributes[3]);
                    cabValue.RequestRideFor(customer);
                }
                FleetOfCabs.Add(cabValue);
            }
        }
    }

    public void AddCab(Cab cab)
    {
        FleetOfCabs.Add(cab);
    }

    public void RemoveCab()
    {
        if (FleetOfCabs[^1].RideInProgress())
        {
            FleetOfCabs.RemoveAt(FleetOfCabs.Count - 1);
        }
    }

    public bool IsEnroute(Customer customer)
    {
        return FleetOfCabs.Any(x => x.CabInfo()?.PassengerName == customer.Name);
    }

    public void RideRequested(Customer customer)
    {
        var availableCabList = FleetOfCabs
            .Where(x => x.IsStatus(CabStatus.Available))
            .ToList();
        if (availableCabList.Count == 0)
        {
            throw new SystemException($"Dispatch failed to pickup {customer.Name} as there are no available cabs.");
        }
        Cab? assignedCab = null;
        foreach (var cab in availableCabList)
        {
            if (assignedCab != null)
            {
                if (cab.IsCloserThan(assignedCab, customer))
                {
                    assignedCab = cab;
                }

                continue;
            }
            
            assignedCab = cab;
        }
        assignedCab?.RequestRideFor(customer);
    }

    public void PickupCustomer(Customer customer)
    {
        if (FleetOfCabs.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }
        var enrouteCab = FleetOfCabs?.FirstOrDefault(x => x.IsEnrouteFor(customer));
        enrouteCab?.PickupAssignedCustomer(customer);
    }

    public void DropOffCustomer()
    {
        if (FleetOfCabs.Count == 0)
        {
            throw new SystemException("Cannot drop off customers as there are no cabs in the fleet");
        }

        var enrouteCab = FleetOfCabs.FirstOrDefault(x => x.IsStatus(CabStatus.TransportingCustomer));
        enrouteCab?.DropOffCustomer();
    }

    public bool NoCabsInFleet()
    {
        return FleetOfCabs.Count == 0;
    }

    public bool CustomersStillInTransport()
    {
        return FleetOfCabs.Any(x => x.ContainsPassenger());
    }

    public string? FindCab(Customer customer)
    {
        return FleetOfCabs.First(x => x.CabInfo()?.PassengerName == customer.Name).CabInfo()?.CabName;
    }

    public bool AllCabsOccupied()
    {
        return FleetOfCabs.All(x => x.ContainsPassenger());
    }

    public string[] ExportCabs()
    {
        return FleetOfCabs
            .Select(x => 
                $"{x.CabInfo()?.CabName}," +
                $"{x.CabInfo()?.PassengerName}," +
                $"{x.CabInfo()?.StartLocation}," +
                $"{x.CabInfo()?.Destination}")
            .ToArray();
    }

    public Cab SignedOutCab()
    {
        return FleetOfCabs.FirstOrDefault()!;
    }
}