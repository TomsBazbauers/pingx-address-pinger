PingX is a simple console tool that allows users to ping multiple IP addresses in parallel. 
It works like the standard Windows `ping` command but allows simultaneous pings to multiple IPs, with response times and other statistics remaining unchanged.


## Features

- **Parallel Pinging**: Ping multiple IP addresses or hostnames simultaneously;
- **Standard Output**: Maintains the familiar ping response time and statistics output;
- **Available network interface display**: Displays available `up` network interfaces, their IP addresses;

## Requirements
- **Operating System**: Windows 10 or later
- **.NET Runtime**: [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0/runtime)
- **Extras**: PingX uses xUnit, Moq, and Fluent Assertions for unit testing

## Installation & Running

### Option (a) - If you trust this code
1. **Download** `PingX.exe` and `pingx.bat` from the [latest release](https://github.com/YourUsername/PingX/releases).
2. **Place** both files in `C:\Users\{YourUserName}\AppData\Local\Microsoft\WindowsApps`
3. **Run** by opening CMD and typing `pingx {IP addresses}`, e.g., `pingx 1.1.1.1 8.8.8.8 192.168.88.251`

### Option (b) - If you do not trust this code
1. **Download or Clone** the source code/repository
2. **Publish** the app by opening the solution's directory and running: `dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false -o publish`
3. **Create** the batch script to run the app without installation by creating a text file with contents: `@echo off "%~dp0PingX.exe" %*` and saving it as a .bat file
4. **Place** both files in `C:\Users\{YourUserName}\AppData\Local\Microsoft\WindowsApps`
5. **Run** by opening CMD and typing `pingx {IP addresses}`, e.g., `pingx 1.1.1.1 8.8.8.8 192.168.88.251`

<br><br>
![pingx-example](https://github.com/user-attachments/assets/91ff4670-9f70-4650-8087-ae3e5f2dc8ea)


