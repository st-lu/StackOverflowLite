# StackOverflowLite
# Design Patterns in StackOverflowLite

The following lines highlight the design patterns used in the StackOverflowLite application, along their purpose, implementation, and benefits.

---

## Repository Design Pattern

By definition, the **Repository Design Pattern** is a structural pattern that provides an abstraction layer between the application logic and the database. It allows the application to interact with the data source while decoupling the data access logic from the business logic.

### Implementation in StackOverflowLite

In our application, the flow is as follows:  
**Controller -> Service -> Repository -> Database**  
The **Repository** acts as an intermediary between the application and the database:  
- The **Controller** handles requests but does not directly access the database.  
- The **Service** uses the repository to fetch or modify data without worrying about how the data is retrieved or stored.  
- The **Repository** encapsulates the data access logic.

### Benefits of This Design Pattern

1. **Abstraction**:  
   The repository hides the complexity of database interactions, such as SQL queries or ORM specifics.

2. **Flexibility**:  
   Databases can be switched (e.g., from SQL to NoSQL) with minimal impact on the application.

3. **Reusability**:  
   Common data access methods (e.g., CRUD operations) can be shared across services.

4. **Testability**:  
   Repositories can be mocked to test business logic without depending on an actual database.

---

## Strategy Design Pattern

The **Strategy Design Pattern** is a behavioral pattern that defines a family of algorithms. It allows the system's behavior to be selected at runtime, promoting flexibility and reusability.

### Implementation in StackOverflowLite

In our application, the **Strategy Design Pattern** is used to filter questions based on different criteria, such as score, views count, or specific keywords.

### Steps of Implementation

1. **Define a Common Interface**:  
   The `IQuestionFilterStrategy` interface serves as the contract that all filtering strategies implement.

2. **Interface Method**:  
   The interface includes the method `ApplyFilter(IEnumerable<Question> questions)` to return a filtered or sorted list of questions.

3. **Implement Filtering Strategies**:  
   Each filtering class builds its own logic for the `ApplyFilter` method.  
   - For example, the `ScoreStrategy` class sorts questions by score, either ascending or descending.

4. **Apply Strategies in the Controller**:  
   The controller applies multiple filtering strategies at runtime based on user requests by instantiating the appropriate filtering class.

---

## Background Task Design Pattern

The **Background Task Design Pattern** focuses on managing and executing tasks asynchronously in the background, separate from the main application flow. This pattern enhances the system's responsiveness and performance by offloading non-critical or time-consuming operations.

### Implementation in StackOverflowLite

1. **ASP.NET Core Background Task Mechanism**:  
   The application uses the `IHostedService` mechanism to manage background tasks.

2. **Queueing Background Work**:  
   When a question or answer is posted, the `QueueBackgroundWorkItemAsync` method is used to enqueue a background task.

3. **Processing the Task**:  
   - The background task calls an endpoint hosting the **NLP model** to analyze the user's text input.
   - The task waits for the analysis results.

4. **User Notification**:  
   Based on the server's response, an email is sent to the user indicating whether their post has been accepted or rejected.

---



## Run project locally:
`git clone https://github.com/st-lu/StackOverflowLite.git`

`cd StackOverflowLite\StackOverflowLiteSolution`

`docker compose -f docker-compose-dev.yaml up -d`
