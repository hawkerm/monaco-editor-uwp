﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Languages
{
    public sealed class ILanguageExtensionPoint
    {
        [JsonProperty("aliases", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Aliases { get; set; }
        [JsonProperty("configuration", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Configuration { get; set; }
        [JsonProperty("extensions", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Extensions { get; set; }
        [JsonProperty("filenamePatterns", NullValueHandling = NullValueHandling.Ignore)]
        public string[] FilenamePatterns { get; set; }
        [JsonProperty("filenames", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Filenames { get; set; }
        [JsonProperty("firstLine", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstLine { get; set; }
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("mimetypes", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Mimetypes { get; set; }
    }
}
