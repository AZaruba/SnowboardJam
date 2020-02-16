using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageServer
{
    // TODO: Add message-by-message subscription
    static List<iMessageClient> l_subscribers;

    /// <summary>
    /// Sends a message to all subscribed clients
    /// </summary>
    /// <param name="id">The ID of the message</param>
    /// <returns>True if message is valid</returns>
    public static bool SendMessage(MessageID id, Message message)
    {
        foreach (iMessageClient subscriber in l_subscribers)
        {
            subscriber.RecieveMessage(id, message);
        }
        return true;
    }

    /// <summary>
    /// Adds a message client to the List of clients.
    /// Will skip adding the client if it is already subscribed
    /// </summary>
    /// <param name="client"></param>
    public static void Subscribe(ref iMessageClient client)
    {
        if (l_subscribers == null)
        {
            l_subscribers = new List<iMessageClient>();
        }

        if (l_subscribers.Contains(client))
        {
            return;
        }
        l_subscribers.Add(client);
    }

    /// <summary>
    /// Removes a message client from the list of clients
    /// if it is in the list.
    /// </summary>
    /// <param name="client"></param>
    public static void Unsubscribe(ref iMessageClient client)
    {
        if (l_subscribers.Contains(client))
        {
            l_subscribers.Remove(client);
        }
    }
}
