using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

// public class DispatchControllerTests
// {
//     private readonly CabContext _cabContext;
//
//     public DispatchControllerTests()
//     {
//         _cabContext = new CabContext(
//             new DbContextOptionsBuilder<CabContext>()
//                 .UseSqlite($"Data Source={Guid.NewGuid()}.db").Options);
//         _cabContext.Database.Migrate();
//         EnsureFleetExistsForSingleUser();
//         EnsureMenuExistsForSingleUser();
//     }
//
//     private void EnsureFleetExistsForSingleUser()
//     {
//         var fleetExists = _cabContext.Fleet.Any(x => x.Id == 1);
//         if (fleetExists) return;
//         _cabContext.Fleet.Add(new Fleet() { Id = 1 });
//         _cabContext.SaveChanges();
//     }
//
//     private void EnsureMenuExistsForSingleUser()
//     {
//         var fleetExists = _cabContext.Menues.Any(x => x.Id == 1);
//         if (fleetExists) return;
//         _cabContext.Menues.Add(new Menu() { Id = 1 });
//         _cabContext.SaveChanges();
//     }
//
//     public void EmptyFleet(int fleetId)
//     {
//         var fleet = _cabContext.Fleet.Include(x => x.Cabs)
//             .FirstOrDefault(x => x.Id == fleetId);
//         _cabContext.CabDrivers.RemoveRange(fleet!.Cabs.ToList());
//         _cabContext.SaveChanges();
//     }
//
//     [Fact]
//     public void CanRegisterAsNewUser()
//     {
//         EmptyFleet(1);
//         var dispatchController = new DispatchController(_cabContext);
//
//         var addCabMessage = dispatchController.RegisterNewUser();
//
//         Assert.Equal("A new user has been registered", addCabMessage);
//     }
//     [Fact]
//     public void CanAddCabsToTheFleet()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//
//         var addCabMessage = dispatchController.AddCab(1);
//
//         Assert.Equal("Added Evan's Cab to fleet", addCabMessage);
//     }
//
//     [Fact]
//     public void CanRemoveCabsFromTheFleet()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.AddCab(1);
//         var removeCab = dispatchController.RemoveCab(1);
//
//         Assert.Equal("Requested cab removed from fleet", removeCab);
//     }
//
//     [Fact]
//     public void CannotRemoveCabsFromEmptyFleet()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//
//         var result = dispatchController.RemoveCab(1);
//
//         Assert.Equal("Cab cannot be removed until passenger dropped off.", result);
//     }
//
//     [Fact]
//     public void CannotRemoveCabsWithPassengerInIt()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.SendCabRequest(1);
//         dispatchController.CabNotifiesPickedUp(1);
//
//         var result = dispatchController.RemoveCab(1);
//
//         Assert.Equal("Requested cab removed from fleet", result);
//     }
//
//     [Fact]
//     public void CanRemoveCabsWithoutPassengerInside()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.SendCabRequest(1);
//         dispatchController.CabNotifiesPickedUp(1);
//
//         var result = dispatchController.RemoveCab(1);
//
//         Assert.Equal("Requested cab removed from fleet", result);
//     }
//
//     [Fact]
//     public void CustomerCanCallInCab()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//
//         var customerCabCall = dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//
//         Assert.Equal("Received customer ride request from Emma", customerCabCall);
//     }
//
//     [Fact]
//     public void TwoCustomersCanCallInCab()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//
//         var customerCabCall = dispatchController.CustomerCabCall("Lisa", "1 Fulton Drive", "1 Destination Lane", 1);
//
//         Assert.Equal("Received customer ride request from Lisa", customerCabCall);
//     }
//
//     [Fact]
//     public void FirstCustomerCallInCancelsSecondCustomerCanCallInCab()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.CustomerCancelledCabRide(1);
//
//         var customerCabCall = dispatchController.CustomerCabCall("Lisa", "1 Fulton Drive", "1 Destination Lane", 1);
//
//         Assert.Equal("Received customer ride request from Lisa", customerCabCall);
//     }
//
//     [Fact]
//     public void CannotPickupUnlessCustomersWaiting()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//
//         var customerCabCall = dispatchController.CustomerCancelledCabRide(1);
//
//         Assert.Equal("This is not a valid option.", customerCabCall.First());
//     }
//
//     [Fact]
//     public void CanCancelPickup()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//
//         var customerCabCall = dispatchController.CustomerCancelledCabRide(1);
//
//         Assert.Equal("Customer cancelled cab ride successfully.", customerCabCall.First());
//     }
//
//     [Fact]
//     public void CanCancelPickupAtAnyTime()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.SendCabRequest(1);
//
//         var customerCabCall = dispatchController.CustomerCancelledCabRide(1);
//
//         Assert.Equal("This is not a valid option.", customerCabCall.First());
//     }
//
//     [Fact]
//     public void CabCanDriveToCustomerAfterCabRequest()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//
//         var sendCabRequest = dispatchController.SendCabRequest(1);
//
//         Assert.Equal("Evan's Cab picked up Emma at 1 Fulton Drive.", sendCabRequest.First());
//         Assert.Equal("Cab assigned to customer.", sendCabRequest.Skip(1).First());
//     }
//
//     [Fact]
//     public void CannotSendCabRequestUntilCustomerCallsIn()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//
//         var sendCabRequest = dispatchController.SendCabRequest(1);
//
//         Assert.Equal("This is not a valid option.", sendCabRequest.First());
//     }
//
//     [Fact]
//     public void CannotSendCabRequestUntilCabsAreInFleet()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//
//         var sendCabRequest = dispatchController.SendCabRequest(1);
//
//         Assert.Equal("This is not a valid option.", sendCabRequest.First());
//     }
//
//     [Fact]
//     public void CabCanPickupCustomer()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.SendCabRequest(1);
//
//         var sendCabRequest = dispatchController.CabNotifiesPickedUp(1);
//
//         Assert.Equal("Notified dispatcher of pickup", sendCabRequest);
//     }
//
//     [Fact]
//     public void CabCannotPickupCustomerIfNoCabsInFleet()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//
//         var sendCabRequest = dispatchController.CabNotifiesPickedUp(1);
//
//         Assert.Equal("This is not a valid option.", sendCabRequest);
//     }
//
//     [Fact]
//     public void CabCannotPickupCustomerIfCustomerNotWaitingPickup()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//
//         var sendCabRequest = dispatchController.CabNotifiesPickedUp(1);
//
//         Assert.Equal("This is not a valid option.", sendCabRequest);
//     }
//
//     [Fact]
//     public void CabCanDropOffCustomer()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.SendCabRequest(1);
//         dispatchController.CabNotifiesPickedUp(1);
//
//         var droppedOff = dispatchController.CabNotifiesDroppedOff(1);
//
//         Assert.Equal("Evan's Cab dropped off Emma at 1 Destination Lane.", droppedOff.First());
//     }
//
//     [Fact]
//     public void CabCanDropOffOnlyOneCustomerAtATime()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.SendCabRequest(1);
//         dispatchController.SendCabRequest(1);
//         dispatchController.CabNotifiesPickedUp(1);
//         dispatchController.CabNotifiesPickedUp(1);
//
//         var droppedOff = dispatchController.CabNotifiesDroppedOff(1);
//
//         Assert.Equal("Evan's Cab dropped off Emma at 1 Destination Lane.", droppedOff.Single());
//     }
//
//     [Fact]
//     public void CabCanDropOffTwoCustomers()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.CustomerCabCall("Lisa", "1 Fulton Drive", "1 Destination Lane", 1);
//         dispatchController.SendCabRequest(1);
//         dispatchController.SendCabRequest(1);
//         dispatchController.CabNotifiesPickedUp(1);
//         dispatchController.CabNotifiesPickedUp(1);
//         dispatchController.CabNotifiesDroppedOff(1);
//
//         var droppedOff = dispatchController.CabNotifiesDroppedOff(1);
//
//         Assert.Equal("Evan's Cab dropped off Lisa at 1 Destination Lane.", droppedOff.Single());
//     }
//
//     [Fact]
//     public void CabCannotDropOffCustomerIfNotInTransport()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//         dispatchController.AddCab(1);
//         dispatchController.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane", 1);
//
//         var droppedOff = dispatchController.CabNotifiesDroppedOff(1);
//
//         Assert.Equal(
//             "This is not a valid option.",
//             droppedOff.First());
//     }
//
//     [Fact]
//     public void InvalidOptionSelectedReturnsError()
//     {
//         var dispatchController = new DispatchController(_cabContext);
//         dispatchController.RegisterNewUser();
//         dispatchController.LoginUser(1);
//
//         var dispatch = dispatchController.CabNotifiesDroppedOff(1);
//
//         Assert.Equal("This is not a valid option.", dispatch.First());
//     }
// }