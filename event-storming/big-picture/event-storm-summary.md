actors:

1. **Customer (Passenger)**:
    - The individual or group requesting and using the cab service.
    - Provides pickup and destination details, makes payment, and provides feedback.

2. **Cab Dispatcher**:
    - Coordinates and manages the assignment of cabs to customers.
    - Communicates with both the customer and the driver to ensure efficient service.
    - Monitors the progress of the ride and handles any issues that arise.

3. **Cab Driver**:
    - The individual driving the cab and providing transportation services.
    - Receives ride details from the dispatcher, picks up the customer, and drives them to their destination.
    - Handles the fare collection and addresses any customer needs during the ride.

4. **Support Staff**:
    - accounting
    - payroll

5. **Technology/Dispatch System**:
    - The platform or software used to manage ride requests, cab assignments, and route tracking.
    - Facilitates communication between the dispatcher, driver, and customer.
    - Often includes GPS tracking, payment processing, and ride history logging.

First method of receiving the fare (see passenger-events.md for an exhaustive list):

3. **Dispatch Service**:
    - The driver receives ride assignments from a central dispatcher who coordinates requests from passengers.
    - Dispatchers use radio communication, mobile apps, or other dispatch systems to assign rides.

relevant events:

1. **Fare Collection**:
    - Record the total fares collected from passengers, including cash, credit/debit card payments, and mobile payments.
    - Track fare receipts issued to passengers.

1. **Cab Driver: Receiving the Assignment**:
    - Receive a ride request from the dispatcher or through a mobile app.
    - Note the customer’s pickup location, destination, and any special instructions.

11. **Cab Driver: Completing the Ride**:
    - Mark the ride as complete in the dispatch system or app.
    - Prepare the cab for the next ride, ensuring it is clean and ready for the next customer.

1. **Dispatch System: Ride Request Reception**:
    - The dispatch system receives a ride request from a customer via phone, mobile app, or online booking platform.
    - The system logs the customer’s details, including pickup location, destination, requested pickup time, and any special instructions.

2. **Dispatcher Events: Checking Availability**:
    - Check the availability of cabs in the area.
    - Determine which cab is closest to the pickup location or best suited for the request.

2. **Dispatch System: Ride Request Confirmation**:
    - The dispatch system confirms the ride request and sends an acknowledgment to the customer.
    - The customer receives an estimated time of arrival (ETA) for the cab.

3. **Dispatch System: Cab Assignment**:
    - The system identifies the nearest available cab and assigns the ride request to the driver.
    - The driver receives the ride details, including the pickup location, destination, and any special instructions.

4. **Dispatch System: Driver Acknowledgment**:
    - The driver confirms the acceptance of the ride assignment.
    - The dispatch system updates the ride status to indicate that a driver is en route.

6. **Dispatch System: Arrival Notification**:
    - Once the driver reaches the pickup location, the dispatch system sends an arrival notification to the customer.
    - The driver marks their arrival in the system.

7. **Dispatch System: Ride Start**:
    - The customer enters the cab, and the driver starts the fare meter.
    - The dispatch system updates the ride status to indicate that the ride is in progress.

10. **Dispatch System: Ride Completion**:
    - Upon arrival at the destination, the driver stops the fare meter and marks the ride as complete in the system.
    - The dispatch system calculates the total fare and processes the payment if using a cashless system.

13. **Dispatch System: Ride Logging and Reporting**:
    - The completed ride details are logged in the system, including fare, route, driver, and customer information.
    - The dispatch system generates reports for accounting, driver performance, and operational analysis.

7. **Human Resources: Time and Attendance Management**:
    - Tracking employee attendance, hours worked, and time off.
    - Managing leave requests, sick days, and vacation schedules.
    - Ensuring adherence to work schedules and addressing attendance issues.