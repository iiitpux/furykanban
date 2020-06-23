using FuryKanban.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuryKanban.Client.Core
{
	public interface IHttpClient
	{
		Task<TResult> GetAsyncEx<TResult>(string url) where TResult : IErrorResult, new();
		Task<TResult> PostAsyncEx<TResult, TValue>(string url, TValue model, string actionName) where TResult : IErrorResult, new();
		Task<TResult> DeleteAsyncEx<TResult>(string url, string actionName) where TResult : IErrorResult, new();
		Task<TResult> PutAsyncEx<TResult, TValue>(string url, TValue model, string actionName) where TResult : IErrorResult, new();
	}
}
