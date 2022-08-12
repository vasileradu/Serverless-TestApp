# Serverless-TestApp

This repository contains all artifacts that were created and used, in order to write the following article: 

__Serverless Computing: An Investigation of Deployment Environments for Web APIs__

_[MDPI (Basel, Switzerland) · Jun 25, 2019](https://doi.org/10.3390/computers8020050)_

__Abstract__:
_Cloud vendors offer a variety of serverless technologies promising high availability and dynamic scaling while reducing operational and maintenance costs. One such technology, serverless computing, or function-as-a-service (FaaS), is advertised as a good candidate for web applications, data-processing, or backend services, where you only pay for usage. Unlike virtual machines (VMs), they come with automatic resource provisioning and allocation, providing elastic and automatic scaling. We present the results from our investigation of a specific serverless candidate, Web Application Programming Interface or Web API, deployed on virtual machines and as function(s)-as-a-service. We contrast these deployments by varying the number of concurrent users for measuring response times and costs. We found no significant response time differences between deployments when VMs are configured for the expected load, and test scenarios are within the FaaS hardware limitations. Higher numbers of concurrent users or unexpected user growths are effortlessly handled by FaaS, whereas additional labor must be invested in VMs for equivalent results. We identified that despite the advantages serverless computing brings, there is no clear choice between serverless or virtual machines for a Web API application because one needs to carefully measure costs and factor-in all components that are included with FaaS._

### Research Questions

FaaS is advertised as a good fit for Web API applications and especially microservices since they have similar granularity. We want to find out how the same application deployed on virtual machines compares to serverless, by looking at monolithic, microservices, and function-as-a-service deployments. We start from the assumption that the existing resource constraints of FaaS are already considered, and the application is within these limits. We try to answer to the following research questions in order to support and structure the investigation:
1. How do different types of deployments in the cloud for Web API compare to one another in terms of response times with respect to load?

2. Are cost differences significant in order to offset the balance in favor of one type of deployment? 

Multiple cloud providers offer serverless platforms, with many similarities between them in terms of hardware specification, configurability, and costs. We compare response times for our function-as-a-service application across different providers, using similar deployments in terms of hardware specification, in order to answer the following research question:

3. Is there any difference between FaaS providers when it comes to response times and costs?


### Design

In order to suppor the investigation, the same Web Api was built as a monolith, as microservices and as function-as-a-service. The monolith and microservices were deployed individually on virtual machines.

![Logical Architecture](/Documents/TestApp-Logical%20Architecture.drawio.png?raw=true "Logical Architecture")

#### Tools and Technologies
- AWS, Azure - host and run everything needed for this paper
- .NET core 2.1, C# - main programming language for services
- PowerShell, Shell - scripts to setup, deploy and tear-down test environemnts
- MSSQL, S3, Files - storage
- AWS Lambda, Azure Functions - FaaS scenarios
- Apache JMeter - run load test
- Telegraf, Docker, InfluxDB, Grafana - collect metrics and visualize test results

#### Deployment

Everything in the diagram below, was setup using scripts, before running the load tests, then immediatly teared down.

![Cloud deployment](/Documents/TestApp-Physical%20Architecture.drawio.png?raw=true "Cloud deployment")

#### Test Resources

In order to reduce costs, all test metrics and data, were manually collected on to a local machine, to be able to quickly dispose of any cloud resource after the load tests have run.

![Test Resources](/Documents/TestApp-TestingDeployemnt_Actual.drawio.png?raw=true "Test Resources")

### Conclusions

The goal of this paper is to help with better decision-making when deploying a Web API into the cloud, with a focus on the end-user’s perception of performance. Specifically, we compared response times for the same application, deployed as function-as-a-service, and as monolith and microservices on virtual machines. Additionally, we looked at costs to find out if there are significant differences
in order to shift the decision in one direction or the other. We started from the assumption that the application’s hardware requirements are within the limits of FaaS and established some test phases where we vary the number of concurrent users.

We found that there are no considerable differences in response times between deployments, when VMs are properly configured with respect to load. This also means that additional effort and money need to be invested in VM deployments in order to match the native capabilities of FaaS. 

Usage predictability is an important factor when deciding between deployments, as it can help reduce costs significantly. The presented results show that VM deployments are several times cheaper when properly configured for the expected load. However, our investigation does not account for the maintenance and configuration costs associated with VMs. More to this, FaaS successfully handles unexpected user growth with its built-in features, whereas additional effort would be required to setup VMs to match FaaS’ auto-scaling capabilities. On the other hand, when payload increases, scaling out does not help, and VMs can provide more unused raw power as opposed to FaaS’s fixed memory and CPU configuration. When it comes to cloud providers, response times seem to be better for AWS Lambda. However, in a real situation, multiple other infrastructure components are used, which affect response times and costs. To sum up, one needs to do a thorough analysis before choosing a cloud provider or a type of deployment, as there are many variables to consider affecting performance and costs.