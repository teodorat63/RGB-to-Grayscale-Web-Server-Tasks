# RGB to Grayscale Web Server - Optimized Version

This README covers the details of the enhanced version of the web server project that converts images from RGB format to grayscale. This optimized server incorporates various improvements in image processing, concurrency handling, and code organization to deliver a more efficient and maintainable solution.

## Introduction
This project is a console-based web server built using the .NET Framework. It allows clients to connect and request image files, which are then converted to grayscale and served back to the client. The server leverages concurrency to handle multiple client connections efficiently and caches responses to improve performance.

## Features:
- Implements a console-based web server using .NET Framework.
- Converts images to grayscale format with optimized algorithms.
- Log all incoming requests and information about their processing, including any errors and details about successful processing.
- Caches responses to incoming requests in memory, enabling faster responses for repeated requests.
- Utilizes the System.Threading.Tasks library to handle multiple client connections efficiently using tasks.
- Improved code organization for better readability and maintainability.

## Usage:
Once the server is running, it will listen for incoming TCP connections on the specified IP address and port. Clients can connect to the server using a web browser. The server supports HTTP GET requests for both web pages and images. When requesting images, users specify the filename in the request URL, and the server converts the image to grayscale before serving it to the client.

## Dependencies:
This project relies on the following dependencies:
- .NET Framework: The project is developed using C# and requires the .NET Framework to run.
- System.Drawing: Used for image processing and manipulation.
- System.Net.Sockets: Provides classes for network communication.
- System.Threading.Tasks: Used for efficient handling of multiple client connections using tasks.
- System.Runtime.Caching: Used for caching grayscale images to improve performance.

## Enhancements Over Previous Version
- Optimized Image Processing Algorithm: The image processing algorithm has been optimized to enhance performance, reducing the time taken to convert images to grayscale.
- Task-Based Concurrency: Replaced manual threading with the Task class from the System.Threading.Tasks library, allowing for better management of concurrent operations.
- Improved Code Organization: The code has been refactored for better readability, maintainability, and adherence to best practices.

## Future Improvements:
Some potential enhancements for this project include:
- Enhancing error handling and logging to provide better feedback to clients.
- Continue to optimize image processing algorithms for even better performance.
- Ensure thread safety and further improve concurrency handling.

## Note
This project is submitted as part of the assignment covering multithreaded programming using the .NET Framework. The optimized version showcases improvements in performance, concurrency handling, and code organization compared to the initial version.
