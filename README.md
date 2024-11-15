
# 🎮 MOCART-Homework 🛒

## 📋 Overview
This project is a Unity assignment for a developer position, featuring a dynamic 3D shelf display that fetches and presents product information from a remote server. The game is hosted on [itch.io](https://uzano101.itch.io/mocart-homework) and can be accessed using the password: `alonuzan`. 🔑

## 📂 Project Structure
Here's a breakdown of the project's main components:

-  **GameLoader.cs**: Initializes the game, fetches product data, and loads the main scene.
-  **ProductFetcher.cs**: Retrieves product information from a remote API.
-  **ShopManager.cs**: Manages products and allows updates to their details.
-  **UIManager.cs**: Displays product information dynamically in the user interface.
-  **Handler.cs**: Handles user interactions for editing product names and prices.
-  **ObjectRotator.cs**: Rotates 3D product models for a dynamic display.

## ✨ Features
-  Fetches product data in real-time from a server.
-  Displays products in a rotating 3D shelf format.
-  Allows users to edit product names and prices directly in the UI.
-  Smooth animations and interactive elements.

## 🎮 How to Play
1.  Visit the [itch.io page](https://uzano101.itch.io/mocart-homework).
2.  Enter the password: `alonuzan`.
3.  View products displayed on the shelf.
4.  Click "Edit" to modify product names or prices, then click "Submit" to save changes.

## 🛠️ Technologies Used
-  **Unity** (version 2023.3.23f1)
-  **C#** for scripting
-  **TextMeshPro** for UI text elements
-  **UnityWebRequest** for server communication

## 🚀 Running the Project Locally
1.  Download the project files and open them in **Unity (2023.3.23f1)**.
2.  Ensure dependencies like TextMeshPro are installed.
3.  Run the `LoadScene` scene to start the game.
