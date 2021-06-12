#nullable enable
using Decembrist.Autoload;
using Decembrist.Converter;
using Godot;

namespace Decembrist.Events
{
    public static class LanEventBusUtils
    {
        /// <summary>
        /// <para><inheritdoc cref="LanEventBus.Emit{T}"/></para>
        /// <para>See <see cref="LanEventBus.Emit{T}"/></para>
        /// </summary>
        public static void FireLanEvent<T>(
            this Node node, 
            string @event, 
            object? payload) where T : class, ITypeSerializer => node.GetLanEventBus().Emit<T>(@event, payload);
        
        /// <summary>
        /// <para><inheritdoc cref="LanEventBus.Emit(string,object?,Decembrist.Converter.ITypeSerializer)"/></para>
        /// <para>See <see cref="LanEventBus.Emit(string,object?,Decembrist.Converter.ITypeSerializer)"/></para>
        /// </summary>
        public static void FireLanEvent(
            this Node node, 
            string @event, 
            object? payload,
            ITypeSerializer serializer) => node.GetLanEventBus().Emit(@event, payload, serializer);
        
        /// <summary>
        /// <para><inheritdoc cref="LanEventBus.Emit(string)"/></para>
        /// <para>See <see cref="LanEventBus.Emit(string)"/></para>
        /// </summary>
        public static void FireLanEvent(this Node node, string @event) => node.GetLanEventBus().Emit(@event);
        
        /// <summary>
        /// <para><inheritdoc cref="LanEventBus.Emit(string,string?)"/></para>
        /// <para>See <see cref="LanEventBus.Emit(string,string?)"/></para>
        /// </summary>
        public static void FireLanEvent(this Node node, string @event, string? payload) =>
            node.GetLanEventBus().Emit(@event, payload);
        
        /// <summary>
        /// <para><inheritdoc cref="LanEventBus.Emit(string,bool?)"/></para>
        /// <para>See <see cref="LanEventBus.Emit(string,bool?)"/></para>
        /// </summary>
        public static void FireLanEvent(this Node node, string @event, bool? payload) =>
            node.GetLanEventBus().Emit(@event, payload);
        
        /// <summary>
        /// <para><inheritdoc cref="LanEventBus.Emit(string,int?)"/></para>
        /// <para>See <see cref="LanEventBus.Emit(string,int?)"/></para>
        /// </summary>
        public static void FireLanEvent(this Node node, string @event, int? payload) =>
            node.GetLanEventBus().Emit(@event, payload);

        /// <summary>
        /// <para><inheritdoc cref="LanEventBus.Emit(string,float?)"/></para>
        /// <para>See <see cref="LanEventBus.Emit(string,float?)"/></para>
        /// </summary>
        public static void FireLanEvent(this Node node, string @event, float? payload) =>
            node.GetLanEventBus().Emit(@event, payload);

    }
}