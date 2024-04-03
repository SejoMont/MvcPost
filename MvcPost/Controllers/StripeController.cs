using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace MvcPost.Controllers
{
    public class StripeController : Controller
    {
        // Método de acción para la vista de pago
        public IActionResult Index()
        {
            return View();
        }

        // Método de acción para procesar el pago cuando se envía el formulario
        [HttpPost]
        public async Task<IActionResult> ProcesarPago(string stripeToken, string name, string amount, string email)
        {
            // Convierte el monto a un valor numérico (por ejemplo, en centavos para USD)
            int.TryParse(amount, out int amountInCents);

            var customers = new CustomerService();

            // Crear instancia de ChargeService para manejar cargos en Stripe
            var charges = new ChargeService();

            // Crear un nuevo cliente en Stripe con el correo electrónico y el token de tarjeta proporcionados
            var customer = await customers.CreateAsync(new CustomerCreateOptions
            {
                Email = email,
                Source = stripeToken
            });

            // Crear un nuevo cargo en Stripe con el monto, descripción y cliente asociado
            var charge = await charges.CreateAsync(new ChargeCreateOptions
            {
                Amount = amountInCents,
                Description = "Compra de " + name,
                Currency = "usd",
                Customer = customer.Id
            });

            return RedirectToAction("Index", "Home");
        }
    }
}
