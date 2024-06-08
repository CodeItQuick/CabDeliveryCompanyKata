# Cab Company TDD Kata
## Story
A cab company wants to implement a new system to handle fare calculations, driver assignments, and ride tracking to 
improve efficiency and customer satisfaction. The goal is to ensure that the system is reliable, easy to maintain, and 
can be extended with new features in the future. The development team will use TDD to build this system incrementally.
The code is structured overall using the Visitor Design Pattern.

### Scenarios

### 1. Driver Assignment

As a dispatcher, I want to assign an available driver to a customer, so that the customer gets picked up.
Given a list of available drivers, assign the driver closest to the customer’s location.

### 2. Fare Calculation

As a customer, I want to know the fare for my ride based on a flat rate, so I can pay the correct amount.

### 3. Customer Ride History

As a customer, I want to view my ride history, so I can track my expenses and review past trips.
Provide a ride history of which cab's you've previously taken.

### Tribal Knowledge
1. The poorest cabbie should pickup the fare.

### Observations
1. Latent Bug: I forgot to have the customer IsInCab flag to be set to false when being dropped off
2. Customer should have more behaviour than it does (there's feature envy here)