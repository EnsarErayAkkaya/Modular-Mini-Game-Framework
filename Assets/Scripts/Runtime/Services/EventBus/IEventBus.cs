using System;

namespace EEA.Services.Events
{
    public interface IEventBus
    {
        /// <summary>
        /// Subscribes a callback to be invoked when an event of the specified type occurs.
        /// </summary>
        /// <remarks>If multiple callbacks are subscribed to the same event type, they will be invoked in
        /// the order they were added.</remarks>
        /// <typeparam name="T">The type of event to subscribe to.</typeparam>
        /// <param name="callback">The action to invoke when the event of type <typeparamref name="T"/> is raised.  This parameter cannot be
        /// <see langword="null"/>.</param>
        void Subscribe<T>(Action<T> callback);

        /// <summary>
        /// Unsubscribes the specified callback from receiving events of the specified type.
        /// </summary>
        /// <remarks>If the specified callback is not currently subscribed, this method has no
        /// effect.</remarks>
        /// <typeparam name="T">The type of event to unsubscribe from.</typeparam>
        /// <param name="callback">The callback to remove from the subscription list.  This callback will no longer be invoked when events of
        /// type <typeparamref name="T"/> are published.</param>
        void Unsubscribe<T>(Action<T> callback);

        /// <summary>
        /// Raise an event with the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventData"></param>
        void Publish<T>(T eventData);
    }
}