
# Strider Assessment
## What was developed?
I created the requested backend to support the `Posterr` actions. To achieve this goal, I wrote the following projects and code.

 - **Strider.Posterr.Api**: This project contains the RestAPI to serve the backend. It contains the minimal of business rules as possible and his resposibility is accept the request and forward into the internal layers, which must contains the validations and business rules. Also it contains the `Program.cs` class which is responsible to configure and inject the dependencies.
 - **Strinder.Poster.Common**: This project contains common classes to multiple project. This helps to avoid having circular reference or creating useless dependency in projects.
 - **Strinder.Poster.Domain**: This project contains the Entities maped from the table database, the Dtos to transport the data between the layers. This project also contains the interfaces (contracts) and all implementing classes must respect.
 - **Strinder.Poster.RelationalData**: Project containing the classes to configure the relational database (Sql Server in this case), the Entity Framework context and migrations, and the repositories implementations.
 - **Strinder.Poster.Service**: Project that implements the majority of the business rules. Also, all the Repository access must be made through the Service layer.

## Tests Coverage
I tried to reproduce my working skills and behaviour during my daily work. That's said, I created this assessment project using TDD.
The tests are in the projects which ends with UnitTests or IntegrationTests depending on the goal of the test and it's related to their production code project.
The unit tests asserts that the methods tests works independing external references. The integration test, creates a API client and runs the actual API, using the real implementations of the services and repositories.
Excluding the Migrations code generated, this project has 97% of code coverage as you can see in [https://prnt.sc/8Lv_1pb0QAHJ](https://prnt.sc/8Lv_1pb0QAHJ)
 
## How to run?
1. Open a terminal in the root of the project (\strider.assessment) and run `docker build -t posterr.api .` 
2. Run `docker-compose up`
3. Access [http://localhost:54023/swagger](http://localhost:54023/swagger) in your prefered browser.
4. Make any request you want using the `swagerUI`
