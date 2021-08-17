#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Organization/src/TianCheng.Organization.Restful/TianCheng.Organization.Restful.csproj", "Organization/src/TianCheng.Organization.Restful/"]
COPY ["Organization/src/TianCheng.Organization/TianCheng.Organization.csproj", "Organization/src/TianCheng.Organization/"]
COPY ["Infrastructure/TianCheng.Controller.Core/src/TianCheng.Controller.Core.csproj", "Infrastructure/TianCheng.Controller.Core/src/"]
COPY ["Infrastructure/Services/TianCheng.ServicesInform/src/TianCheng.ServicesInform.csproj", "Infrastructure/Services/TianCheng.ServicesInform/src/"]
COPY ["Infrastructure/TianCheng.ConsulHelper/src/TianCheng.ConsulHelper.csproj", "Infrastructure/TianCheng.ConsulHelper/src/"]
COPY ["Infrastructure/TianCheng.Common/src/TianCheng.Common.csproj", "Infrastructure/TianCheng.Common/src/"]
COPY ["Infrastructure/Services/TianCheng.Service.Core/src/TianCheng.Service.Core.csproj", "Infrastructure/Services/TianCheng.Service.Core/src/"]
COPY ["Infrastructure/DAL/TianCheng.DAL/src/TianCheng.DAL.csproj", "Infrastructure/DAL/TianCheng.DAL/src/"]
COPY ["Infrastructure/Services/TianCheng.Services.AuthJwt/src/TianCheng.Services.AuthJwt.csproj", "Infrastructure/Services/TianCheng.Services.AuthJwt/src/"]
COPY ["Organization/src/TianCheng.Organization.DTO/TianCheng.Organization.DTO.csproj", "Organization/src/TianCheng.Organization.DTO/"]
COPY ["Organization/src/TianCheng.Organization.Services/TianCheng.Organization.Services.csproj", "Organization/src/TianCheng.Organization.Services/"]
COPY ["Infrastructure/DAL/TianCheng.DAL.MongoDB/src/TianCheng.DAL.MongoDB.csproj", "Infrastructure/DAL/TianCheng.DAL.MongoDB/src/"]
RUN dotnet restore "Organization/src/TianCheng.Organization.Restful/TianCheng.Organization.Restful.csproj"
COPY . .
WORKDIR "/src/Organization/src/TianCheng.Organization.Restful"
RUN dotnet build "TianCheng.Organization.Restful.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TianCheng.Organization.Restful.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TianCheng.Organization.Restful.dll"]