# concept_long_running_task_api_call

This repository provides a conceptional implementation of how to handle long-running calls to an RESTful API.

## Flow

First, call the `/api/Orders` with a `POST` request.  

The result of this Operation will return this response:  

```
HTTP/1.1 202 Accepted
Date: Thu, 27 Jun 2024 12:24:40 GMT
Server: Kestrel
Location: /api/Tasks/43ea50cd-44a5-487e-b9f9-1879b1aa302e/Status
Content-Length: 0


```

Following the provided `Location`-Header, we get this response:  

```
HTTP/1.1 301 Moved Permanently
Date: Thu, 27 Jun 2024 12:24:48 GMT
Server: Kestrel
Location: /api/Tasks/43ea50cd-44a5-487e-b9f9-1879b1aa302e/Status
Content-Length: 0
Retry-After: 10


```

Now we wait 10 seconds until the task is finished.  

After the task is completed, we can change the response:  

```
HTTP/1.1 301 Moved Permanently
Date: Thu, 27 Jun 2024 12:25:30 GMT
Server: Kestrel
Location: /api/Orders/43ea50cd-44a5-487e-b9f9-1879b1aa302e
Content-Length: 0


```

We again follow this redirect and now get our created resource:  

```
HTTP/1.1 200 OK
Date: Thu, 27 Jun 2024 12:25:33 GMT
Server: Kestrel
Transfer-Encoding: chunked
Content-Type: application/json; charset=utf-8

{"id":"d6568476-009e-43bd-a9a3-2ac68f02e1a0"}
```

## Considerations

First things first.  

It is not entirely well-documented how the `Retry-After`-Header works.  
Using the Swagger Docs "Try it out" feature (which uses `fetch`) ignored the `Retry-After`-Header.  

This resulted in an error: → Too many redirects  
