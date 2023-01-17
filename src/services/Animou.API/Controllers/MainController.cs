using Animou.Business.Interfaces;
using Animou.Business.Notifications;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Animou.API.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly INotifier _notifier;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public MainController(INotifier notifier, IStringLocalizer<SharedResource> localizer)
        {
            _notifier = notifier;
            _localizer = localizer;
        }

        protected bool IsValidOperation() => !_notifier.HasNotification();

        protected ActionResult CustomResponse(object? result = null)
        {
            if (IsValidOperation()) return Ok(result);

            var notificationsRaw = _notifier.GetNotifications().Select(n => n.Message).ToList();
            var NotificationsLocalized = notificationsRaw.Select(_localizer.GetString);
            var messages = NotificationsLocalized.Select(l => l.Value).ToArray();

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", messages }
            }));
        }

        protected ActionResult CustomResponse(ValidationResult validationResult) 
        {
            foreach (var error in validationResult.Errors)
                _notifier.Handle(new Notification(error.ErrorMessage));

            return CustomResponse();
        }

        protected void NotifyError(string message) => _notifier.Handle(new Notification(message));
    }
}
