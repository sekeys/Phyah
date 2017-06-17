using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore.Messages
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    public class DynamicJson : DynamicData
    {
        public DynamicJson(bool ignorecase) : base(ignorecase)
        {

        }
        public DynamicJson() : this(false)
        {
        }
        public override void Deserialize(string value)
        {
            var jobject = JObject.Parse(value);
            ReadProperties(this, jobject);
        }
        private void ReadArrary(DynamicData dynamic, JArray jobject)
        {
            foreach (var item in jobject.Children())
            {
                switch (item.Type)
                {
                    case JTokenType.Array:
                        var t = new DynamicJson();
                        this.ReadArrary(t, (JArray)item);
                        dynamic.Set(item.Path, t);
                        break;
                    case JTokenType.Boolean:
                        dynamic.Set(item.Path, (bool)item.Value<bool>());
                        break;
                    case JTokenType.Float:
                        dynamic.Set(item.Path, item.Value<float>());
                        break;
                    case JTokenType.Bytes:
                        dynamic.Set(item.Path, item.Value<byte[]>());
                        break;
                    case JTokenType.Constructor:
                        dynamic.Set(item.Path, item.Value<string>());
                        break;
                    case JTokenType.Date:
                        dynamic.Set(item.Path, item.Value<DateTime>());
                        break;
                    case JTokenType.Guid:
                        dynamic.Set(item.Path, item.Value<Guid>());
                        break;
                    case JTokenType.Integer:
                        dynamic.Set(item.Path, (int)item.Value<int>());
                        break;
                    case JTokenType.Null:
                        dynamic.Set(item.Path, null);
                        break;
                    case JTokenType.String:
                        dynamic.Set(item.Path, item.Value<string>());
                        break;
                    case JTokenType.TimeSpan:
                        dynamic.Set(item.Path, item.Value<TimeSpan>());
                        break;
                    case JTokenType.Undefined:
                        break;
                    case JTokenType.Uri:
                        dynamic.Set(item.Path, item.Value<Uri>());
                        break;
                    case JTokenType.Raw:
                        var temp = new DynamicJson();
                        temp.Deserialize(item.Value<string>());
                        dynamic.Set(item.Path, temp);
                        break;
                    case JTokenType.Object:
                        var temp1 = new DynamicJson();
                        this.ReadProperties(temp1, item.Value<JObject>());
                        dynamic.Set(item.Path, temp1);
                        break;
                    case JTokenType.Property:
                        dynamic.Set(item.Path, item.Value<object>());
                        break;
                    default: break;
                }
            }
        }
        private void ReadProperties(DynamicData dynamic, JObject jobject)
        {
            foreach (var item in jobject.SelectTokens("*"))
            {
                switch (item.Type)
                {
                    case JTokenType.Array:
                        var t = new DynamicJson();
                        this.ReadArrary(t, (JArray)item);
                        dynamic.Set(item.Path, t);
                        break;
                    case JTokenType.Boolean:
                        dynamic.Set(item.Path, (bool)item.Value<bool>());
                        break;
                    case JTokenType.Float:
                        dynamic.Set(item.Path, item.Value<float>());
                        break;
                    case JTokenType.Bytes:
                        dynamic.Set(item.Path, item.Value<byte[]>());
                        break;
                    case JTokenType.Constructor:
                        dynamic.Set(item.Path, item.Value<string>());
                        break;
                    case JTokenType.Date:
                        dynamic.Set(item.Path, item.Value<DateTime>());
                        break;
                    case JTokenType.Guid:
                        dynamic.Set(item.Path, item.Value<Guid>());
                        break;
                    case JTokenType.Integer:
                        dynamic.Set(item.Path, (int)item.Value<int>());
                        break;
                    case JTokenType.Null:
                        dynamic.Set(item.Path, null);
                        break;
                    case JTokenType.String:
                        dynamic.Set(item.Path, item.Value<string>());
                        break;
                    case JTokenType.TimeSpan:
                        dynamic.Set(item.Path, item.Value<TimeSpan>());
                        break;
                    case JTokenType.Undefined:
                        break;
                    case JTokenType.Uri:
                        dynamic.Set(item.Path, item.Value<Uri>());
                        break;
                    case JTokenType.Raw:
                        var temp = new DynamicJson();
                        temp.Deserialize(item.Value<string>());
                        dynamic.Set(item.Path, temp);
                        break;
                    case JTokenType.Object:
                        var temp1 = new DynamicJson();
                        this.ReadProperties(temp1, item.Value<JObject>());
                        dynamic.Set(item.Path, temp1);
                        break;
                    case JTokenType.Property:
                        dynamic.Set(item.Path, item.Value<object>());
                        break;
                    default: break;
                }
            }
        }
        /*
         * //
        // 摘要:
        //     No token type has been set.
        None = 0,
        //
        // 摘要:
        //     A JSON object.
        Object = 1,
        //
        // 摘要:
        //     A JSON array.
        Array = 2,
        //
        // 摘要:
        //     A JSON constructor.
        Constructor = 3,
        //
        // 摘要:
        //     A JSON object property.
        Property = 4,
        //
        // 摘要:
        //     A comment.
        Comment = 5,
        //
        // 摘要:
        //     An integer value.
        Integer = 6,
        //
        // 摘要:
        //     A float value.
        Float = 7,
        //
        // 摘要:
        //     A string value.
        String = 8,
        //
        // 摘要:
        //     A boolean value.
        Boolean = 9,
        //
        // 摘要:
        //     A null value.
        Null = 10,
        //
        // 摘要:
        //     An undefined value.
        Undefined = 11,
        //
        // 摘要:
        //     A date value.
        Date = 12,
        //
        // 摘要:
        //     A raw JSON value.
        Raw = 13,
        //
        // 摘要:
        //     A collection of bytes value.
        Bytes = 14,
        //
        // 摘要:
        //     A Guid value.
        Guid = 15,
        //
        // 摘要:
        //     A Uri value.
        Uri = 16,
        //
        // 摘要:
        //     A TimeSpan value.
        TimeSpan = 17
         * */
        public override string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
