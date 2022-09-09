using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Extensions
{
	public static class MediatorExtension
	{
		public static TResponse SendSync<TResponse>(this IMediator mediator, IRequest<TResponse> request, CancellationToken cancellationToken = default)
		{
			var task = Task.Run(async () => await mediator
				.Send(request, cancellationToken)
				.ConfigureAwait(false)
			);
			return task.Result;
		}

		public static void SendSync(this IMediator mediator, IRequest request, CancellationToken cancellationToken = default)
		{
			var task = Task.Run(async () => await mediator
				.Send(request, cancellationToken)
				.ConfigureAwait(false)
			);
		}
	}
}
