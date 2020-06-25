# VerveGroupEmplomentTask
Create an ASP.Net MVC (.Net Framework and C#) website with a page containing a text box to enter
a name in and a submit button to search GitHub for the name.<br>
Have the back end call the GitHub users API (e.g. https://api.github.com/users/robconery) and get
the users name, location and avatar url from the returned json. Use the repos_url value that’s
returned to get a list of all the repos for the user. Do not use any third party tool for managing the
api connection.<br>
Display the results below the search box. The results should show the username, location, avatar
and the 5 repositories with the highest stargazer_count. For each repository, show the Name, Full
Name, Description and Stargazers. The Name should have a link to the repository.

## Technology & Tools
*	Polly 3.1.1
*	Asp.net Core 3.0
*	Entity Framework core 3.1.1
*	SQL Server 3.1.1 
*	Version Control (Git)
*	Moq 4.13.1
* Xunit 2.4.1
* Xunit Runner 2.4.1 <br/>
## Patterns
*	Retry Pattern  
*	Circuit-Breaker Pattern <br/>
## Security
* HTTPS <br/>
## Features
* Example .yml file for azure
* Backup Database in case of unsuccessful Api calls
