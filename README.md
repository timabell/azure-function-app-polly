Example of polly retries in a http triggered function app

https://github.com/timabell/azure-function-app-polly

```
[2024-12-18T22:52:47.755Z] Worker process started and initialized.

Functions:

  HttpTriggerFunction: [GET,POST] http://localhost:7071/api/HttpTriggerFunction

For detailed output, run func with --verbose flag.
[2024-12-18T22:52:54.911Z] Executing 'Functions.HttpTriggerFunction' (Reason='This function was programmatically called via the host APIs.', Id=12345b89-e0b7-46f7-a8fb-d5bed0c87a91)
[2024-12-18T22:52:55.046Z] Start processing HTTP request GET http://localhost:8899/
[2024-12-18T22:52:55.058Z] Sending HTTP request GET http://localhost:8899/
[2024-12-18T22:52:55.096Z] Received HTTP response headers after 17.6235ms - 429
[2024-12-18T22:52:55.104Z] Execution attempt. Source: 'HttpTriggerFunctionClient-example-handler//Retry', Operation Key: '', Result: '429', Handled: 'True', Attempt: '0', Execution Time: 43.5664ms
[2024-12-18T22:52:55.113Z] Resilience event occurred. EventName: 'OnRetry', Source: 'HttpTriggerFunctionClient-example-handler//Retry', Operation Key: '', Result: '429'
[2024-12-18T22:52:56.882Z] Sending HTTP request GET http://localhost:8899/
[2024-12-18T22:52:56.886Z] Received HTTP response headers after 4.1432ms - 429
[2024-12-18T22:52:56.887Z] Execution attempt. Source: 'HttpTriggerFunctionClient-example-handler//Retry', Operation Key: '', Result: '429', Handled: 'True', Attempt: '1', Execution Time: 4.8842ms
[2024-12-18T22:52:56.888Z] Resilience event occurred. EventName: 'OnRetry', Source: 'HttpTriggerFunctionClient-example-handler//Retry', Operation Key: '', Result: '429'
[2024-12-18T22:53:00.811Z] Sending HTTP request GET http://localhost:8899/
[2024-12-18T22:53:00.812Z] Received HTTP response headers after 1.0909ms - 429
[2024-12-18T22:53:00.812Z] Execution attempt. Source: 'HttpTriggerFunctionClient-example-handler//Retry', Operation Key: '', Result: '429', Handled: 'True', Attempt: '2', Execution Time: 1.5748ms
[2024-12-18T22:53:00.812Z] Resilience event occurred. EventName: 'OnRetry', Source: 'HttpTriggerFunctionClient-example-handler//Retry', Operation Key: '', Result: '429'
[2024-12-18T22:53:06.825Z] Sending HTTP request GET http://localhost:8899/
[2024-12-18T22:53:06.827Z] Received HTTP response headers after 1.232ms - 200
[2024-12-18T22:53:06.827Z] Execution attempt. Source: 'HttpTriggerFunctionClient-example-handler//Retry', Operation Key: '', Result: '200', Handled: 'False', Attempt: '3', Execution Time: 1.6907ms
[2024-12-18T22:53:06.841Z] End processing HTTP request after 11809.1965ms - 200
[2024-12-18T22:53:06.909Z] Executed 'Functions.HttpTriggerFunction' (Succeeded, Id=12345b89-e0b7-46f7-a8fb-d5bed0c87a91, Duration=12021ms)
```

Notes.

- Using named http client because the generic one didn't get injected so the retry didn't work
- Azurite has to be running, see `docker-compose.yml` for the setup
- Originally had aspnet bits in the function app but gpt suggested removing them and that works and is simpler
- [Microsoft.Extensions.Http.Polly](https://github.com/App-vNext/Polly.Extensions.Http?tab=readme-ov-file) is deprecated, long live [polly+resilience](https://learn.microsoft.com/en-gb/dotnet/core/resilience/http-resilience?tabs=dotnet-cli)

Usage

```shell
./local-setup.sh # runs docker azurite
./run.sh & # runs function app (backgrounded)
./fail-serve.sh & # runs a server that always returns 500 (backgrounded)
# or ./happy-server.sh to see 200 responses. Optionally run this after the first retry to see the eventual success
./trigger.sh # triggers the function app to start making requests
```
