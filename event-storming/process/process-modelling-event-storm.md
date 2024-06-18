Event storming is a technique used in process modeling to identify and visualize the key events, actors, and processes within a business. Here’s a sample event storming process for a cab company:

### Step 1: Identify Key Domains

**Customer Interaction**:
**Ride Dispatch**:
**Ride Execution**:
**Payment Processing**:
**Driver Management**:
**Human Resources**:

### Step 2: Brainstorm Key Events

**Customer Interaction**:
- Customer Requests Ride
- Ride Confirmed to Customer
- Customer Cancels Ride

**Ride Dispatch**:
- Ride Request Received
- Ride Request Confirmed
- Driver Request Sent
- Driver Request Confirmed
- Ride Assigned to Driver

**Ride Execution**:
- Customer Picked Up
- Ride in Progress
- Customer Dropped Off
- Ride Completed

**Payment Processing**:
- Fare Calculated
- Payment Processed
- Receipt Issued

**Driver Management**:
- Add New Cab Driver
- Remove Cab Driver
- Cancel Cab Driver Fare
- Driver Logs Out
- Accept/Reject Cab Fare
- Status: Ready for pickup 
- Pickup Passenger
- Drop Off Passenger

**Human Resources**:
- List Cab Drivers, Fares Received

### Step 3: Arrange Events in Sequence

**1. Driver Management**:
- Add New Cab Driver
 
**2. Customer Interaction**
- Customer Requests Ride

**3. Ride Dispatch**
- Ride Request Received
- Pick from Available Driver List
- Driver Request Sent

**4. Driver Management**:
- Accept Cab Fare

**5. Ride Dispatch**
- Driver Request Confirmed
- Ride Assigned to Driver
- Ride Request Confirmed

**6. Customer Interaction**
- Ride Confirmed to Customer

**7. Ride Execution**:
- Customer Picked Up
- Ride in Progress
- Customer Dropped Off
- Ride Completed

**8. Payment Processing**
- Fare Calculated
- Payment Processed
- Receipt Issued

**9. Driver Management**:
- Status: Ready for pickup

**10. Ride Dispatch**
- Set driver status to ready for pickup

**Optionally: Human Resources**:
- List Cab Drivers, Fares Received

### Step 4: Identify Actors Involved

- **Customer**: Requests ride, cancels ride, makes payment
- **Dispatcher**: Receives ride request, assigns ride to driver
- **Driver**: Acknowledges assignment, drives to pickup, completes ride
- **Payment Processor**: Processes payment
- **HR Manager**: Calculates Fares Received to pay cab drivers

### Step 5: Identify Supporting Systems and Processes

- **Dispatch System**: Manages ride requests and assignments
- **Payment System**: Handles fare calculation and payment processing
- **HR System**: Manages hiring, training, and performance reviews

### Step 6: Visualize the Event Storm

1. **Driver Management**: Add New Cab Driver
2. **Customer Interaction**: Customer Requests Ride
3. **Ride Dispatch**: Ride Request Received
4. **Ride Dispatch**: Find Available Driver from Driver List
5. **Ride Dispatch**: Driver Request Sent
6. **Driver Management**: Accept Cab Fare
7. **Ride Dispatch**: Driver Request Confirmed
8. **Ride Dispatch**: Ride Assigned to Driver
9. **Ride Dispatch**: Ride Request Confirmed
10. **Customer Interaction**: Ride Confirmed to Customer
11. **Ride Execution**: Customer Picked Up
12. **Ride Execution**: Ride in Progress
13. **Ride Execution**: Customer Dropped Off
14. **Ride Execution**: Ride Completed
15. **Payment Processing**: Fare Calculated
16. **Payment Processing**: Payment Processed
17. **Payment Processing**: Receipt Issued
18. **Driver Management**: Status: Ready for pickup
19. **Ride Dispatch**: Set driver status to ready for pickup
20. **Optionally: Human Resources**: List Cab Drivers, Fares Received
