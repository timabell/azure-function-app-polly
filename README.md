Example of polly retries in a http triggered function app

```
[2024-12-18T00:11:11.275Z] Start processing HTTP request GET http://localhost:8899/
[2024-12-18T00:11:11.281Z] Sending HTTP request GET http://localhost:8899/
[2024-12-18T00:11:11.313Z] Received HTTP response headers after 17.0399ms - 500
[2024-12-18T00:11:11.319Z] Retry 1 encountered an error:  InternalServerError. Waiting 00:00:01 before next retry.
[2024-12-18T00:11:12.002Z] Host lock lease acquired by instance ID '000000000000000000000000000C0158'.
[2024-12-18T00:11:12.322Z] Sending HTTP request GET http://localhost:8899/
[2024-12-18T00:11:12.327Z] Received HTTP response headers after 4.2533ms - 500
[2024-12-18T00:11:12.328Z] Retry 2 encountered an error:  InternalServerError. Waiting 00:00:02 before next retry.
[2024-12-18T00:11:14.329Z] Sending HTTP request GET http://localhost:8899/
[2024-12-18T00:11:14.331Z] Received HTTP response headers after 2.2101ms - 500
[2024-12-18T00:11:14.333Z] Retry 3 encountered an error:  InternalServerError. Waiting 00:00:04 before next retry.
[2024-12-18T00:11:18.331Z] Sending HTTP request GET http://localhost:8899/
[2024-12-18T00:11:18.332Z] Received HTTP response headers after 0.66ms - 500
[2024-12-18T00:11:18.334Z] End processing HTTP request after 7069.3016ms - 500
```

Notes.

- Using named http client because the generic one didn't get injected so the retry didn't work
- Azurite has to be running, see `docker-compose.yml` for the setup
- Originally had aspnet bits in the function app but gpt suggested removing them and that works and is simpler

Usage

```shell
./local-setup.sh # runs docker azurite
./run.sh & # runs function app (backgrounded)
./fail-serve.sh & # runs a server that always returns 500 (backgrounded)
# or ./happy-server.sh to see 200 responses. Optionally run this after the first retry to see the eventual success
./trigger.sh # triggers the function app to start making requests
```
