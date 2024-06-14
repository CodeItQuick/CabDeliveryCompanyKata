# System Design Flow

1. **Ride Dispatch: Ride Request Received**:
   - **Command**: "Request Ride"
   - **Event**: "Customer Ride Request Received"
   - **Actor**: Customer
   - **Aggregate**: Ride

2. **Ride Dispatch**: Find Available Driver:
   - **Command**: "Retrieve Available Drivers"
   - **Event**: "List Available Drivers"
   - **Actor**: Dispatch
   - **Aggregate**: Driver

3. **Ride Dispatch**: Driver Request Sent
   - **Command**: "RequestDriver"
   - **Event**: "Request Driver"
   - **Actor**: Dispatch
   - **Aggregate**: Driver

4. **Ride Dispatch**: Driver Request Confirmed
   - **Command**: "ConfirmedDriverRequest"
   - **Event**: "Confirmed Driver Request"
   - **Actor**: Driver
   - **Aggregate**: Driver

5. **Ride Dispatch**: Customer Assigned to Driver
   - **Command**: "AssignedCustomerToDriver"
   - **Event**: "Confirmed Driver Request"
   - **Actor**: Driver
   - **Aggregate**: Ride

6. **Ride Dispatch**: Driver Ready For Pickup Status Received
   - **Command**: "DriverAvailable"
   - **Event**: "Driver Ready For Pickup"
   - **Actor**: Driver
   - **Aggregate**: Ride
