### An Example Overall Big-Picture Event Storm

1. **Driver Management**: Add New Cab Driver
2. **Customer Interaction**: Customer Requests Ride
3. **Ride Dispatch**: Ride Request Received
4. **Ride Dispatch**: Driver Request Sent (**How is this sent? to the whole fleet? to a section of the fleet? etc.**)
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

Question:
* How does the dispatch system know that the fare was completed, or if it was cancelled, etc?
* There must be some interaction between the cabbie and the dispatch that is missing here