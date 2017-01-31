CSHARP_COMPILER := mcs
SERVER_DIR := AdressbuchServer
SERVER_SOURCES:= Program.cs ControllerServer.cs ClientSocket.cs ServerSocket.cs Model.cs Person.cs
CLIENT_DIR := AdressbuchClientConsole
CLIENT_SOURCES := ClientSocket.cs ControllerClient.cs Person.cs Program.cs View.cs

all: server client

server:
	cd $(SERVER_DIR) && $(CSHARP_COMPILER) $(SERVER_SOURCES) -o bin/Debug/server.exe

client:
	cd $(CLIENT_DIR) && $(CSHARP_COMPILER) $(CLIENT_SOURCES) -o bin/Debug/client.exe
