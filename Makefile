CSHARP_COMPILER := mcs
COMMON_DIR := common
COMMON_SOURCES:= Person.cs
SERVER_DIR := AdressbuchServer
SERVER_SOURCES:= Program.cs ControllerServer.cs ClientSocket.cs ServerSocket.cs Model.cs
CLIENT_DIR := AdressbuchClientConsole
CLIENT_SOURCES := ClientSocket.cs ControllerClient.cs Program.cs View.cs

all: server client

server:
	cd $(SERVER_DIR) && $(CSHARP_COMPILER) $(SERVER_SOURCES) ../$(COMMON_DIR)/$(COMMON_SOURCES) -o bin/Debug/server.exe

client:
	cd $(CLIENT_DIR) && $(CSHARP_COMPILER) $(CLIENT_SOURCES) ../$(COMMON_DIR)/$(COMMON_SOURCES) -o bin/Debug/client.exe
