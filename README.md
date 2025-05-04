# Product Search Assignment

## 1. OVERVIEW

### Project Stack:
- **Front-end**: .Net Core MVC
- **Back-end**: .Net Web Api (Microservice)
- **Database**: SQLite
- **Target Framework**: .Net 8 

### Project Startup Sequence:
1. **1st run**: ProductService Microservice
2. **2nd run**: ProductSearch.MVC

### Project Architecture Patterns:
- **Front-end**: MVC Pattern
- **Backend/Microservice**: Repository

### Caching:
- Using Redis

### Redis container:
- Docker for running Redis

## 2. HOW TO SET UP PROJECT

1. Clone the repository into your local machine.
2. Open the solution and perform the following:
   - Clean the solution
   - Run `dotnet restore`
3. Build the solution, it should build without any errors.
4. If asked for an SSL certificate, please add it.
5. Make sure you set both services to HTTP for testing.
6. Set the solution as **multiple startup projects** and keep the **ProductService** project first.
7. Run the project.

## 3. Running and Navigating

1. Ensure the `ProductDb.sqlite` file is located at `ProductService\ProductService.Database` folder within the project. It contains 100 records for testing.
2. It is recommended to run Redis first.
   - Follow these steps to run Redis:
     1. Go to the `ProductService` folder inside the project where `docker-compose.yml` is located.
     2. Ensure Docker is installed on your machine.
     3. Open the command prompt at that path and enter the command:  
        `docker-compose up -d`
     4. Docker will pull Redis and start it.
     5. The configuration for Redis is already set in the `docker-compose.yml` file.
     6. Ensure Redis is running on port 6379.
	 7. To stop Redis use `docker-compose down`
3. After running the projects:
   - You will see the Swagger UI for the ProductService, where you can find the XML comments.
   - The MVC application will run at the HTTP port and should automatically open in a new tab. If not, access it via:  
     `http://localhost:5260/`
   - You will land on the search page with filters and a search button.
   - Select the desired filters and hit search. It will redirect to the result page.
   - The result page supports both **pagination** and **sorting**.
   
   In general, the system uses **Redis cache** for dynamic filters on the search page and for storing records on the result page.

## 4. Demonstration Videos
You can find two videos demonstrating the project.  
Video 1: https://github.com/ajaybaraiya6/ProductSearchWithRedis/blob/master/Videos/1%20Setup%20and%20run%20solution.mp4  
Video 2: https://github.com/ajaybaraiya6/ProductSearchWithRedis/blob/master/Videos/2%20Running%20DEMO.mp4  

## 5. Final Deliverables and Remaining Tasks

### Things Completed:
- Redis caching implementation is done.
- Project functionality is complete.

### Areas of Improvement:
- Price and discount filters need to preserve their state across sorting and pagination.

### Things Remaining Due to Limited Bandwidth of Time:
- Order microservice and documentation to show how they will communicate.
- Unit test cases and performance report.

## 6. Note from Me
- I would love to hear any feedback from you.  
- Thank you, and I look forward to hearing from you!
