# stock-trading-with-dotnet-isc1134

Summary: This sample dotnet code is for course ISC1134 - Stock Trading with dotnet. 
For access to this course, register and login at [learning.intersystems.com](https://learning.intersystems.com) and 
then navigate to the course page: 
[Stock Trading with DotNet](https://learning.intersystems.com/course/view.php?name=.NET%20Financial%20Play)

### IDE

We recommend to use Visual Studio Code to run this sample code.

### How to run

1.  Verify you have an [<span class="urlformat">instance of InterSystems IRIS</span>](https://learning.intersystems.com/course/view.php?name=Get%20InterSystems%20IRIS), and an IDE that supports Node.js (such as **Visual Studio Code**). If you are using AWS, Azure, or GCP, that you have followed the steps to [change the password for InterSystems IRIS](https://docs.intersystems.com/irislatest/csp/docbook/DocBook.UI.Page.cls?KEY=ACLOUD#ACLOUD_interact).

2.  If you are using AWS, GCP, or Microsoft Azure, load the sample stock data into InterSystems IRIS:  
    `$ iris load http://github.com/intersystems/Samples-Stock-Data`  
    If you are using InterSystems Labs, the sample stock data is already loaded. You can skip to the next step.

In your IDE:
1. Clone this repo and open the repo in Visual Studio Code.
2. Copy `C#.csproj`, `InterSystems.Data.IRISClient.dll`, `InterSystems.Data.XEP.dll` from *enviroment* folder into your working directory.
For example, you want to use XEP to connect to InterSystems IRIS server, put those 3 files in *Solutions/XEP folder*.

In the terminal of Visual Studio Code:

1. Navigate to working directory: ```cd Solutions/XEP```

2. Run: ```dotnet run ```

### Contents

* ADO.NET Sample code - Connect your DotNet application to InterSystems IRIS using ADO.NET to store and retrieve data with SQL.
* XEP Sample code - Connect your DotNet application to InterSystems IRIS to store real-time objects.
* Native API Sample code - Connect your DotNet application to InterSystems IRIS to store data natively and use methods built within InterSystems IRIS.
* Multi-model Sample code - Use ADO.NET, Native API, and XEP side-by-side to query data relationally, populate values using methods written within InterSystems IRIS, and store objects directly.
* Entity Framework Sample code - Use the third-party tool, Entity Framework, to do object relational mapping and interact with data in InterSystems IRIS.

Any and all code provided in these materials is provided solely for demonstrative and illustrative purposes and is not intended for use in production. 
