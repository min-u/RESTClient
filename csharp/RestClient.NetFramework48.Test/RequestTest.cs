using Microsoft.VisualStudio.TestTools.UnitTesting;

using MockHttpServer;

using Newtonsoft.Json;

using RESTClient;
using RESTClient.Enum;

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RestClient.Test
{
    [TestClass]
    public class RequestTest
    {
        #region Test JsonClass
        public class BodyObject
        {
            [JsonProperty("int")]
            public int intValue { get; set; }

            [JsonProperty("string")]
            public string strValue { get; set; }


            public override bool Equals(object obj)
            {
                BodyObject source = (BodyObject) obj;
                return source.intValue == this.intValue && source.strValue == this.strValue;
            }
        }


        public class ResponseObject
        {
            [JsonProperty("int")]
            public int intValue { get; set; }
        }
        #endregion

        private MockServer mockServer;

        [TestInitialize]
        public void Initialize()
        {
            var mockeServerHandlers = new List<MockHttpHandler>() {
                #region empty body and httpStatus.OK
                new MockHttpHandler("/rest", "GET", (req, res, parms) => {
                    res.StatusCode = (int) HttpStatusCode.OK;
                }),
                new MockHttpHandler("/rest", "PATCH", (req, res, parms) => {
                    res.StatusCode = (int) HttpStatusCode.OK;
                }),
                new MockHttpHandler("/rest", "POST", (req, res, parms) => {
                    res.StatusCode = (int) HttpStatusCode.OK;
                }),
                new MockHttpHandler("/rest", "PUT", (req, res, parms) => {
                    res.StatusCode = (int) HttpStatusCode.OK;
                }),
                new MockHttpHandler("/rest", "DELETE", (req, res, parms) => {
                    res.StatusCode = (int) HttpStatusCode.OK;
                }),
                #endregion

                #region callWhenJson or callWhenXml
                new MockHttpHandler("/rest/json", "GET", (req, res, parms) => {
                    var obj = new ResponseObject() { intValue = 1000};

                    res.StatusCode = (int) HttpStatusCode.OK;
                    res.ContentType = "application/json";
                    res.Content(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)));
                }),


                new MockHttpHandler("/rest/xml", "GET", (req, res, parms) => {
                    var obj = new ResponseObject() { intValue = 1000};

                    res.StatusCode = (int) HttpStatusCode.OK;
                    res.ContentType = "text/xml";

                    using(MemoryStream ms = new MemoryStream())
                    {
                        System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                        xml.Serialize(ms, obj);

                        byte[] buffer = new byte[ms.Length];

                        ms.Position = 0;
                        ms.Read(buffer, 0, buffer.Length);

                        res.Content(buffer);
                    }
                }),
                #endregion

                #region (req/res) json body and httpStatus.OK
                new MockHttpHandler("/rest/json/body", "POST", (req, res, parms) => {
                    byte[] buffer;
                    using(MemoryStream ms = new MemoryStream())
                    {
                        req.InputStream.CopyTo(ms);

                        buffer = new byte[ms.Length];

                        ms.Position = 0;
                        ms.Read(buffer, 0, buffer.Length);
                    }

                    var reqObject = JsonConvert.DeserializeObject<BodyObject>(Encoding.UTF8.GetString(buffer));

                    res.StatusCode = (int) HttpStatusCode.OK;
                    res.ContentType = "application/json";
                    res.Content(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reqObject)));
                }),

                new MockHttpHandler("/rest/json/body", "PUT", (req, res, parms) => {
                    byte[] buffer;
                    using(MemoryStream ms = new MemoryStream())
                    {
                        req.InputStream.CopyTo(ms);

                        buffer = new byte[ms.Length];

                        ms.Position = 0;
                        ms.Read(buffer, 0, buffer.Length);
                    }

                    var reqObject = JsonConvert.DeserializeObject<BodyObject>(Encoding.UTF8.GetString(buffer));

                    res.StatusCode = (int) HttpStatusCode.OK;
                    res.ContentType = "application/json";
                    res.Content(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reqObject)));
                }),

                new MockHttpHandler("/rest/json/body", "PATCH", (req, res, parms) => {
                    byte[] buffer;
                    using(MemoryStream ms = new MemoryStream())
                    {
                        req.InputStream.CopyTo(ms);

                        buffer = new byte[ms.Length];

                        ms.Position = 0;
                        ms.Read(buffer, 0, buffer.Length);
                    }

                    var reqObject = JsonConvert.DeserializeObject<BodyObject>(Encoding.UTF8.GetString(buffer));

                    res.StatusCode = (int) HttpStatusCode.OK;
                    res.ContentType = "application/json";
                    res.Content(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reqObject)));
                }),
                

                #endregion
            };

            mockServer = new MockServer(8080, mockeServerHandlers);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockServer.Dispose();
        }

        #region call() Method Success
        [TestMethod]
        public void Test_Call_HttpMethodGet_Success()
        {
            var response = Request.Call(new RequestInfo() {
                URI = "http://localhost:8080/rest"
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Test_Call_HttpMethodPATCH_Success()
        {
            var response = Request.Call(new RequestInfo() {
                URI = "http://localhost:8080/rest",
                Method = HttpMethod.PATCH
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Test_Call_HttpMethodPOST_Success()
        {
            var response = Request.Call(new RequestInfo() {
                URI = "http://localhost:8080/rest",
                Method = HttpMethod.POST
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Test_Call_HttpMethodPUT_Success()
        {
            var response = Request.Call(new RequestInfo() {
                URI = "http://localhost:8080/rest",
                Method = HttpMethod.PUT
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Test_Call_HttpMethodDELETE_Success()
        {
            var response = Request.Call(new RequestInfo() {
                URI = "http://localhost:8080/rest",
                Method = HttpMethod.DELETE
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

    }
}
