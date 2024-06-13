System design event storming is an extension of the event storming methodology focused on understanding and designing the architecture and interactions of a system. It involves a detailed exploration of the system's components, their interactions, and the events that flow through the system. Here's an overview of what system design event storming typically looks like:

### Key Components

1. **Domain Events**: Significant occurrences within the system, typically expressed in past tense (e.g., "User Registered," "Order Shipped").

2. **Commands**: Actions or instructions that trigger domain events (e.g., "Register User," "Ship Order").

3. **Actors**: Entities (people, systems, or external services) that perform actions or trigger events (e.g., User, Payment Gateway).

4. **Aggregates**: Clusters of related domain objects that ensure consistency and encapsulate business rules (e.g., Order, Customer).

5. **Policies (or Sagas)**: Long-running processes or business rules that react to events and may trigger further commands (e.g., "Send Welcome Email").

6. **Read Models/Queries**: Data views or projections that represent information needed by users or other systems (e.g., "Order Status," "User Profile").

7. **Bounded Contexts**: Logical boundaries within the system that encapsulate related models and services, often aligned with subdomains.

### Steps in System Design Event Storming

1. **Preparation**:
    - Gather a diverse group of stakeholders, including domain experts, architects, developers, and other relevant participants.
    - Provide materials like sticky notes, markers, and large surfaces (e.g., whiteboards or large sheets of paper).

2. **Identify Domain Events**:
    - Start by brainstorming and writing down domain events that occur within the system, one per sticky note.
    - Place these events on the timeline in the order they occur.

3. **Identify Commands**:
    - Identify the commands that trigger each event and place them before the corresponding event.

4. **Identify Actors**:
    - Determine the actors who perform the commands and place them above the commands.

5. **Define Aggregates**:
    - Group related events and commands to identify aggregates and place them around the related events.

6. **Add Policies**:
    - Identify business rules or policies that influence the process and add these policies around the relevant events and commands.

7. **Define Read Models/Queries**:
    - Determine the data views or projections needed by users and place these read models around the events that generate the data.

8. **Identify Bounded Contexts**:
    - Group related aggregates, commands, events, and policies into bounded contexts to define logical boundaries within the system.

9. **Explore Interactions**:
    - Map out interactions between different bounded contexts, identifying integration points, message flows, and dependencies.

10. **Refine and Document**:
    - Discuss and refine the model, ensuring all key interactions and components are captured.
    - Document the final system design using diagrams, photos, or formal documentation tools.

### Example Flow

1. **User Registration**:
    - **Command**: "Register User"
    - **Event**: "User Registered"
    - **Actor**: User
    - **Aggregate**: User

2. **Order Placement**:
    - **Command**: "Place Order"
    - **Event**: "Order Placed"
    - **Actor**: User
    - **Aggregate**: Order

3. **Payment Processing**:
    - **Command**: "Process Payment"
    - **Event**: "Payment Processed"
    - **Actor**: Payment Gateway
    - **Aggregate**: Payment

4. **Shipping**:
    - **Policy**: "Initiate Shipping"
    - **Event**: "Order Shipped"
    - **Actor**: Shipping Service
    - **Aggregate**: Order

### Benefits

- **Holistic Understanding**: Provides a comprehensive view of the system's architecture, highlighting key components and their interactions.
- **Collaboration**: Fosters collaboration among stakeholders to build a shared understanding of the system.
- **Problem Identification**: Helps identify potential issues, integration points, and areas for improvement within the system.
- **Visual Representation**: Facilitates visual thinking, making it easier to understand complex interactions and dependencies.

System design event storming is a powerful technique for capturing the intricacies of system architecture and interactions, leading to better design decisions and more effective system implementation.