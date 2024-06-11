# Cab Company TDD Kata
## Story
A cab company wants to implement a new system to handle fare calculations, driver assignments, and ride tracking to 
improve efficiency and customer satisfaction. The goal is to ensure that the system is reliable, easy to maintain, and 
can be extended with new features in the future. The development team will use TDD to build this system incrementally.
The code is structured overall using the Visitor Design Pattern.

### Scenarios

Note: Only one passenger calls at a time, otherwise the line is busy. 
Passenger must call in to get the cab, no other methods available for MVP (see event-storming/passenger-events.md for exhaustive list)

### Implement a UX with the following screens:
Main Navigation Menu:
```shell
1. Passenger Menu
2. Dispatcher Menu
3. Cab Driver Menu
4. HR Menu
0. Exit 
```

Passenger Menu:
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

Cab Driver Menu:
```shell
1. Choose Cab Driver From List
2. Add New Cab Driver
3. Cancel Cab Driver Fare
4. Back to Main Navigation Menu
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
