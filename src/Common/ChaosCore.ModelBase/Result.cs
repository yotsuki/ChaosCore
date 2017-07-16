using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ChaosCore.ModelBase
{
    [DataContract]
    [JsonObject()]
    public class Result
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Message { get; set; }

        [IgnoreDataMember]
        public Dictionary<string, object> Datas { get; set; } = new Dictionary<string, object>();

        public virtual bool Success { get { return Code == 0; } }

        public Result()
        {
            Code = 0;
            Message = string.Empty;
        }

        public Result(int code = 0, string msg = "")
        {
            Code = code;
            Message = msg;
        }

        public Result Failure(int code = -1, string errmsg = "")
        {
            this.Code = code;
            this.Message = errmsg;
            return this;
        }

        public static Result New(int code = 0, string msg = "")
        {
            return new Result(code,msg);
        }

        public Result<TResult> New<TResult>()
        {
            return new Result<TResult>(Code, Message);
        }

        public Result<TResult> New<TResult>(TResult value)
        {
            return new Result<TResult>(Code, Message) { Value = value };
        }

        public static explicit operator int(Result result1)
        {
            if (result1 == null) {
                return 0xff;
            }
            return result1.Code;
        }

        public static implicit operator bool(Result result1)
        {
            if (result1 == null) {
                return false;
            }
            return result1.Success;
        }

        public static explicit operator Result(int code)
        {
            return new Result(code);
        }

        public static implicit operator Result(bool code)
        {
            return new Result(code?0:1);
        }
    }

    public class Result<TResult>: Result
    {
        public TResult Value { get; set; }

        public override bool Success
        {
            get{
                return Code == 0 && Value != null;
            }
        }

        public Result():base(0,""){}

        public Result(int code = 0, string msg = "")
            :base(code,msg)
        {
        }
        public Result<TResult> Failure(int code = -1, string errmsg = "")
        {
            this.Code = code;
            this.Message = errmsg;
            return this;
        }
        public Result<TResult> Final(TResult value)
        {
            this.Value = value;
            return this;
        }
        public Result<TResult> Final(Result value)
        {
            Code = value.Code;
            Message = value.Message;
            return this;
        }
        public static implicit operator Result<TResult>(TResult result1)
        {
            var result2 = new Result<TResult>();
            result2.Value = result1;
            return result2;
        }

        public static implicit operator TResult(Result<TResult> result1)
        {
            return result1.Value;
        }
    }
}

