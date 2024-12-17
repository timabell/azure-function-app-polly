#!/bin/bash -v
while true; do echo -e 'HTTP/1.1 500 Internal Server Error\r\nContent-Length: 0\r\n\r\n' | nc -l -p 8899; done
