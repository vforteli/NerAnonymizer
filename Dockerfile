FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .

COPY NerAnonymizer/*.csproj ./NerAnonymizer/
COPY NerAnonymizerFuncish/*.csproj ./NerAnonymizerFuncish/

COPY finbert-ner-onnx/model.onnx ./finbert-ner-onnx/
COPY finbert-ner-onnx/vocab.txt ./finbert-ner-onnx/
COPY finbert-ner-onnx/config.json ./finbert-ner-onnx/

WORKDIR /source/NerAnonymizerFuncish
RUN dotnet restore

WORKDIR /source

COPY NerAnonymizer/. ./NerAnonymizer/
COPY NerAnonymizerFuncish/. ./NerAnonymizerFuncish/

WORKDIR /source/NerAnonymizerFuncish
RUN dotnet publish -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
COPY --from=build /source/finbert-ner-onnx ./finbert-ner-onnx/

USER $APP_UID
ENTRYPOINT ["dotnet", "NerAnonymizerFuncish.dll"]