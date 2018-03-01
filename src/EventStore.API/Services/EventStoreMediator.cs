using EventStore.Core;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Requests;
using EventStore.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.Web.Services
{
    public class EventStoreMediator: IMediator
    {
        private IMediator _inner;
        private IHttpContextAccessor _httpContextAccessor;
        private IEventStoreContext _appDataContext;

        public EventStoreMediator(MultiInstanceFactory multiInstanceFactory, SingleInstanceFactory singleInstanceFactory, IHttpContextAccessor httpContextAccessor, IEventStoreContext appDataContext)
        {
            _inner = new Mediator(singleInstanceFactory, multiInstanceFactory);
            _httpContextAccessor = httpContextAccessor;
            _appDataContext = appDataContext;
        }
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
        {
            return _inner.Publish(notification, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
        {
            TryToSetUserIdFromHttpContext(request, _httpContextAccessor.HttpContext);

            return _inner.Send(request, cancellationToken);
        }

        public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            TryToSetUserIdFromHttpContext(request, _httpContextAccessor.HttpContext);

            return _inner.Send(request, cancellationToken);
        }

        private void TryToSetUserIdFromHttpContext(dynamic request, HttpContext httpContext)
        {
            httpContext.Request.Query.TryGetValue("tenantId", out StringValues tenant);

            if (request.GetType().IsSubclassOf(typeof(BaseRequest)) && string.IsNullOrEmpty(tenant))
                (request as BaseRequest).TenantId = new Guid(httpContext.Request.GetHeaderValue("Tenant"));

            if (request.GetType().IsSubclassOf(typeof(BaseRequest)) && !string.IsNullOrEmpty(tenant))
                (request as BaseRequest).TenantId = new Guid(tenant);

            if (request.GetType().IsSubclassOf(typeof(BaseRequest)))
                _appDataContext.TenantId = (request as BaseRequest).TenantId;

            if (request.GetType().IsSubclassOf(typeof(BaseAuthenticatedRequest)))
                (request as BaseAuthenticatedRequest).Username = httpContext.User.Identity.Name;
        }
    }
}
