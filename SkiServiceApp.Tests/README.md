# Ski Service App Tests

<!--TOC-->
  - [Overview](#overview)
  - [Test Structure](#test-structure)
    - [Mirroring Example](#mirroring-example)
  - [Docker Integration](#docker-integration)
  - [Running the Tests](#running-the-tests)
<!--/TOC-->

## Overview

Welcome to the testing suite of the Ski Service App. 
This suite uses xUnit to ensure the application runs as intended and 
performs its expected functionalities.

The tests are structured to validate various components of the Ski Service App, 
ensuring each part is functioning correctly and efficiently. 
They are primarily focused on the ViewModel layer, model data integrity,
and authentication processes.

## Test Structure

The test project mirrors the structure of the main application. 
For every component, service, or feature in the `SkiServiceApp` you want to test, 
create a corresponding test file in `SkiServiceApp.Tests`. 
This mirroring ensures that navigating the test project is intuitive and easy to understand.

### Mirroring Example

If you have a class you want to test in `SkiServiceApp.Common`, you should place the corresponding test in `SkiServiceApp.Tests.Common`.

Similarly, ViewModel tests corresponding to `SkiServiceApp.ViewModels` are located in `SkiServiceApp.Tests.ViewModels`.

## Docker Integration

For a subset of tests, specifically those under the `[Collection("Docker Collection")]`, a real Docker environment is spun up to ensure the tests run in a real-world scenario. This is crucial for integration testing and ensuring that the app interacts correctly with external services and dependencies.

`DockerFixture` Manages the setup and teardown of the Docker environment for tests.

## Running the Tests

Before running the tests, ensure you have Docker installed and configured, as some tests require a Docker environment to simulate real-world scenarios.

1. Clone the repository and navigate to the `SkiServiceApp.Tests` directory.
2. Use your preferred method to run xUnit tests. This could be through your IDE or via the command line using `dotnet test`.
