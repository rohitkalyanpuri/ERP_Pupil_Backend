
using System.Collections.Generic;

namespace Pupil.Model
{
    public interface IResponse
    {
        string Message { get; set; }

        string Error { get; set; }
        bool Success { get;  }
        StatusCode Status { get; set; }
    }

    public interface ISingleResponse<TModel> : IResponse
    {
        TModel Data { get; set; }
    }

    public interface IListResponse<TModel> : IResponse
    {
        IEnumerable<TModel> Data { get; set; }
    }

    public class Response : IResponse
    {
        public StatusCode Status { get; set; }
        public bool Success { get { return Status == StatusCode.Ok; } }
        public string Message { get; set; }
        public string Error { get; set; }
    }

    public class SingleResponse<TModel> : ISingleResponse<TModel>
    {
        public StatusCode Status { get; set; }
        public bool Success { get { return Status == StatusCode.Ok; } }
        public string Message { get; set; }
        public TModel Data { get; set; }
        public string Error { get; set; }
    }

    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public StatusCode Status { get; set; }
        public bool Success { get { return Status == StatusCode.Ok; } }
        public string Message { get; set; }
        public IEnumerable<TModel> Data { get; set; }
        public string Error { get; set; }
    }
    

}
