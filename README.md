# .NetApiAutomation
.Net Automation framework for testing API

# Setup
1. Download and install Microsoft .NET SDK
2. Download and install VSCode
3. Download and install Git
4. On command prompt, type : git clone https://github.com/jredimer2/.NetApiAutomation.git
5. Open the folder .NetApiAutomation in VSCode

# Running test
6. On VSCode terminal type 'dotnet build'
7. On VSCode terminal type 'dotnet test'
 

# Adding Secrets
dotnet user-secrets init
dotnet user-secrets set "Email" "eve.holt@reqres.in" 
dotnet user-secrets set "Password" "password" 


# Using CLI
* I wasn't able to implement Command Line Interface for the test. I've run out of time and I had issues installing NUnit.ConsoleRunner. No time to debug it.
