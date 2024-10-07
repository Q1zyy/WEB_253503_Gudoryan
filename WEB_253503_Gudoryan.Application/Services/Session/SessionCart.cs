using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Application.Extensions;
using System.Text.Json.Serialization;

namespace WEB_253503_Gudoryan.Application.Services.Session
{
    public class SessionCart : Cart
    {

        [JsonIgnore]
        private ISession _session;

        public SessionCart()
        {

        }

        public SessionCart(ISession session)
        {
            _session = session;
        }

        public static SessionCart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            var cart = session?.Get<SessionCart>("cart") ?? new SessionCart(session);
            cart._session = session;
            return cart;
        }

        public override void AddToCart(Game game)
        {
            base.AddToCart(game);
            SaveCart();
        }

        public override void RemoveFromCart(int id)
        {
            base.RemoveFromCart(id);
            SaveCart();
        }     
        
        public override void DecreaseFromCart(int id)
        {
            base.DecreaseFromCart(id);
            SaveCart();
        }

        public override void ClearAll()
        {
            base.ClearAll();
            SaveCart();
        }

        private void SaveCart()
        {
            _session.Set<SessionCart>("cart", this);
        }

    }
}
