<h3>RESTClient(C#)</h3>

<h4>.NET framework 종속정</h4>

- .NET Framework >= 4.8
- Newtonsoft.JSON >= 12.0.0

<br/>  
 
<h4>.NET 종속정</h4>

- .NET 5
- Newtonsoft.JSON >= 12.0.3

<br/>  

<h4>지원 HttpMethod</h4>

- GET
- POST
- PATCH
- PUT
- DELETE
- HEAD  

<br/>  

<h4>사용 방법</h4>

<h5> .NET Framework</h5>

```csharp
using RestClient

public Response RestCall() {
    Response resp = Request.Call(new RequestInfo() {
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

// 비동기.
public async Task<Response> RestCall() {
    Response resp = await RequestAsync.Call(new RequestInfo() {
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


<h5>.NET 5</h5>

```csharp
using RestClient

public Response RestCall() {
    var resp = Request.GetInstance.Call(new RequestInfo() {
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

// 비동기.
public async Task<Response> RestCallAsync() {
    var resp = await RequestAsync.GetInstalce.Call(new RequestInfo() {
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

<br />

<h4>RequestInfo</h4>
HttpWebRequest의 정보를 설정하는 객체 이며 다음과 같은 프로퍼티를 가지고 있습니다.

<br />  
<br />  


<h5>Property<h5>

- URI: string: URL or URI를 설정 합니다. (QueryString 제외)
- Method: HttpMethod: HttpMethod 를 설정 합니다. (default: Get)
- Query: dynamic: QueyString 을 익명 타입으로 지정 합니다.
- Body: dynamic: POST, PUT, PATCH로 전송할 body를 익명 타입으로 지정 합니다.
  ```csharp
  var info = new RequestInfo() {
     Query = {
         key = value, 
         key1 = value1,
         key2 = value2,
     },
     Body = {
         key = value, 
         key1 = value1,
         key2 = value2,
     }
  }
  ```
  Query 와 Body는 dynamic 형식이기 때문에, Class를 사용하도 무방 합니다. 단, Class를 사용하는 경우 field를 기준으로 Qeury와 Body를 만들어 냅니다.
 - RequestMediaType: MediaType: 요청 MediaType을 지정 합니다. JSON / TEXT / FORM 만 지원합니다. (default:JSON)
 - Headers: dynamic : body, query 와 동일 합니다.
 - Encoding: Encoding: body의 encoding 방식을 지정 합니다. (default: UTF8)
 - ThrowRestExceptionWhenStatusNotOK: bool: 요청을 전송하고 응답받은 httpStatus.OK (200)이 아닌 경우   
 Exception 을 throw 할지 여부를 설정 합니다. (defualt : true)  
 true: HttpStatus.OK 가 아닌 경우 Exception throw 합니다.  
 false: HttpStatus.OK 가 아니 더라고 Repsonse 객체를 응답 합니다.
 - KeepAlive: bool: keepAlive 여부를 설정 합니다. (default: true)
 - TimeoutSecond: int: 요청에 대한 timeout 시간을 절성 합니다. (default: 1s)
 - ContinueTimeoutSeconds: int: 100-Continue가 수신될 때까지 기다릴 제한 초입니다.

<br />

<h4>Response</h4>
요청에 대한 응답 정보를 답은 객체 입니다.  

<br />  
<br />  


<h5>Property<h5>

- StatusCode : HttpStatusCode: 응답 결과 StatusCode
- Body: byte[]: 응답 Body
- Headers: List<KeyValuePair<string, string>>: 응답 헤더
- ResponseDataType: MediaType: 응답 MediaType

<br />

<h5>Method<h5>

- GetBodyString(): string: Body를 string 으로 변환 합니다.
- DeserializeBody<T>(): T: Body를 Newtonsoft.Json를 이용하여 T 객체로 변환 합니다.

<br />
<br />

<h4>RESTException</h4>
httpWebRequest 요청 과정에서 에러가 발생하면 throw 하는 Exception 객체 입니다.  

기본적으로는 요청과정에서의 에러와 응답 httpStatus != OK 인 경우 throw 되는 객체 이지만,  
ThrowRestExceptionWhenStatusNotOK 설정에 따라 응답 결과가 httpStatus != OK 인 경우 throw 되지 않습니다.


<br />  
<br />  


<h5>Property<h5>

- Response: Response: 응답 결과에 대한 Response 객체 입니다.
- WebExceptionStstus: WebExceptionStatus: HttpWebException의 ExceptionStatus 입니다.
