using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace BexioLibTest.Unit
{
    /// <summary>
    /// Every generated model must read the example values documented in the bexio OpenAPI description.
    /// </summary>
    public class ModelExampleTests
    {
        public static IEnumerable<object[]> Models()
        {
            var examples = JObject.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "model-examples.json")));
            foreach (var p in examples.Properties())
            {
                yield return new object[] { p.Name };
            }
        }

        [Theory]
        [MemberData(nameof(Models))]
        public void Model_reads_the_documented_example(string modelName)
        {
            var examples = JObject.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "model-examples.json")));
            var json = examples[modelName].ToString();
            var type = typeof(bexio_lib.Models.BexioContact).Assembly.GetType("bexio_lib.Models." + modelName);
            Assert.NotNull(type);

            var model = JsonConvert.DeserializeObject(json, type);
            Assert.NotNull(model);

            // properties that carry an example must have been read
            var read = JObject.Parse(JsonConvert.SerializeObject(model, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            foreach (var prop in JObject.Parse(json).Properties())
            {
                Assert.True(read.ContainsKey(prop.Name), $"{modelName}.{prop.Name} was not read");
            }
        }
    }
}
