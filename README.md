# StackOverflowLite

The required documents for AMSS can be found at: https://github.com/st-lu/StackOverflowLite/tree/main/Diagrams%20AMSS

# Design patterns used in StackOverflowLite

## Repository Design Pattern

By definition, the Repository Design Pattern is a structural pattern that provides an abstraction layer between the application logic and the database. It allows the application to interact with the data source while decoupling the data access logic from the business logic.

In our StackOverflowLite application, the flow is as follows: Controller -> Service -> Repository -> Database, thus the Repository acts as an intermediary between the app and our database. The Controller only handles the requests. The Service doesn't have to know how to fetch the data; the Service only knows HOW to use it and return it. It is the job of the Repository to retrieve the data. There are quite a few upsides to this design choice:  
- **Abstraction**: The repository hides the complexity of database interactions, such as SQL queries or ORM specifics.  
- **Flexibility**: Databases can be switched (e.g., from SQL to NoSQL) with minimal impact on the rest of the application.  
- **Reusability**: Common data access methods (such as CRUD operations) can be shared across services.  
- **Testability**: Repositories can be mocked to test logic without depending on an actual database.

---

## Strategy Design Pattern

The Strategy Design Pattern is a behavioral pattern that defines a family of algorithms. It allows the behavior of a system to be selected at runtime, promoting flexibility and reusability.

In our application, the Strategy Design Pattern is used to filter questions based on different criteria, such as score, views count, or certain keywords.

Steps of setting this up:  
1. We defined a common interface `IQuestionFilterStrategy` that all filtering strategies implemented.  
2. The interface has only one method: `ApplyFilter(IEnumerable<Question> questions)` that returns a filtered or sorted list of questions.  
3. Each filtering class builds its own logic of the `ApplyFilter` method. For example, the class `ScoreStrategy` sorts questions based on their scores, either ascending or descending.  
4. In the Controller, when the requests are received, multiple filtering strategies can be applied at once by instantiating the filtering class that the user requests.

---

## Background Task Design Pattern

The Background Task Design Pattern is focused on managing and executing tasks that run asynchronously in the background, separate from the main application flow. It helps improve the responsiveness and performance of the system by offloading non-critical or time-consuming operations.

In our app, the background task was built on ASP.NET Core's background task mechanism named `IHostedService`.  
- When a question or answer is posted, the `QueueBackgroundWorkItemAsync` method queues a task to run in the background.  
- In this background task, our endpoint hosting the NLP model is called to analyze the user text input and waits for the result of this analysis.  
- Based on the response from the server, the user is notified via email whether their post has been accepted or rejected.



---



## Run project locally:
`git clone https://github.com/st-lu/StackOverflowLite.git`

`cd StackOverflowLite\StackOverflowLiteSolution`

`docker compose -f docker-compose-dev.yaml up -d`
