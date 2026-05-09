using System;

namespace RogueDefense.Logic.Network;

public static class StreamHelper
{
    public static void WriteIntoStream(StreamPeer stream, MessageType type, Resource message)
    {
        stream.Put8((sbyte)type);
        stream.PutVar(message, true);
    }
    public static Tuple<MessageType, Resource> ReadFromStream(StreamPeer stream)
    {
        MessageType type = (MessageType)stream.Get8();
        Variant obj = stream.GetVar(true);
        return new Tuple<MessageType, Resource>(type, (Resource)obj);
    }
}