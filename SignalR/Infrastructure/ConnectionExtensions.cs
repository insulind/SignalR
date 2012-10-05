﻿using System.Threading.Tasks;

namespace SignalR
{
    public static class ConnectionExtensions
    {
        internal static Task Close(this ITransportConnection connection, string connectionId)
        {
            var command = new Command
            {
                Type = CommandType.Disconnect
            };

            return connection.Publish(new ConnectionMessage(connectionId, command));
        }

        internal static Task Abort(this ITransportConnection connection, string connectionId)
        {
            var command = new Command
            {
                Type = CommandType.Abort
            };

            return connection.Publish(new ConnectionMessage(connectionId, command));
        }

        /// <summary>
        /// Sends a message to all connections subscribed to the specified signal. An example of signal may be a
        /// specific connection id, or fully qualified group name (Use <see cref="IGroupManager"/> to manipulate groups).
        /// </summary>
        /// <param name="connection">The connection</param>
        /// <param name="signal">The signal to publish to.</param>
        /// <param name="value">The value to publish.</param>
        /// <param name="exclude">The list of connection ids to exclude</param>
        /// <returns>A task that represents when the broadcast is complete.</returns>
        public static Task Publish(this IConnection connection, string signal, object value, params string[] exclude)
        {
            var message = new ConnectionMessage(signal, value)
            {
                ExcludedSignals = exclude
            };

            return connection.Publish(message);
        }

        /// <summary>
        /// Broadcasts a value to all connections. Excluding the connection is specified.
        /// </summary>
        /// <param name="connection">The connection</param>
        /// <param name="value">The value to broadcast.</param>
        /// <param name="exclude">The list of connection ids to exclude</param>
        /// <returns>A task that represents when the broadcast is complete.</returns>
        public static Task Broadcast(this IConnection connection, object value, params string[] exclude)
        {
            return connection.Publish(connection.DefaultSignal, value, exclude);
        }
    }
}
