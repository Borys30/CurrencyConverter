Description: 
A simple desktop application for converting currencies using live exchange rates provided by Frankfurter API. 
Built with .NET Framework 4.8, WPF, XAML, MVVM architecture. 


Developed functionality: 
- Select source and target currencies. 
- Choose custom start and end dates. 
- Live currency rates will be requested via [Frankfurter API](https://www.frankfurter.app/) according to the last business date. 
- Visibility the historical rates for the user provided data range. 
- Using async/await and thread safe principles to guarantee UI main thread responsiveness and data consistency. 
- Manipulated data using LINQ. 
- Built-in caching to minimize API calls. 
- Clean WPF UI with data binding via XAML and dependency injection. 
- Following no code behind principles for developing loose coupled code. 
- Unit testing code samples using xUnit and NSubstitute frameworks. 


Technical stack and tools: 
- .NET Framework 4.8. 
- WPF (XAML) and MVVM Pattern. 
- Frankfurter API. 
- xUnit and NSubstitute for testing. 


Steps to run: 
1. Clone repo 'git clone git@github.com:Borys30/CurrencyConverter.git'. 
2. Open solution 'CurrencyConverter.sln' and run. 