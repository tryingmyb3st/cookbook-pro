# FROM mcr.microsoft.com/dotnet/sdk:8.0

# WORKDIR /app

# RUN dotnet tool install --global dotnet-ef
# ENV PATH="$PATH:/root/.dotnet/tools"

# # Копируем ВЕСЬ Cookbook как есть
# COPY src/Cookbook ./src/Cookbook

# WORKDIR /app/src/Cookbook

# RUN dotnet restore Cookbook.sln

# WORKDIR /app/src/Cookbook/CookbookWebApi

# EXPOSE 5000

# CMD ["dotnet", "run", "--urls=http://0.0.0.0:5000"]


FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Копируем проект
COPY src/Cookbook ./src/Cookbook
WORKDIR /app/src/Cookbook

# Восстанавливаем зависимости
RUN dotnet restore Cookbook.sln

# Устанавливаем локально dotnet-ef
RUN dotnet new tool-manifest --force
RUN dotnet tool install dotnet-ef --version 8.0.0

WORKDIR /app/src/Cookbook/CookbookWebApi
EXPOSE 5000

# Используем локальный dotnet-ef для миграций при запуске
CMD ["sh", "-c", "dotnet tool run dotnet-ef database update && dotnet run --urls=http://0.0.0.0:5000"]