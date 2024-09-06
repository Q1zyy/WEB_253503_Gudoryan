﻿
namespace WEB_253503_Gudoryan.Domain.Models
{
	public class ResponseData<T>
	{
		// запрашиваемые данные
		public T? Data { get; set; }
		// признак успешного завершения запроса
		public bool Successful { get; set; } = true;
		// сообщение в случае неуспешного завершения
		public string? ErrorMessage { get; set; }
		/// <summary>
		/// Получить объект успешного ответа
		/// </summary>
		/// <param name="data">передаваемые данные</param>
		/// <returns></returns>
		public static ResponseData<T> Success(T data)
		{
			return new ResponseData<T> { Data = data };
		}
		/// <summary>
		/// Получение объекта ответа с ошибкой
		/// </summary>
		/// <param name="message">Сообщение об ошибке</param>
		/// <param name="data">Передаваемые данные</param>
		/// <returns></returns>
		public static ResponseData<T> Error(string message, T? data = default)
		{
			return new ResponseData<T>
			{
				ErrorMessage = message,
				Successful = false,
				Data = data
			};
		}
	}
}