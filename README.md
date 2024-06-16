# Cab Company Dispatch Screen
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
```shell
1. (Incoming Radio) Add New Cab Driver
2. (Incoming Radio) Remove Cab Driver
3. (Outgoing Radio) Send Cab Driver Ride Request
4. (Incoming Radio) Cab Notifies Passenger Picked Up
4. (Incoming Radio) All Cab Notifies Passenger Dropped Off
5. (Incoming Call) Cancel Cab Driver Fare
6. (Incoming Call) Back to Main Navigation Menu
```

Write the following acceptance tests:
TheCabCompanyCanPickupACustomerAtAnAddress
TheCabCompanyCanPickupTwoCustomerAtTwoAddresses
