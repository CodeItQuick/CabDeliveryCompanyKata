# Cab Company TDD Kata
## Story
A cab company wants to implement a new system to handle fare calculations, driver assignments, and ride tracking to 
improve efficiency and customer satisfaction. The goal is to ensure that the system is reliable, easy to maintain, and 
can be extended with new features in the future. The development team will use TDD to build this system incrementally.
The code is structured overall using the Visitor Design Pattern.

### Scenarios

### 0. Implement a REPL with the following options:
```shell
1. Add cabbie to fleet
2. Remove cabbie from fleet
3. Add customer to call list
4. Remove customer from call list 
5. Deploy cabbie fleet to pickup and drop off customers
```

### 1. Driver Assignment

As a dispatcher, I want to assign an available driver to a customer, so that the customer gets picked up.

### 2. Fare Calculation

As a customer, I want to know the fare for my ride based on a starting rate of $5/cab ride, so I can pay the correct amount.

### 3. Customer Ride History

As a customer, I want to view my ride history, so I can track my expenses and review past trips.
Provide a detailed ride history, that includes the name of the cab driver, and how expensive the fare was (should be $5/ride).

### 4. Cabbie Ride History

As a cabbie, I want to view my fare history, so I can track my revenue and review past trips.

### 5. Driver/Customer Ratings and Feedback

As a driver I want to be able to provide ratings and feedback for cabbies.

### 6. Individual Driver Pricing and Cabbie Discount codes

As a driver I want to set my own price for my fares.
As a cab company, I want to offer discounts to some of my patrons in the form of coupon codes.

### 7. Multi-Person Rides

As a cabbie, I can only seat up to 4 people in my cab.

### 8. Multi-Stop Rides

As a cabbie, I want to be able to offer multiple stops on my rides.

### Tribal Knowledge

1. The poorest cabbie should pickup the fare.
