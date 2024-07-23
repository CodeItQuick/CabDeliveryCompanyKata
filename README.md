# Cab Company Dispatch Screen

## TODO
1. Add the Event Storming into Miro
2. ~~Fix Command/Query Separation~~
3. ~~Add More testing around the DispatchController~~
4. Get the Aggregates Out (fix the object oriented code to figure out the aggregates)
5. ~~Limit Menu~~
6. ~~Figure out Customer Aggregate~~
7. Add More Features - Cab company records customer name on call-in
8. ~~Add More Features - State persists through file saving/loading (in-progress)~~
   ~~8a. Create DispatcherCoordinator Concept~~
    ~~8b. Create tests around DispatcherCoordinator and move the tests around/refactor as some are in the wrong spot~~
    ~~8c. Create Service Layer~~
    ~~8d. Persist state to file system~~
8. Add More Features - Cab's have some concept of general location
9. Repository needs to be injected into CabService
10. Export in CabService is a private method and doesn't belong there
## Story
A cab company currently operates with "old school" technology. The cab company wants to upgrade their technology offering
to include a software solution that helps them pickup customers from one location and drop them off at another location.
You, the developer, are tasked with building the ride dispatch system that allows for incoming customer requests and
outgoing driver assignments.

You have been given a sample UX designed menu screen for the dispatch. Previously the team got together and did event storming
on this problem, and this is provided in the "event-storming" folder. The output of the big picture event storming is given
in the folder structure. Some big-picture UX design screens are given as well. Design the following UI, and any
forgotten requirements, for the system.

Dispatcher Menu:
```
Current Passenger Requests: 1
Current Passenger Request Confirmations: 2
Current Driver Requests: 0
Current Driver Request Confirmations: 2
```
```
0. Exit
1. (Incoming Radio) Add New Cab Driver
2. (Incoming Radio) Remove Cab Driver
3. (Outgoing Radio) Send Cab Driver Ride Requested
4. (Incoming Radio) Cab Notifies Passenger Picked Up
5. (Incoming Radio) Cab Notifies Passenger Dropped Off
6. (Incoming Call) Cancel Cab Driver Fare
7. (Incoming Call) Customer Request Ride
```
# Acceptance Tests/Possible Features

1. The Cab Company Can Pickup A Customer At An Address  
2. The Cab Company Can Pickup Multiple Customer's At Different Addresses  
3. The Cab Company understands districts and prioritizes cabs that are in the correct district already
4. The Cab Company keeps track of transaction fees
5. The Cab Company keeps track of driver and customer names
6. The Cab Company has historical ride information for each cab driver
7. The Cab Company's dispatcher system persists the data, so it can recover if the system crashes

Noteable Constraints: 
No location data for cabs or customers  
Fare amount not recorded yet  
