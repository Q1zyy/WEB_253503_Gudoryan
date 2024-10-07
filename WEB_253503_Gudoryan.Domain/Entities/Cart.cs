using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_Gudoryan.Domain.Entities
{
	public class Cart
	{

		/// <summary>
		/// Список объектов в корзине
		/// key - идентификатор объекта
		/// </summary>
		public Dictionary<int, CartItem> CartItems { get; set; } = new Dictionary<int, CartItem>();

		/// <summary>
		/// Количество объектов в корзине
		/// </summary>
		public int Count
		{
			get => CartItems.Sum(item => item.Value.Count);
		}

		/// <summary>
		/// Общее количество калорий
		/// </summary>
		public decimal TotalPrice
		{
			get => CartItems.Sum(item => item.Value.Game.Price * item.Value.Count);
		}

		/// <summary>
		/// Добавить объект в корзину
		/// </summary>
		/// <param name="dish">Добавляемый объект</param>
		public virtual void AddToCart(Game game)
		{
			if (CartItems.ContainsKey(game.Id)) 
			{
				CartItems[game.Id].Count++;
			}
			else
			{
				CartItems[game.Id] = new CartItem
				{
					Game = game,
					Count = 1
				};
			}
		}


        /// <summary>
        /// Уменьшает количество объектов из корзина
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void DecreaseFromCart(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems[id].Count--;
                if (CartItems[id].Count == 0)
                {
                    CartItems.Remove(id);
                }
            }
        }


        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveFromCart(int id)
		{
			CartItems.Remove(id); 
		}

		/// <summary>
		/// Очистить корзину
		/// </summary>
		public virtual void ClearAll()
		{
			CartItems.Clear();
		}


	}
}
