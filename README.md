# .NET Developer Interview Challenge: Secure & AI-Assisted Notification API

## Overview
Welcome to the interview assignment! You are tasked with completing and securing a backend RESTful Web API for a **Notification Management System** using **.NET 10** and **SQLite**. 

To simulate a modern development workflow, **you are encouraged and expected to use AI assistants** (such as GitHub Copilot, ChatGPT, or Claude) to accelerate your coding, generate logic, and verify your implementation.

**Repository Name:** `interview-dotnet-api`  
**C# Namespace:** `interview_dotnet_api`  
**Time Limit:** 60 Minutes (includes 10 Minutes for intial setup)

---

## Getting Started

1. Clone this repository or extract the provided ZIP file.
2. Open the project:
   * Visual Studio: open `interview-dotnet-api.sln` (File → Open → Project/Solution). Do *not* use "Open Folder" — open the `.sln` so the project loads with full Solution Explorer and F5 support.
   * VS Code: open the project folder and install the **C# Dev Kit** extension when prompted.
   * Rider: open `interview-dotnet-api.sln`.
3. Verify your machine is ready (recommended before the live session) by running the readiness check from the project root:
   ```bash
   ./scripts/check-readiness.sh          # macOS / Linux / WSL / Git Bash
   pwsh ./scripts/check-readiness.ps1     # Windows PowerShell
   ```
   It confirms the .NET 10 SDK, EF Core CLI tools, a build, and that the app starts and serves OpenAPI.
4. Run the migrations to initialize your local SQLite database:
   ```bash
   dotnet ef database update
   ```
5. Start the application:
   ```bash
   dotnet run
   ```
   Then browse the interactive API reference (Scalar UI) at `/scalar`, e.g. [https://localhost:5001/scalar](https://localhost:5001/scalar) or [http://localhost:5000/scalar](http://localhost:5000/scalar). The raw OpenAPI document is served at `/openapi/v1.json`.

   > **Note:** If your browser warns about an untrusted certificate on the HTTPS URL, run `dotnet dev-certs https --trust` once to trust the local dev certificate. (Not needed if you use the `http://localhost:5000` URL.)
---

## Technical Objectives

### Task 1: Secure the API (20 Minutes)
The starter project has a `NotificationController` with fully functional CRUD endpoints, but it is currently completely insecure and open to the public.

1. **Enforce Authentication**: Protect all `Notification` endpoints using the pre-configured JWT authentication system. Unauthenticated requests must return `401 Unauthorized`.
2. **Implement Data Isolation**: Modify the data access logic so that users can **only** view, dispatch, update, or delete notifications belonging to them.
   * Extract the User ID from the authenticated token claims (`User.FindFirst(ClaimTypes.NameIdentifier)`).
   * Ensure User A cannot access, read, or delete User B's notifications by guessing a notification ID.

### Task 2: AI-Assisted Smart Categorisation (20 Minutes)
Use an AI assistant of your choice to implement an automated keyword/channel parser.

1. **The Feature**: When a new notification is created (`POST /api/notifications`), the system must automatically assign a `Category` tag (e.g., *Marketing*, *Security*, *Billing*, or *System*) based on keywords found in the notification title or body message.
2. **The AI Constraint**: Write a prompt for your AI assistant to generate a lightweight, regex-based, or dictionary-based classification method in C#. Integration of external paid AI APIs is **not** required—keep the logic local and fast.
3. **Update Model**: Ensure the `NotificationItem` model handles and saves this new `Category` property to the database.

### Task 3: AI Submission & Review (10 Minutes)
Document your workflow to demonstrate how you collaborate with AI.

1. Save your AI prompt history. You can export a chat link, take screenshots, or copy-paste the text into a `prompts.txt` file.
2. Ask your AI assistant to review your Task 1 security implementation for potential vulnerabilities, and note any changes you made based on its feedback.

---

## Evaluation Criteria
Your submission will be scored out of 100 points based on:

* **Security Awareness (40%):** Correct application of user claims mapping and complete prevention of horizontal privilege escalation (ID tampering across tenant notifications).
* **AI Collaboration (30%):** Prompt engineering skills. We look for specific, iterative, and critical prompts rather than blind copy-pasting of broken code.
* **Code Quality & Velocity (20%):** Working endpoints that compile successfully, utilize proper HTTP status codes, and use asynchronous EF Core methods.
* **Git Hygiene (10%):** Atomic commits detailing what was achieved in each phase.

---

Good luck!
