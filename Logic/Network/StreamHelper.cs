using System;

namespace RogueDefense.Logic.Network;

public static class StreamHelper
{
    public static void WriteIntoStream(StreamPeer stream, MessageType type, Resource message)
    {
        stream.Put8((sbyte)type);
        stream.PutVar(message);
    }
    public static Tuple<MessageType, Resource> ReadFromStream(StreamPeer stream)
    {
        MessageType type = (MessageType)stream.Get8();
        Resource message = (Resource)stream.GetVar();
        return new Tuple<MessageType, Resource>(type, message);
    }
}