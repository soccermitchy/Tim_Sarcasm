﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using VoiceManager;
//
//    var configuration = Configuration.FromJson(jsonString);

namespace TimSarcasm
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Configuration
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("createVoiceChannelId")]
        public ulong CreateVoiceChannelId { get; set; }
        [JsonProperty("voiceChannelCategory")]
        public ulong VoiceChannelCategory { get; set; }
        
        [JsonProperty("spamRole")]
        public ulong SpamRoleId { get; set; }

        [JsonProperty("modLogChannel")]
        public ulong ModLogChannelId { get; set; }

    }

    public partial class Configuration
    {
        public static Configuration FromJson(string json) => JsonConvert.DeserializeObject<Configuration>(json, TimSarcasm.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Configuration self) => JsonConvert.SerializeObject(self, TimSarcasm.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
