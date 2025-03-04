### **Introduction**

- **Name:** A DOTNET IMPLEMENTATION OF REPORT GENERATION FOR A DIGITAL ONE HEALTH SURVEILLANCE SYSTEM
- **Overview:** A Report Generation implementation for a Digital One Health surveillance system built with ASP.NET Core that integrates environmental, veterinary, and human health data through a unified reporting system. The platform leverages advanced AI models (Gemini API and locally-hosted Llama3) to generate actionable reports from submitted health data.

### **Technology Stack**

- **Frontend:** Bootstrap, Razor Views
- **Backend:** ASP.NET Core, C#
- **Database:** SQL Server
- **AI Processing:** Gemini API (Cloud), Llama3 via Ollama (Local)
- **API Gateway:** Flask (Python)
- **Hosting**: Locally exposed via Ngrok

### **Features**

- **Secure Authentication:** Role-based login using ASP.NET Core Identity with role-based access controls
- **Form Submissions:**  Specialized data collection interfaces for environmental scientists, veterinarians, public health specialists, and health officers
- **Report Generation:** AI-generated reports of cross-domain health data using large language models
- **Public Report Viewing:** Curated, admin-approved reports accessible to the general public

### **Installation & Setup**

- **Requirements**

    - .NET SDK 8.0 or later
    - Python & Flask
    - SQL Server
    - Ngrok account (for public exposure)

- **LLM Requirements**
    - Ollama installed (ollama.ai)
    - Llama3 model pulled and configured
 ```
   # Install Llama3 model via Ollama
   ollama pull llama3

   # Verify installation
   ollama list
```

- **System Requirements**
    - Minimum 16GB RAM recommended for running Llama3
    - 10GB+ free disk space for model storage
 
      
- **Steps to Run**

1. **Clone the repositories**
```
   # Main application
   git clone https://github.com/amxrac/report-gen/
   cd digital-one-health
   
   # Flask LLM API Gateway
   git clone https://github.com/amxrac/flask-server
```
   
2. Configure database connection
```
   # Update appsettings.json with your connection string
```
3. Apply database migrations
```
   dotnet ef database update
```
4. Set up the Flask API Gateway
```
cd flask-server
pip install -r requirements.txt
python app.py
```
5. Run the ASP.NET Core application:
```
   cd report-gen
   dotnet run
```
6. Expose via Ngrok (optional)
```
   ngrok http 5230
```


### **Project Structure**

- **ASP.NET Core**
```
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
/app.py 
/requirements.txt
```

### **Usage Guide**

- **User Roles & Permissions**
    
    - Environmentalist: Environmentalists submit data on ecological conditions, pollution metrics, and environmental risk factors that may impact health
    - Veterinary Doctor: Veterinarians submit data on animal health incidents, zoonotic disease patterns, and treatment outcomes across domestic and wildlife populations
    - Specialist: Public Health Specialists submit data on cross-domain data, identify correlations between environmental, animal, and human health indicators
    - Health Officer: Health Officers submit data on human health incidents, disease outbreaks, and community health metrics
    - Admin: System administrators review submissions and approve reports for public display

- **Report Submission**
    - Users log in and submit forms based on their role.
    - Forms are stored.
    - Admin can approve reports for public display
      
- **AI Report Generation**    
    - Admin selects forms through which a report based on submitted data can be generated and selects the model for processing.
    - The system calls either the Flask API (for the local model) or Gemini API (remote).

### **API Documentation**

- **Flask LLM API**
	- Endpoint: `POST /generate`
    - Request:
 ```
{ "prompt": "Generate a One Health report based on data..." }
```
Response:
```
{ "response": "Generated report text..." }
```

### **Contributing**

Contributions are welcome from public health professionals, developers, and data scientists
