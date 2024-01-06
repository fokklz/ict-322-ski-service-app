# Ski Service App

<!--TOC-->
  - [Overview](#overview)
  - [Prerequisites](#prerequisites)
  - [Getting Started](#getting-started)
    - [Running the Backend](#running-the-backend)
    - [Running the App](#running-the-app)
<!--/TOC-->

## Overview

This app is designed for Jetstream-Service, a company that has invested in new 
touchscreen-enabled hardware for efficient data management of ski service orders. 
The app allows employees in workshops and administration to mutate order master data 
through mobile tablets or phones, offering an intuitive and easy-to-use interface, 
essential for an environment where employees often work with gloves. 
The goal is to minimize clicks or touch inputs for order management, 
ensuring the app is task-appropriate and user-friendly.

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker & Docker Compose](https://www.docker.com/products/docker-desktop)
- [MAUI Enabled Visual Studio 2022](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation?view=net-maui-8.0&tabs=vswin)

## Getting Started

Clone the repository from GitHub and navigate to the project directory.

```shell
git clone https://github.com/fokklz/ict-322-ski-service-app.git
cd ict-322-ski-service-app
```

### Running the Backend

For the app to work, the backend must be running. Since the backend is not hosted anywhere, you will need to run it locally. <br />
To run the backend, you can use the following command in the root directory of the project. (`ict-322-ski-service-app`)

```shell
docker compose up -d
```
The backend will be available at [http://localhost:8000](http://localhost:8000/swagger)

To shut down the backend, you can run the following command in the same directory.

```shell
docker compose down
```

### Running the App

To run the app, you can open the `SkiServiceApp.sln` file in Visual Studio 2022 and run the app with the green play button. <br />
For the most part, the app was developed with Android in mind. The device used was a `Tablet 420 8In`. <br />

By default, the app will try to search for local connections. You can alter this behavior by defining `API:BaseURL` in the `appsettings.json` file - be careful with emulators because they use special local IPs to access the host machine. <br />

WinUI was occasionally tested to ensure it's even working there; further testing was not done. Since we are all working on Windows, we were not able to test the app on MacOS/iOS and decided to skip Linux as well for the scope of this project. 

When we worked on platform-specific code, we tried to keep all operating systems in mind; we just did not test all.