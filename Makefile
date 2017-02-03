CSHARP_COMPILER := mcs
COMMON_SOURCES:= common/Person.cs common/ClientSocket.cs
SERVER_SOURCES:= AdressbuchServer/Program.cs AdressbuchServer/ControllerServer.cs AdressbuchServer/ServerSocket.cs AdressbuchServer/Model.cs
CLIENT_SOURCES := AdressbuchClientConsole/ControllerClient.cs AdressbuchClientConsole/Program.cs AdressbuchClientConsole/View.cs

all: server client

server:
	$(CSHARP_COMPILER) $(SERVER_SOURCES) $(COMMON_SOURCES) -out:AdressbuchServer/bin/Debug/server.exe

client:
	$(CSHARP_COMPILER) $(CLIENT_SOURCES) $(COMMON_SOURCES) -out:AdressbuchClientConsole/bin/Debug/client.exe
