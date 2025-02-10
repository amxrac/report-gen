### **Introduction**

- **Name:** A DOTNET IMPLEMENTATION OF REPORT GENERATION FOR A DIGITAL ONE HEALTH SURVEILLANCE SYSTEM
- **Description:** A Report Generation implementation for a Digital One Health surveillance system using ASP.NET Core that allows role-based form submissions, report generation using Gemini API and Deepseek-R1 via Ollama locally, and public report viewing.
- **Purpose:** To track and assess environmental, veterinary, and human health risks through an integrated reporting system.
- **Current Status:** Accessible via Ngrok

### **Technology Stack**

- **Frontend:** Bootstrap, Razor Views
- **Backend:** ASP.NET Core, C#
- **Database:** SQL Server
- **AI Models:** Gemini API (Remote), DeepSeek-R1 via Ollama (Local)
- **Hosting**: Locally exposed via Ngrok

### **Features**

- **User Authentication:** Role-based login using ASP.NET Core Identity
- **Form Submissions:** Role-specific forms (Environmentalist, Veterinary, Health Officer, Specialist)
- **Report Generation:** AI-generated reports using Gemini or DeepSeek-R1
- **Public Report Viewing:** Admin-approved reports are available on the homepage
- **Editing & Versioning:** Users can edit their submitted forms

### **Installation & Setup**

- **Requirements**

	- .NET SDK
	- Python & Flask
	- SQL Server / Azure SQL
	- Ngrok (for exposing locally hosted services)

- **Steps to Run**

1. Clone the repository.
2. Set up the database using EF migrations.
3. Run the Flask API for the LLM:
		```
```bash
	python app.py
```
4. Start the ASP.NET Core application:
   		```
```bash
	dotnet run
```
5. Expose the API via Ngrok:
   	```
```bash
	ngrok http 5230
```

### **Project Structure**

- **ASP.NET Core**
  	```
```bash
	/Controllers
	/Models 
	/Services 
	/Views 
	/wwwroot
	/Data
	/Documents
	/ViewModels
```
**Flask API**
	```
```bash
	/app.py 
	/requirements.txt
```

### **Usage Guide**

- **User Roles & Permissions**
    
    - Environmentalist: Submit environmental forms
    - Veterinary Doctor: Submit veterinary forms
    - Specialist: Submit Public Health Specialist forms
    - Health Officer: Submit public health forms
    - Admin: Approves forms for report generation
- **Report Submission**
    - Users log in and submit forms based on their role.
    - Forms are stored.
    - Admin can approve reports for public display.
- **AI Report Generation**    
    - Admin selects forms through which a report based on submitted data can be generated and selects the model for processing.
    - The system calls either the Flask API (for the local model) or Gemini API (remote).

### **API Documentation**

- **Flask LLM API**
	- Endpoint: `POST /generate`
    - Payload:
      ```
 ```bash
{ "prompt": "Generate a One Health report based on data..." }
```
Response:
	```
```bash
{ "response": "Generated report text..." }
```


### **Deployment & Hosting**

- Currently exposed via Ngrok.
- Future plans: Host ASP.NET Core API on Railway / Azure.

### **Contributing & Future Plans**

- Improve UI/UX for better report viewing.
- Store generated reports in the database.
- Implement a proper admin dashboard.
