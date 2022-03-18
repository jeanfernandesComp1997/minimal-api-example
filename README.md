# Dotnet 6 Minimal API com EFCore MySql Exemplo
Seguindo boas práticas citadas pelo Eduardo Pires (dev.io)

## Requisitos

    - Dotnet 6 SDK
    - Entity Framework Core
    - MySql

### Entity Framework
    
    - Instalando o CLI do Entity Framework
    ```bash
        dotnet tool install --global dotnet-ef
    ```

    ```bash
        dotnet tool update --global dotnet-ef
    ```

### MySql com Docker

    - Executando um container MySql no docker
    ```bash
        docker run --name mysql-cluster -p 3306:3306 -e MYSQL_ROOT_PASSWORD=password -d mysql
    ```

    - Executando o mesmo container posteriormente
    ```bash
        docker ps -a
    ```

    ```bash
        docker container start id_container
    ```

## Criando a primeira Migration

    ```bash
        dotnet ef migrations add Initial -p ./Minimal.Api.csproj
    ```

## Aplicando Migration

    ```bash
        dotnet ef database update -p ./Minimal.Api.csproj
    ```