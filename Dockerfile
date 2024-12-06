FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
ARG TARGET_ARCH=arm64

RUN apt update
RUN apt install -y clang zlib1g-dev

WORKDIR /source

COPY *.sln .

COPY NerAnonymizer/*.csproj ./NerAnonymizer/
COPY NerAnonymizerFuncish/*.csproj ./NerAnonymizerFuncish/

COPY finbert-ner-onnx/model.onnx ./finbert-ner-onnx/
COPY finbert-ner-onnx/vocab.txt ./finbert-ner-onnx/
COPY finbert-ner-onnx/config.json ./finbert-ner-onnx/

WORKDIR /source/NerAnonymizerFuncish
RUN dotnet restore -a $TARGET_ARCH

WORKDIR /source

COPY NerAnonymizer/. ./NerAnonymizer/
COPY NerAnonymizerFuncish/. ./NerAnonymizerFuncish/

WORKDIR /source/NerAnonymizerFuncish
RUN dotnet publish -a $TARGET_ARCH -c Release -o /app --self-contained

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled

WORKDIR /app
COPY --from=build /app ./
COPY --from=build /source/finbert-ner-onnx ./finbert-ner-onnx/

USER $APP_UID
ENTRYPOINT ["./NerAnonymizerFuncish"]

EXPOSE 8080