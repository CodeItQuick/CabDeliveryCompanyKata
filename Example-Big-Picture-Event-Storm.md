### An Example Overall Big-Picture Event Storm

1. **Driver Management**: Add New Cab Driver
2. **Customer Interaction**: Customer Requests Ride
3. **Ride Dispatch**: Ride Request Received
4. **Ride Dispatch**: Find Available Driver
5. **Ride Dispatch**: Driver Request Sent
6. **Driver Management**: Accept Cab Fare
7. **Ride Dispatch**: Driver Request Confirmed
8. **Ride Dispatch**: Ride Assigned to Driver
9. **Customer Interaction**: Ride Confirmed to Customer
10. **Ride Execution**: Customer Picked Up
11. **Ride Execution**: Ride in Progress
12. **Ride Execution**: Customer Dropped Off
13. **Ride Execution**: Ride Completed
14. **Payment Processing**: Fare Calculated
15. **Payment Processing**: Payment Processed
16. **Payment Processing**: Receipt Issued
17. **Driver Management**: Status: Send ready for pickup status
18. **Ride Dispatch**: Driver Ready For Pickup Status Received
19. **Optionally: Human Resources**: List Cab Drivers, Fares Received

Question:
* How does the dispatch system know that the fare was completed, or if it was cancelled, etc?
* There must be some interaction between the cabbie and the dispatch that is missing here