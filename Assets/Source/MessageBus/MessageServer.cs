using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Allow clients to only subscribe to certain senders/filter out messages (see multiple-menus problem)
public class MessageServer
{
    static List<iMessageClient> l_subscribers;
    static Dictionary<MessageID, List<iMessageClient>> m_subscribers;

    /// <summary>
    /// Sends a message to all subscribed clients
    /// </summary>
    /// <param name="id">The ID of the message</param>
    /// <returns>True if message is valid</returns>
    public static bool SendMessage(MessageID id, Message message)
    {
        if (m_subscribers == null)
        {
            return false;
        }

        if (!m_subscribers.TryGetValue(id, out l_subscribers))
        {
            return false;
        }

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
    public static void Subscribe(ref iMessageClient client, MessageID messageID)
    {
        if (m_subscribers == null)
        {
            m_subscribers = new Dictionary<MessageID, List<iMessageClient>>();
        }

        if (m_subscribers.ContainsKey(messageID))
        {
            if (!m_subscribers.TryGetValue(messageID, out l_subscribers))
            {
                // this should never happen
            }
            l_subscribers.Add(client);
        }
        else
        {
            l_subscribers = new List<iMessageClient>();
            l_subscribers.Add(client);
            m_subscribers.Add(messageID, l_subscribers);
        }
    }

    /// <summary>
    /// Removes a message client from the list of clients
    /// if it is in the list.
    /// </summary>
    /// <param name="client"></param>
    public static void Unsubscribe(ref iMessageClient client, MessageID messageID)
    {
        if (!m_subscribers.TryGetValue(messageID, out l_subscribers))
        {
            return;
        }

        if (l_subscribers.Contains(client))
        {
            l_subscribers.Remove(client);
        }
    }

    public static void OnSceneChange()
    {
        if (m_subscribers != null)
        {
            m_subscribers.Clear();
        }
        else
        {
            m_subscribers = new Dictionary<MessageID, List<iMessageClient>>();
        }
    }
}
