
# Loan Comparission

Proud to present "Loan Comparission" project.This project aims to simplify the loan application process for users by allowing them to submit a single application and receive offers from multiple loan providers. 
This project uses Angular based frontend to capture the users data and submits to backend lambda function where its then forwarded to multiple loan providers and process the loan and finally displays the compiled results back to the user.

## Tech Stack

**Client:** Angular 17, NgRx, Material UI Components

**Server:** AWS Lambda, Amazon API Gateway, Amazon SNS/SQS, DynamoDB

**Authentication:**   Auth0

**Infrastructure:** AWS SAM / CloudFormation

## Components Description

**UI:**  

The Angular application serves as the entry point for users, providing a user-friendly form to input their loan application data. Material UI components are used to ensure a responsive and intuitive design.

**Loan Submission Lambda:**  

AWS Lambda function acts as the initial receiver of the loan application data from the UI. A a unique identifier (GUID) is generated for the UI, stores the application data in DynamoDB, and posts a message to an Amazon SQS queue (Process) to be processed by the loan processor.

**Loan Processor Lambda:**  

AWS Lambda function designed to process loan applications. It polls the SQS queue to receive new applications, transforms the application data to meet individual loan provider requirements, makes API calls to the providers, and compiles the results. The results are then posted back to a designated SQS queue (Processed), where they can be retrieved and displayed to the user.

**API Gateway:**  

Amazon API Gateway acts as the HTTP endpoint for the Angular app, facilitating secure communication between the frontend and backend services. It also integrates with Auth0 to manage authentication and authorization.

**Auth Lambda:**  

AWS Lambda function for handling authentication. It works in conjunction with API Gateway and Auth0 to authenticate users before they can submit loan applications, ensuring secure access to the application.

**DynamoDB:**  

Amazon DynamoDB stores loan application data and processing results. It's designed for high-performance data retrieval, enabling the application to quickly access individual loan applications and their corresponding offers based on the GUID.