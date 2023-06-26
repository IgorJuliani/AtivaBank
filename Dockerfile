FROM registry.access.redhat.com/ubi8/dotnet-60 AS runtime
EXPOSE 8080
WORKDIR /
COPY /AtivaBank ./app
WORKDIR /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENTRYPOINT ["dotnet", "AtivaBank.dll"]
