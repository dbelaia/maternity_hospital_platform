# Maternity Hospital Platform
Maternity Hospital Platform is a distributed system which serves a private maternity hospital with a focus on two main directions: an ability to create a doctor appointment for patients and provide the set of real-time dashboards and reports for key stakeholders and head (business) department. 

## Set up instructions
To run the application we need to build and run out micriservices and buil/run API Gateway.

1. Activate environment for API Gateway:
```
.\venv\Scripts\activate   
```
2. Reload API Gateway:
```
uvicorn gateway:app --reload    
```
3. Test application via Postman

## Microservice Architecture Suitability 
As it was already meantioned, the project will cover two pretty independant core functionalities - ordering doctor appointment online and provideing statistics for key stakeholders. That is why microservice artitecture is the best choice to separate those functionalities on two services which will process its data, store it in their own databases and which will be independent in terms of development, deployment ans scaling. It is planned to have at least tree levels of users: 

* **Patients** who will be able to log in and order a specific doctor appointment by choosing date, time and doctor;
* **Doctors** who will receive patients' request for an appointment for a specific date/time and will be able to reject/approve those requests based on the schedule;
* **"Stakeholder" users** who will monitor dashboards and reports to make key business decisions.

## Service Boundaries
### Appointment management service
This service will be responsible for doctor appontment management: initiating request for appointment and changing its further statuses: waiting for approvement, approved/rejected, completed. The lobby mechanics will be implemented for this service:
1. A patient selects the doctor and date/time for the appointment;
2. The doctor logs to the platform and subscribes for the Websocket channel;
3. Appointment service sends a notification to the doctor when new appointment request is available for him;
4. The doctor can either approve or reject the request;
5. After the request gets its status (approved or rejected), the patient recieve a real-time notification about it via WebSocket;
6. The status of the appointment is saved/updated into the database for Appointment Management Service.

### Analytics service
The service will be responsible for collecting, storing and delivering data for future dashboards and reports, in other words, it will be responsible for making data accessible for business analysis:
1. Collect data from different sources. It is planned to process data from the cloud (GCS bucket with CSV files );
2. Store the data in Analytics service database;
3. Deleiver data for dashboards, reports or user request from UI
4. Run job which will update all replicas once per day
5. Delivar data for Datawarehouse for further business reporting

### System Architecture Diagram
![System Architecture](images/Lab_2_System_Architecture.png)

## Technology Stack and Communication
* **Appointment management service**: C# (.NET Core) + SignalR for WebSocket, PostgreSQL, Redis (Cache)
* **Analytics service**: C# (.NET Core), MS SQL, Redis (Cache)
* **API Gateway**: Python (FastAPI)
* **User-service communication**: RESTful API
* **Inter-service communication**: gRPC (data will be transaferd in Protobuf format)
* **Testing**: MSTest framework
* **Logging**: ELK stack
* **Datawarehouse**: SQL Server with materiakized views for reporting
* **ETL**: Microsoft SSIS

**Inter-service communication**: there will be transferred data about income from different doctor appointments and then will be used for reporting and analysis.

## Data Management
### Core endpoints for Appointment management service:
1. **POST/doctor**:
Add new doctor to the system: http://localhost:8000/doctor

Responce:
```
{
  "firstName": "Diana",
  "lastName": "Belaia",
  "sex": "M",
  "occupation": "Obstetrician",
  "dateOfBirth": "2002-08-18"
}
```
2. **PUT/doctor/Id**:
Update doctor info:
http://localhost:8000/doctor/5

Responce:
```
{
  "doctorID": 5,
  "firstName": "Samvel",
  "lastName": "Bareian",
  "sex": "M",
  "occupation": "Obstetrician",
  "dateOfBirth": "1999-08-18"
}
```

3. **GET /appointment/{Id}**:
Get appointment: http://localhost:8000/appointmenthistory/2

Responce:
```
{
  "appointmentName": "Diana",
  "price": 10000
}
```
### Core endpoints for Appointment management service:
1. **GET /operation/{Id}**:
Get operation by Id: http://localhost:8000/operation/3

Responce:
```
{
  "operationName": "C-section",
  "price": 40000
}
```
2. **POST /operation/**:
Insert new operation: http://localhost:8000/operation

Body:
```
{
  "operationName": "C-section",
  "price": 40000
}
```
3. **PUT /operation/{Id}**:
Update operation info: http://localhost:8000/operation/5

Body:
```
{
  "operationID": 5,
  "operationName": "C-section",
  "price": 30000
}
```
## Deployment

Docker Compose will be used for project deployment. Each service will be containerized, ensuring that dependencies are isolated and consistent across different environments. Docker Compose simplifies running, scaling, and managing the services by defining them in a single docker-compose.yml file, making it easy to deploy, stop, and scale your microservice architecture.

## Scalability
Horizontal scalability - Analytical service can be extended and separated on smaller and independent services to handle new kinds of statistics.
