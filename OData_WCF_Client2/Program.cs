using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OData_WCF_Client
{
    class Program
    {

        private static string _serviceUrl = "http://localhost:58936/CatalogDataService.svc/";

        static void Main(string[] args)
        {

            //// TEST WCF SERVICE WITH SERVICE REFERENCE
            //TestWithServiceReference();

            // TEST WCF SERVICE WITH Simple.OData.Client
            // more infos: 
            //      https://github.com/object/Simple.OData.Client
            //      https://github.com/object/Simple.OData.Client/wiki/Retrieving-data
            TestWithSimpleOdataClient().Wait();

            // TEST WCF SERVICE WITH ODataLib
            // more infos: 
            // http://odata.github.io/odata.net/#04-01-basic-crud-operations
            // ... Missing :)

            // WebClient
            TestWithWebClient();

            // HttpRequest
            TestWithHttpRequest();

            Console.WriteLine("-- END --");
            Console.ReadLine();
        }

        static void TestWithServiceReference()
        {
            //var ctx = new StdRef.CatalogContext(new Uri(_serviceUrl));
            //var products = from c in ctx.Products
            //               where c.Name.StartsWith("Prod")
            //               select c;

            //Console.WriteLine("-- TEST WCF SERVICE WITH SERVICE REFERENCE --");
            //foreach (var item in products)
            //{
            //    Console.WriteLine(item.Name);
            //}
            //Console.WriteLine();
        }

        static async Task TestWithSimpleOdataClient()
        {
            var client = new Simple.OData.Client.ODataClient(_serviceUrl);

            // Untyped syntax
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-- TEST WCF SERVICE WITH SIMPLE ODATA CLIENT LIBRARY (UNTYPED SYNTAX) --");

            var uProducts = await client.For("Products").FindEntriesAsync();
            Console.WriteLine("-- Untyped syntax -> client.For(\"Products\").FindEntriesAsync() ");
            foreach (var item in uProducts.ToList())
            {
                Console.WriteLine(item["Name"]);
            }
            Console.WriteLine();

            // Untyped syntax, filter Code = 'prd1'
            uProducts = await client.FindEntriesAsync("Products?$filter=Code eq 'prd1'");
            Console.WriteLine("-- Untyped syntax -> Products?$filter=Code eq 'prd1' ");
            foreach (var item in uProducts.ToList())
            {
                Console.WriteLine(item["Name"]);
            }
            Console.WriteLine();

            // Untyped syntax, filtering name 'Prod*'
            uProducts = await client.FindEntriesAsync("Products?$filter=startswith(Name,'Prod')");
            Console.WriteLine("-- Untyped syntax -> Products?$filter=startswith(Name,'Prod') ");
            foreach (var item in uProducts.ToList())
            {
                Console.WriteLine(item["Name"]);
            }
            Console.WriteLine();

            //Typed syntax
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine("-- TEST WCF SERVICE WITH SIMPLE ODATA CLIENT LIBRARY (TYPED SYNTAX) --");

            var tProducts = await client.For<Product>().FindEntriesAsync();
            Console.WriteLine("-- Typed syntax -> client.For<Product>().FindEntriesAsync() ");
            foreach (var item in tProducts.ToList())
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            // Typed syntax, filter Code = 'prd1'
            tProducts = await client.For<Product>().Filter(x => x.Code == "prd1").FindEntriesAsync();
            Console.WriteLine("-- Typed syntax -> Products?$filter=Code eq 'prd1' ");
            foreach (var item in tProducts.ToList())
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            // Typed syntax, filtering name 'Prod*'
            tProducts = await client.For<Product>().Filter(x => x.Name.StartsWith("Prod")).FindEntriesAsync();
            Console.WriteLine("-- Typed syntax -> Products?$filter=startswith(Name,'Prod') ");
            foreach (var item in tProducts.ToList())
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            //Dynamic syntax
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-- TEST WCF SERVICE WITH SIMPLE ODATA CLIENT LIBRARY (DYNAMIC SYNTAX) --");

            var dexpr = Simple.OData.Client.ODataDynamic.Expression;
            var dProducts = await client.For(dexpr.Products).FindEntriesAsync();
            Console.WriteLine("-- Dynamic syntax -> client.For(x => x.Products).FindEntriesAsync() ");
            foreach (var item in dProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            // Dynamic syntax, filter Code = 'prd1'
            dProducts = await client.For(dexpr.Products).Filter(dexpr.Code == "prd1").FindEntriesAsync();
            Console.WriteLine("-- Dynamic syntax -> Products?$filter=Code eq 'prd1' ");
            foreach (var item in dProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            // Dynamic syntax, filtering name 'Prod*'
            dProducts = await client.For(dexpr.Products).Filter(dexpr.Name.StartsWith("Prod")).FindEntriesAsync();
            Console.WriteLine("-- Dynamic syntax -> Products?$filter=startswith(Name,'Prod') ");
            foreach (var item in dProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

        }

        static void TestWithWebClient()
        {
            var client = new System.Net.WebClient();
            client.BaseAddress = _serviceUrl;

            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-- TEST WCF SERVICE WITH HTTP WEBCLIENT --");

            // all products
            var data = client.DownloadString("Products?$format=json"); // NB: $format=json
            var wrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductWrapperWitWC>(data);

            // data = {"odata.metadata":"http://localhost:58936/CatalogDataService.svc/$metadata#Products","value":[{"ID":1,"Name":"Prodotto 1","Code":"prd1"},{"ID":2,"Name":"Prodotto 2","Code":"prd2"},{"ID":3,"Name":"Prodotto 3","Code":"prd3"},{"ID":4,"Name":"Prodotto 4","Code":"prd4"},{"ID":5,"Name":"Prodotto 5","Code":"prd5"}]}

            var uProducts = wrapper.value;
            Console.WriteLine("-- WebClient -> Extract all products: ");
            foreach (var item in uProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            // filter Code = 'prd1'
            data = client.DownloadString("Products?$format=json&$filter = Code eq 'prd1'"); // NB: $format=json
            wrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductWrapperWitWC>(data);
            uProducts = wrapper.value;
            Console.WriteLine("-- WebClient -> Estract Products?$filter=Code eq 'prd1' ");
            foreach (var item in uProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            // filtering name 'Prod*'
            data = client.DownloadString("Products?$format=json&$filter=startswith(Name,'Prod')"); // NB: $format=json
            wrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductWrapperWitWC>(data);
            uProducts = wrapper.value;
            Console.WriteLine("-- WebClient -> Products?$filter=startswith(Name,'Prod') ");
            foreach (var item in uProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

        }

        static void TestWithHttpRequest()
        {
            System.Net.WebRequest request ;
            System.Net.HttpWebResponse response;
            System.IO.Stream stream;
            System.IO.StreamReader reader;

            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-- TEST WCF SERVICE WITH HTTPREQUEST --");

            // all products
            request = System.Net.WebRequest.Create(_serviceUrl + "Products?$format=json"); // NB: $format=json
            response = (System.Net.HttpWebResponse)request.GetResponse();
            stream = response.GetResponseStream();
            reader = new System.IO.StreamReader(stream);

            string data = reader.ReadToEnd();
            var wrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductWrapperWitWC>(data);

            // data = {"odata.metadata":"http://localhost:58936/CatalogDataService.svc/$metadata#Products","value":[{"ID":1,"Name":"Prodotto 1","Code":"prd1"},{"ID":2,"Name":"Prodotto 2","Code":"prd2"},{"ID":3,"Name":"Prodotto 3","Code":"prd3"},{"ID":4,"Name":"Prodotto 4","Code":"prd4"},{"ID":5,"Name":"Prodotto 5","Code":"prd5"}]}

            var uProducts = wrapper.value;
            Console.WriteLine("-- HttpRequest -> Extract all products: ");
            foreach (var item in uProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            // filter Code = 'prd1'
            request = System.Net.WebRequest.Create(_serviceUrl + "Products?$format=json&$filter = Code eq 'prd1'"); // NB: $format=json
            response = (System.Net.HttpWebResponse)request.GetResponse();
            stream = response.GetResponseStream();
            reader = new System.IO.StreamReader(stream);

             data = reader.ReadToEnd();
            wrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductWrapperWitWC>(data);
            uProducts = wrapper.value;
            Console.WriteLine("-- HttpRequest -> Estract Products?$filter=Code eq 'prd1' ");
            foreach (var item in uProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            // filtering name 'Prod*'
            request = System.Net.WebRequest.Create(_serviceUrl + "Products?$format=json&$filter=startswith(Name,'Prod')"); // NB: $format=json
            response = (System.Net.HttpWebResponse)request.GetResponse();
            stream = response.GetResponseStream();
            reader = new System.IO.StreamReader(stream);

            data = reader.ReadToEnd();
            wrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductWrapperWitWC>(data);
            uProducts = wrapper.value;
            Console.WriteLine("-- HttpRequest -> Products?$filter=startswith(Name,'Prod') ");
            foreach (var item in uProducts)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();

            reader.Close();
            stream.Close();
            response.Close();

        }
    }

    internal class ProductWrapperWitWC
    {
        public string metadata { get; set; }
        public IList<Product> value { get; set; }
    }

    internal class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

}
