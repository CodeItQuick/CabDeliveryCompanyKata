### An Example Overall Big-Picture Event Storm

1. **Driver Management**: Add New Cab Driver
2. **Customer Interaction**: Customer Requests Ride
3. **Ride Dispatch**: Ride Request Received
4. **Ride Dispatch**: Find Available Driver
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
18. **Driver Management**: Status: Send ready for pickup status
19. **Ride Dispatch**: Driver Ready For Pickup Status Received
20. **Optionally: Human Resources**: List Cab Drivers, Fares Received

Question:
* How does the dispatch system know that the fare was completed, or if it was cancelled, etc?
* There must be some interaction between the cabbie and the dispatch that is missing here