# Cab Company TDD MVP
## Story
A cab company wants to implement a new system to handle fare calculations, driver assignments, and ride tracking to 
improve efficiency and customer satisfaction. The goal is to ensure that the system is reliable, easy to maintain, and 
can be extended with new features in the future. The development team will use TDD to build this system incrementally.
The code is structured overall using the Visitor Design Pattern.

### Scenarios

Note: Only one passenger calls at a time, otherwise the line is busy. 
Passenger must call in to get the cab, no other methods available for MVP (see event-storming/passenger-events.md for exhaustive list)

### Visualize the Event Storm

1. **Driver Management**: Add New Cab Driver
2. **Customer Interaction**: Customer Requests Ride
3. **Ride Dispatch**: Ride Request Received
4. **Ride Dispatch**: Driver Request Sent
5. **Driver Management**: Accept Cab Fare
6. **Ride Dispatch**: Driver Request Confirmed
7. **Ride Dispatch**: Ride Assigned to Driver
8. **Ride Dispatch**: Ride Request Confirmed
9. **Customer Interaction**: Ride Confirmed to Customer
10. **Ride Execution**: Customer Picked Up
11. **Ride Execution**: Ride in Progress
12. **Ride Execution**: Customer Dropped Off
13. **Ride Execution**: Ride Completed
14. **Payment Processing**: Fare Calculated
15. **Payment Processing**: Payment Processed
16. **Payment Processing**: Receipt Issued
17. **Driver Management**: Status: Ready for pickup
18. **Optionally: Human Resources**: List Cab Drivers, Fares Received

### Implement a UX with the following screens:
Main Navigation Menu:
```shell
1. Passenger Menu
2. Dispatcher Menu
3. Cab Driver Menu
4. HR Menu
0. Exit 
```

Passenger Menus:
```shell
1. Choose Passenger From List
2. Add New Passenger
3. Cancel Passenger Fare
4. Back to Main Navigation Menu
```

```shell
1. Call Cab Company
2. Pay Cabbie
3. Back to Passenger Menu
```
Call Cab Company Information:
```
Enter starting location: <input>
Enter destination: <input>
```

Dispatcher Menu:
```
Current Passenger Requests: 1
Current Passenger Request Confirmations: 2
Current Driver Requests: 0
Current Driver Request Confirmations: 2
```
```shell
1. Check Cabs Available
2. List Cabs Drivers, Locations, Availability
3. Assign Cab Driver to Ride Request
4. Back to Main Navigation Menu
```

Cab Driver Menus:
```shell
1. Choose Cab Driver From List
2. Add New Cab Driver
3. Remove Cab Driver
4. Cancel Cab Driver Fare
5. Back to Main Navigation Menu
```

```
Current Status: (Request Outstanding, Request Accepted, Fare Started, Fare Ended, Free)
```
```shell
1. Accept/Reject Cab Fare
2. Pickup Passenger
3. Drop Off Passenger
4. Back to Main Navigation Menu
```

HR Menu:
```shell
1. List Cab Drivers, Fares Received
2. Back to Main Navigation Menu 
```
