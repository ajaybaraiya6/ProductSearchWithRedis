# Product Search Assignment

# 1. OVERVIEW

# Project Stack: 
Front-end: .Net Core MVC
Back-end: .Net Web Api (Microservice)
Database: SQLite
Target Framework: .Net 8 

# Project Startup Sequence: 
1st run: ProductService Microservice
2nd run: ProductSearch.MVC

# Project Architecture Patterns
Front end: MVC Pattern 
Backend/Microservice: Repository

# Caching 
Using redis

# Redis container
Docker for running redis

# 2. HOW TO SET UP PROJECT 
Clone repository into local
Open solution and do clean solution and dotnet restore
Build solution and it should build without any error.
If asked for ssl certification please add it. 
Make sure you set both the services to http for testing.
Then set the solution as multiple startup project and keep the Product Service project first.
Run the project. 

# 3. Running and navigating.
Make sure you find the ProductDb.sqlite file at ProductService\ProductService.Database folder with in project it contain 100 records for testing.	
It is recommended to run the redis first 
	- Follow this steps to run redis. 
		- 1. Go to product service folder inside the project where docker-compose.yml
		- 2. ensure you have docker installed on your machine
		- 3. open cmd for that path and enter command
			'docker-compose up -d'
		- 4. It will pull redis and start it. 
		- 5. We already have configured this in our file docker-compose.yml
		- 6. Make sure it is running on our port 6379.
Now once we run projects we will see the Swagger for our product service where you can find the XMl comments.
MVC will run at the http port, should automatically come up in new tab, otherwise access it via http://localhost:5260/
We will be on search page with filters and search button.
Select desire filters and hit search and it will redirect on result page. 
it has pagination and sorting both.
In general it is using redis cache for the dynamic filters on search page and for our result page records. 

# 4. You can find out the two videos for the demonstration. 

# 5. Final deliverable points and remaining things. 
	- Things completed: 
		- Redis Caching implementation done
		- Project functionality done
	- Area of improvement 
		- There is one price and discount filters require to preserve on sorting and pagination
		
	- Things remained due to limitted bandwidth of time
		- order microservice and document to show how they will communicate
		- unit test case and performance report. 
		
# 6. Note from me.
	- I would like to here any feed back from you.  
	- Thank you and looking forward. 
	




