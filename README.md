# NerAnonymizer

Perhaps eventually a working anonymizer using finbert-ner and ML.Net

Needs a working wordpiece tokenizer first though...

## Build

docker build --tag nerfuncish:v0.3.0 .

## Run

docker run -p 8000:8080 -it nerfuncish:v0.3.0
