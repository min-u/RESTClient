<h2>RedisClient(C#)</h2>
<h3>dependancy</h3>

 - .NET Framework >= 4.8
 - Newtonsoft.JSON >= 12.0.0.0

<h3>Support </h3>

 - GET
 - POST
 - PATCH
 - PUT
 - DELETE
 - HEAD


<h2>Basic Uasge</h2>

```csharp
using RestClient

public async Response RestCall() {
    Response resp = Request.call(new RequestInfo() {
        URI = 'http://example.com/books/sports/1',
        Method = HttpMethod.GET,
        RequestDataType = MediaType.JSON,
        ResponseDataType = MediaType.JSON,
        Query = new {
            deleted = false
        }
    });
    return resp;
}

resp.StatusCode => System.Net.HttpStatusCode
resp.Encoding => Systen.Encoding
resp.Headers => List<KeyValuePair<string, string>>
resp.Body => byte[]

// when ResponseDataType in (JSON, XML)
resp.DeserializeBody<T> => JsonConvert.DeserializeObject<T>

// when ResponseDataType not in (JSON, XML)
resp.GetBodyString = string
```

<h3>Async Uasge</h3>

```csharp
using RestClient

public async Task<Response> RestCall() {
    Response resp = await RequestAsync.call(new RequestInfo() {
        URI = 'http://example.com/books/sports/1',
        Method = HttpMethod.GET,
        RequestDataType = MediaType.JSON,
        ResponseDataType = MediaType.JSON,
        Query = new {
            deleted = false
        }
    });

    return resp;
}

resp.StatusCode => System.Net.HttpStatusCode
resp.Encoding => Systen.Encoding
resp.Headers => List<KeyValuePair<string, string>>
resp.Body => byte[]

// when ResponseDataType in (JSON, XML)
resp.DeserializeBody<T> => JsonConvert.DeserializeObject<T>

// when ResponseDataType not in (JSON, XML)
resp.GetBodyString = string
```