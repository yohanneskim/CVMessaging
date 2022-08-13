# CVMessaging
This project is an initial design of CareView Messaging Service
In short, based on the microservices principal, each messaging or notification funtion is implemented on its on space (function app or logic app). And these each function is behind the Azure API Management, so any changes can be made individually without interfering other services. All security, throttling, logging functions are handled by the API Management.
